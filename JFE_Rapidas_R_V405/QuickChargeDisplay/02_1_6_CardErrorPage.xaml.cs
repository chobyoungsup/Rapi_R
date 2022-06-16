using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Windows.Navigation;
using System.Timers;
using System.Threading;
using System.Media;

namespace QuickChargeDisplay
{
    /// <summary>
    /// Page2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CardErrorStartPage5 : Page
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer WatchDog;

        public CardErrorStartPage5()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "CardErrorStartPage5_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            //AVRIO.avrio.StopButton = false;
           // AVRIO.avrio.StartButton = false;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/02/06.png");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }

            else
            {//영어임
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/06.png");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();

            }
            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

            BackImage.StartAnimate();

            timer = new System.Timers.Timer(1000 * 60); // 
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();
            if (((AVRIO.avrio.rCommandValue & 8) > 0) || ((AVRIO.avrio.rCommandValue & 16) > 0)) 
            {
                AVRIO.avrio.EventMsg = "Card Error 216 TsBatteryFinish";
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
            }
        }
        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (AVRIO.avrio.StartButton)
            {

            }

            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        NavigationService nav = NavigationService.GetNavigationService(this);
                        nav.Navigate(PageManager.GetPage(PageId._01_대기화면));
                    });
                }
                catch { }//  AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
             BackImage.StopAnimate();
             BackImage.Dispose();

             
             AVRIO.avrio.StopButtonPlay = false;
             AVRIO.avrio.StartButtonPlay = false;
             //AVRIO.avrio.StopButton = false;
             //AVRIO.avrio.StartButton = false;
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
                
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {// 1 분간 방치시 첫 화면 이동 
            try
            {
                this.Dispatcher.Invoke((ThreadStart)delegate()
                {
                    NavigationService nav = NavigationService.GetNavigationService(this);
                    nav.Navigate(PageManager.GetPage(PageId._01_대기화면));
                });
            }
            catch { }// AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
        }

    }
}
