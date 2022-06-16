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
using System.Timers;
using System.Threading;


namespace QuickChargeDisplay
{
    /// <summary>
    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CompulsionChargePage : Page
    {
        private System.Timers.Timer timer;
        private UInt16 nSeqNum = 0;

        public CompulsionChargePage()
        {
            InitializeComponent();

           
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Enabled = false;
            }
            catch { }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "CompulsionChargePage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.AdminPage = AVRIO.AdminPage.ManualControl;
            //  AVRIO.avrio.BatteryChacrge = true;
            SetLagnaange();
            if (AVRIO.avrio.RapidasLoginType == 2)
            {
                LogSampling.Visibility = Visibility.Visible;
                //EquipmentInfo.Visibility = Visibility.Visible;
                SetupMaxium.Visibility = Visibility.Visible;
                Imgmanu.Visibility = Visibility.Collapsed;
                LogSampling.IsEnabled = true;
                SetupMaxium.IsEnabled = true;
                //   EquipmentInfo.IsEnabled = true;
            }
            else
            {
                LogSampling.Visibility = Visibility.Collapsed;
                // EquipmentInfo.Visibility = Visibility.Collapsed;
                SetupMaxium.Visibility = Visibility.Collapsed;
                Imgmanu.Visibility = Visibility.Visible;
                LogSampling.IsEnabled = false;
                SetupMaxium.IsEnabled = false;
                //  EquipmentInfo.IsEnabled = false;

            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = true;
            timer.Start();

            nSeqNum = 0;
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


        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                AVRIO.avrio.EventMsg = nSeqNum.ToString() + " = " + QCDV.ManualControl.BMSReady.ToString() + " = " + QCDV.ManualControl.CurrentBatStatus.ToString() + " = " + QCDV.ManualControl.CurrentBatStatus.ToString();
                AVRIO.avrio.EventMsg = " >>>>>>>>>>>>>>>>>>" + AVRIO.avrio.CurrentStatus.ToString();
            }
            catch { }

            /*
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { CellVolt. =  = QCDV.ManualControl.AverageCellVoltage; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { this.PCS_Init.Visibility = Visibility.Visible; });
            */

        }

        private void BtnPraRun_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.EventMsg = "BMSMainContactor_1";
            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
        }

        private void BtnPCSReadyON_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryReady;
        }

        private void BtnTsBatteryRun_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //       if (AVRIO.avrio.DspLgBatteryStatus == 0)
            {
                AVRIO.avrio.EventMsg = "TsBatteryStart";
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryStart;
            }
        }

        private void BtnTsFinish_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //         if (QCDV.ManualControl.CurrentBatStatus == (int)AVRIO.SysBatteryStatus.SysBatteryRunning)
            {
                AVRIO.avrio.EventMsg = "TsBatteryFinish";
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
            }
        }

        private void BtnBmsSleep_Click(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.WaitFualtFlag = false;
            //  AVRIO.avrio.EventMsg = "BMSMainContactor_0";
            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
            // AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_1";
            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
        }

        private void BtnWakeup_Click(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.WaitFualtFlag = true;
            AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.WaitFualtFlag = true;
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
        }
    }
}
