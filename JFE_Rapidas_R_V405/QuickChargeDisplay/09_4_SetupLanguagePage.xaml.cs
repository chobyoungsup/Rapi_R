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
using System.ComponentModel;
using System.Data;
using System.Drawing;


namespace QuickChargeDisplay
{
    /// <summary>
    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetupLanguagePage : Page
    {
        bool LangJapanType;// = true;
        public SetupLanguagePage()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        int Language = 0;
        DayOfWeek CurrentChkDayOfWeek = DayOfWeek.Sunday;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "SetupLanguagePage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;


            this.lbl_Day.Visibility = Visibility.Collapsed;

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

            //cbs
            this.btnUseReboot.Content = "Reboot OS\nEnable";
            //this.lbl_Day.Content = "";
            if (!AVRIO.avrio.UseReboot)
            {
                this.btnUseReboot.Content = "Reboot OS\nDisable";

                this.DaySun.Visibility = Visibility.Collapsed;
                this.DayMon.Visibility = Visibility.Collapsed;
                this.DayTue.Visibility = Visibility.Collapsed;
                this.DayWed.Visibility = Visibility.Collapsed;
                this.DayThu.Visibility = Visibility.Collapsed;
                this.DayFri.Visibility = Visibility.Collapsed;
                this.DaySat.Visibility = Visibility.Collapsed;

                this.chk_byDay.Visibility = Visibility.Collapsed;
                this.chk_byweek.Visibility = Visibility.Collapsed;


                this.textBox3.Visibility = Visibility.Collapsed;
                this.textBox4.Visibility = Visibility.Collapsed;
                this.rectangle1.Visibility = Visibility.Collapsed;
                this.rectangle2.Visibility = Visibility.Collapsed;

                AVRIO.avrio.UseReboot = false;
            }

            // Admin Password 확인
            AVRIO.avrio.Rapidaslanguage = Convert.ToInt32(reg.GetValue("LANGUAGE", "0"));
            if (AVRIO.avrio.Rapidaslanguage == 0)
                LangJapanType = true;
            else LangJapanType = false;

            if (LangJapanType)
            {
                this.ImglJapan.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.ImglEnglish.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

            }
            else
            {
                this.ImglJapan.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.ImglEnglish.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }

           // reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");
            // AVRIO.avrio.Rapidasweekreboot = Convert.ToInt32(reg.GetValue("REBOOTWEEK", "1"));
            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);


            textBox3.Text = AVRIO.avrio.RebootTimehh.ToString();
            textBox4.Text = AVRIO.avrio.RebootTimemm.ToString();



