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
using System.Xml.Linq;
using System.Timers;
using System.Threading;
using Microsoft.Win32;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;


namespace QuickChargeDisplay
{
    /// <summary>
    /// PowerMeterPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetupMomentlPage : Page
    {

        [DllImport("kernel32.dll")]
        private static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        private static extern bool GetSystemTime(ref SYSTEMTIME lpSystemTime);


        // [DllImport("coredll.dll")]
        //private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        //[DllImport("coredll.dll")]
        //private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        //[DllImport("coredll.dll")]
        //private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        //[DllImport("coredll.dll")]
        // private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        // private System.Timers.Timer timer;
        // private UInt16 nSeqNum = 0;

        private struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }


        //private void SetTime(string i_Time)
        //{
        //}

        public SetupMomentlPage()
        {
            InitializeComponent();

           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "SetupMomentlPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            STimeH.Text = System.DateTime.Now.ToString("yyyy");
            textBox1.Text = System.DateTime.Now.ToString("MM");
            textBox2.Text = System.DateTime.Now.ToString("dd");
            textBox3.Text = System.DateTime.Now.ToString("HH");
            textBox4.Text = System.DateTime.Now.ToString("mm");
            textBox5.Text = System.DateTime.Now.ToString("ss");
             Tsbcversion.Text = "V" + string.Format("{0:F2}", (Convert.ToDouble(AVRIO.avrio.nSbcVersion) / 100));
            Tdspversion.Text = AVRIO.avrio.nDspVersion;//)));
            
            //RegistryKey reg;
            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");


            RegistryKey regKey;
            RegistryKey reg;
            if (Environment.Is64BitOperatingSystem)
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
            }
            else
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
            }


            // Admin Password 확인
            TxtCharging.Text = Convert.ToString(reg.GetValue("CHARGETIME", "30"));

           // reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("Admin");
            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\Admin", true);

            // Admin Password 확인
            tSerial.Text = Convert.ToString(reg.GetValue("SerialNumber", "0123456789"));

            if (AVRIO.avrio.RapidasLoginType == 2)
            {
                LogSampling.Visibility = Visibility.Visible;
                EquipmentInfo.Visibility = Visibility.Visible;
                SetupMaxium.Visibility = Visibility.Visible;
                Imgmanu.Visibility = Visibility.Collapsed;
                LogSampling.IsEnabled = true;
                SetupMaxium.IsEnabled = true;
                EquipmentInfo.IsEnabled = true;
            }
            else
            {
                LogSampling.Visibility = Visibility.Collapsed;
                EquipmentInfo.Visibility = Visibility.Collapsed;
                SetupMaxium.Visibility = Visibility.Collapsed;
                Imgmanu.Visibility = Visibility.Visible;
                LogSampling.IsEnabled = false;
                SetupMaxium.IsEnabled = false;
                EquipmentInfo.IsEnabled = false;

            }

            SetLagnaange();

            //AVRIO.avrio.AdminPage = AVRIO.AdminPage.ManualControl;
            //timer = new System.Timers.Timer(1000 * 2);
            //timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.AutoReset = true;
            //timer.Start();
            //nSeqNum = 0;            
            // this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            // this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Ready.Visibility = Visibility.Collapsed; });
        }


        #region 관리자 메뉴 텝

        private void SetLagnaange()
        {
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Langanglab1.Content = "管理設定";
                Langanglab2.Content = "Log抽出";
                Langanglab3.Content = "時間の設定";
                Langanglab4.Content = "言語設定";
                Langanglab5.Content = "充電履歴";
                Langanglab6.Content = "エラー履歴";
                Langanglab7.Content = "パスワード";
                Langanglab8.Content = "受電電力設定";
                Langanglab9.Content = "電流設定";
                Langanglab10.Content = "Manual";
            }
            else
            {
                Langanglab1.Content = "Settings";
                Langanglab2.Content = "Log";
                Langanglab3.Content = "Time";
                Langanglab4.Content = "Language";
                Langanglab5.Content = "Charge";
                Langanglab6.Content = "Error";
                Langanglab7.Content = "Password";
                Langanglab8.Content = "Set Power";
                Langanglab9.Content = "Set current";
                Langanglab10.Content = "Manual";

            }
        }

        private void TroubleDetails_MouseDown(object sender, MouseButtonEventArgs e)
        {// 관리 설정
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.ManualControl;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_1_관리설정));
            }
            catch { }
        }

        private void SetupEquipment_MouseDown(object sender, MouseButtonEventArgs e)
        {// 수전전력 설정
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.Managerment;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_2_수전전력설정));
            }
            catch { }
        }

        private void AdjustUnitPrice_MouseDown(object sender, MouseButtonEventArgs e)
        {// 일시정지
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.Moment;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_3_일시설정));
            }
            catch { }
        }

        private void PowerMeter_MouseDown(object sender, MouseButtonEventArgs e)
        {// 언어 설정
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.Language;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_4_언어설정));
            }
            catch { }
        }

        private void CahrgeHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {//충전 이력
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.ChargeHistory;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_6_충전이력));
            }
            catch { }
        }

        private void TroubleHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {// 에러 이력
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.FaultHistory;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_7_에러이력));
            }
            catch { }
        }

        private void PassWord_MouseDown(object sender, MouseButtonEventArgs e)
        {// 패스워드 변경
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.Password;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_5_패쓰워드));
            }
            catch { }
        }

        private void LogSampling_MouseDown(object sender, MouseButtonEventArgs e)
        {// 로그 추출
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.LogSampling;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_8_Log추출));
            }
            catch { }
        }

        private void SetupMaxium_MouseDown(object sender, MouseButtonEventArgs e)
        {//최대 전류 설정
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.SetupMaxiumCurrent;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_9_최대전력설정));
            }
            catch { }
        }

        private void EquipmentInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {// 강제 충전
            try
            {
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.CompulsionCharge;
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_10_강제충전));
            }
            catch { }
        }

        private void logOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                //AVRIO.avrio.RunMode = AVRIO.avrio.PrevRunMode;

                // ControlHelper.FindAncestor<Window>(this).Close();

                //AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                // AVRIO.avrio.AdminManuClose = true;
                //NavigationService nav = NavigationService.GetNavigationService(this);
                //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));

                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
            }
            catch { }
        }
        #endregion


        private void RecCharging_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            // window.InputValue = this.TxtCharging.Text;

            if (window.ShowDialog() == true)
            {

                int ampea;

                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 255 || ampea < 1)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        AVRIO.avrio.ChargingTimeMax = (Byte)ampea;
                        this.TxtCharging.Text = ampea.ToString();

                        //RegistryKey reg;
                        //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");

                        RegistryKey regKey;
                        RegistryKey reg;
                        if (Environment.Is64BitOperatingSystem)
                        {
                            regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
                        }
                        else
                        {
                            regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
                        }



                        reg.SetValue("CHARGETIME", AVRIO.avrio.ChargingTimeMax);
                        QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CHARGETIME", AVRIO.avrio.ChargingTimeMax.ToString());
                    }
                    //
                }
            }

        }

        private void BusinessTimeHHRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.STimeH.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 2100 || ampea < 2012)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        this.STimeH.Text = ampea.ToString();
                    //
                }
            }
        }

        private void rectangle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.textBox1.Text;
            if (window.ShowDialog() == true)
            {
                int hour;
                if (int.TryParse(window.InputValue, out hour))
                {
                    if (hour > 12 || hour < 1)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (hour < 10)
                        {
                            this.textBox1.Text = "0";
                            this.textBox1.Text += hour.ToString();
                        }
                        else
                            this.textBox1.Text = hour.ToString();
                    }//
                }
            }
        }

        private void rectangle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.textBox2.Text;
            if (window.ShowDialog() == true)
            {
                int hour;
                if (int.TryParse(window.InputValue, out hour))
                {
                    if (hour > 31 || hour < 1)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (hour < 10)
                        {
                            this.textBox2.Text = "0";
                            this.textBox2.Text += hour.ToString();
                        }
                        else
                            this.textBox2.Text = hour.ToString();
                    }
                    //
                }
            }
        }

        private void rectangle3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.textBox3.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 23 || ampea < 0)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.textBox3.Text = "0";
                            this.textBox3.Text += ampea.ToString();

                        }
                        else
                            this.textBox3.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void rectangle4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.textBox4.Text;
            if (window.ShowDialog() == true)
            {
                int min;
                if (int.TryParse(window.InputValue, out min))
                {

                    if (min > 59 || min < 0)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (min < 10)
                        {
                            this.textBox4.Text = "0";
                            this.textBox4.Text += min.ToString();

                        }
                        else
                            this.textBox4.Text = min.ToString();
                    }
                    //
                }
            }
        }

        private void rectangle5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.textBox5.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 59 || ampea < 0)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.textBox5.Text = "0";
                            this.textBox5.Text += ampea.ToString();

                        }
                        else this.textBox5.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {


            SYSTEMTIME systime = new SYSTEMTIME();
            CultureInfo enUS = new CultureInfo("en-US");
            string strTime = DateTime.ParseExact((STimeH.Text + textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text + textBox5.Text), "yyyyMMddHHmmss", enUS).AddHours(-9).ToString("yyyyMMddHHmmss");
            systime.wYear = (short)Convert.ToInt16(strTime.Substring(0, 4));
            systime.wMonth = (short)Convert.ToInt16(strTime.Substring(4, 2));
            systime.wDay = (short)Convert.ToInt16(strTime.Substring(6, 2));
            systime.wHour = (short)Convert.ToInt16(strTime.Substring(8, 2));
            systime.wMinute = (short)Convert.ToInt16(strTime.Substring(10, 2));
            systime.wSecond = (short)Convert.ToInt16(strTime.Substring(12, 2));
            SetSystemTime(ref systime);



            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();

        }



    }
}
