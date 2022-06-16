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
using System.Media;
using System.Threading;
using System.Timers;

namespace QuickChargeDisplay
{
    /// <summary>
    /// CompleteChargePage.xaml에 대한 상호 작용 논리
    /// </summary>
        
    public partial class CompleteChargePage : Page
    {
        private SoundPlayer voicePlayer = new SoundPlayer();
        private System.Timers.Timer timer;
        private System.Timers.Timer timer1;
        private System.Timers.Timer WatchDog;
        public CompleteChargePage()
        {
            InitializeComponent();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "CompleteChargePage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.nSelectCommand12 = 2;
            //if (AVRIO.avrio.DisChargeFalg)
            //{
            //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBMSBatteryDischargeStandby;
            //}
            //else AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            //AVRIO.avrio.StartButton = false;
           // AVRIO.avrio.StopButton = false;

            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/15a15b15c15d.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/15abcd.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            BackImage.StartAnimate();

            timer = new System.Timers.Timer(1000 * 58); // 5초간 대기 (테스트)
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();


            timer1 = new System.Timers.Timer(2000); // 2초간 대기 (테스트)
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed1);
            timer1.AutoReset = true;           


            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

          //  voicePlayer.Play();
        }

        void timer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
          //  if (AVRIO.avrio.CurrentStatus != AVRIO.SysStatus.SysStandby)
            {
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;

            timer1.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
           // AVRIO.avrio.StartButton = false;
           // AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            BackImage.StopAnimate();
            BackImage.Dispose();

            

            if (WatchDog != null)
            {
                WatchDog.Enabled = false;
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;
            }
                

            if (timer != null)
            {
                timer.Enabled = false;
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
                

            if (timer1 != null)
            {
                timer1.Enabled = false;
                timer1.Stop();
                timer1.Dispose();
                timer1 = null;
            }
                

            AVRIO.avrio.PageStanByStop = false;
        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.StartButton)
            {

            }
            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {

                    WatchDog.Enabled = false;
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;

                    timer1.Start();
                
            }

            //AVRIO.avrio.StartButton = false;
            //AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
        //    if (AVRIO.avrio.DisChargeFalg)
        //    {
        //        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBMSBatteryDischargeStandby;
        //    }
        //    else AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;

        //    timer1.Start();
        }
    }
}
