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
using System.Diagnostics;
using System.Threading;

namespace QuickChargeDisplay
{

    /// <summary>
    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
    /// </summary>


    public partial class LogSamplingPage : Page
    {
        public LogSamplingPage()
        {
            InitializeComponent();

           
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        bool watchlog = false;
        bool filebms = false;
        bool filefault = false;
        bool filehistory = false;
        bool filelog = false;
        bool buttonclick = true;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "LogSamplingPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            if (AVRIO.avrio.RapidasLoginType == 2)
            {
                //LogSampling.Visibility = Visibility.Visible;
                EquipmentInfo.Visibility = Visibility.Visible;
                SetupMaxium.Visibility = Visibility.Visible;

                Imgmanu.Visibility = Visibility.Collapsed;
                //  LogSampling.IsEnabled = true;
                SetupMaxium.IsEnabled = true;
                EquipmentInfo.IsEnabled = true;
            }
            else
            {
                //   LogSampling.Visibility = Visibility.Collapsed;
                EquipmentInfo.Visibility = Visibility.Collapsed;
                SetupMaxium.Visibility = Visibility.Collapsed;
                Imgmanu.Visibility = Visibility.Visible;
                // LogSampling.IsEnabled = false;
                SetupMaxium.IsEnabled = false;
                EquipmentInfo.IsEnabled = false;

            }

            SetLagnaange();

            this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.ImgFault.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.imageLog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            watchlog = false;

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

            AVRIO.avrio.FanOffset = Convert.ToUInt16(reg.GetValue("FANTEMPOFFSET", "5"));
            Txtemperuter.Text = Convert.ToString(AVRIO.avrio.FanOffset);
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
            //AVRIO.avrio.AdminManuClose = true;
            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
        }
        #endregion


