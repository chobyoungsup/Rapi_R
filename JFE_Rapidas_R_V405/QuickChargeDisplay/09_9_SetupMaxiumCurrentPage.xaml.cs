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
    public partial class SetupMaxiumCurrentPage : Page
    {
        public SetupMaxiumCurrentPage()
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
            AVRIO.avrio.EventMsg = "SetupMaxiumCurrentPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;


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
            TxtSoc_0_0.Text = Convert.ToString(reg.GetValue("Soc00", "125"));
            TxtSoc_0_1.Text = Convert.ToString(reg.GetValue("Soc01", "100"));
            TxtSoc_0_2.Text = Convert.ToString(reg.GetValue("Soc02", "60"));

            TxtSoc_1_0.Text = Convert.ToString(reg.GetValue("Soc10", "90"));
            TxtSoc_1_1.Text = Convert.ToString(reg.GetValue("Soc11", "90"));
            TxtSoc_1_2.Text = Convert.ToString(reg.GetValue("Soc12", "55"));

            TxtSoc_2_0.Text = Convert.ToString(reg.GetValue("Soc20", "75"));
            TxtSoc_2_1.Text = Convert.ToString(reg.GetValue("Soc21", "75"));
            TxtSoc_2_2.Text = Convert.ToString(reg.GetValue("Soc22", "40"));

            TxtSoc_3_0.Text = Convert.ToString(reg.GetValue("Soc30", "65"));
            TxtSoc_3_1.Text = Convert.ToString(reg.GetValue("Soc31", "65"));
            TxtSoc_3_2.Text = Convert.ToString(reg.GetValue("Soc32", "30"));

            TxtSoc_4_0.Text = Convert.ToString(reg.GetValue("Soc40", "55"));
            TxtSoc_4_1.Text = Convert.ToString(reg.GetValue("Soc41", "55"));
            TxtSoc_4_2.Text = Convert.ToString(reg.GetValue("Soc42", "20"));

            TxtSoc_5_0.Text = Convert.ToString(reg.GetValue("Soc50", "30"));
            TxtSoc_5_1.Text = Convert.ToString(reg.GetValue("Soc51", "25"));
            TxtSoc_5_2.Text = Convert.ToString(reg.GetValue("Soc52", "5"));


            AVRIO.avrio.AdminPage = AVRIO.AdminPage.DeviceSetting;

            if (AVRIO.avrio.RapidasLoginType == 2)
            {
                LogSampling.Visibility = Visibility.Visible;
                EquipmentInfo.Visibility = Visibility.Visible;
                //SetupMaxium.Visibility = Visibility.Visible;
                Imgmanu.Visibility = Visibility.Collapsed;
                LogSampling.IsEnabled = true;
                //   SetupMaxium.IsEnabled = true;
                EquipmentInfo.IsEnabled = true;
            }
            else
            {
                LogSampling.Visibility = Visibility.Collapsed;
                EquipmentInfo.Visibility = Visibility.Collapsed;
                //   SetupMaxium.Visibility = Visibility.Collapsed;
                Imgmanu.Visibility = Visibility.Visible;
                LogSampling.IsEnabled = false;
                //  SetupMaxium.IsEnabled = false;
                EquipmentInfo.IsEnabled = false;

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
            //window.InputValue = this.TxtSoc_0_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_0_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_0_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //window.InputValue = this.TxtSoc_0_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_0_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_0_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_0_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_0_2.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_1_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_1_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_1_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_1_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_1_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {

                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_1_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_1_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_1_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_1_2.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_2_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_2_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_2_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_2_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_2_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_2_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_2_2.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_3_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_3_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_3_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_3_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_3_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_3_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_3_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_3_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_3_2.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_4_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_4_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_4_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_4_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_4_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_4_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_4_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_4_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //   AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_4_2.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_5_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //  window.InputValue = this.TxtSoc_5_0.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //     AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_5_0.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_5_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            //   window.InputValue = this.TxtSoc_5_1.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //  AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_5_1.Text = ampea.ToString();
                    }
                    //
                }
            }
        }

        private void Rectime_5_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            // window.InputValue = this.TxtSoc_5_2.Text;
            if (window.ShowDialog() == true)
            {
                int ampea;
                if (int.TryParse(window.InputValue, out ampea))
                {
                    if (ampea > 128 || ampea < 5)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        //AVRIO.avrio.BusinessStimeHH = ampea;
                        this.TxtSoc_5_2.Text = ampea.ToString();
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


            AVRIO.avrio.TimeWattSoc_0_0 = Convert.ToInt32(TxtSoc_0_0.Text);
            AVRIO.avrio.TimeWattSoc_0_1 = Convert.ToInt32(TxtSoc_0_1.Text);
            AVRIO.avrio.TimeWattSoc_0_2 = Convert.ToInt32(TxtSoc_0_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc00", TxtSoc_0_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc01", TxtSoc_0_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc02", TxtSoc_0_2.Text);
            reg.SetValue("Soc00", TxtSoc_0_0.Text);
            reg.SetValue("Soc01", TxtSoc_0_1.Text);
            reg.SetValue("Soc02", TxtSoc_0_2.Text);

            AVRIO.avrio.TimeWattSoc_1_0 = Convert.ToInt32(TxtSoc_1_0.Text);
            AVRIO.avrio.TimeWattSoc_1_1 = Convert.ToInt32(TxtSoc_1_1.Text);
            AVRIO.avrio.TimeWattSoc_1_2 = Convert.ToInt32(TxtSoc_1_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc10", TxtSoc_1_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc11", TxtSoc_1_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc12", TxtSoc_1_2.Text);
            reg.SetValue("Soc10", TxtSoc_1_0.Text);
            reg.SetValue("Soc11", TxtSoc_1_1.Text);
            reg.SetValue("Soc12", TxtSoc_1_2.Text);

            AVRIO.avrio.TimeWattSoc_2_0 = Convert.ToInt32(TxtSoc_2_0.Text);
            AVRIO.avrio.TimeWattSoc_2_1 = Convert.ToInt32(TxtSoc_2_1.Text);
            AVRIO.avrio.TimeWattSoc_2_2 = Convert.ToInt32(TxtSoc_2_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc20", TxtSoc_2_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc21", TxtSoc_2_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc22", TxtSoc_2_2.Text);
            reg.SetValue("Soc20", TxtSoc_2_0.Text);
            reg.SetValue("Soc21", TxtSoc_2_1.Text);
            reg.SetValue("Soc22", TxtSoc_2_2.Text);

            AVRIO.avrio.TimeWattSoc_3_0 = Convert.ToInt32(TxtSoc_3_0.Text);
            AVRIO.avrio.TimeWattSoc_3_1 = Convert.ToInt32(TxtSoc_3_1.Text);
            AVRIO.avrio.TimeWattSoc_3_2 = Convert.ToInt32(TxtSoc_3_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc30", TxtSoc_3_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc31", TxtSoc_3_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc32", TxtSoc_3_2.Text);
            reg.SetValue("Soc30", TxtSoc_3_0.Text);
            reg.SetValue("Soc31", TxtSoc_3_1.Text);
            reg.SetValue("Soc32", TxtSoc_3_2.Text);

            AVRIO.avrio.TimeWattSoc_4_0 = Convert.ToInt32(TxtSoc_4_0.Text);
            AVRIO.avrio.TimeWattSoc_4_1 = Convert.ToInt32(TxtSoc_4_1.Text);
            AVRIO.avrio.TimeWattSoc_4_2 = Convert.ToInt32(TxtSoc_4_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc40", TxtSoc_4_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc41", TxtSoc_4_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc42", TxtSoc_4_2.Text);
            reg.SetValue("Soc40", TxtSoc_4_0.Text);
            reg.SetValue("Soc41", TxtSoc_4_1.Text);
            reg.SetValue("Soc42", TxtSoc_4_2.Text);

            AVRIO.avrio.TimeWattSoc_5_0 = Convert.ToInt32(TxtSoc_5_0.Text);
            AVRIO.avrio.TimeWattSoc_5_1 = Convert.ToInt32(TxtSoc_5_1.Text);
            AVRIO.avrio.TimeWattSoc_5_2 = Convert.ToInt32(TxtSoc_5_2.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc50", TxtSoc_5_0.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc51", TxtSoc_5_1.Text);
            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Soc52", TxtSoc_5_2.Text);
            reg.SetValue("Soc50", TxtSoc_5_0.Text);
            reg.SetValue("Soc51", TxtSoc_5_1.Text);
            reg.SetValue("Soc52", TxtSoc_5_2.Text);

            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();

        }
    }
}
