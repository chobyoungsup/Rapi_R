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
using Microsoft.Win32;

namespace QuickChargeDisplay
{
    /// <summary>
    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetupPasswordPage : Page
    {
        public SetupPasswordPage()
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
            AVRIO.avrio.EventMsg = "SetupPasswordPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            if (AVRIO.avrio.RapidasLoginType == 2)
            {

                curPwmanual.Visibility = Visibility.Visible;
                newPwmanual.Visibility = Visibility.Visible;
                confirmPwmanual.Visibility = Visibility.Visible;
                Lab1.Visibility = Visibility.Visible;
                Lab2.Visibility = Visibility.Visible;
                Lab3.Visibility = Visibility.Visible;
                rectCurPwmanual.Visibility = Visibility.Visible;
                rectNewPwmanual.Visibility = Visibility.Visible;
                rectConfirmPwmanual.Visibility = Visibility.Visible;
                btnUpdatePassword_Copy.Visibility = Visibility.Visible;
                btnUpdatePassword_CopyReset.Visibility = Visibility.Visible;

                LogSampling.Visibility = Visibility.Visible;
                EquipmentInfo.Visibility = Visibility.Visible;
                SetupMaxium.Visibility = Visibility.Visible;
                //ImgPassword.Visibility = Visibility.Visible;
                Imgmanu.Visibility = Visibility.Collapsed;
                LogSampling.IsEnabled = true;
                SetupMaxium.IsEnabled = true;
                EquipmentInfo.IsEnabled = true;
            }
            else
            {
                curPwmanual.Visibility = Visibility.Collapsed;
                newPwmanual.Visibility = Visibility.Collapsed;
                confirmPwmanual.Visibility = Visibility.Collapsed;
                Lab1.Visibility = Visibility.Collapsed;
                Lab2.Visibility = Visibility.Collapsed;
                Lab3.Visibility = Visibility.Collapsed;
                rectCurPwmanual.Visibility = Visibility.Collapsed;
                rectNewPwmanual.Visibility = Visibility.Collapsed;
                rectConfirmPwmanual.Visibility = Visibility.Collapsed;
                btnUpdatePassword_Copy.Visibility = Visibility.Collapsed;
                btnUpdatePassword_CopyReset.Visibility = Visibility.Collapsed;

                //  ImgPassword.Visibility = Visibility.Collapsed;
                LogSampling.Visibility = Visibility.Collapsed;
                EquipmentInfo.Visibility = Visibility.Collapsed;
                SetupMaxium.Visibility = Visibility.Collapsed;
                Imgmanu.Visibility = Visibility.Visible;
                LogSampling.IsEnabled = false;
                SetupMaxium.IsEnabled = false;
                EquipmentInfo.IsEnabled = false;

            }
            newPwmanual.Password = "";
            curPwmanual.Password = "";
            confirmPwmanual.Password = "";

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
            //AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
            //AVRIO.avrio.RunMode = AVRIO.avrio.PrevRunMode;

            // ControlHelper.FindAncestor<Window>(this).Close();

            //AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
            //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            //  AVRIO.avrio.AdminManuClose = true;
            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
        }
        #endregion




        private void rectCurPwadmin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.curPw.Password;
            if (window.ShowDialog() == true)
            {
                curPw.Password = window.InputValue;
            }
        }

        private void rectNewPwadmin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.newPw.Password;
            if (window.ShowDialog() == true)
            {
                newPw.Password = window.InputValue;
            }
        }

        private void rectConfirmPwadmin_MouseDown(object sender, MouseButtonEventArgs e)
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
            string password = Convert.ToString(reg.GetValue("ADMINPASS", "1234"));

            if (password == curPw.Password)
            {
                if (newPw.Password == confirmPw.Password && (newPw.Password.Length == 4) && (confirmPw.Password.Length == 4))
                {
                    reg.SetValue("ADMINPASS", confirmPw.Password);

                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "ADMINPASS", confirmPw.Password);
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

        private void rectCurPwmanual_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.curPwmanual.Password;
            if (window.ShowDialog() == true)
            {
                curPwmanual.Password = window.InputValue;
            }
        }

        private void rectNewPwmanual_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.newPwmanual.Password;
            if (window.ShowDialog() == true)
            {
                newPwmanual.Password = window.InputValue;
            }
        }

        private void rectConfirmPwmanual_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.confirmPwmanual.Password;
            if (window.ShowDialog() == true)
            {
                confirmPwmanual.Password = window.InputValue;
            }
        }

        private void btnUpdatePassword_Copy_Click(object sender, RoutedEventArgs e)
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
            string password = Convert.ToString(reg.GetValue("MANUPASS", "8611"));

            if (password == curPwmanual.Password)
            {
                if (newPwmanual.Password == confirmPwmanual.Password && (newPwmanual.Password.Length == 4) && (confirmPwmanual.Password.Length == 4))
                {
                    reg.SetValue("MANUPASS", confirmPwmanual.Password);

                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "MANUPASS", confirmPwmanual.Password);
                    OkDialog dlg = new OkDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();
                    newPwmanual.Password = "";
                    curPwmanual.Password = "";
                    confirmPwmanual.Password = "";
                }
                else
                {
                    PasswordFaultDialog dlg = new PasswordFaultDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();
                    newPwmanual.Password = "";
                    curPwmanual.Password = "";
                    confirmPwmanual.Password = "";

                }

            }
            else
            {
                PasswordFaultDialog dlg = new PasswordFaultDialog();
                dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                dlg.ShowDialog();
                newPwmanual.Password = "";
                curPwmanual.Password = "";
                confirmPwmanual.Password = "";

            }
        }

        private void btnUpdateReset_Click(object sender, RoutedEventArgs e)
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

            reg.SetValue("ADMINPASS", "1234");
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "ADMINPASS", "1234");

            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();
            newPw.Password = "";
            curPw.Password = "";
            confirmPw.Password = "";
        }

        private void btnUpdatePassword_CopyReset_Click(object sender, RoutedEventArgs e)
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

            reg.SetValue("MANUPASS", "8611");
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "MANUPASS", "8611");

            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();
            newPwmanual.Password = "";
            curPwmanual.Password = "";
            confirmPwmanual.Password = "";
        }

    }
}
