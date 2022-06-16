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

using System.Windows.Media.Animation;
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
    /// ConfirmChargePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConfirmChargePage : Page
    {
        private System.Timers.Timer WatchDog;
        public ConfirmChargePage()
        {
            InitializeComponent();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ConfirmChargePage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.nSelectCommand12 = 2;
           // AVRIO.avrio.StartButton = false;
           // AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/05/충전완료14.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/14.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            BackImage.StartAnimate();

            nextPageTimer = new System.Timers.Timer(1000 * 60);            
            nextPageTimer.AutoReset = false;
            nextPageTimer.Elapsed += new ElapsedEventHandler(nextPageTimer_Elapsed);
            nextPageTimer.Start();

            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

            Double tempSoc = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
            chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc * 100) / 100));
            //textBox1.Text = Convert.ToString(Math.Floor((tempSoc * 100) / 100));
           
        }

        // 5초 후 Thank you 화면으로 갱신하기 위한 타이머
        private System.Timers.Timer nextPageTimer;

        void timer_WatchDog(object sender, ElapsedEventArgs e)
        {
            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {

                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysWorkDone;
                AVRIO.avrio.StopButtonPlay = false;
                AVRIO.avrio.StopButton = false;
            }


        }

        void nextPageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysWorkDone;
        }
       

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
           // AVRIO.avrio.StartButton = false;
          //  AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            BackImage.StopAnimate();
            BackImage.Dispose();

            try
            {
                if (WatchDog != null)
                {
                    WatchDog.Enabled = false;
                    WatchDog.Stop();
                    WatchDog.Dispose();
                    WatchDog = null;
                }
                    
            }
            catch { }
            if (nextPageTimer != null)
            {
                nextPageTimer.Enabled = false;
                nextPageTimer.Stop();
                nextPageTimer.Dispose();
                nextPageTimer = null;
            }
        }
    }
}