        private void Btn_FileSave_Click(object sender, RoutedEventArgs e)
        {
            if (buttonclick)
            {
                buttonclick = false;
                DriveInfo[] ListDrives = DriveInfo.GetDrives();
                DriveInfo usbDrive = null;
                foreach (DriveInfo drive in ListDrives)
                {
                    if (drive.DriveType == DriveType.Removable)
                    {
                        usbDrive = drive;
                        break;
                    }
                }

                if (usbDrive == null)
                {
                    MessageBox.Show("No removable device. Please! Check USB device.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    buttonclick = true;
                    return;
                }

                try
                {
                    DateTime dt = DateTime.Now;
                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
                    string srcRoot = @"D:\QuickChargeApp\";// Directory.GetCurrentDirectory();


                    string dstRoot = usbDrive.Name;
                    string bmsRoot = dstRoot + @"BMS\";
                    string logRoot = dstRoot + @"LOG\";
                    if (filebms == true)
                    {

                        try
                        {
                            CopyFolder(srcRoot, dstRoot, "BMS");
                            if (System.IO.File.Exists(bmsRoot + logFile))
                            {
                                // Use a try block to catch IOExceptions, to
                                // handle the case of the file already being
                                // opened by another process.
                                try
                                {
                                    System.IO.File.Delete(bmsRoot + logFile);
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch { }
                    }

                    if (filefault == true)
                    {
                        try
                        {
                            CopyFolder(srcRoot, dstRoot, "Fault");
                        }
                        catch { }
                    }

                    if (filehistory == true)
                    {
                        try
                        {
                            CopyFolder(srcRoot, dstRoot, "History");
                        }
                        catch { }
                    }

                    if (watchlog)
                    {
                        try
                        {
                            CopyFolder(srcRoot, dstRoot, "Process");

                            CopyFolder(srcRoot, dstRoot, "Exception_LOG");

                            CopyFolder(srcRoot, dstRoot, "Touch_LOG");

                        }
                        catch { }
                    }


                    if (filelog == true)
                    {
                        try
                        {
                            CopyFolder(srcRoot, dstRoot, "LOG");


                            if (System.IO.File.Exists(logRoot + logFile))
                            {
                                // Use a try block to catch IOExceptions, to
                                // handle the case of the file already being
                                // opened by another process.
                                try
                                {
                                    System.IO.File.Delete(logRoot + logFile);
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch { }
                    }
                    OkDialog dlg = new OkDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();

                }
                catch (Exception ex)
                {
                    AVRIO.avrio.EventMsg = ex.ToString();
                    MessageBox.Show("File copy fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                buttonclick = true;
            }
            else return;

        }

        void CopyFolder(string srcRoot, string dstRoot, string DirectoryName)
        {
            try
            {
                string srcDir = srcRoot + @"\" + DirectoryName;
                string dstDir = dstRoot + DirectoryName;

                if (!Directory.Exists(srcDir))
                {
                    buttonclick = true;
                    return;
                }
                if (!Directory.Exists(dstDir))
                    Directory.CreateDirectory(dstDir);

                string[] files = Directory.GetFiles(srcDir, "*.log");

                foreach (string fileName in files)
                {
                    File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);
                }
            }
            catch { }
            buttonclick = true;
        }

        private void ImgBMS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (filebms)
            {
                filebms = false;
                this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                filebms = true;
                this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
        }

        private void ImgHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (filehistory)
            {
                filehistory = false;
                this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                filehistory = true;
                this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
        }

        private void ImgFault_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (filefault)
            {
                filefault = false;
                this.ImgFault.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                filefault = true;
                this.ImgFault.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
        }

        private void imageLog_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (filelog)
            {
                filelog = false;
                this.imageLog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                filelog = true;
                this.imageLog.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
        }

        //private void btn_WatchDog_Click(object sender, RoutedEventArgs e)
        //{
        //    string strTempsss = "";
        //    try
        //    {

        //        string strTemp = btn_WatchDog.Content.ToString();

        //        if (strTemp == "WatchDog  Enable")
        //        {
        //            //WatchDog
        //            Process[] prs = Process.GetProcesses();
        //            foreach (Process pr in prs)
        //            {
        //                if (pr.ProcessName == "RapidasWatchDog")
        //                {
        //                    // pr.Exited += new EventHandler(ProcessExited);
        //                    strTempsss = "Kill";
        //                    pr.Kill();

        //                }
        //            }

        //            Thread.Sleep(1000);

        //            bool Result = false;
        //            prs = Process.GetProcesses();
        //            foreach (Process pr in prs)
        //            {
        //                if (pr.ProcessName == "RapidasWatchDog")
        //                {
        //                    Result = true;
        //                }
        //            }


        //            if (!Result)
        //            {
        //                this.btn_WatchDog.Content = "WatchDog  Disable";
        //            }
        //            else
        //            {
        //                MessageBox.Show("Please Retry Kill");
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            string Names = "RapidasWatchDog.exe";
        //            string fullPathss = @"C:\WatchDog";
        //            Process ps2 = new Process();

        //            ps2.StartInfo.FileName = Names;
        //            ps2.StartInfo.WorkingDirectory = fullPathss;
        //            ps2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        //            // ps2.EnableRaisingEvents = true;

        //            strTempsss = "Start WatchDog";
        //            ps2.Start();
        //            this.btn_WatchDog.Content = "WatchDog  Enable";
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Process Err : " + strTempsss);
        //        return;
        //    }
        //}

        private void ImgWatchdog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (watchlog)
            {
                watchlog = false;
                this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
            }
            else
            {
                watchlog = true;
                this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
            }
        }


        private void RecCharging_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPadWindow window = new KeyPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);

            if (window.ShowDialog() == true)
            {
                int temperture;
                if (int.TryParse(window.InputValue, out temperture))
                {
                    if (temperture > 25 || temperture < 0)
                    {
                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (temperture < 10)
                        {
                            this.Txtemperuter.Text = "0";
                            this.Txtemperuter.Text += temperture.ToString();
                        }
                        else
                        {
                            this.Txtemperuter.Text = temperture.ToString();
                        }


                        AVRIO.avrio.FanOffset = (ushort)temperture;

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

                        reg.SetValue("FANTEMPOFFSET", AVRIO.avrio.FanOffset);
                        QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "FANTEMPOFFSET", AVRIO.avrio.FanOffset.ToString());

                        reg = null;
                    }
                }
            }
        }


    }
}


#region  Cbs Edit  QC Team ㅅㅂㅅㄲㄷ...
//Progressbar Control만 만들어 쓰면 됨(x:name="Prog_FileCopy")


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.IO;
//using System.Xml.Linq;
//using Microsoft.Win32;
//using System.Diagnostics;
//using System.Threading;
//using System.Timers;

//namespace QuickChargeDisplay
//{

//    /// <summary>
//    /// SetupEquipmentPage.xaml에 대한 상호 작용 논리
//    /// </summary>

//    //cbs
//    public delegate void ProgressChangeDelegate(string Persentage, string Content);
//    public delegate void Completedelegate();
//    public delegate void DownBtn();

//    public partial class LogSamplingPage : Page
//    {
//        //cbs
//        public event ProgressChangeDelegate OnProgressChanged;
//        public event Completedelegate OnComplete;
//        public event DownBtn OnPress;

//        public LogSamplingPage()
//        {
//            InitializeComponent();

//            //cbs
//            OnProgressChanged += new ProgressChangeDelegate(Percent);
//            OnComplete += new Completedelegate(Complete);
//            OnPress += new DownBtn(BtnClicked);
//        }

//        /// <summary>
//        /// Log Out
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        /// 

//        bool watchlog = false;
//        bool filebms = false;
//        bool filefault = false;
//        bool filehistory = false;
//        bool filelog = false;

//        bool FaultFinish = false;
//        string strPercent;
//        string USB_Locate = "";

//        //cbs
//        void Percent(string Per, string Content)
//        {
//            this.Dispatcher.Invoke((ThreadStart)delegate ()
//            {
//                Btn_FileSave.Content = Content;
//                strPercent = Convert.ToString(Per);
//                this.Prog_FileCopy.Value = Convert.ToDouble(strPercent);                
//            });
//        }
//        //cbs
//        void Complete()
//        {
//            this.Dispatcher.Invoke((ThreadStart)delegate ()
//            {
//                Btn_FileSave.IsEnabled = true;
//                this.Prog_FileCopy.Value = 0;
//                TotalFileCount = 0;
//                RemainlFileCount = 0;

//                if (!FaultFinish)
//                {
//                    OkDialog dlg = new OkDialog();
//                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
//                    dlg.ShowDialog();
//                }

//                Btn_FileSave.Content = "File save to USB";
//                Btn_FileSave.IsEnabled = true;
//                Prog_FileCopy.Visibility = Visibility.Hidden;
//            });
//        }
//        //cbs
//        void BtnClicked()
//        {
//            this.Dispatcher.Invoke((ThreadStart)delegate ()
//            {
//               Btn_FileSave.IsEnabled = false;
//            });
//        }



//        private void Page_Loaded(object sender, RoutedEventArgs e)
//        {
//            Prog_FileCopy.Visibility = Visibility.Hidden;
//            USB_Locate = null;
//            watchlog = false;
//            filebms = false;
//            filefault = false;
//            filehistory = false;
//            filelog = false;

//            FaultFinish = false;
//            strPercent = "0";

//            if (AVRIO.avrio.nMouseCuser)
//                this.Cursor = Cursors.Arrow;
//            if (AVRIO.avrio.RapidasLoginType == 2)
//            {
//                //LogSampling.Visibility = Visibility.Visible;
//                EquipmentInfo.Visibility = Visibility.Visible;
//                SetupMaxium.Visibility = Visibility.Visible;

//                Imgmanu.Visibility = Visibility.Collapsed;
//                //  LogSampling.IsEnabled = true;
//                SetupMaxium.IsEnabled = true;
//                EquipmentInfo.IsEnabled = true;
//            }
//            else
//            {
//                //   LogSampling.Visibility = Visibility.Collapsed;
//                EquipmentInfo.Visibility = Visibility.Collapsed;
//                SetupMaxium.Visibility = Visibility.Collapsed;
//                Imgmanu.Visibility = Visibility.Visible;
//                // LogSampling.IsEnabled = false;
//                SetupMaxium.IsEnabled = false;
//                EquipmentInfo.IsEnabled = false;

//            }

//            SetLagnaange();

//            this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            this.ImgFault.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            this.imageLog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            watchlog = false;

//            RegistryKey reg;
//            reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");

//            AVRIO.avrio.FanOffset = Convert.ToUInt16(reg.GetValue("FANTEMPOFFSET", "5"));
//            Txtemperuter.Text = Convert.ToString(AVRIO.avrio.FanOffset);
//        }



//        #region 관리자 메뉴 텝

//        private void SetLagnaange()
//        {
//            if (AVRIO.avrio.Rapidaslanguage == 0)
//            {
//                Langanglab1.Content = "管理設定";
//                Langanglab2.Content = "Log抽出";
//                Langanglab3.Content = "時間の設定";
//                Langanglab4.Content = "言語設定";
//                Langanglab5.Content = "充電履歴";
//                Langanglab6.Content = "エラー履歴";
//                Langanglab7.Content = "パスワード";
//                Langanglab8.Content = "受電電力設定";
//                Langanglab9.Content = "電流設定";
//                Langanglab10.Content = "Manual";
//            }
//            else
//            {
//                Langanglab1.Content = "Settings";
//                Langanglab2.Content = "Log";
//                Langanglab3.Content = "Time";
//                Langanglab4.Content = "Language";
//                Langanglab5.Content = "Charge";
//                Langanglab6.Content = "Error";
//                Langanglab7.Content = "Password";
//                Langanglab8.Content = "Set Power";
//                Langanglab9.Content = "Set current";
//                Langanglab10.Content = "Manual";

//            }
//        }

//        private void TroubleDetails_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 관리 설정
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.ManualControl;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_1_관리설정));
//                }
//            }
//            catch { }
//        }

//        private void SetupEquipment_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 수전전력 설정
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.Managerment;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_2_수전전력설정));
//                }
//            }
//            catch { }
//        }

