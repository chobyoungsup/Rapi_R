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
    public partial class ChargeStartPage : Page
    {
        private SoundPlayer voicePlayer = new SoundPlayer();
        private System.Timers.Timer timer;
        // private System.Timers.Timer AutoBattryCharge;
        private System.Timers.Timer WatchDog;
        //  private ChargeStartDialog dlg = null;   // 충전시작 다이얼로그

        public ChargeStartPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ChargeStartPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.nSelectCommand12 = 3;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/09a09b.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/09a09b.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            BackImage.StartAnimate();

            //AutoBattryCharge = new System.Timers.Timer(1000 * 15 );
            //AutoBattryCharge.Elapsed += new ElapsedEventHandler(timer_AutoCharge);
            //AutoBattryCharge.AutoReset = false;
            //AutoBattryCharge.Enabled = true;

            timer = new System.Timers.Timer(1000 * 60); // 30초간 대기 (테스트)
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();

            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

            AVRIO.avrio.StartRunSeq = false;
            AVRIO.avrio.ChargeValue = 80;
            AVRIO.avrio.StartRunSeq = false;

            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            //  AVRIO.avrio.StopButton = false;
            //  AVRIO.avrio.StartButton = false;

        }

        /// <summary>
        /// 대기화면으로 가기위한 타이머
        /// </summary>        
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;

        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.StartButton)
            {
                //   AVRIO.avrio.ChargeValue = QCDV.ChargeMoney.ChargeValue = 80;
                //   AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;

            }

            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {

                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;

            }
        }

        //private void timer_AutoCharge(object sender, ElapsedEventArgs e)
        //{
        //    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysReady;
        //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;

        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
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

            AVRIO.avrio.StartRunSeq = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            // AVRIO.avrio.StopButton= false;
            // AVRIO.avrio.StartButton = false;
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //    AVRIO.avrio.ChargeValue = QCDV.ChargeMoney.ChargeValue = 80;
            //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;
        }


    }
}
