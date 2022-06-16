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
using System.Windows.Media.Animation;

namespace QuickChargeDisplay
{
    /// <summary>
    /// IdCardPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChargeStopPage : Page
    {
        private System.Timers.Timer WatchDog;
        private System.Timers.Timer bmsTimer = null;
        public static DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(ChargeStopPage),
                                                                                           new PropertyMetadata(0.0, new PropertyChangedCallback(OnBatteryChanged)));

        public static DependencyProperty SocPercentProperty = DependencyProperty.Register("SocPercent", typeof(double), typeof(ChargeStopPage),
                                                                                            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSocPercentChanged)));
        private static void OnBatteryChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ChargeStopPage page = sender as ChargeStopPage;

            double value = (double)e.NewValue;

            if (value >= 10) page.p10.Visibility = Visibility.Visible; else page.p10.Visibility = Visibility.Collapsed;
            if (value > 20) page.p20.Visibility = Visibility.Visible; else page.p20.Visibility = Visibility.Collapsed;
            if (value > 30) page.p30.Visibility = Visibility.Visible; else page.p30.Visibility = Visibility.Collapsed;
            if (value > 40) page.p40.Visibility = Visibility.Visible; else page.p40.Visibility = Visibility.Collapsed;
            if (value > 50) page.p50.Visibility = Visibility.Visible; else page.p50.Visibility = Visibility.Collapsed;
            if (value > 60) page.p60.Visibility = Visibility.Visible; else page.p60.Visibility = Visibility.Collapsed;
            if (value > 70) page.p70.Visibility = Visibility.Visible; else page.p70.Visibility = Visibility.Collapsed;
            if (value > 80) page.p80.Visibility = Visibility.Visible; else page.p80.Visibility = Visibility.Collapsed;
            if (value > 90) page.p90.Visibility = Visibility.Visible; else page.p90.Visibility = Visibility.Collapsed;
            if (value > 100) page.p100.Visibility = Visibility.Visible; else page.p100.Visibility = Visibility.Collapsed;
        }

        private static void OnSocPercentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ChargeStopPage page = sender as ChargeStopPage;

            double value = (double)e.NewValue;

            if (value > 100)
                value = 100;

            if (value < 0)
                value = 0;

            page.SetAnimation(value, 110);
        }

        public double Battery
        {
            get { return (double)GetValue(BatteryProperty); }
            set { SetValue(BatteryProperty, value); }
        }

        public double SocPercent
        {
            get { return (double)GetValue(SocPercentProperty); }
            set { SetValue(SocPercentProperty, value); }
        }

        private DoubleAnimation batteryAnimation;


        public ChargeStopPage()
        {
            InitializeComponent();

           

            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
               
                Txttimemm1.Content = "0";
                SetPower1.Content = "0";
            });
            QCDV.BmsInfo.Visible = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ChargeStopPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));
                //AVRIO.avrio.ChargeRemainTime
                // if (AVRIO.avrio.ChargeRemainTimemm == 0)
                Txttimemm1.Content = "0";
                // else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTimemm);
                SetPower1.Content = "0";
            });  

            AVRIO.avrio.nSelectCommand12 = 2;
            AVRIO.avrio.nSelectCommand34 = 2;
            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeCancel;
            


            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/05/충전중12.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/12.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }

            BackImage.StartAnimate();

            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
               
                Txttimemm1.Content = "0";
                SetPower1.Content = "0";

            }); 
            QCDV.BmsInfo.Visible = Visibility.Visible;

            this.SetAnimation(this.SocPercent, 110);
            bmsTimer = new System.Timers.Timer(1000); // 1 sec
            bmsTimer.Elapsed += new System.Timers.ElapsedEventHandler(bmsTimer_Elapsed);
            bmsTimer.AutoReset = false;
            bmsTimer.Start();
            this.Dispatcher.Invoke((ThreadStart)delegate()
            {

                Txttimemm1.Content = "0";
                SetPower1.Content = "0";

            }); 
            //this.Dispatcher.Invoke((ThreadStart)delegate()
            //{
            //    if (AVRIO.avrio.ChargeRemainTimemm == 0)
            //        Txttimemm.Content = "0";
            //    else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTimemm);
            //    Double tempSoc = Convert.ToDouble(AVRIO.avrio.OutAmpare);
            //    SetPower.Content = Convert.ToString(Math.Floor((tempSoc * 100) / 100));
            //});  
        }



        private void SetAnimation(double from, double to)
        {
            this.Dispatcher.Invoke((ThreadStart)delegate()
            {

                Txttimemm1.Content = "0";
                SetPower1.Content = "0";

            }); 

            int count = (int)(to - from) / 10;

            batteryAnimation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromMilliseconds(count * 500)));
            batteryAnimation.RepeatBehavior = RepeatBehavior.Forever;
            this.BeginAnimation(ChargingPage.BatteryProperty, batteryAnimation);
        }


        void bmsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                 Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                 chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));

                 Txttimemm1.Content = "0";              
                 SetPower1.Content = "0";

            });          
                 

            if (bmsTimer != null)
            {
                bmsTimer.Stop();
                bmsTimer = null;
            }

            if (QCDV.BmsInfo.Visible == Visibility.Visible)
            {
                QCDV.BmsInfo.Visible = Visibility.Hidden;
            }
            else
            {
                QCDV.BmsInfo.Visible = Visibility.Visible;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            BackImage.StopAnimate();
            BackImage.Dispose();

            QCDV.BmsInfo.Visible = Visibility.Hidden;

            if (bmsTimer != null)
            {
                bmsTimer.Enabled = false;
                bmsTimer.Stop();
                bmsTimer.Dispose();
                bmsTimer = null;
            }
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
        }
    }
}