//        private void AdjustUnitPrice_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 일시정지
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.Moment;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_3_일시설정));
//                }
//            }
//            catch { }
//        }

//        private void PowerMeter_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 언어 설정
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.Language;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_4_언어설정));
//                }
//            }
//            catch { }
//        }

//        private void CahrgeHistory_MouseDown(object sender, MouseButtonEventArgs e)
//        {//충전 이력
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.ChargeHistory;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_6_충전이력));
//                }
//            }
//            catch { }
//        }

//        private void TroubleHistory_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 에러 이력
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.FaultHistory;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_7_에러이력));
//                }
//            }
//            catch { }
//        }

//        private void PassWord_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 패스워드 변경
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.Password;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_5_패쓰워드));
//                }
//            }
//            catch { }
//        }

//        private void LogSampling_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 로그 추출
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.LogSampling;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_8_Log추출));
//                }
//            }
//            catch { }
//        }

//        private void SetupMaxium_MouseDown(object sender, MouseButtonEventArgs e)
//        {//최대 전류 설정
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.SetupMaxiumCurrent;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_9_최대전력설정));
//                }
//            }
//            catch { }
//        }

//        private void EquipmentInfo_MouseDown(object sender, MouseButtonEventArgs e)
//        {// 강제 충전
//            try
//            {
//                if (Btn_FileSave.IsEnabled)
//                {
//                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.CompulsionCharge;
//                    NavigationService nav = NavigationService.GetNavigationService(this);
//                    nav.Navigate(PageManager.GetPage(PageId._10_10_강제충전));
//                }
//            }
//            catch { }
//        }

