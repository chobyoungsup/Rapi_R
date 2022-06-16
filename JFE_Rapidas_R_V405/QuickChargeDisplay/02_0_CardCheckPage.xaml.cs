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
    /// Page2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CardCheckStartPage3 : Page
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer ButtonDelayTimer;
        private System.Timers.Timer WatchDog;
        public CardCheckStartPage3()
        {
            InitializeComponent();

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "CardCheckStartPage3_Loaded ";


            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            //AVRIO.avrio.StartButton = false;
            //AVRIO.avrio.StopButton = false;
            AVRIO.avrio.nSelectCommand12 = 5;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            AVRIO.avrio.CardCheckFlag = false;
            AVRIO.avrio.CardCheckFlag4 = false;
            AVRIO.avrio.CardCheckFlag5 = false;
            AVRIO.avrio.CardCheckFlag6 = false;

            if (AVRIO.avrio.Rapidaslanguage == 0)
            {//
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/03a03b.gif");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {//영어임
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/03ab.gif");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();

            }
            BackImage.StartAnimate();

            timer = new System.Timers.Timer(1000 * 60); // 60초간 대기 (테스트)
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();

            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
           // WatchDog.Enabled = true;

            ButtonDelayTimer = new System.Timers.Timer(1000 * 2);
            ButtonDelayTimer.Elapsed += new ElapsedEventHandler(timer5_Elapsed);
            ButtonDelayTimer.AutoReset = false;
            ButtonDelayTimer.Start();
            
        }


        void timer5_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                WatchDog.Enabled = true;
            }
            catch { }
        }


        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
      
            if (AVRIO.avrio.StartButton )
            {// 이게 들어 가는 것인지 모르겠음

            }

            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {

                WatchDog.Enabled = false;
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

            if (AVRIO.avrio.CardCheckFlag)
            {
                WatchDog.Enabled = false;
                AVRIO.avrio.CardCheckFlag = false;
                CardCheckSuccess();
               // AVRIO.avrio.CardCheckFlag = false;
            }

            if (AVRIO.avrio.CardCheckFlag4)
            {
                WatchDog.Enabled = false;
                AVRIO.avrio.CardCheckFlag4 = false;
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        NavigationService nav = NavigationService.GetNavigationService(this);
                        nav.Navigate(PageManager.GetPage(PageId._02_4_카드에러));
                    });
                }
                catch { }
            }
            if (AVRIO.avrio.CardCheckFlag5)
            {
                WatchDog.Enabled = false;
                AVRIO.avrio.CardCheckFlag5 = false;
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        NavigationService nav = NavigationService.GetNavigationService(this);
                        nav.Navigate(PageManager.GetPage(PageId._02_5_카드에러));
                    });
                }
                catch { }
            }
            if (AVRIO.avrio.CardCheckFlag6)
            {
                AVRIO.avrio.CardCheckFlag6 = false;
                WatchDog.Enabled = false;
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        NavigationService nav = NavigationService.GetNavigationService(this);
                        nav.Navigate(PageManager.GetPage(PageId._02_6_카드에러));
                    });
                }
                catch { }
            }
        }



        public void CardCheckSuccess()
        {
            // 뭔가가 하나 추가 되어야 함
            if (AVRIO.avrio.Rapidaspassword == 1)
            {
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._02_7_시작패스워드));
            }
            else
            {
                 AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
            }
        }


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            BackImage.StopAnimate();
            BackImage.Dispose();

            //AVRIO.avrio.StartButton = false;
            //AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            AVRIO.avrio.CardCheckFlag = false;
            AVRIO.avrio.CardCheckFlag4 = false;
            AVRIO.avrio.CardCheckFlag5 = false;
            AVRIO.avrio.CardCheckFlag6 = false;
            
 
            
            
            

            if (ButtonDelayTimer != null)
            {
                ButtonDelayTimer.Enabled = false;
                ButtonDelayTimer.Stop();
                ButtonDelayTimer.Dispose();
                ButtonDelayTimer = null;
            }

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
            catch { }
          //  AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
        }

    }
}