            //cbs 2020
            if (!AVRIO.avrio.RebootSelectDay) //Week
            {
                textBox3.Text = AVRIO.avrio.RebootTimehh.ToString();
                textBox4.Text = AVRIO.avrio.RebootTimemm.ToString();
                //this.lbl_Day.Visibility = Visibility.Collapsed;
                this.chk_byDay.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.chk_byweek.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.chk_byweek.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                switch (AVRIO.avrio.Rapidasweekreboot)
                {
                    case 1:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 2:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 3:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 4:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 5:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 6:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            break;
                        }
                    case 7:
                        {
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        }

                }
            }
            else
            {
                textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].TimeHH.ToString();
                textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].Timemm.ToString();

               // this.lbl_Day.Visibility = Visibility.Visible;

                this.chk_byDay.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.chk_byweek.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.chk_byDay.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));



                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                foreach (DayOfWeek day in AVRIO.avrio._RebootFunc.Keys)
                {
                    if (AVRIO.avrio._RebootFunc[day].IsEnable)
                    {
                        switch (day)
                        {
                            case DayOfWeek.Sunday:
                                this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;
                            case DayOfWeek.Monday:
                                this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;

                            case DayOfWeek.Tuesday:
                                this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;
                            case DayOfWeek.Wednesday:
                                this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;
                            case DayOfWeek.Thursday:
                                this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;
                            case DayOfWeek.Friday:
                                this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;
                            case DayOfWeek.Saturday:
                                this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                                break;

                        }
                    }
                }
            }


            SetLagnaange();
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

        private void btnComfirm_Click(object sender, RoutedEventArgs e)
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

            if (Language == 0)
            {
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "LANGUAGE", "0");
                reg.SetValue("LANGUAGE", "0");
                AVRIO.avrio.Rapidaslanguage = 0;
            }
            else
            {
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "LANGUAGE", "1");
                reg.SetValue("LANGUAGE", "1");
                AVRIO.avrio.Rapidaslanguage = 1;
            }

            if (AVRIO.avrio.RebootSelectDay)
            {
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "RebootSelect", "Day");

                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_Enable", "false");
                //
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable)
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_Enable", "true");
                else
                    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_Enable", "false");


                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_HH", this.textBox3.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_HH", this.textBox3.Text);


                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_mm", this.textBox4.Text);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_mm", this.textBox4.Text);

            }
            else
            {
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "RebootSelect", "Week");
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", AVRIO.avrio.Rapidasweekreboot.ToString());
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTTIMEHH", AVRIO.avrio.RebootTimehh.ToString());
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTTIMEMM", AVRIO.avrio.RebootTimemm.ToString());
            }


            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();

            SetLagnaange();

        }

        private void ImglJapan_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.ImglJapan.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            this.ImglEnglish.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            Language = 0;

        }

        private void ImglEnglish_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.ImglJapan.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.ImglEnglish.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            Language = 1;
        }

        private void DaySun_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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

                AVRIO.avrio.Rapidasweekreboot = 1;
                reg.SetValue("REBOOTWEEK", 1);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "1");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
               // this.lbl_Day.Content = DayOfWeek.Sunday.ToString();
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable = false;
                    this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].Timemm.ToString();

                    this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Sunday;
                }
            }
        }
        private void DayMon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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

                AVRIO.avrio.Rapidasweekreboot = 2;
                reg.SetValue("REBOOTWEEK", 2);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "2");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
              //  this.lbl_Day.Content = DayOfWeek.Monday.ToString();

                if (AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable = false;
                    this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Monday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Monday].Timemm.ToString();

                    this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Monday;
                }
            }
        }
        private void DayTue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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

                AVRIO.avrio.Rapidasweekreboot = 3;
                reg.SetValue("REBOOTWEEK", 3);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "3");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
              //  this.lbl_Day.Content = DayOfWeek.Tuesday.ToString();
                if (AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable = false;
                    this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].Timemm.ToString();

                    this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Tuesday;
                }
            }
        }
        private void DayWed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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


                AVRIO.avrio.Rapidasweekreboot = 4;
                reg.SetValue("REBOOTWEEK", 4);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "4");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
               // this.lbl_Day.Content = DayOfWeek.Wednesday.ToString();

                if (AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable = false;
                    this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].Timemm.ToString();

                    this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Wednesday;
                }
            }
        }
        private void DayThu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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

                AVRIO.avrio.Rapidasweekreboot = 5;
                reg.SetValue("REBOOTWEEK", 5);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "5");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
               // this.lbl_Day.Content = DayOfWeek.Thursday.ToString();

                if (AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable = false;
                    this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].Timemm.ToString();

                    this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Thursday;
                }
            }
        }
        private void DayFri_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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

                AVRIO.avrio.Rapidasweekreboot = 6;
                reg.SetValue("REBOOTWEEK", 6);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "6");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

                reg = null;
            }
            else
            {
               // this.lbl_Day.Content = DayOfWeek.Friday.ToString();

                if (AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable = false;
                    this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Friday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Friday].Timemm.ToString();

                    this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Friday;
                }
            }
        }
        private void DaySat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
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


                AVRIO.avrio.Rapidasweekreboot = 7;
                reg.SetValue("REBOOTWEEK", 7);
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTWEEK", "7");

                this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));

                reg = null;
            }
            else
            {
               // this.lbl_Day.Content = DayOfWeek.Saturday.ToString();

                if (AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable)
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable = false;
                    this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                }
                else
                {
                    AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable = true;
                    this.textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].TimeHH.ToString();
                    this.textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].Timemm.ToString();

                    this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                    CurrentChkDayOfWeek = DayOfWeek.Saturday;
                }
            }
        }

        private void rectangle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);

                // window.InputValue = this.TxtCharging.Text;

                if (window.ShowDialog() == true)
                {

                    int hh;

                    if (int.TryParse(window.InputValue, out hh))
                    {
                        if (hh > 23 || hh < 0)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            AVRIO.avrio.RebootTimehh = hh;
                            this.textBox3.Text = hh.ToString();

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

                            reg.SetValue("REBOOTTIMEHH", AVRIO.avrio.RebootTimehh);
                            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTTIMEHH", AVRIO.avrio.RebootTimehh.ToString());
                            reg = null;
                        }
                        //
                    }
                }
            }
            else
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);

                if (window.ShowDialog() == true)
                {

                    int hh;

                    if (int.TryParse(window.InputValue, out hh))
                    {
                        if (hh > 23 || hh < 0)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            //AVRIO.avrio._RebootFunc[CurrentChkDayOfWeek].TimeHH = hh;

                            foreach (DayOfWeek Day in AVRIO.avrio._RebootFunc.Keys)
                            {
                                AVRIO.avrio._RebootFunc[Day].TimeHH = hh;
                            }

                            this.textBox3.Text = hh.ToString();

                            //switch (CurrentChkDayOfWeek)
                            //{
                            //    case DayOfWeek.Sunday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Monday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Tuesday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Wednesday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Thursday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Friday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_HH", this.textBox3.Text); break;
                            //    case DayOfWeek.Saturday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_HH", this.textBox3.Text); break;
                            //}
                            
                            
                              
                        }
                    }
                }
            }
        }

        private void rectangle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!AVRIO.avrio.RebootSelectDay)
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);

                // window.InputValue = this.TxtCharging.Text;

                if (window.ShowDialog() == true)
                {

                    int mm;

                    if (int.TryParse(window.InputValue, out mm))
                    {
                        if (mm > 59 || mm < 0)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            AVRIO.avrio.RebootTimemm = mm;
                            this.textBox4.Text = mm.ToString();

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


                            reg.SetValue("REBOOTTIMEMM", AVRIO.avrio.RebootTimemm);
                            QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "REBOOTTIMEMM", AVRIO.avrio.RebootTimemm.ToString());

                            reg = null;
                        }
                        //
                    }
                }
            }
            else
            {
                KeyPadWindow window = new KeyPadWindow();
                window.Owner = ControlHelper.FindAncestor<Window>(this);

                // window.InputValue = this.TxtCharging.Text;

                if (window.ShowDialog() == true)
                {

                    int mm;

                    if (int.TryParse(window.InputValue, out mm))
                    {
                        if (mm > 59 || mm < 0)
                        {
                            MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            // AVRIO.avrio._RebootFunc[CurrentChkDayOfWeek].Timemm = mm;


                            foreach (DayOfWeek Day in AVRIO.avrio._RebootFunc.Keys)
                            {
                                AVRIO.avrio._RebootFunc[Day].Timemm = mm;
                            }

                            this.textBox4.Text = mm.ToString();

                            //switch (CurrentChkDayOfWeek)
                            //{
                            //    case DayOfWeek.Sunday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Monday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Tuesday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Wednesday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Thursday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Friday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_mm", this.textBox4.Text); break;
                            //    case DayOfWeek.Saturday: QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_mm", this.textBox4.Text); break;
                            //}


                         
                        }
                    }
                }
            }
        }

        private void useReboot(object sender, RoutedEventArgs e)
        {
            //cbs

            if (AVRIO.avrio.UseReboot == false)
            {
                this.btnUseReboot.Content = "Reboot OS\nEnable";
                this.DaySun.Visibility = Visibility.Visible;
                this.DayMon.Visibility = Visibility.Visible;
                this.DayTue.Visibility = Visibility.Visible;
                this.DayWed.Visibility = Visibility.Visible;
                this.DayThu.Visibility = Visibility.Visible;
                this.DayFri.Visibility = Visibility.Visible;
                this.DaySat.Visibility = Visibility.Visible;

                this.chk_byDay.Visibility = Visibility.Visible;

                this.chk_byweek.Visibility = Visibility.Visible;
                this.textBox3.Visibility = Visibility.Visible;
                this.textBox4.Visibility = Visibility.Visible;
                this.rectangle1.Visibility = Visibility.Visible;
                this.rectangle2.Visibility = Visibility.Visible;

                AVRIO.avrio.UseReboot = true;

                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "UseReboot", "True");
            }
            else if (AVRIO.avrio.UseReboot == true)
            {
                this.btnUseReboot.Content = "Reboot OS\nDisable";
                this.DaySun.Visibility = Visibility.Collapsed;
                this.DayMon.Visibility = Visibility.Collapsed;
                this.DayTue.Visibility = Visibility.Collapsed;
                this.DayWed.Visibility = Visibility.Collapsed;
                this.DayThu.Visibility = Visibility.Collapsed;
                this.DayFri.Visibility = Visibility.Collapsed;
                this.DaySat.Visibility = Visibility.Collapsed;

                this.chk_byDay.Visibility = Visibility.Collapsed;
                this.chk_byweek.Visibility = Visibility.Collapsed;
                this.textBox3.Visibility = Visibility.Collapsed;
                this.textBox4.Visibility = Visibility.Collapsed;

                this.rectangle1.Visibility = Visibility.Collapsed;
                this.rectangle2.Visibility = Visibility.Collapsed;

                AVRIO.avrio.UseReboot = false;

                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "UseReboot", "False");
            }

            //일 주 단위 구분

        }

        private void chk_byDay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.RebootSelectDay = true;
          //  this.lbl_Day.Visibility = Visibility.Visible;

            this.chk_byweek.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.chk_byDay.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            textBox3.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].TimeHH.ToString();
            textBox4.Text = AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].Timemm.ToString();

            this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));

            foreach (DayOfWeek day in AVRIO.avrio._RebootFunc.Keys)
            {
                if (AVRIO.avrio._RebootFunc[day].IsEnable)
                {
                    switch (day)
                    {
                        case DayOfWeek.Sunday:
                            this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Monday:
                            this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Tuesday:
                            this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Wednesday:
                            this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Thursday:
                            this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Friday:
                            this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;
                        case DayOfWeek.Saturday:
                            this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                            break;

                    }
                }
            }
        }

        private void chk_byweek_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.RebootSelectDay = false;
           // this.lbl_Day.Visibility = Visibility.Collapsed;
            this.chk_byweek.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            this.chk_byDay.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));


            textBox3.Text = AVRIO.avrio.RebootTimehh.ToString();
            textBox4.Text = AVRIO.avrio.RebootTimemm.ToString();


            switch (AVRIO.avrio.Rapidasweekreboot)
            {
                case 1:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 2:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 3:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 4:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 5:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 6:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        break;
                    }
                case 7:
                    {
                        this.DaySun.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayMon.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayTue.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayWed.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayThu.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DayFri.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
                        this.DaySat.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
                        break;
                    }

            }
        }

        private void lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (AVRIO.avrio.RebootSelectDay)
            //{
            //    DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ((System.Windows.Controls.Label)sender).Name);
            //    this.lbl_Day.Content = dayOfWeek.ToString();
            //    if (AVRIO.avrio._RebootFunc.ContainsKey(dayOfWeek))
            //    {
            //        this.textBox3.Text = AVRIO.avrio._RebootFunc[dayOfWeek].TimeHH.ToString();
            //        this.textBox4.Text = AVRIO.avrio._RebootFunc[dayOfWeek].Timemm.ToString();
            //    }
            //    else
            //    {
            //        this.textBox3.Text = "00";
            //        this.textBox4.Text = "00";
            //    }
            //}
        }
    }
}
