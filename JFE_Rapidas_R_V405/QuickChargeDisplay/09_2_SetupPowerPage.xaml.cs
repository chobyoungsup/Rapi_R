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
    public partial class SetupPowerPage : Page
    {
        public SetupPowerPage()
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
            AVRIO.avrio.EventMsg = "SetupPowerPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;

            //cbs 배터리 충전제한설정 
            this.label26_Copy.Visibility = Visibility.Collapsed;
            this.txtSocSet.Visibility = Visibility.Collapsed;
            this.Rectime_3_3_Copy.Visibility = Visibility.Collapsed;
            //txtSocSet.Text = AVRIO.avrio.ChargeStartSOC.ToString();


            SetLagnaange();
            //RegistryKey reg;
            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("TIMEWATT");

            RegistryKey regKey;
            RegistryKey reg;
            if (Environment.Is64BitOperatingSystem)
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\TIMEWATT", true);
            }
            else
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\TIMEWATT", true);
            }


            // Admin Password 확인
            time_0_0.Text = Convert.ToString(reg.GetValue("Time00", "22"));
            time_0_1.Text = Convert.ToString(reg.GetValue("Time01", "00"));
            time_0_2.Text = Convert.ToString(reg.GetValue("Time02", "10"));
            time_0_3.Text = Convert.ToString(reg.GetValue("Time03", "00"));
            time_0_4.Text = Convert.ToString(reg.GetValue("Time04", "28"));

            time_1_0.Text = Convert.ToString(reg.GetValue("Time10", "10"));
            time_1_1.Text = Convert.ToString(reg.GetValue("Time11", "00"));
            time_1_2.Text = Convert.ToString(reg.GetValue("Time12", "14"));
            time_1_3.Text = Convert.ToString(reg.GetValue("Time13", "00"));
            time_1_4.Text = Convert.ToString(reg.GetValue("Time14", "20"));

            time_2_0.Text = Convert.ToString(reg.GetValue("Time20", "14"));
            time_2_1.Text = Convert.ToString(reg.GetValue("Time21", "00"));
            time_2_2.Text = Convert.ToString(reg.GetValue("Time22", "18"));
            time_2_3.Text = Convert.ToString(reg.GetValue("Time23", "30"));
            time_2_4.Text = Convert.ToString(reg.GetValue("Time24", "10"));

            time_3_0.Text = Convert.ToString(reg.GetValue("Time30", "18"));
            time_3_1.Text = Convert.ToString(reg.GetValue("Time31", "30"));
            time_3_2.Text = Convert.ToString(reg.GetValue("Time32", "21"));
            time_3_3.Text = Convert.ToString(reg.GetValue("Time33", "30"));
            time_3_4.Text = Convert.ToString(reg.GetValue("Time34", "20"));

            time_4_0.Text = Convert.ToString(reg.GetValue("Time40", "21"));
            time_4_1.Text = Convert.ToString(reg.GetValue("Time41", "30"));
            time_4_2.Text = Convert.ToString(reg.GetValue("Time42", "22"));
            time_4_3.Text = Convert.ToString(reg.GetValue("Time43", "00"));
            time_4_4.Text = Convert.ToString(reg.GetValue("Time44", "20"));


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
            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");

            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);

            bool Gridcheck = Convert.ToBoolean(reg.GetValue("GridOnlyMode", "False"));
            if (Gridcheck)
            {
                AVRIO.avrio.GridOnlyMode = true;
                this.ImgGrid.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
            else
            {
                AVRIO.avrio.GridOnlyMode = false;
                this.ImgGrid.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
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
            // AVRIO.avrio.AdminManuClose = true;
            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
        }
        #endregion


        private void Rectime_0_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_0_0.Text;
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
                            this.time_0_0.Text = "0";
                            this.time_0_0.Text += hour.ToString();

                            this.time_4_2.Text = "0";
                            this.time_4_2.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_0_0.Text = hour.ToString();
                            this.time_4_2.Text = hour.ToString();

                        }
                    }
                }
            }
        }

        private void Rectime_0_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_0_1.Text;
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
                            this.time_4_3.Text = "0";
                            this.time_4_3.Text += min.ToString();

                            this.time_0_1.Text = "0";
                            this.time_0_1.Text += min.ToString();
                        }
                        else
                        {
                            this.time_4_3.Text = min.ToString();
                            this.time_0_1.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_0_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_0_2.Text;
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
                            this.time_0_2.Text = "0";
                            this.time_0_2.Text += hour.ToString();

                            this.time_1_0.Text = "0";
                            this.time_1_0.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_0_2.Text = hour.ToString();
                            this.time_1_0.Text = hour.ToString();

                        }
                    }

                    //
                }
            }
        }

        private void Rectime_0_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_0_3.Text;
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
                            this.time_0_3.Text = "0";
                            this.time_0_3.Text += min.ToString();

                            this.time_1_1.Text = "0";
                            this.time_1_1.Text += min.ToString();
                        }
                        else
                        {
                            this.time_0_3.Text = min.ToString();
                            this.time_1_1.Text = min.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_0_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_0_4.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 28 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.time_0_4.Text = "0";
                            this.time_0_4.Text += ampea.ToString();
                        }
                        else
                            this.time_0_4.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_1_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_1_0.Text;
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
                            this.time_1_0.Text = "0";
                            this.time_1_0.Text += hour.ToString();

                            this.time_0_2.Text = "0";
                            this.time_0_2.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_1_0.Text = hour.ToString();
                            this.time_0_2.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_1_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_1_1.Text;
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
                            this.time_1_1.Text = "0";
                            this.time_1_1.Text += min.ToString();

                            this.time_0_3.Text = "0";
                            this.time_0_3.Text += min.ToString();
                        }
                        else
                        {
                            this.time_1_1.Text = min.ToString();
                            this.time_0_3.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_1_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //   window.InputValue = this.time_1_2.Text;
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
                            this.time_1_2.Text = "0";
                            this.time_1_2.Text += hour.ToString();

                            this.time_2_0.Text = "0";
                            this.time_2_0.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_1_2.Text = hour.ToString();
                            this.time_2_0.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_1_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_1_3.Text;
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
                            this.time_2_1.Text = "0";
                            this.time_2_1.Text += min.ToString();

                            this.time_1_3.Text = "0";
                            this.time_1_3.Text += min.ToString();
                        }
                        else
                        {
                            this.time_1_3.Text = min.ToString();
                            this.time_2_1.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_1_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_1_4.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 28 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.time_1_4.Text = "0";
                            this.time_1_4.Text += ampea.ToString();
                        }
                        else
                            this.time_1_4.Text = ampea.ToString();
                    }
                }
            }
        }

        private void Rectime_2_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_2_0.Text;
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
                            this.time_2_0.Text = "0";
                            this.time_2_0.Text += hour.ToString();

                            this.time_1_2.Text = "0";
                            this.time_1_2.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_2_0.Text = hour.ToString();
                            this.time_1_2.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_2_1.Text;
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
                            this.time_2_1.Text = "0";
                            this.time_2_1.Text += min.ToString();

                            this.time_1_3.Text = "0";
                            this.time_1_3.Text += min.ToString();
                        }
                        else
                        {
                            this.time_2_1.Text = min.ToString();
                            this.time_1_3.Text = min.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_2_2.Text;
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
                            this.time_2_2.Text = "0";
                            this.time_2_2.Text += hour.ToString();

                            this.time_3_0.Text = "0";
                            this.time_3_0.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_2_2.Text = hour.ToString();
                            this.time_3_0.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_2_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_2_3.Text;
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
                            this.time_2_3.Text = "0";
                            this.time_2_3.Text += min.ToString();

                            this.time_3_1.Text = "0";
                            this.time_3_1.Text += min.ToString();
                        }
                        else
                        {
                            this.time_2_3.Text = min.ToString();
                            this.time_3_1.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_2_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_2_4.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 28 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.time_2_4.Text = "0";
                            this.time_2_4.Text += ampea.ToString();
                        }
                        else
                            this.time_2_4.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_3_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //indow.InputValue = this.time_3_0.Text;
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
                            this.time_3_0.Text = "0";
                            this.time_3_0.Text += hour.ToString();

                            this.time_2_2.Text = "0";
                            this.time_2_2.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_3_0.Text = hour.ToString();
                            this.time_2_2.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_3_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //   window.InputValue = this.time_3_1.Text;
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
                            this.time_3_1.Text = "0";
                            this.time_3_1.Text += min.ToString();

                            this.time_2_3.Text = "0";
                            this.time_2_3.Text += min.ToString();
                        }
                        else
                        {
                            this.time_3_1.Text = min.ToString();
                            this.time_2_3.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_3_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_3_2.Text;
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
                            this.time_3_2.Text = "0";
                            this.time_3_2.Text += hour.ToString();

                            this.time_4_0.Text = "0";
                            this.time_4_0.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_3_2.Text = hour.ToString();
                            this.time_4_0.Text = hour.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_3_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_3_3.Text;
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
                            this.time_3_3.Text = "0";
                            this.time_3_3.Text += min.ToString();

                            this.time_4_1.Text = "0";
                            this.time_4_1.Text += min.ToString();
                        }
                        else
                        {
                            this.time_3_3.Text = min.ToString();
                            this.time_4_1.Text = min.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_3_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_3_4.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 28 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.time_3_4.Text = "0";
                            this.time_3_4.Text += ampea.ToString();
                        }
                        else
                            this.time_3_4.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_4_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.time_4_0.Text;
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
                            this.time_4_0.Text = "0";
                            this.time_4_0.Text += hour.ToString();

                            this.time_3_2.Text = "0";
                            this.time_3_2.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_4_0.Text = hour.ToString();
                            this.time_3_2.Text = hour.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_4_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_4_1.Text;
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
                            this.time_4_1.Text = "0";
                            this.time_4_1.Text += min.ToString();

                            this.time_3_3.Text = "0";
                            this.time_3_3.Text += min.ToString();
                        }
                        else
                        {
                            this.time_4_1.Text = min.ToString();
                            this.time_3_3.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_4_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_4_2.Text;
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
                            this.time_4_2.Text = "0";
                            this.time_4_2.Text += hour.ToString();

                            this.time_0_0.Text = "0";
                            this.time_0_0.Text += hour.ToString();
                        }
                        else
                        {
                            this.time_4_2.Text = hour.ToString();
                            this.time_0_0.Text = hour.ToString();

                        }
                    }
                    //
                }
            }
        }

        private void Rectime_4_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_4_3.Text;
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
                            this.time_4_3.Text = "0";
                            this.time_4_3.Text += min.ToString();

                            this.time_0_1.Text = "0";
                            this.time_0_1.Text += min.ToString();
                        }
                        else
                        {
                            this.time_4_3.Text = min.ToString();
                            this.time_0_1.Text = min.ToString();

                        }

                    }
                    //
                }
            }
        }

        private void Rectime_4_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_4_4.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 28 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ampea < 10)
                        {
                            this.time_4_4.Text = "0";
                            this.time_4_4.Text += ampea.ToString();
                        }
                        else
                            this.time_4_4.Text = ampea.ToString();

                    }
                    //
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //RegistryKey reg;
            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("TIMEWATT");

            RegistryKey regKey;
            RegistryKey reg;
            if (Environment.Is64BitOperatingSystem)
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\TIMEWATT", true);
            }
            else
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\TIMEWATT", true);
            }

            AVRIO.avrio.TimeWattTime_0_0 = Convert.ToDouble(time_0_0.Text);
            AVRIO.avrio.TimeWattTime_0_1 = Convert.ToDouble(time_0_1.Text);
            AVRIO.avrio.TimeWattTime_0_2 = Convert.ToDouble(time_0_2.Text);
            AVRIO.avrio.TimeWattTime_0_3 = Convert.ToDouble(time_0_3.Text);
            AVRIO.avrio.TimeWattTime_0_4 = Convert.ToDouble(time_0_4.Text);
            reg.SetValue("Time00", time_0_0.Text);
            reg.SetValue("Time01", time_0_1.Text);
            reg.SetValue("Time02", time_0_2.Text);
            reg.SetValue("Time03", time_0_3.Text);
            reg.SetValue("Time04", time_0_4.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time00", time_0_0.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time01", time_0_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time02", time_0_2.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time03", time_0_3.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time04", time_0_4.Text);

            AVRIO.avrio.TimeWattTime_1_0 = Convert.ToDouble(time_1_0.Text);
            AVRIO.avrio.TimeWattTime_1_1 = Convert.ToDouble(time_1_1.Text);
            AVRIO.avrio.TimeWattTime_1_2 = Convert.ToDouble(time_1_2.Text);
            AVRIO.avrio.TimeWattTime_1_3 = Convert.ToDouble(time_1_3.Text);
            AVRIO.avrio.TimeWattTime_1_4 = Convert.ToDouble(time_1_4.Text);
            reg.SetValue("Time10", time_1_0.Text);
            reg.SetValue("Time11", time_1_1.Text);
            reg.SetValue("Time12", time_1_2.Text);
            reg.SetValue("Time13", time_1_3.Text);
            reg.SetValue("Time14", time_1_4.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time10", time_1_0.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time11", time_1_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time12", time_1_2.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time13", time_1_3.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time14", time_1_4.Text);

            AVRIO.avrio.TimeWattTime_2_0 = Convert.ToDouble(time_2_0.Text);
            AVRIO.avrio.TimeWattTime_2_1 = Convert.ToDouble(time_2_1.Text);
            AVRIO.avrio.TimeWattTime_2_2 = Convert.ToDouble(time_2_2.Text);
            AVRIO.avrio.TimeWattTime_2_3 = Convert.ToDouble(time_2_3.Text);
            AVRIO.avrio.TimeWattTime_2_4 = Convert.ToDouble(time_2_4.Text);
            reg.SetValue("Time20", time_2_0.Text);
            reg.SetValue("Time21", time_2_1.Text);
            reg.SetValue("Time22", time_2_2.Text);
            reg.SetValue("Time23", time_2_3.Text);
            reg.SetValue("Time24", time_2_4.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time20", time_2_0.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time21", time_2_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time22", time_2_2.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time23", time_2_3.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time24", time_2_4.Text);

            AVRIO.avrio.TimeWattTime_3_0 = Convert.ToDouble(time_3_0.Text);
            AVRIO.avrio.TimeWattTime_3_1 = Convert.ToDouble(time_3_1.Text);
            AVRIO.avrio.TimeWattTime_3_2 = Convert.ToDouble(time_3_2.Text);
            AVRIO.avrio.TimeWattTime_3_3 = Convert.ToDouble(time_3_3.Text);
            AVRIO.avrio.TimeWattTime_3_4 = Convert.ToDouble(time_3_4.Text);
            reg.SetValue("Time30", time_3_0.Text);
            reg.SetValue("Time31", time_3_1.Text);
            reg.SetValue("Time32", time_3_2.Text);
            reg.SetValue("Time33", time_3_3.Text);
            reg.SetValue("Time34", time_3_4.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time30", time_3_0.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time31", time_3_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time32", time_3_2.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time33", time_3_3.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time34", time_3_4.Text);

            AVRIO.avrio.TimeWattTime_4_0 = Convert.ToDouble(time_4_0.Text);
            AVRIO.avrio.TimeWattTime_4_1 = Convert.ToDouble(time_4_1.Text);
            AVRIO.avrio.TimeWattTime_4_2 = Convert.ToDouble(time_4_2.Text);
            AVRIO.avrio.TimeWattTime_4_3 = Convert.ToDouble(time_4_3.Text);
            AVRIO.avrio.TimeWattTime_4_4 = Convert.ToDouble(time_4_4.Text);
            reg.SetValue("Time40", time_4_0.Text);
            reg.SetValue("Time41", time_4_1.Text);
            reg.SetValue("Time42", time_4_2.Text);
            reg.SetValue("Time43", time_4_3.Text);
            reg.SetValue("Time44", time_4_4.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time40", time_4_0.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time41", time_4_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time42", time_4_2.Text); QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time43", time_4_3.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Time44", time_4_4.Text);

            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();
        }

        private void ImgGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

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


            if (AVRIO.avrio.GridOnlyMode)
            {
                this.ImgGrid.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                AVRIO.avrio.GridOnlyMode = false;
                reg.SetValue("GridOnlyMode", false);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "GridOnlyMode", "false");
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
            }
            else
            {
                this.ImgGrid.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                AVRIO.avrio.GridOnlyMode = true;
                reg.SetValue("GridOnlyMode", true);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "GridOnlyMode", "true");
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
            }
        }

        //cbs
        private void SetSOC_MouseDown(object sender, MouseButtonEventArgs e)
        {

            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.time_4_2.Text;
            if (window.ShowDialog() == true)
            {
                try
                {
                    byte soc;
                    if (byte.TryParse(window.InputValue, out soc))
                    {
                        txtSocSet.Text = soc.ToString();
                        AVRIO.avrio.ChargeStartSOC = soc;// (byte)(soc - 5); ;
                        AVRIO.avrio.ChargeBattSOCLimit = (byte)(soc + 5);
                        QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "StartSOC", AVRIO.avrio.ChargeStartSOC.ToString());
                       // QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "ChargeBattLimit", AVRIO.avrio.ChargeBattSOCLimit.ToString());

                    }
                }
                catch
                {
                    MessageBox.Show("Err Input");
                }

            }
        }
    }
}
