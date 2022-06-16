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
using System.Windows.Shapes;

namespace QuickChargeDisplay
{
    /// <summary>
    /// _09_10_SetupVoltagePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class _09_10_SetupVoltagePage : Page
    {
        public _09_10_SetupVoltagePage()
        {
            InitializeComponent();

        }

        //cbs 2020.02.07
        //-32,768 ~ 32,767

        //cbs 2020.06.11 decimal 로 변경
        private decimal d_VoltageSlopeMin = 0.8m;
        private decimal d_VoltageSlopeMax = 1.2m;
        private decimal d_VoltageOffsetMin = -10m;
        private decimal d_VoltageOffsetMax = 10m;

        private decimal d_CurrentSlopeMin = 0.8m;
        private decimal d_CurrentSlopeMax = 1.2m;
        private decimal d_CurrentOffsetMin = -10m;
        private decimal d_CurrentOffsetMax = 10m;

        private int CacuScale = 10000; 

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "_09_10_SetupVoltagePage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            //QCDV.DSPCalibration.Voltage_Slope = "1.0006";
            //QCDV.DSPCalibration.Voltage_Offset = "-9.0007";
            //QCDV.DSPCalibration.Current_Slope = "1.1208";
            //QCDV.DSPCalibration.Current_Offset = "-8.0007";

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
                        
            txt_voltage_SlopeSet.Text = string.Format("{0:f4}", (AVRIO.avrio.nSET_Voltage_Slope * 0.0001)); 
            txt_voltage_OffsetSet.Text = string.Format("{0:f4}", (AVRIO.avrio.nSET_Voltage_Offset * 0.0001));
            txt_current_SlopeSet.Text = string.Format("{0:f4}", (AVRIO.avrio.nSET_Current_Slope * 0.0001));
            txt_Current_OffsetSet.Text = string.Format("{0:f4}", (AVRIO.avrio.nSET_Current_Offset * 0.0001));

        }

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
                Langanglab11.Content = "電圧設定";
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
                Langanglab11.Content = "Voltage";

            }
        }
               

        private void logOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //  ControlHelper.FindAncestor<Window>(this).Close();
            // AVRIO.avrio.AdminManuClose = true;

            AVRIO.avrio.bDSPCalibration = false;
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
        }

        #region 캘리브레이션
        private void VoltageSlopeSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow2 window = new KeyPadWindow2(txt_voltage_SlopeSet.Text);
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            if (window.ShowDialog() == true)
            {
                try
                {
                    decimal slope;
                    if (decimal.TryParse(window.InputValue, out slope))
                    {
                        if (slope >= d_VoltageSlopeMin && slope <= d_VoltageSlopeMax)
                        {
                            txt_voltage_SlopeSet.Text = string.Format("{0:f4}", slope);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(" Range Err \r\n Voltage Slope  \r\n {0} ~ {1}", d_VoltageSlopeMin, d_VoltageSlopeMax));
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Range Err");
                }
            }
        }

        private void VoltageOffsetSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow2 window = new KeyPadWindow2(txt_voltage_OffsetSet.Text);
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            if (window.ShowDialog() == true)
            {
                try
                {
                    decimal offset;
                    if (decimal.TryParse(window.InputValue, out offset))
                    {
                        if (offset >= d_VoltageOffsetMin && offset <= d_VoltageOffsetMax)
                        {
                            txt_voltage_OffsetSet.Text = string.Format("{0:f4}", offset);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(" Range Err \r\n Voltage Offset  \r\n {0} ~  {1}", d_VoltageOffsetMin, d_VoltageOffsetMax));
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Range Err");
                }
            }
        }

        private void CurrentSlopeSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow2 window = new KeyPadWindow2(txt_current_SlopeSet.Text);
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            if (window.ShowDialog() == true)
            {
                try
                {
                    decimal slope;
                    if (decimal.TryParse(window.InputValue, out slope))
                    {
                        if (slope >= d_CurrentSlopeMin && slope <= d_CurrentSlopeMax)
                        {
                            txt_current_SlopeSet.Text = string.Format("{0:f4}", slope);

                        }
                        else
                        {
                            MessageBox.Show(string.Format(" Range Err \r\n Current Slope  \r\n {0} ~ {1}", d_CurrentSlopeMin, d_CurrentSlopeMax));
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Range Err");
                }
            }
        }

        private void CurrentOffsetSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow2 window = new KeyPadWindow2(txt_Current_OffsetSet.Text);
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            if (window.ShowDialog() == true)
            {
                try
                {
                    decimal offset;
                    if (decimal.TryParse(window.InputValue, out offset))
                    {
                        if (offset >= d_CurrentOffsetMin && offset <= d_CurrentOffsetMax)
                        {
                            txt_Current_OffsetSet.Text = string.Format("{0:f4}", offset);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(" Range Err \r\n Current Offset  \r\n {0} ~  {1}", d_CurrentOffsetMin, d_CurrentOffsetMax));
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Range Err");
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal fTemp = (decimal.Parse(txt_voltage_SlopeSet.Text) * CacuScale);
                AVRIO.avrio.nSET_Voltage_Slope = (int)fTemp;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "VoltageSlope", (AVRIO.avrio.nSET_Voltage_Slope).ToString());

                fTemp = (decimal.Parse(txt_voltage_OffsetSet.Text) * CacuScale);
                AVRIO.avrio.nSET_Voltage_Offset = (int)fTemp;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "VoltageOffset", (AVRIO.avrio.nSET_Voltage_Offset).ToString());

                fTemp = (decimal.Parse(txt_current_SlopeSet.Text) * CacuScale);
                AVRIO.avrio.nSET_Current_Slope = (int)fTemp;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CurrentSlope", (AVRIO.avrio.nSET_Current_Slope).ToString());

                fTemp = (decimal.Parse(txt_Current_OffsetSet.Text) * CacuScale);
                AVRIO.avrio.nSET_Current_Offset = (int)fTemp;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CurrentOffset", (AVRIO.avrio.nSET_Current_Offset).ToString());

            }
            catch
            {
                MessageBox.Show("Range Value Err");
                return;
            }
            OkDialog dlg = new OkDialog();
            dlg.Owner = ControlHelper.FindAncestor<Window>(this);
            dlg.ShowDialog();
            AVRIO.avrio.bDSPCalibration = true;
        }
        #endregion

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

       
    }
}
