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
using System.Drawing;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Resources;

namespace QuickChargeDisplay
{
    /// <summary>
    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagementPage : Page
    {
        public ManagementPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ManagementPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;


            //  AVRIO.avrio.AdminPage = AVRIO.AdminPage.DeviceSetting;
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
            int Cardcheck = Convert.ToInt32(reg.GetValue("CARDCHECK", "0"));
            int Passcheck = Convert.ToInt32(reg.GetValue("PASSWORD", "0"));
            int BusinessCheck = Convert.ToInt32(reg.GetValue("BusinessCheck", "0"));

            AVRIO.avrio.BusinessStimeHH = Convert.ToInt32(reg.GetValue("BusinessStimeHHH", "09"));
            AVRIO.avrio.BusinessStimeMM = Convert.ToInt32(reg.GetValue("BusinessStimeMMM", "00"));
            AVRIO.avrio.BusinessEtimeHH = Convert.ToInt32(reg.GetValue("BusinessEtimeHHH", "24"));
            AVRIO.avrio.BusinessEtimeMM = Convert.ToInt32(reg.GetValue("BusinessEtimeMMM", "00"));

            if (BusinessCheck == 0)
            {
                this.BusinessCheckchk.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.BusinessCheckbar = 0;
                STimeH.IsEnabled = false;
                STimeM.IsEnabled = false;
                ETimeH.IsEnabled = false;
                ETimeM.IsEnabled = false;
            }
            else
            {
                this.BusinessCheckchk.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.BusinessCheckbar = 1;
                STimeH.IsEnabled = true;
                STimeM.IsEnabled = true;
                ETimeH.IsEnabled = true;
                ETimeM.IsEnabled = true;
            }

            if (Cardcheck == 0)
            {
                this.ImgCardCheck.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.Rapidascardcheck = 0;
            }
            else
            {
                this.ImgCardCheck.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.Rapidascardcheck = 1;
            }

            if (Passcheck == 0)
            {
                AVRIO.avrio.Rapidaspassword = 0;
                this.ImgPassWordCheck.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                AVRIO.avrio.Rapidaspassword = 1;
                this.ImgPassWordCheck.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }

            if (AVRIO.avrio.BusinessStimeHH < 10)
            {
                STimeH.Text = "0";
                STimeH.Text += Convert.ToString(AVRIO.avrio.BusinessStimeHH);
            }
            else STimeH.Text = Convert.ToString(AVRIO.avrio.BusinessStimeHH);

            if (AVRIO.avrio.BusinessStimeMM < 10)
            {
                STimeM.Text = "0";
                STimeM.Text += Convert.ToString(AVRIO.avrio.BusinessStimeMM);
            }
            else STimeM.Text = Convert.ToString(AVRIO.avrio.BusinessStimeMM);

            if (AVRIO.avrio.BusinessEtimeHH < 10)
            {
                ETimeH.Text = "0";
                ETimeH.Text += Convert.ToString(AVRIO.avrio.BusinessEtimeHH);
            }
            else ETimeH.Text = Convert.ToString(AVRIO.avrio.BusinessEtimeHH);

            if (AVRIO.avrio.BusinessEtimeMM < 10)
            {
                ETimeM.Text = "0";
                ETimeM.Text += Convert.ToString(AVRIO.avrio.BusinessEtimeMM);
            }
            else ETimeM.Text = Convert.ToString(AVRIO.avrio.BusinessEtimeMM);

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
            newPw.Password = "";
            curPw.Password = "";
            confirmPw.Password = "";
            SetLagnaange();
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



                //AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                //NavigationService nav = NavigationService.GetNavigationService(this);
                //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));


                // AVRIO.avrio.AdminManuClose = true;
                // ControlHelper.FindAncestor<Window>(this).Close();

                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));

            }
            catch { }
        }
        #endregion



        private void rectConfirmPw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.confirmPw.Password;
            if (window.ShowDialog() == true)
            {
                confirmPw.Password = window.InputValue;
            }
        }

        private void btnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
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

            if (password == curPw.Password)
            {
                if (newPw.Password == confirmPw.Password && (newPw.Password.Length == 4) && (confirmPw.Password.Length == 4))
                {
                    reg.SetValue("CHARGINGPASS", confirmPw.Password);
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CHARGINGPASS", confirmPw.Password);
                    OkDialog dlg = new OkDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();
                    newPw.Password = "";
                    curPw.Password = "";
                    confirmPw.Password = "";

                }
                else
                {
                    PasswordFaultDialog dlg = new PasswordFaultDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();
                    newPw.Password = "";
                    curPw.Password = "";
                    confirmPw.Password = "";

                }

            }
            else
            {
                PasswordFaultDialog dlg = new PasswordFaultDialog();
                dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                dlg.ShowDialog();
                newPw.Password = "";
                curPw.Password = "";
                confirmPw.Password = "";
            }
        }

        private void rectNewPw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.newPw.Password;
            if (window.ShowDialog() == true)
            {
                newPw.Password = window.InputValue;
            }
        }

        private void rectCurPw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.curPw.Password;
            if (window.ShowDialog() == true)
            {
                curPw.Password = window.InputValue;
            }
        }


        private void BusinessTimeHHRect_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);
                //  window.InputValue = this.STimeH.Text;
                if (window.ShowDialog() == true)
                {
                    int hour;
                    if (int.TryParse(window.InputValue, out hour))
                    {
                        if (hour > 24 || hour < 1)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (hour < 10)
                            {
                                this.STimeH.Text = "0";
                                this.STimeH.Text += hour.ToString();
                            }
                            else
                                this.STimeH.Text = hour.ToString();
                        }
                    }
                }
            }
        }

        private void BusinessTimeMMRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);
                //  window.InputValue = this.STimeM.Text;
                if (window.ShowDialog() == true)
                {
                    int min;
                    if (int.TryParse(window.InputValue, out min))
                    {
                        if (min > 59)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (min < 10)
                            {
                                this.STimeM.Text = "0";
                                this.STimeM.Text += min.ToString();
                            }
                            else
                                this.STimeM.Text = min.ToString();
                        }
                    }
                }
            }
        }

        private void BusinessTimeSSERect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);
                //  window.InputValue = this.ETimeH.Text;
                if (window.ShowDialog() == true)
                {
                    int hour;
                    if (int.TryParse(window.InputValue, out hour))
                    {

                        if (hour > 24 || hour < 1)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (hour < 10)
                            {
                                this.ETimeH.Text = "0";
                                this.ETimeH.Text += hour.ToString();
                            }
                            else
                                this.ETimeH.Text = hour.ToString();
                        }
                    }
                }
            }
        }

        private void BusinessTimeMMERect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);
                //  window.InputValue = this.ETimeM.Text;
                if (window.ShowDialog() == true)
                {
                    int min;
                    if (int.TryParse(window.InputValue, out min))
                    {

                        if (min > 59)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (min < 10)
                            {
                                this.ETimeM.Text = "0";
                                this.ETimeM.Text += min.ToString();
                            }
                            else
                                this.ETimeM.Text = min.ToString();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.BusinessStimeHH = Convert.ToInt32(STimeH.Text);
            AVRIO.avrio.BusinessStimeMM = Convert.ToInt32(STimeM.Text);
            AVRIO.avrio.BusinessEtimeHH = Convert.ToInt32(ETimeH.Text);
            AVRIO.avrio.BusinessEtimeMM = Convert.ToInt32(ETimeM.Text);

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


            reg.SetValue("BusinessStimeHHH", AVRIO.avrio.BusinessStimeHH);
            reg.SetValue("BusinessStimeMMM", AVRIO.avrio.BusinessStimeMM);
            reg.SetValue("BusinessEtimeHHH", AVRIO.avrio.BusinessEtimeHH);
            reg.SetValue("BusinessEtimeMMM", AVRIO.avrio.BusinessEtimeMM);

            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessStimeHHH", AVRIO.avrio.BusinessStimeHH.ToString());
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessStimeMMM", AVRIO.avrio.BusinessStimeMM.ToString());
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessEtimeHHH", AVRIO.avrio.BusinessEtimeHH.ToString());
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessEtimeMMM", AVRIO.avrio.BusinessEtimeMM.ToString());

            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();

        }

        private void ImgCardCheck_MouseDown(object sender, MouseButtonEventArgs e)
        {

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

            if (AVRIO.avrio.Rapidascardcheck == 0)
            {
                this.ImgCardCheck.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.Rapidascardcheck = 1;
                reg.SetValue("CARDCHECK", 1);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CARDCHECK", "1");
            }
            else
            {
                this.ImgCardCheck.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.Rapidascardcheck = 0;
                reg.SetValue("CARDCHECK", 0);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CARDCHECK", "0");
            }
        }

        private void ImgPassWordCheck_MouseDown(object sender, MouseButtonEventArgs e)
        {

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

            if (AVRIO.avrio.Rapidaspassword == 0)
            {
                this.ImgPassWordCheck.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.Rapidaspassword = 1;
                reg.SetValue("PASSWORD", 1);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "PASSWORD", "1");
            }
            else
            {
                this.ImgPassWordCheck.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.Rapidaspassword = 0;
                reg.SetValue("PASSWORD", 0);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "PASSWORD", "0");
            }
        }

        private void ImgGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BusinessCheckchk_MouseDown(object sender, MouseButtonEventArgs e)
        {
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


            if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                this.BusinessCheckchk.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.BusinessCheckbar = 0;
                reg.SetValue("BusinessCheck", 0);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessCheck", "0");
                STimeH.IsEnabled = false;
                STimeM.IsEnabled = false;
                ETimeH.IsEnabled = false;
                ETimeM.IsEnabled = false;
            }
            else
            {
                this.BusinessCheckchk.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.BusinessCheckbar = 1;
                reg.SetValue("BusinessCheck", 1);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "BusinessCheck", "1");
                STimeH.IsEnabled = true;
                STimeM.IsEnabled = true;
                ETimeH.IsEnabled = true;
                ETimeM.IsEnabled = true;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
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

            reg.SetValue("CHARGINGPASS", "0000");

            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CHARGINGPASS", "0000");
            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();

            newPw.Password = "";
            curPw.Password = "";
            confirmPw.Password = "";

        }



    }
}