//        private void logOut_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (Btn_FileSave.IsEnabled)
//            {
//                //AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
//                //AVRIO.avrio.RunMode = AVRIO.avrio.PrevRunMode;

//                ControlHelper.FindAncestor<Window>(this).Close();

//                //AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
//                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
//                AVRIO.avrio.AdminManuClose = true;
//                //NavigationService nav = NavigationService.GetNavigationService(this);
//                //nav.Navigate(PageManager.GetPage(PageId._08_패스워드입력));
//            }
//            else
//            {
//                MessageBox.Show("Please wait until the download is complete", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//                return;
//            }
//        }
//        #endregion

//        private void Btn_FileSave_Click(object sender, RoutedEventArgs e)
//        {
//           // if (buttonclick)
//            {
//                //  buttonclick = false;




//                DriveInfo[] ListDrives = DriveInfo.GetDrives();
//                DriveInfo usbDrive = null;
//                foreach (DriveInfo drive in ListDrives)
//                {
//                    if (drive.DriveType == DriveType.Removable)
//                    {
//                        usbDrive = drive;
//                        break;
//                    }
//                }

//                if (usbDrive == null)
//                {
//                    MessageBox.Show("No removable device. Please! Check USB device.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

//                    return;
//                }

//                Prog_FileCopy.Visibility = Visibility.Visible;
//                OnPress();
//                USB_Locate = usbDrive.Name;

