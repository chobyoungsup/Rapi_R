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

using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;

namespace QuickChargeDisplay
{
    /// <summary>
    /// TroubleDetailsPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TroubleHistoryPage : Page
    {
        public TroubleHistoryPage()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



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

            //  ControlHelper.FindAncestor<Window>(this).Close();

            //AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
            //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            //  AVRIO.avrio.AdminManuClose = true;
            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
        }
        #endregion


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "TroubleHistoryPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;
            try
            {
                SetLagnaange();
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.FaultHistory;
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
                // Fault File Prefix : F
                // faultDir = Directory.GetCurrentDirectory() + @"\Fault";

                faultDir = @"D:\QuickChargeApp" + "\\" + @"Fault";

                if (!Directory.Exists(faultDir))
                {
                    MessageBox.Show("No Error List.");
                    return;
                }

                // 고장내역 로그파일 리스트 저장
                faultFiles = new ObservableCollection<string>(Directory.GetFiles(faultDir, @"F*.log").Reverse());

                if (faultFiles.Count == 0)
                {
                    cbFaultDate.ItemsSource = null;
                    faultHistoryList.ItemsSource = null;
                    return;
                }


                // 고장내역 날짜 생성
                faultDate = new ObservableCollection<string>();
                foreach (string fn in faultFiles)
                {
                    string date = System.IO.Path.GetFileNameWithoutExtension(fn).Remove(0, 1);
                    date = string.Format("{0}/{1}/{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
                    faultDate.Add(date);
                }

                // 가장 최근 고장 날짜 로그 디스플레이
                cbFaultDate.ItemsSource = faultDate;
                if (faultDate.Count > 0)
                {
                    faultHistoryList.ItemsSource = LoadFaultFile(faultFiles[0]);
                    cbFaultDate.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private string faultDir;
        private ObservableCollection<string> faultFiles;
        private ObservableCollection<string> faultDate;
        private ObservableCollection<FaultRecord> faultList = new ObservableCollection<FaultRecord>();

        private ObservableCollection<FaultRecord> LoadFaultFile(string fileName)
        {
            ObservableCollection<FaultRecord> list = new ObservableCollection<FaultRecord>();

            try
            {
                // 날짜
                string date = System.IO.Path.GetFileNameWithoutExtension(fileName).Remove(0, 1);
                date = string.Format("{0}/{1}/{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));

                StreamReader rd = File.OpenText(fileName);

                int index = 0;
                string line;
                while (!string.IsNullOrEmpty((line = rd.ReadLine())))
                {
                    string[] tokens = line.Split(',');

                    if (tokens.Length < 6) continue;

                    // No.
                    int seq = int.Parse(tokens[0].Trim());

                    // 고장시간
                    string time = tokens[1].Trim();
                    time = string.Format("{0}:{1}:{2}", time.Substring(0, 2), time.Substring(2, 2), time.Substring(4, 2));

                    // 복귀시간
                    string time2 = tokens[2].Trim();

                    if (!string.IsNullOrEmpty(time2) && time2.Length >= 6)
                        time2 = string.Format("{0}:{1}:{2}", time2.Substring(0, 2), time2.Substring(2, 2), time2.Substring(4, 2));

                    // 고장 코드
                    int code = int.Parse(tokens[3].Trim());

                    // 고장 내역 
                    string detail = tokens[4];

                    // 확인 여부
                    bool check = tokens[5].Equals("Y") ? true : false;

                    FaultRecord record = new FaultRecord(seq, date, time, time2, code, detail, check);
                    list.Insert(0, record);
                    index++;
                }

                rd.Close();
            }
            catch
            {
                // MessageBox.Show(ex.Message);
            }

            return list;
        }

        private void cbFaultDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFaultDate.ItemsSource == null)
                return;

            int index = cbFaultDate.SelectedIndex;

            if (index < faultFiles.Count)
                faultHistoryList.ItemsSource = LoadFaultFile(faultFiles[index]);
        }

        private void faultHistoryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FaultRecord record = faultHistoryList.SelectedItem as FaultRecord;

            if (record != null)
            {
                record.Check = true;

                // 파일로 저장
                string fileName = faultFiles[cbFaultDate.SelectedIndex];

                ObservableCollection<FaultRecord> list = faultHistoryList.ItemsSource as ObservableCollection<FaultRecord>;

                if (list == null) return;

                using (TextWriter tw = new StreamWriter(fileName))
                {
                    foreach (FaultRecord r in list)
                    {
                        StringBuilder sb = new StringBuilder();

                        // Seq
                        sb.Append(r.Number); sb.Append(",");
                        // 고장시간
                        sb.Append(r.Time.Replace(":", "")); sb.Append(",");
                        // 복귀시간
                        sb.Append(r.Time2.Replace(":", "")); sb.Append(",");
                        // 고장 코드
                        sb.Append(r.Code); sb.Append(",");
                        // 고장 내역
                        sb.Append(r.Details); sb.Append(",");
                        // 확인 여부
                        sb.Append(r.Check ? "Y" : "N");

                        tw.WriteLine(sb.ToString());
                    }
                }
            }
        }


    }
}
