using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Drawing;
using System.Windows.Resources;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.IO;
//using System.Timers;
using System.Media;
using System.Threading;
using System.Timers;

namespace QuickChargeDisplay
{
    /// <summary>
    /// ConnectorPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConnectorPage7 : Page
    {
        private SoundPlayer voicePlayer = new SoundPlayer();
        private System.Timers.Timer timer;
        private System.Timers.Timer WatchDog1;
        private System.Timers.Timer ButtonDelayTimer;
        
        public ConnectorPage7()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ConnectorPage7_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.nSelectCommand34 = 1;
            AVRIO.avrio.nSelectCommand12 = 4;
            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeCurrent;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/Rapidas_R_Connect.gif");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/08a08b.gif");
                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
                AVRIO.avrio.StartRunSeq = false;

            }
           BackImage.StartAnimate();           

            timer = new System.Timers.Timer(1000 * 60 ); // 30초간 대기 (테스트)
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();

          //  WatchDog1 = new System.Timers.Timer(200);
          //  WatchDog1.Elapsed += new ElapsedEventHandler(timer_WatchDog);
         //   WatchDog1.AutoReset = true;


            AVRIO.avrio.StartRunSeq = false;

         //   AVRIO.avrio.ChargeMode = (int)QuickChargeDisplay.ChargeMode.Soc;
            AVRIO.avrio.ChargeValue = 80;
            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeCurrent;

            ButtonDelayTimer = new System.Timers.Timer(500);
            ButtonDelayTimer.Elapsed += new ElapsedEventHandler(timer5_Elapsed);
            ButtonDelayTimer.AutoReset = false;
            ButtonDelayTimer.Start();


            WatchDog1 = new System.Timers.Timer(300);
            WatchDog1.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog1.AutoReset = true;

            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            //AVRIO.avrio.StopButton = false;
           // AVRIO.avrio.StartButton = false; 
        }


        /// <summary>
        /// 대기화면으로 가기위한 타이머 
        /// </summary>        
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
             AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
        }
        void timer5_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            try
            {
          
                WatchDog1.Enabled = true;
            }
            catch {
                WatchDog1 = new System.Timers.Timer(300);
                WatchDog1.Elapsed += new ElapsedEventHandler(timer_WatchDog);
                WatchDog1.AutoReset = true;
                WatchDog1.Enabled = true;
            }
        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.Rapidascardcheck == 1)
            {
                if (AVRIO.avrio.StartButton || AVRIO.avrio.StartButtonPlay)
                {
                    try
                    {
                        WatchDog1.Enabled = false;
                    }
                    catch { }

                    if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;
                  
                    AVRIO.avrio.StopButtonPlay = false;
                    AVRIO.avrio.StartButtonPlay = false;
                }
            }
            else
            {

                if (AVRIO.avrio.StartButton || AVRIO.avrio.StartButtonPlay)
                {
                    try
                    {
                        WatchDog1.Enabled = false;
                    }
                    catch { }

                    if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;
                    else AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;

                    AVRIO.avrio.StopButtonPlay = false;
                    AVRIO.avrio.StartButtonPlay = false;
                    AVRIO.avrio.StartButton = false;
                }

            }
            if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            BackImage.StopAnimate();
            BackImage.Dispose();

            try
            {
                if (ButtonDelayTimer != null)
                {
                    ButtonDelayTimer.Enabled = false;
                    ButtonDelayTimer.Stop();
                    ButtonDelayTimer.Dispose();
                    ButtonDelayTimer = null;
                }
            }
            catch { }
            try
            {
                if (WatchDog1 != null)
                {
                    WatchDog1.Enabled = false;
                    WatchDog1.Stop();
                    WatchDog1.Dispose();
                    WatchDog1 = null;
                }
            }
            catch { }

            try
            {
                if (timer != null)
                {
                    timer.Enabled = false;
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
            catch { }

            AVRIO.avrio.StartRunSeq = false;
            //voicePlayer.Stop();
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
  
        }


    }
}