//                FaultFinish = false;
//                //cbs
//                Thread WriteThread = new Thread(CopyFolder);
//                WriteThread.Start();


//                try
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcRoot = @"D:\QuickChargeApp\";// Directory.GetCurrentDirectory();

//                    string dstRoot = usbDrive.Name;
//                    string bmsRoot = dstRoot + @"BMS\";
//                    string logRoot = dstRoot + @"LOG\";




//                    if (filebms == true)
//                    {

//                        try
//                        {
//                            //cbs
//                            //ThreadStart starter = () => CopyFolder(srcRoot, dstRoot, "BMS");
//                            //Thread WtiteThread = new Thread(starter);
//                            //WtiteThread.Start();



//                            //   CopyFolder(srcRoot, dstRoot, "BMS");


//                            if (System.IO.File.Exists(bmsRoot + logFile))
//                            {
//                                // Use a try block to catch IOExceptions, to
//                                // handle the case of the file already being
//                                // opened by another process.
//                                try
//                                {
//                                    System.IO.File.Delete(bmsRoot + logFile);
//                                }
//                                catch
//                                {

//                                }
//                            }
//                        }
//                        catch { }
//                    }

//                    if (filefault == true)
//                    {
//                        try
//                        {
//                           // CopyFolder(srcRoot, dstRoot, "Fault");
//                        }
//                        catch { }
//                    }

//                    if (filehistory == true)
//                    {
//                        try
//                        {
//                          //  CopyFolder(srcRoot, dstRoot, "History");
//                        }
//                        catch { }
//                    }

//                    if (watchlog)
//                    {
//                        try
//                        {
//                           // CopyFolder(srcRoot, dstRoot, "Process");

//                          //  CopyFolder(srcRoot, dstRoot, "Exception_LOG");

//                           // CopyFolder(srcRoot, dstRoot, "Touch_LOG");

//                        }
//                        catch { }
//                    }


//                    if (filelog == true)
//                    {
//                        try
//                        {
//                            //CopyFolder(srcRoot, dstRoot, "LOG");


//                            if (System.IO.File.Exists(logRoot + logFile))
//                            {
//                                // Use a try block to catch IOExceptions, to
//                                // handle the case of the file already being
//                                // opened by another process.
//                                try
//                                {
//                                    System.IO.File.Delete(logRoot + logFile);
//                                }
//                                catch
//                                {

//                                }
//                            }
//                        }
//                        catch { }
//                    }
//                    //OkDialog dlg = new OkDialog();
//                    //dlg.Owner = ControlHelper.FindAncestor<Window>(this);
//                    //dlg.ShowDialog();

//                }
//                catch (Exception ex)
//                {
//                    AVRIO.avrio.EventMsg = ex.ToString();
//                    MessageBox.Show("File copy fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//                    return;
//                }

//            }
//            //else return;

//        }



//        //cbs
//        public long TotalFileCount = 0;
//        public long RemainlFileCount = 0;


//        void CopyFolder()
//        {
//            try
//            {
//                if (filebms)
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcDir = @"D:\QuickChargeApp\BMS";
//                    string dstDir = USB_Locate + @"BMS\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    string[] files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;

//                        OnProgressChanged(Convert.ToString(SetValue), "BMS Downloading...");                        
//                    }

//                    if (System.IO.File.Exists(dstDir + logFile))
//                    {
//                        // Use a try block to catch IOExceptions, to
//                        // handle the case of the file already being
//                        // opened by another process.
//                        try
//                        {
//                            System.IO.File.Delete(dstDir + logFile);
//                        }
//                        catch
//                        {

//                        }
//                    }

