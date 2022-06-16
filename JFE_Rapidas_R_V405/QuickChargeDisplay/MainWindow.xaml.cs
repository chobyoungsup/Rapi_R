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
using Microsoft.Win32;
using System.Media;
using System.Timers;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace QuickChargeDisplay
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public class Win32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
        public static void PreventScreenAndSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS |
                                    EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                    EXECUTION_STATE.ES_AWAYMODE_REQUIRED |
                                    EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }
        public static void AllowMonitorPowerdown()
        {
            Console.WriteLine(SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS));
        }
    } 

    public partial class MainWindow : Window
    {
        private System.Timers.Timer timer = null;
        private System.Timers.Timer hidetimer;

        //TEST 중 대기화면 터치 시 콘솔창이 보임 (win10)


        [DllImport("Kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_MINIMIZE = 6;
        const int SW_RESTORE = 9;

        public MainWindow()
        {
            InitializeComponent();
            DisableWPFTabletSupport();
            AVRIO.avrio.SysDataChanged += new AVRIO.SysDataChangedEvent(avrio_SysDataChanged);
            Win32.PreventScreenAndSleep();

            //cbs 2020.04.02 주석처리
            //// 마우스 커서 없애기
            // AVRIO.avrio.nMouseCuser = true;
            //if (AVRIO.avrio.nMouseCuser)
            //this.Cursor = Cursors.None;
        }
        void avrio_SysDataChanged(string dataName, object value)
        {

        //    switch (dataName)
        //    {
        //        case "SoundPlayQS":
         //           {
         //               SoundControl((string)value);
         //           }
         //           break;
         //   }
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Win32.AllowMonitorPowerdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "MainWindow_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            string subRoot = "AVRIO";
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
            int Cardcheck = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "CARDCHECK", "0"));
            int Passcheck = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "PASSWORD", "0"));
            int BusinessCheck = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "BusinessCheck", "0"));

            try
            {
                AVRIO.avrio.nSbcVersion = Convert.ToInt32(reg.GetValue("SBCVERSION", "200"));
            }
            catch {             
            }
            string s_ADMINPASS = QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "ADMINPASS", "1234");
            string s_MANUPASS = QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "MANUALPASS", "8611");
            string s_CHARGINGPASS = QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "CHARGINGPASS", "0000");
            string s_LANGUAGE = QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "LANGUAGE", "0");
            string s_CHARGETIME = QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "CHARGETIME", "30");
            reg.SetValue("ADMINPASS", s_ADMINPASS);
            reg.SetValue("MANUALPASS", s_MANUPASS);
            reg.SetValue("CHARGINGPASS", s_CHARGINGPASS);
            reg.SetValue("LANGUAGE", s_LANGUAGE);

            if (BusinessCheck == 0)
            {
                AVRIO.avrio.BusinessCheckbar = 0;
            }
            else
            {
                AVRIO.avrio.BusinessCheckbar = 1;
            }

            if (Cardcheck == 0)
            {
                AVRIO.avrio.Rapidascardcheck = 0;
            }
            else
            {
                AVRIO.avrio.Rapidascardcheck = 1;
            }

            if (Passcheck == 0)
            {
                AVRIO.avrio.Rapidaspassword = 0;
            }
            else
            {
                AVRIO.avrio.Rapidaspassword = 1;
            }

            if (Convert.ToInt32(s_LANGUAGE) == 0)
            {
                AVRIO.avrio.Rapidaslanguage = 0;
            }
            else
            {
                AVRIO.avrio.Rapidaslanguage = 1;
            }

            AVRIO.avrio.BusinessStimeHH = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "BusinessStimeHHH", "09"));
            AVRIO.avrio.BusinessStimeMM = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "BusinessStimeMMM", "00"));
            AVRIO.avrio.BusinessEtimeHH = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "BusinessEtimeHHH", "24"));
            AVRIO.avrio.BusinessEtimeMM = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "BusinessEtimeMMM", "00"));

            reg.SetValue("BusinessCheck", AVRIO.avrio.BusinessCheckbar); reg.SetValue("PASSWORD", AVRIO.avrio.Rapidaspassword);
            reg.SetValue("CARDCHECK", AVRIO.avrio.Rapidascardcheck);
            reg.SetValue("BusinessStimeHHH", AVRIO.avrio.BusinessStimeHH);
            reg.SetValue("BusinessStimeMMM", AVRIO.avrio.BusinessStimeMM);
            reg.SetValue("BusinessEtimeHHH", AVRIO.avrio.BusinessEtimeHH);
            reg.SetValue("BusinessEtimeMMM", AVRIO.avrio.BusinessEtimeMM);


            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("AVRIO");
            reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
            reg.SetValue("CHARGETIME", s_CHARGETIME);

            AVRIO.avrio.GridOnlyMode = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "GridOnlyMode", "false"));
            AVRIO.avrio.GridInputPowerMax = Convert.ToUInt16(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "GridInputPowerMax", "2800")); //Convert.ToUInt16(reg.GetValue("GridInputPowerMax", "2800"));
            reg.SetValue("GridOnlyMode", AVRIO.avrio.GridOnlyMode);
            reg.SetValue("GridInputPowerMax", AVRIO.avrio.GridInputPowerMax);

            AVRIO.avrio.Rapidasweekreboot = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "REBOOTWEEK", "1"));
            AVRIO.avrio.RebootTimehh = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "REBOOTTIMEHH", "0"));
            AVRIO.avrio.RebootTimemm = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "REBOOTTIMEMM", "0"));
            AVRIO.avrio.FanOffset = Convert.ToUInt16(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "FANTEMPOFFSET", "5"));


            reg.SetValue("REBOOTWEEK", AVRIO.avrio.Rapidasweekreboot);
            reg.SetValue("REBOOTTIMEHH", AVRIO.avrio.RebootTimehh);
            reg.SetValue("REBOOTTIMEMM", AVRIO.avrio.RebootTimemm);
            reg.SetValue("FANTEMPOFFSET", AVRIO.avrio.FanOffset);



            //AVRIO.avrio.EventMsg = "33311111111111111111111111111111111111";
