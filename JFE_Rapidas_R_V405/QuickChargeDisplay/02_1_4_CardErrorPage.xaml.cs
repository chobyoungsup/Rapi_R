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
    public partial class CardErrorStartPage4 : Page
    {

        private System.Timers.Timer timer;
        private System.Timers.Timer WatchDog;
        public CardErrorStartPage4()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "CardErrorStartPage4_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            //AVRIO.avrio.StopButton = false;
            //AVRIO.avrio.StartButton = false;
            //AVRIO.avrio.nSelectCommand12 = 4;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/02/04.png");

                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {//
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/04.png");

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
                AVRIO.avrio.EventMsg = "Card Error 214 TsBatteryFinish";
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
            }

            // soundPlayer.Play();
            // BackImage.StartAnimate();
            // 1. 배터리 SOC가 80% 이상일 경우 충전을 진행할 수 없으므로
            // 2. 경고 창을 띄어준다.

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
                catch { }
            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //AVRIO.avrio.StopButton = false;
            //AVRIO.avrio.StartButton = false;
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