//                }
//                if(filefault)
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcDir = @"D:\QuickChargeApp\FAULT";
//                    string dstDir = USB_Locate + @"FAULT\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    string[] files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "FAULT Downloading...");
//                        Thread.Sleep(1);
//                    }
//                }
//                if (filehistory)
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcDir = @"D:\QuickChargeApp\HISTORY";
//                    string dstDir = USB_Locate + @"HISTORY\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    string[] files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "HISTORY Downloading...");
//                        Thread.Sleep(1);
//                    }
//                }
//                if(watchlog)
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcDir = @"D:\QuickChargeApp\Exception_LOG";
//                    string dstDir = USB_Locate + @"Exception_LOG\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    string[] files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "Exception_LOG Downloading...");
//                        Thread.Sleep(1);
//                    }

//                    if (System.IO.File.Exists(dstDir + logFile))
//                    {
//                        // Use a try block to catch IOExceptions, to
//                        // handle the case of the file already being
//                        // opened by another process.
//                        try
//                        {
//                            System.IO.File.Delete(dstDir + logFile);
//                        }
//                        catch
//                        {

//                        }
//                    }

//                    dt = DateTime.Now;
//                    logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    srcDir = @"D:\QuickChargeApp\Process";
//                    dstDir = USB_Locate + @"Process\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "Process Downloading...");
//                        Thread.Sleep(1);
//                    }

//                    if (System.IO.File.Exists(dstDir + logFile))
//                    {
//                        // Use a try block to catch IOExceptions, to
//                        // handle the case of the file already being
//                        // opened by another process.
//                        try
//                        {
//                            System.IO.File.Delete(dstDir + logFile);
//                        }
//                        catch
//                        {

//                        }
//                    }

//                    dt = DateTime.Now;
//                    logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    srcDir = @"D:\QuickChargeApp\Touch_LOG";
//                    dstDir = USB_Locate + @"Touch_LOG\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "Touch_LOG Downloading...");
//                        Thread.Sleep(1);
//                    }

//                    if (System.IO.File.Exists(dstDir + logFile))
//                    {
//                        // Use a try block to catch IOExceptions, to
//                        // handle the case of the file already being
//                        // opened by another process.
//                        try
//                        {
//                            System.IO.File.Delete(dstDir + logFile);
//                        }
//                        catch
//                        {

//                        }
//                    }

//                }
//                if(filelog)
//                {
//                    DateTime dt = DateTime.Now;
//                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
//                    string srcDir = @"D:\QuickChargeApp\LOG";
//                    string dstDir = USB_Locate + @"LOG\";

//                    if (!Directory.Exists(srcDir))
//                    {
//                        return;
//                    }
//                    if (!Directory.Exists(dstDir))
//                        Directory.CreateDirectory(dstDir);

//                    string[] files = Directory.GetFiles(srcDir, "*.log");

//                    //cbs
//                    TotalFileCount = files.Length;
//                    RemainlFileCount = TotalFileCount;

//                    foreach (string fileName in files)
//                    {
//                        File.Copy(fileName, dstDir + @"\" + System.IO.Path.GetFileName(fileName), true);

//                        //cbs
//                        RemainlFileCount--;
//                        double Percent = RemainlFileCount * 100.0 / TotalFileCount;

//                        int nPer = (int)Percent;
//                        int SetValue = 100;

//                        SetValue = SetValue - nPer;
//                        OnProgressChanged(Convert.ToString(SetValue), "LOG Downloading...");
//                        Thread.Sleep(1);
//                    }
//                }

//                OnComplete();

//            }
//            catch(Exception err)
//            {
//                AVRIO.avrio.EventMsg = err.ToString();
//                MessageBox.Show("File copy fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//                FaultFinish = true;
//                OnComplete();
//                return;
//            }

//        }

//        private void ImgBMS_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (filebms)
//            {
//                filebms = false;
//                this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            }
//            else
//            {
//                filebms = true;
//                this.ImgBMS.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
//            }
//        }

//        private void ImgHistory_MouseDown(object sender, MouseButtonEventArgs e)
//        {

