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
    /// ChargingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChargingPage : Page
    {
        int Faultnum = 0;
        private System.Timers.Timer WatchDog;
        private System.Timers.Timer bmsTimer = null;
        public static DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(ChargingPage),
                                                                                           new PropertyMetadata(0.0, new PropertyChangedCallback(OnBatteryChanged)));
        int one = 0;
        public static DependencyProperty SocPercentProperty = DependencyProperty.Register("SocPercent", typeof(double), typeof(ChargingPage),
                                                                                            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSocPercentChanged)));
        private static void OnBatteryChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {         

            try
            {   
                ChargingPage page = sender as ChargingPage;
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
            catch { }

        }

        private static void OnSocPercentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ChargingPage page = sender as ChargingPage;

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
            set { SetValue(SocPercentProperty, value); }        }

        private DoubleAnimation batteryAnimation;

        public ChargingPage()
        {
            InitializeComponent();

           

            QCDV.BmsInfo.Visible = Visibility.Hidden;
        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.FualtstopFalg || Faultnum != 0)
            {
                if (AVRIO.avrio.ChargeSOC >= 100) AVRIO.avrio.ChargeSOC = 100;
                if (AVRIO.avrio.ChargeRemainTime >= 120) AVRIO.avrio.ChargeRemainTime = 120;

                Faultnum++;
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                        chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));                        
                        Txttimemm.Content = "0";
                        SetPower.Content = "0";
                    });

                    if (AVRIO.avrio.StartButton)
                    {

                    }

                    if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
                    {

                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                    }
                }
                catch { }

            }else
            try
            {
                if (AVRIO.avrio.ChargeSOC >= 100) AVRIO.avrio.ChargeSOC = 100;
                if (AVRIO.avrio.ChargeRemainTime >= 120) AVRIO.avrio.ChargeRemainTime = 120;


                this.Dispatcher.Invoke((ThreadStart)delegate()
                {
                    Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                    chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));
                    if (AVRIO.avrio.ChargeRemainTime == 0)
                        Txttimemm.Content = "0";
                    else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTime);
                    Double tempSoc = Convert.ToDouble(AVRIO.avrio.OutAmpare);
                    SetPower.Content = Convert.ToString(Math.Floor((tempSoc * 100) / 100));
                });

                if (AVRIO.avrio.StartButton)
                {

                }

                if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
                {

                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                }
            }
            catch { }

            one++;
            if (one >= 3 && one <= 5)
            {
                one = 6;
                AVRIO.avrio.StartchargeSOC = AVRIO.avrio.ChargeSOC;
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ChargingPage_Loaded ";


            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            //cbs
            QCDV.BmsInfo.Visible = Visibility.Hidden;

            Faultnum = 0;
            AVRIO.avrio.nSelectCommand12 = 3;
            AVRIO.avrio.StartchargeSOC = 0;
            AVRIO.avrio.StopchargeSOC = 0;
           // AVRIO.avrio.StartButton = false;
           // AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            one = 0;
            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeStart; 
            

            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                if (AVRIO.avrio.ChargeRemainTime == 0)
                    Txttimemm.Content = "0";
                else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTime);


                Double tempSoc = Convert.ToDouble(AVRIO.avrio.OutAmpare);
                SetPower.Content = Convert.ToString(Math.Floor((tempSoc * 100) / 100));
            }); 

           // AVRIO.avrio.PageStanByStop = true;

            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/05/충전중11.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/11.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            BackImage.StartAnimate();

            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

            //cbs
          //  QCDV.BmsInfo.Visible = Visibility.Visible;

             this.SetAnimation(this.SocPercent, 110);

            //cbs 주석처리
             //bmsTimer = new System.Timers.Timer(1000); // 1 sec
             //bmsTimer.Elapsed += new System.Timers.ElapsedEventHandler(bmsTimer_Elapsed);
             //bmsTimer.AutoReset = false;
             //bmsTimer.Start();

        }

        private void SetAnimation(double from, double to)
        {
            try
            {
                int count = (int)(to - from) / 10;

                batteryAnimation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromMilliseconds(count * 500)));
                batteryAnimation.RepeatBehavior = RepeatBehavior.Forever;
                this.BeginAnimation(ChargingPage.BatteryProperty, batteryAnimation);
            }
            catch { }
        }

      
        void bmsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (bmsTimer != null)
            {
                bmsTimer.Stop();
                bmsTimer = null;
            }

            //cbs 주석처리
            if (QCDV.BmsInfo.Visible == Visibility.Visible)
            {
                QCDV.BmsInfo.Visible = Visibility.Hidden;
            }
            else
            {
                QCDV.BmsInfo.Visible = Visibility.Visible;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (bmsTimer != null)
            {
                bmsTimer.Stop();
                bmsTimer = null;
            }
        }
        
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Faultnum = 0;
            this.Dispatcher.Invoke((ThreadStart)delegate()
            {

                Txttimemm.Content = "0";
                SetPower.Content = "0";

            }); 

            one = 0;
            //AVRIO.avrio.StopchargeSOC = AVRIO.avrio.ChargeSOC;
            
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

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


            if (WatchDog != null)
            {
                WatchDog.Enabled = false;
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;

            }
                
            AVRIO.avrio.StopchargeSOC = AVRIO.avrio.ChargeSOC;
            //AVRIO.avrio.StopchargeSOC = AVRIO.avrio.ChargeSOC;
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //if (AVRIO.avrio.DisChargeFalg)
            //{
            //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBMSBatteryDischargeVehicleFinish;
            //}
            //else AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

        }

        private void ChargeDevice_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            bmsTimer = new System.Timers.Timer(1000); // 2 sec
            bmsTimer.Elapsed += new System.Timers.ElapsedEventHandler(bmsTimer_Elapsed);
            bmsTimer.Start();
        }

        private void ChargeDevice_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (bmsTimer != null)
            {
                bmsTimer.Stop();
                bmsTimer = null;
            }
        }
    }
}