#if DEBUG
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = -500;
#else
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
#endif
            
            QCDV.BmsInfo.Visible = System.Windows.Visibility.Hidden;            
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;            
            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
            // 대기화면부터 충전완료 화면까지 전체 Loading Test
            // 해당 리소스( 사운드, 동영상 ) 파일이 없을시 디버깅 에러
            for (int i = 0; i < 10; i++)
            {
                MainFrame.Navigate(PageManager.GetPage((PageId)i));
            }
            MainFrame.Navigate(PageManager.GetPage(PageId._01_대기화면));


            hidetimer = new System.Timers.Timer(1000);
            hidetimer.Elapsed += new ElapsedEventHandler(hidetimer_Elapsed);
            hidetimer.AutoReset = false;
            hidetimer.Start();
                       
        }

        void hidetimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        public void SetPage(PageId id)
        {
            MainFrame.Navigate(PageManager.GetPage(id));
        }
       
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch( e.Key )
            {
                case Key.D0:
                    //SetPage(PageId._00_멈춤화면);
                    break;

                case Key.Q:
                    // AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                    // AVRIO.avrio.EventMsg = "BMSMainContactor_1";
                    //SetPage(PageId._01_대기화면);
                    break;

                case Key.W:
                    //AVRIO.avrio.IsControlBDComm = true;
                    //AVRIO.avrio.IsBMSCanComm = true;
                    // AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                    // AVRIO.avrio.EventMsg = "BMSMainContactor_0";
                    //SetPage(PageId._02_카드체크);
                    // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsFinish;
                    // AVRIO.bmsio.b_CanThreadfinish = true;
                    /*
                    AVRIO.avrio.CpStat = 1;
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeStart;                                
                    SetPage(PageId._02_카드체크);
                    */ 
                    break;

                case Key.D1:
                    // AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFinish;
                   // SetPage(PageId._02_7_시작패스워드);
                    break;

                case Key.D2:
                    // AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFinish;
                    //SetPage(PageId._03_커넥터연결);
                    break;

                case Key.D3:
                    //AVRIO.avrio.IsControlBDComm = false;
                    //AVRIO.avrio.IsBMSCanComm = true;
                    // AVRIO.bmsio.byBMSStatus = 3;  
                    // AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_1";  
                    // AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                    // SetPage(PageId._04_충전중화면);
                    // AVRIO.avrio.EventMsg = "화면전환이벤트1,100";
                    //SetPage(PageId._03_접속확인중);
                    break;

                case Key.D4:
                   // AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";  
                    //AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;                    
                    // AVRIO.avrio.EventMsg = "BMS Fault 초기화";
                    // AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                    // SetPage(PageId._04_2_충전종료화면);
                   // SetPage(PageId._05_0_절연저항측정화면);
                    break;

                case Key.D5:
                    // AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFinish;
                    //SetPage(PageId._05_1_충전중화면);
                    break;

                case Key.D6:
                    // AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.TimeSync;
                    //SetPage(PageId._05_2_충전종료처리중);
                    // SetPage(PageId._07_충전량표시);
                    break;    
                    
                case Key.D7:
                    //SetPage(PageId._06_커넥터분리);
                    break;

                case Key.D8:
                    //SetPage(PageId._07_결재확인);
                    break;

                case Key.D9:
                    //SetPage(PageId._08_충전완료);
                    break;

                case Key.F1:
                    //SetPage(PageId._09_패스워드입력);
                    break;

                case Key.F2:
                    //SetPage(PageId._10_0_관리자메뉴);
                    break;

                case Key.F3:
                    //SetPage(PageId._10_1_관리설정);
                    break;


                case Key.F4:
                    //SetPage(PageId._10_2_수전전력설정);
                    break;


                case Key.T:
                    //SetPage(PageId._10_3_일시설정);
                    break;


                case Key.F6:
                    //SetPage(PageId._10_4_언어설정);
                    break;


                case Key.F7:
                    //SetPage(PageId._10_5_패쓰워드);
                    break;


                case Key.F8:
                    //SetPage(PageId._10_6_충전이력);
                    break;

                case Key.F10:
                    //SetPage(PageId._02_4_카드에러);
                    break;
                case Key.F11:
                    //SetPage(PageId._02_5_카드에러);
                    break;
                case Key.Z:
                    //SetPage(PageId._02_6_카드에러);
                    break;
                case Key.I:
                    //SetPage(PageId._10_7_에러이력);
                    break;


                case Key.O:
                    //SetPage(PageId._10_8_Log추출);
                    break;


                case Key.P:
                    //SetPage(PageId._10_9_최대전력설정);
                    break;

                case Key.F12:
                    //SetPage(PageId._10_10_강제충전);
                    break;


                case Key.PageUp:
                    {
                        //double value = QCDV.Charging.ChargingWatt;

                        //value += 10.1;

                        //if (value > 100.0)
                        //    value = 100;

                        //QCDV.Charging.ChargingWatt = value;
                        //QCDV.ChargeMoney.ChargedSoc = value;
                    }
                    break;

                case Key.PageDown:
                    {
                        //double value = QCDV.Charging.ChargingWatt;

                        //value -= 10.1;

                        //if (value < 0.0)
                        //    value = 0.0;

                        //QCDV.Charging.ChargingWatt = value;    
                        //QCDV.ChargeMoney.ChargedSoc = value;
                    }
                    break;
                case Key.S:
                    {
                        var handle = GetConsoleWindow();
                        ShowWindow(handle, SW_SHOW);
                    }
                    break;
                case Key.H:
                    {
                        var handle = GetConsoleWindow();
                        ShowWindow(handle, SW_HIDE);
                    }
                    break;
                default:
                    break;
            }            
        }
        
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {   
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            this.Dispatcher.BeginInvoke((ThreadStart)delegate() { CloseWindow(); });
        }

        public void ShowFaultDialog()
        {
            if (AVRIO.avrio.IsFaultDialog) 
                //if (!AVRIO.avrio.IsFaultDialog && !AVRIO.avrio.IsFaultDialog)
            {
                FaultDialog dlg = new FaultDialog();
                dlg.ShowDialog();
            }
        }

        public static void DisableWPFTabletSupport()
        { // Get a collection of the tablet devices for this window. 
            
            TabletDeviceCollection devices = System.Windows.Input.Tablet.TabletDevices;
            if (devices.Count > 0)
            { // Get the Type of InputManager. 
            
                Type inputManagerType = typeof(System.Windows.Input.InputManager);
                // Call the StylusLogic method on the InputManager.Current instance. 
                object stylusLogic = inputManagerType.InvokeMember("StylusLogic", BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, InputManager.Current, null);
                if (stylusLogic != null)
                { // Get the type of the stylusLogic returned from the call to StylusLogic. 
                    Type stylusLogicType = stylusLogic.GetType(); // Loop until there are no more devices to remove. 
                  

                    while (devices.Count > 0)
                    { // Remove the first tablet device in the devices collection. 
                        stylusLogicType.InvokeMember("OnTabletRemoved", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, stylusLogic, new object[] { (uint)0 });
                      
                    }
                }
            }
        }

        private void CloseWindow()
        {
            Win32.AllowMonitorPowerdown();   
            this.Close();
        }

#region sound control
        private SoundPlayer soundPlayer = new SoundPlayer();

        public void SoundControl(string code)
        {
            //Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "voice/대기화면_volumeup3.wav");
            //string location = "Voice/" + code + ".wav";
            //try
            //{
            //    soundPlayer.SoundLocation = location;
            //    soundPlayer.Load();
            //    soundPlayer.Play();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);           
        }
    }
}