//            if (filehistory)
//            {
//                filehistory = false;
//                this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            }
//            else
//            {
//                filehistory = true;
//                this.ImgHistory.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
//            }
//        }

//        private void ImgFault_MouseDown(object sender, MouseButtonEventArgs e)
//        {

//            if (filefault)
//            {
//                filefault = false;
//                this.ImgFault.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            }
//            else
//            {
//                filefault = true;
//                this.ImgFault.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
//            }
//        }

//        private void imageLog_MouseDown(object sender, MouseButtonEventArgs e)
//        {

//            if (filelog)
//            {
//                filelog = false;
//                this.imageLog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            }
//            else
//            {
//                filelog = true;
//                this.imageLog.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
//            }
//        }

//        //private void btn_WatchDog_Click(object sender, RoutedEventArgs e)
//        //{
//        //    string strTempsss = "";
//        //    try
//        //    {

//        //        string strTemp = btn_WatchDog.Content.ToString();

//        //        if (strTemp == "WatchDog  Enable")
//        //        {
//        //            //WatchDog
//        //            Process[] prs = Process.GetProcesses();
//        //            foreach (Process pr in prs)
//        //            {
//        //                if (pr.ProcessName == "RapidasWatchDog")
//        //                {
//        //                    // pr.Exited += new EventHandler(ProcessExited);
//        //                    strTempsss = "Kill";
//        //                    pr.Kill();

//        //                }
//        //            }

//        //            Thread.Sleep(1000);

//        //            bool Result = false;
//        //            prs = Process.GetProcesses();
//        //            foreach (Process pr in prs)
//        //            {
//        //                if (pr.ProcessName == "RapidasWatchDog")
//        //                {
//        //                    Result = true;
//        //                }
//        //            }


//        //            if (!Result)
//        //            {
//        //                this.btn_WatchDog.Content = "WatchDog  Disable";
//        //            }
//        //            else
//        //            {
//        //                MessageBox.Show("Please Retry Kill");
//        //                return;
//        //            }

//        //        }
//        //        else
//        //        {
//        //            string Names = "RapidasWatchDog.exe";
//        //            string fullPathss = @"C:\WatchDog";
//        //            Process ps2 = new Process();

//        //            ps2.StartInfo.FileName = Names;
//        //            ps2.StartInfo.WorkingDirectory = fullPathss;
//        //            ps2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
//        //            // ps2.EnableRaisingEvents = true;

//        //            strTempsss = "Start WatchDog";
//        //            ps2.Start();
//        //            this.btn_WatchDog.Content = "WatchDog  Enable";
//        //        }
//        //    }
//        //    catch
//        //    {
//        //        MessageBox.Show("Process Err : " + strTempsss);
//        //        return;
//        //    }
//        //}

//        private void ImgWatchdog_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (watchlog)
//            {
//                watchlog = false;
//                this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckFalse.png", UriKind.Relative));
//            }
//            else
//            {
//                watchlog = true;
//                this.ImgWatchdog.Source = new BitmapImage(new Uri("image/CheckTrue.png", UriKind.Relative));
//            }
//        }


//        private void RecCharging_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            KeyPadWindow window = new KeyPadWindow();
//            window.Owner = ControlHelper.FindAncestor<Window>(this);

//            if (window.ShowDialog() == true)
//            {
//                int temperture;
//                if (int.TryParse(window.InputValue, out temperture))
//                {
//                    if (temperture > 25 || temperture < 0)
//                    {
//                        MessageBox.Show("Input Number Fail!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//                    }
//                    else
//                    {
//                        if (temperture < 10)
//                        {
//                            this.Txtemperuter.Text = "0";
//                            this.Txtemperuter.Text += temperture.ToString();
//                        }
//                        else
//                        {
//                            this.Txtemperuter.Text = temperture.ToString();
//                        }


//                        AVRIO.avrio.FanOffset =(ushort)temperture;

//                        RegistryKey reg;
//                        reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");

//                        reg.SetValue("FANTEMPOFFSET", AVRIO.avrio.FanOffset);
//                        QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "FANTEMPOFFSET", AVRIO.avrio.FanOffset.ToString());

//                        reg = null;
//                    }
//                }
//            }
//        }


//    }
//}
#endregion
