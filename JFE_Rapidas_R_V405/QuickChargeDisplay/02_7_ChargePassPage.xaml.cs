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
using System.IO;
using System.Media;
using System.Timers;
using System.Drawing;
using System.Windows.Resources;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using Microsoft.Win32;


namespace QuickChargeDisplay
{
    /// <summary>
    /// ChargeMoneyPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChargePassPage : Page
    {
        private System.Timers.Timer timer12;
        private System.Timers.Timer WatchDog;
        public string InputValue = "";

        public ChargePassPage()
        {
            InitializeComponent();

            // if (AVRIO.avrio.Rapidaslanguage == 2)
            //{
            //    Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/07_E.bmp");

            //    Bitmap bitmap = new Bitmap(stream);

            //    BackImage.LoadSmile(bitmap);
            //    BackImage.StopAnimate();
            //    stream.Dispose();
            //}
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ChargePassPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            // AVRIO.avrio.StopButton = false;            
            AVRIO.avrio.nSelectCommand12 = 5;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            if (AVRIO.avrio.Rapidaslanguage == 0)
            {//
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/03/03_패스워드_베이스.png");
                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {//영어임
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/03_패스워드_베이스.png");

                Bitmap bitmap = new Bitmap(stream);

                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();

            }
            BackImage.StartAnimate();

            timer12 = new System.Timers.Timer(1000 * 60);
            timer12.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer12.AutoReset = false;
            timer12.Start();

            WatchDog = new System.Timers.Timer(200);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            BackImage.StopAnimate();
            BackImage.Dispose();
            
            ValueTxt.Text = "";

            if (WatchDog != null)
            {
                WatchDog.Enabled = false;
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;
            }                

            if (timer12 != null)
            {
                timer12.Enabled = false;
                timer12.Stop();
                timer12.Dispose();
                timer12 = null;
            }
                
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
        }
        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            //// if (AVRIO.avrio.StartButton || AVRIO.avrio.StartButtonPlay)
            // {
            // AVRIO.avrio.StopButton = false;
            // AVRIO.avrio.StartButton = false;
            // }

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
                // AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;

                // AVRIO.avrio.StopButton = false;
                // AVRIO.avrio.StartButton = false;
                AVRIO.avrio.StopButtonPlay = false;
                AVRIO.avrio.StartButtonPlay = false;
            }

        }
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
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

        private void img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (InputValue.Length >= 4)
            {

            }
            else
                InputValue = ValueTxt.Text += "1";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 1 : " + InputValue;
        }

        private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "2";
            AVRIO.avrio.EventMsg = " ChargePassPage Touch 2 : " + InputValue;
        }

        private void img3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "3";
            AVRIO.avrio.EventMsg = " ChargePassPage Touch 3 : " + InputValue;
        }

        private void img4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "4";
            AVRIO.avrio.EventMsg = " ChargePassPage Touch 4 : " + InputValue;
        }

        private void img5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "5";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 5 : " + InputValue;
        }

        private void img6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "6";
            AVRIO.avrio.EventMsg = " ChargePassPage Touch 6 : " + InputValue;
        }

        private void img7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "7";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 7 : " + InputValue;
        }

        private void img8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "8";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 8 : " + InputValue;
        }

        private void img9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "9";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 9 : " + InputValue;
        }

        private void img0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else InputValue = ValueTxt.Text += "0";

            AVRIO.avrio.EventMsg = " ChargePassPage Touch 0 : " + InputValue;
        }

        private void imgcl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue = ValueTxt.Text = "";
            //if (InputValue.Length > 0)
            //    InputValue = InputValue.Remove(InputValue.Length - 1, 1);
        }

        private void imgOK_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 5 || InputValue.Length <= 3)
            {

            }
            else
            {
                //비밀번호 체크
                //RegistryKey reg;
                //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("Admin");
                RegistryKey regKey;
                RegistryKey reg;
                if (Environment.Is64BitOperatingSystem)
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\Admin", true);
                }
                else
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\Admin", true);
                }
                
                // Admin Password 확인
                string password = Convert.ToString(reg.GetValue("CHARGINGPASS", "1234"));

                if (password == ValueTxt.Text)
                {
                    AVRIO.avrio.EventMsg = " ChargePassPage PassWordCheck OK   AVRIO.TsCommand.TsVehicleReady";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                }
            }
            InputValue = ValueTxt.Text = "";

        }

        private void imgBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue = ValueTxt.Text;
            if (InputValue.Length > 0)
            {
                InputValue = InputValue.Remove(InputValue.Length - 1, 1);
                ValueTxt.Text = InputValue;
            }
        }

    }
}
