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
using System.Drawing;
using System.Windows.Resources;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Media;
using System.Threading;
using System.Timers;
using QuickChargeConfig;
using Microsoft.Win32;

namespace QuickChargeDisplay
{
    /// <summary>
    /// _00_1_CardCheckPage.xaml에 대한 상호 작용 논리
    /// DSP 와 SBC 버전이 틀리면 자동 내부 배터리 충전 및
    /// 충전 시작이 안된다.
    /// </summary>

    public partial class StandByPage1 : Page
    {

        private System.Timers.Timer chargeReadyChkTimer;
        private ChargeWarnningDialog dlg = null;
        private System.Timers.Timer timer;
        private System.Timers.Timer AutoBattryCharge;
        private System.Timers.Timer WatchDog;
        private System.Timers.Timer BussinessTime;
        private System.Timers.Timer StartTimer;
        private System.Timers.Timer ButtonDelayTimer;
        private System.Timers.Timer StartCardcheckTimer;
        private System.Timers.Timer StartPasscheckTimer;
        private System.Timers.Timer StanbyCardCheckTimer;

        private System.Timers.Timer Apptimer;

        //cbs
        int pollCount = 0;

        public StandByPage1()
        {
            Thread.Sleep(3000);  
            InitializeComponent();

           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "StandByPage1_Loaded ";


            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;
            //cbs
            pollCount = 0;

            AVRIO.avrio.nSelectCommand12 = 5;
            if (AVRIO.avrio.RunMode != AVRIO.RunningMode.Admin)
            {

                AVRIO.avrio.StopButtonPlay = false;
                AVRIO.avrio.StartButtonPlay = false;

                if (((AVRIO.avrio.rCommandValue & 8) > 0))
                {
                    AVRIO.avrio.EventMsg = "Page_Loaded !!!  TsBatteryFinish  ";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                }
                else
                {
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                    AVRIO.avrio.EventMsg = "Page_Loaded !!!  TsStandby  ";
                }

                AutoBattryCharge = new System.Timers.Timer(1000 * 2);
                AutoBattryCharge.Elapsed += new ElapsedEventHandler(timer_AutoCharge);
                AutoBattryCharge.AutoReset = true;
                AutoBattryCharge.Enabled = true;

                AVRIO.avrio.OnebatteryCharge = false;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;

                WatchDog = new System.Timers.Timer(200);
                WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
                WatchDog.AutoReset = true;

                BussinessTime = new System.Timers.Timer(1000);
                BussinessTime.Elapsed += new ElapsedEventHandler(timer_BussinessTime);
                BussinessTime.AutoReset = true;
                BussinessTime.Enabled = true;

                StartTimer = new System.Timers.Timer(200);
                StartTimer.Elapsed += new ElapsedEventHandler(timer_StartTimer);
                StartTimer.AutoReset = true;
                StartTimer.Enabled = false;

                ButtonDelayTimer = new System.Timers.Timer(1000);
                ButtonDelayTimer.Elapsed += new ElapsedEventHandler(ButtonDelayTimer_Elapsed);
                ButtonDelayTimer.AutoReset = false;
                ButtonDelayTimer.Start();

                StartCardcheckTimer = new System.Timers.Timer(300);
                StartCardcheckTimer.Elapsed += new ElapsedEventHandler(StartCardcheckTimer_Elapsed);
                StartCardcheckTimer.AutoReset = true;

                StartPasscheckTimer = new System.Timers.Timer(300);
                StartPasscheckTimer.Elapsed += new ElapsedEventHandler(StartPasscheckTimer_Elapsed);
                StartPasscheckTimer.AutoReset = true;

                StanbyCardCheckTimer = new System.Timers.Timer(300);
                StanbyCardCheckTimer.Elapsed += new ElapsedEventHandler(StanbyCardCheckTimer_Elapsed);
                StanbyCardCheckTimer.AutoReset = true;

                Apptimer = new System.Timers.Timer(500);
                Apptimer.Elapsed += new ElapsedEventHandler(Apptimer_Elapsed);
                Apptimer.AutoReset = true;
                Apptimer.Start();

                AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargingStat;
                AVRIO.avrio.CardCheckFlag = false;
                
                if (AVRIO.avrio.Rapidaslanguage == 0)
                {
                    Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/01a01b.gif");
                    Bitmap bitmap = new Bitmap(stream);
                    BackImage.LoadSmile(bitmap);
                    BackImage.StopAnimate();
                    stream.Dispose();
                }
                else
                {
                    Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/01a_E01b.gif");
                    Bitmap bitmap = new Bitmap(stream);
                    BackImage.LoadSmile(bitmap);
                    BackImage.StopAnimate();
                    stream.Dispose();
                }
                try
                {
                    BackImage.StartAnimate();
                }
                catch { }

                if (AVRIO.avrio.BussinessFalg == 2)
                {
                    try
                    {
                        this.Dispatcher.Invoke((ThreadStart)delegate()
                        {
                            NavigationService nav = NavigationService.GetNavigationService(this);
                            nav.Navigate(PageManager.GetPage(PageId._00_멈춤화면));
                        });
                    }
                    catch { }
                }
                else if (AVRIO.avrio.BusinessCheckbar == 1)
                {
                    double NowTime = Convert.ToDouble(System.DateTime.Now.ToString("HHmm"));
                    string firsttime = Convert.ToString(AVRIO.avrio.BusinessStimeHH);
                    if (AVRIO.avrio.BusinessStimeMM < 10)
                    {
                        firsttime += "0";
                    }
                    firsttime += Convert.ToString(AVRIO.avrio.BusinessStimeMM);
                    double TimeWattTime_1 = Convert.ToDouble(firsttime);

                    string firsttime1 = Convert.ToString(AVRIO.avrio.BusinessEtimeHH);
                    if (AVRIO.avrio.BusinessEtimeMM < 10)
                    {
                        firsttime1 += "0";
                    }

                    firsttime1 += Convert.ToString(AVRIO.avrio.BusinessEtimeMM);
                    double TimeWattTime_2 = Convert.ToDouble(firsttime1);

                    if ((NowTime < TimeWattTime_1) || (NowTime > TimeWattTime_2))
                    {
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._00_멈춤화면));
                            });
                        }
                        catch { 
                        
                        }
                    }
                    else
                    {
                        AVRIO.avrio.BusinessTime = false; // 혹쉬나 시나리오 시작 함수 막는 부분
                        if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal)

                            if (AVRIO.avrio.IsControlBDComm == false ||
                                AVRIO.avrio.IsBMSCanComm == false ||
                                AVRIO.bmsio.byBMSStatus != 3
                              //  AVRIO.avrio.nDspVersion != AVRIO.avrio.nSbcVersion
                            )
                            {
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;

                                AVRIO.avrio.WarnningType = AVRIO.WARNNINGTYPE.Charge_Ready;

                                dlg = new ChargeWarnningDialog();
                               dlg.Show();

                                chargeReadyChkTimer = new System.Timers.Timer(1000);
                                chargeReadyChkTimer.Elapsed += new ElapsedEventHandler(chargeReadyChkTimer_Elapsed);
                                chargeReadyChkTimer.AutoReset = true;
                                chargeReadyChkTimer.Start();
                            }
                    }
                }
                else if (AVRIO.avrio.BussinessFalg == 1)
                {
                    if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal)

                        if (AVRIO.avrio.IsControlBDComm == false ||
                            AVRIO.avrio.IsBMSCanComm == false ||
                            AVRIO.bmsio.byBMSStatus != 3 
                            )
                        {

                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                            AVRIO.avrio.WarnningType = AVRIO.WARNNINGTYPE.Charge_Ready;
                            dlg = new ChargeWarnningDialog();
                            dlg.Show();

                            chargeReadyChkTimer = new System.Timers.Timer(1000);
                            chargeReadyChkTimer.Elapsed += new ElapsedEventHandler(chargeReadyChkTimer_Elapsed);
                            chargeReadyChkTimer.AutoReset = true;
                            chargeReadyChkTimer.Start();
                        }
                }
                else
                {
                    AVRIO.avrio.BusinessTime = false; // 혹쉬나 시나리오 시작 함수 막는 부분
                    if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal)

                        if (AVRIO.avrio.IsControlBDComm == false ||
                            AVRIO.avrio.IsBMSCanComm == false ||
                            AVRIO.bmsio.byBMSStatus != 3 //||
                          //  AVRIO.avrio.nDspVersion != AVRIO.avrio.nSbcVersion
                            )
                        {
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;

                            AVRIO.avrio.WarnningType = AVRIO.WARNNINGTYPE.Charge_Ready;
                            dlg = new ChargeWarnningDialog();
                            dlg.Show();

                            chargeReadyChkTimer = new System.Timers.Timer(1000);
                            chargeReadyChkTimer.Elapsed += new ElapsedEventHandler(chargeReadyChkTimer_Elapsed);
                            chargeReadyChkTimer.AutoReset = true;
                            chargeReadyChkTimer.Start();
                        }

                }                
                Thread.Sleep(300);
            }

            if ((AVRIO.bmsio.byBMSStatus != 3))// && (QCDV.ManualControl.CurrentBatStatus == 0) )
            {
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        //cbs
        void Apptimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((ThreadStart)delegate ()
            {
                pollCount %= 2;
                if (pollCount == 0)
                {
                    this.Appstatus.Fill = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    this.Appstatus.Fill = System.Windows.Media.Brushes.Transparent;
                }
                pollCount++;
            });
        }
        void StanbyCardCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.bmsio.byBMSStatus == 3)
            {
                if (AVRIO.avrio.DspLgBatteryStatus == 0)
                {
                    if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                    {
                        if (AVRIO.avrio.CardCheckFlag)
                        {
                            AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => TsVehicleReady;";
                            AVRIO.avrio.CardCheckFlag = false;
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                        }
                        if (AVRIO.avrio.CardCheckFlag4)
                        {
                            AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_4_카드에러;";
                            AVRIO.avrio.CardCheckFlag4 = false;
                            try
                            {
                                this.Dispatcher.Invoke((ThreadStart)delegate()
                                {
                                    NavigationService nav = NavigationService.GetNavigationService(this);
                                    nav.Navigate(PageManager.GetPage(PageId._02_4_카드에러));
                                });
                            }
                            catch { }
                        }
                        if (AVRIO.avrio.CardCheckFlag5)
                        {
                            AVRIO.avrio.CardCheckFlag5 = false;
                            AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_5_카드에러;";
                            try
                            {
                                this.Dispatcher.Invoke((ThreadStart)delegate()
                                {
                                    NavigationService nav = NavigationService.GetNavigationService(this);
                                    nav.Navigate(PageManager.GetPage(PageId._02_5_카드에러));
                                });
                            }
                            catch { }
                        }
                        if (AVRIO.avrio.CardCheckFlag6)
                        {
                            AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_6_카드에러;";
                           
                            AVRIO.avrio.CardCheckFlag6 = false;
                            try
                            {
                                this.Dispatcher.Invoke((ThreadStart)delegate()
                                {
                                    NavigationService nav = NavigationService.GetNavigationService(this);
                                    nav.Navigate(PageManager.GetPage(PageId._02_6_카드에러));
                                });
                            }
                            catch { }
                        }
                    }
                }
                else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                {
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                }
                else
                {
                    if (AVRIO.avrio.CardCheckFlag)
                    {
                        AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => TsVehicleReady;";
                           
                        AVRIO.avrio.CardCheckFlag = false;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                    }

                    if (AVRIO.avrio.CardCheckFlag4)
                    {
                        AVRIO.avrio.CardCheckFlag4 = false;
                        AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_4_카드에러";
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_4_카드에러));
                            });
                        }
                        catch { }
                    }
                    if (AVRIO.avrio.CardCheckFlag5)
                    {
                        AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_5_카드에러";
                       
                        AVRIO.avrio.CardCheckFlag5 = false;
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_5_카드에러));
                            });
                        }
                        catch { }
                    }
                    if (AVRIO.avrio.CardCheckFlag6)
                    {
                        AVRIO.avrio.CardCheckFlag6 = false;
                        AVRIO.avrio.EventMsg = "StanbyCardCheckTimer_Elapsed => _02_6_카드에러";
                       
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_6_카드에러));
                            });
                        }
                        catch { }
                    }
                }
            }
            else
            {
                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        void timer_BussinessTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.BussinessFalg == 2)
            {
                try
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate()
                    {
                        NavigationService nav = NavigationService.GetNavigationService(this);
                        nav.Navigate(PageManager.GetPage(PageId._00_멈춤화면));
                    });
                }
                catch { }
            }
            else if (AVRIO.avrio.BusinessCheckbar == 1)
            {
                double NowTime = Convert.ToDouble(System.DateTime.Now.ToString("HHmm"));
                string firsttime = Convert.ToString(AVRIO.avrio.BusinessStimeHH);
                if (AVRIO.avrio.BusinessStimeMM < 10)
                {
                    firsttime += "0";
                }
                firsttime += Convert.ToString(AVRIO.avrio.BusinessStimeMM);
                double TimeWattTime_1 = Convert.ToDouble(firsttime);

                string firsttime1 = Convert.ToString(AVRIO.avrio.BusinessEtimeHH);
                if (AVRIO.avrio.BusinessEtimeMM < 10)
                {
                    firsttime1 += "0";
                }

                firsttime1 += Convert.ToString(AVRIO.avrio.BusinessEtimeMM);
                double TimeWattTime_2 = Convert.ToDouble(firsttime1);

                if ((NowTime < TimeWattTime_1) || (NowTime > TimeWattTime_2))
                {
                    try
                    {
                        this.Dispatcher.Invoke((ThreadStart)delegate()
                        {
                            NavigationService nav = NavigationService.GetNavigationService(this);
                            nav.Navigate(PageManager.GetPage(PageId._00_멈춤화면));
                        });
                    }
                    catch { }
                }
                else
                {
                    AVRIO.avrio.BusinessTime = false; // 혹쉬나 시나리오 시작 함수 막는 부분
                }
            }
            else
            {
                AVRIO.avrio.BusinessTime = false; // 혹쉬나 시나리오 시작 함수 막는 부분
            }
        }

        void ButtonDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            WatchDog.Enabled = true;
        }

        void StartCardcheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        { //카드 설정후 시작 또는 Start버튼
            if (AVRIO.avrio.Rapidascardcheck == 1)
            {
                if (AVRIO.bmsio.byBMSStatus == 3)
                {
                    if (AVRIO.avrio.DspLgBatteryStatus == 0)
                    {
                        if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                        {
                            AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => _02_카드체크;";
                            try
                            {
                                this.Dispatcher.Invoke((ThreadStart)delegate()
                                {
                                    NavigationService nav = NavigationService.GetNavigationService(this);
                                    nav.Navigate(PageManager.GetPage(PageId._02_카드체크));
                                });
                            }
                            catch { }
                        }
                    }
                    else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                    {
                        AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => TsBatteryFinish;";
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                    }
                    else
                    {
                        AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => _02_카드체크;";
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_카드체크));
                            });
                        }
                        catch { }                      
                    }
                }
                else
                {
                  //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;                    
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                }  

            }else
            if (AVRIO.avrio.Rapidaspassword == 1)
            {
                if (AVRIO.bmsio.byBMSStatus == 3)
                {
                    if (AVRIO.avrio.DspLgBatteryStatus == 0)
                    {
                        if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                        {
                            AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => _02_7_시작패스워드;";
                            try
                            {
                                this.Dispatcher.Invoke((ThreadStart)delegate()
                                {
                                    NavigationService nav = NavigationService.GetNavigationService(this);
                                    nav.Navigate(PageManager.GetPage(PageId._02_7_시작패스워드));
                                });
                            }
                            catch { }
                        }
                    }
                    else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                    {
                        AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => TsBatteryFinish;";
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                    }
                    else
                    {
                        AVRIO.avrio.EventMsg = "StartCardcheckTimer_Elapsed => _02_7_시작패스워드;";
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_7_시작패스워드));
                            });
                        }
                        catch { }
                    }
                }
                else
                {
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                 //   AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                  //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                }
            }
            else
            {
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
               // AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
              //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            }
        }

        void StartPasscheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {//PassWord 설정후 시작 또는 Start버튼
            if (AVRIO.bmsio.byBMSStatus == 3)
            {
                if (AVRIO.avrio.DspLgBatteryStatus == 0)
                {
                    if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                    {
                        AVRIO.avrio.EventMsg = "StartPasscheckTimer_Elapsed => _02_7_시작패스워드;";
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_7_시작패스워드));
                            });
                        }
                        catch { }
                    }
                }
                else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                {
                    AVRIO.avrio.EventMsg = "StartPasscheckTimer_Elapsed => AVRIO.TsCommand.TsBatteryFinish;";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                }

                else
                {
                    AVRIO.avrio.EventMsg = "StartPasscheckTimer_Elapsed => _02_7_시작패스워드;";
                    try
                    {
                        this.Dispatcher.Invoke((ThreadStart)delegate()
                        {
                            NavigationService nav = NavigationService.GetNavigationService(this);
                            nav.Navigate(PageManager.GetPage(PageId._02_7_시작패스워드));
                        });
                    }
                    catch { }
                }
            }
            else
            {

              //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        void timer_StartTimer(object sender, System.Timers.ElapsedEventArgs e)
        { //인증 없이 시작 또는 Start버튼
            if (AVRIO.bmsio.byBMSStatus == 3)
            {
                if (AVRIO.avrio.DspLgBatteryStatus == 0)
                {
                    if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                    {
                        AVRIO.avrio.EventMsg = "timer_StartTimer => AVRIO.TsCommand.TsVehicleReady;";
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                    }
                }
                else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                {
                    AVRIO.avrio.EventMsg = "timer_StartTimer => AVRIO.TsCommand.TsBatteryFinish;";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                }
                else
                {
                    AVRIO.avrio.EventMsg = "timer_StartTimer => AVRIO.TsCommand.TsVehicleReady;";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                }
            }
            else
            {
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        bool onecheck = false;

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.AdminPage != AVRIO.AdminPage.AdminPage)
            {
                if (AVRIO.avrio.Rapidascardcheck == 1)
                {
                    if (AVRIO.avrio.CardCheckFlag)// 카드 인증 완료..
                    {
                        if (AVRIO.avrio.DspLgBatteryStatus == 2)
                        {
                            try
                            {
                                AutoBattryCharge.Enabled = false;
                            }
                            catch { }
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                           // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            StartTimer.Enabled = true;
                        }
                        else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                        {
                            try
                            {
                                AutoBattryCharge.Enabled = false;
                            }
                            catch { }
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                            StartTimer.Enabled = true;
                        }
                        else
                        {
                            try
                            {
                                AutoBattryCharge.Enabled = false;
                            }
                            catch { }
                            if (AVRIO.avrio.DspLgBatteryStatus == 1)
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                           // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            StartTimer.Enabled = true;
                        }
                    }
                    if (AVRIO.avrio.CardCheckFlag4)
                    {
                        AVRIO.avrio.CardCheckFlag4 = false;
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_4_카드에러));
                            });
                        }
                        catch { }
                    }
                    if (AVRIO.avrio.CardCheckFlag5)
                    {
                        AVRIO.avrio.CardCheckFlag5 = false;
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_5_카드에러));
                            });
                        }
                        catch { }
                    }
                    if (AVRIO.avrio.CardCheckFlag6)
                    {
                        AVRIO.avrio.CardCheckFlag6 = false;
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._02_6_카드에러));
                            });
                        }
                        catch { }
                    }
                }
                if ((AVRIO.avrio.StartButton || AVRIO.avrio.StartButtonPlay) && !onecheck)
                {
                    if(!AVRIO.avrio.Fualtstop  &&  !AVRIO.avrio.IsFaultDialog && !AVRIO.avrio.FualtstopFalg)
 
                        onecheck = true;

                    if (!AVRIO.avrio.Fualtstop && !AVRIO.avrio.IsFaultDialog && !AVRIO.avrio.FualtstopFalg)
                        {
                            if (AVRIO.avrio.Rapidascardcheck == 1)
                            {
                                AVRIO.avrio.EventMsg = "Button start !!!  Play_Funtion1   ";
                                Play_Funtion1();
                            }
                            else
                            {
                                AVRIO.avrio.EventMsg = "Button start !!!  Play_Funtion  ";
                                Play_Funtion();
                            }
                        }
                    onecheck = false;
                    AVRIO.avrio.StopButtonPlay = false;
                    AVRIO.avrio.StartButtonPlay = false;
                }

                if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
                {
                    if (AVRIO.avrio.DspLgBatteryStatus == 2)
                    {
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                        AVRIO.avrio.StopButtonPlay = false;
                        AVRIO.avrio.StartButtonPlay = false;
                    }
                    else
                    {
                        if (((AVRIO.avrio.rCommandValue & 8) > 0))
                        {
                            AVRIO.avrio.EventMsg = "Stop Button !!!  TsBatteryFinish  ";
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                        }
                        else
                        {
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            AVRIO.avrio.EventMsg = "Stop Button !!!  TsStandby  ";
                        }

                        AVRIO.avrio.StopButtonPlay = false;
                        AVRIO.avrio.StartButtonPlay = false;
                    }
                }
                AVRIO.avrio.StopButtonPlay = false;
                AVRIO.avrio.StartButtonPlay = false;
            }
            if (AVRIO.bmsio.bySOC == 0 && (AVRIO.bmsio.byBMSStatus != 3))
            {

                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            AVRIO.avrio.CardCheckFlag = false;

            BackImage.StopAnimate();
            BackImage.Dispose();

            try
            {
                if (StanbyCardCheckTimer != null)
                {
                    StanbyCardCheckTimer.Enabled = false;
                    StanbyCardCheckTimer.Stop();
                    StanbyCardCheckTimer.Dispose();
                    StanbyCardCheckTimer = null;
                }
            }
            catch { }

            try
            {
                //StartcheckTimer
                if (timer != null)
                {
                    timer.Enabled = false;
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
            catch { }

            try
            {
                if (BussinessTime != null)
                {
                    BussinessTime.Enabled = false;
                    BussinessTime.Stop();
                    BussinessTime.Dispose();
                    BussinessTime = null;
                }
            }
            catch { }

            try
            {
                if (AutoBattryCharge != null)
                {
                    AutoBattryCharge.Enabled = false;
                    AutoBattryCharge.Stop();
                    AutoBattryCharge.Dispose();
                    AutoBattryCharge = null;
                }
            }
            catch { }

            try
            {
                if (WatchDog != null)
                {
                    WatchDog.Enabled = false;
                    WatchDog.Stop();
                    WatchDog.Dispose();
                    WatchDog = null;
                }
            }
            catch { }

            try
            {
                if (StartTimer != null)
                {
                    StartTimer.Enabled = false;
                    StartTimer.Stop();
                    StartTimer.Dispose();
                    StartTimer = null;
                }
            }
            catch { }


            try
            {
                if (ButtonDelayTimer != null)
                {
                    ButtonDelayTimer.Enabled = false;
                    ButtonDelayTimer.Stop();
                    ButtonDelayTimer.Dispose();
                    ButtonDelayTimer = null;
                }
            }
            catch { }

            try
            {
                if (StartCardcheckTimer != null)
                {
                    StartCardcheckTimer.Enabled = false;
                    StartCardcheckTimer.Stop();
                    StartCardcheckTimer.Dispose();
                    StartCardcheckTimer = null;
                }
            }
            catch { }

            try
            {
                if (StartPasscheckTimer != null)
                {
                    StartPasscheckTimer.Enabled = false;
                    StartPasscheckTimer.Stop();
                    StartPasscheckTimer.Dispose();
                    StartPasscheckTimer = null;
                }
            }
            catch { }

            try
            {
                if (chargeReadyChkTimer != null)
                {
                    chargeReadyChkTimer.Enabled = false;
                    chargeReadyChkTimer.Stop();
                    chargeReadyChkTimer.Dispose();
                    chargeReadyChkTimer = null;
                }

            }
            catch { }

            if (dlg != null)
            {
                dlg.Close();               
                dlg = null;
            }


            //cbs
            if(Apptimer != null)
            {
                Apptimer.Close();
                Apptimer.Dispose();
                Apptimer = null;
            }

            onecheck = false;
        }

        private void chargeReadyChkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //try
            //{
            //    if (AVRIO.avrio.IsControlBDComm == true &&
            //         AVRIO.avrio.IsBMSCanComm == true &&
            //         AVRIO.bmsio.byBMSStatus == 3
            //        )
            //    {
            //        this.Dispatcher.Invoke((ThreadStart)delegate ()
            //        {
            //            dlg.Close();
            //            dlg = null;
            //        });

            //        if (chargeReadyChkTimer != null)
            //        {
            //            chargeReadyChkTimer.Close();
            //            chargeReadyChkTimer = null;
            //        }
            //    }
            //}
            //catch { }


            //cbs 2018.11.15
            try
            {
                if (dlg != null)
                {
                    // Console.WriteLine("dlg Alive");

                    if (AVRIO.avrio.bWarnningWindowClosed)
                    {
                        this.Dispatcher.Invoke((ThreadStart)delegate ()
                        {
                            dlg.Close();
                            dlg = null;
                        });
                        // Console.WriteLine("dlg", dlg);
                    }
                }
                else
                {
                    // Console.WriteLine("dlg null");
                }


                if (AVRIO.avrio.IsControlBDComm == true &&
                     AVRIO.avrio.IsBMSCanComm == true &&
                     AVRIO.bmsio.byBMSStatus == 3
                    )
                {
                    this.Dispatcher.Invoke((ThreadStart)delegate ()
                    {
                        if (dlg != null)
                        {
                            dlg.Close();
                            dlg = null;
                        }
                    });
                }
            }
            catch { }
        }


        //cbs
        DateTime dt;
        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dt = DateTime.Now;
            AVRIO.Log2.Eventlog = "Touch Event!!" +  dt.ToString();

            if (!AVRIO.avrio.Fualtstop && !AVRIO.avrio.IsFaultDialog && !AVRIO.avrio.FualtstopFalg)
            {
                if (AVRIO.avrio.Rapidascardcheck == 1)
                {
                    AVRIO.avrio.EventMsg = "Display Touch start !!!  Play_Funtion1  ";
                    Play_Funtion1();
                }
                else
                {
                    AVRIO.avrio.EventMsg = "Display Touch start !!!  Play_Funtion  ";
                    Play_Funtion();
                }
            }
        }

        #region 시나리오 시작 함수

        private void Play_Funtion()
        {
          
            if (!AVRIO.avrio.BusinessTime)
            {
                if (AVRIO.avrio.IsBMSCanComm == true &&
                    AVRIO.avrio.IsControlBDComm == true &&
                    AVRIO.bmsio.bBMSFaultFlag == false &&
                    AVRIO.bmsio.bBMSCanCommErrorFlag == false)
                {
                        if (AVRIO.avrio.Rapidaspassword == 1)
                        {
                            if (AVRIO.avrio.DspLgBatteryStatus == 2)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                               // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                StartPasscheckTimer.Start();
                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                StartPasscheckTimer.Start();
                            }
                            else
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                if (AVRIO.avrio.DspLgBatteryStatus == 1)
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                StartPasscheckTimer.Start();
                            }
                        }
                        else
                        {
                            if (AVRIO.avrio.DspLgBatteryStatus == 2)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                StartTimer.Enabled = true;
                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                StartTimer.Enabled = true;
                            }
                            else
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                if (AVRIO.avrio.DspLgBatteryStatus == 1)
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                               // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                StartTimer.Enabled = true;
                            }

                        }
                    }
                //}
                else
                {       

                    DateTime dt = DateTime.Now;
                    string msg;
                    if (AVRIO.avrio.IsFaultDialog == true)
                    {
                        if (AVRIO.avrio.IsBMSCanComm == false)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CANFail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS CAN PORT OPEN FAIL" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.avrio.IsControlBDComm == false)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "Control BD PORT OPEN FAIL" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSCellVoltageFaultFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS_CellVoltageError" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSFaultFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_Fault;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS FAULT" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CANFail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS Can Arrive Chk Fail" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.avrio.IsFaultDialog == false)
                        {
                            if (AVRIO.avrio.WaitFualtFlag)
                                AVRIO.avrio.IsFaultDialog = true;
                        }

                    }
                }
            }
            onecheck = false;
        }

        private void Play_Funtion1()
        {

            AVRIO.avrio.EventMsg = "Play_Funtion1() Play";
            if (!AVRIO.avrio.BusinessTime)
            {
                if (AVRIO.avrio.IsBMSCanComm == true &&
                    AVRIO.avrio.IsControlBDComm == true &&
                    AVRIO.bmsio.bBMSFaultFlag == false &&
                    AVRIO.bmsio.bBMSCanCommErrorFlag == false)
                {
                   // if (AVRIO.avrio.nDspVersion == AVRIO.avrio.nSbcVersion)
                    {
                        if (AVRIO.avrio.Rapidascardcheck == 1)
                        {
                            if (AVRIO.avrio.DspLgBatteryStatus == 2)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 1";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                StartCardcheckTimer.Start();

                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 2";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                StartCardcheckTimer.Start();
                            }
                            else
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                if (AVRIO.avrio.DspLgBatteryStatus == 1)
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 3";
                                StartCardcheckTimer.Start();
                            }
                        }
                        else
                        {
                            if (AVRIO.avrio.DspLgBatteryStatus == 2)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 4";
                                StartTimer.Enabled = true;
                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 5";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                StartTimer.Enabled = true;
                            }
                            else
                            {
                                try
                                {
                                    AutoBattryCharge.Enabled = false;
                                }
                                catch { }
                                if (AVRIO.avrio.DspLgBatteryStatus == 1)
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                                AVRIO.avrio.EventMsg = " StartCardcheckTimer.Start(); 6";
                                //AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                StartTimer.Enabled = true;
                            }

                        }

                    }

                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string msg;
                    if (AVRIO.avrio.IsFaultDialog == true)
                    {
                        if (AVRIO.avrio.IsBMSCanComm == false)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CANFail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS CAN PORT OPEN FAIL" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.avrio.IsControlBDComm == false)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "Control BD PORT OPEN FAIL" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSCellVoltageFaultFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS_CellVoltageError" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSFaultFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_Fault;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS FAULT" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                        {
                            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CANFail;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "BMS Can Arrive Chk Fail" + ",N";
                            ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }

                        if (AVRIO.avrio.IsFaultDialog == false)
                        {
                            if (AVRIO.avrio.WaitFualtFlag)
                            AVRIO.avrio.IsFaultDialog = true;
                        }

                    }
                }
            }
            onecheck = false;            
        }
        #endregion

        #region 자동충전

        private void timer_AutoCharge(object sender, ElapsedEventArgs e)
        {
            #region Mr lee Edit
            // 자동 충전 모드
            if (AVRIO.avrio.IsBMSCanComm == true &&
                AVRIO.avrio.IsControlBDComm == true &&
                AVRIO.bmsio.bBMSFaultFlag == false &&
                AVRIO.bmsio.bBMSCanCommErrorFlag == false)
            {

                if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal && AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                {
                    if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
                    {
                        //AVRIO.bmsio.byBMSStatus // 충전 시작
                        if ((AVRIO.bmsio.byBMSStatus == 3) &&
                           (!AVRIO.avrio.OnebatteryCharge) &&
                            (AVRIO.avrio.DspLgBatteryStatus == 1) && (AVRIO.bmsio.bySOC < 0x50))
                        {
                            if (/*((AVRIO.avrio.sCommandValue & 16) > 0) && */((AVRIO.avrio.rCommandValue & 8) > 0))
                            {
                                AVRIO.avrio.OnebatteryCharge = false;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryStart;
                                AVRIO.avrio.EventMsg = "Auto Charging TsBatteryStart";// +AVRIO.avrio.TsCommand.ToString();
                            }
                            else
                            {
                                AVRIO.avrio.EventMsg = "Auto Charging TsBatteryFinish";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
                            }
                        }
                    }
                    if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
                    {
                        //AVRIO.bmsio.byBMSStatus // pcs ready
                        if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 0) && (AVRIO.avrio.DspLgBatteryStatus == 0) && (AVRIO.bmsio.bySOC < 0x50))
                        {
                            AVRIO.avrio.OnebatteryCharge = false;
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryReady;
                        }
                    }
                    if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
                    {
                        //AVRIO.bmsio.byBMSStatus // 준비 완료 PRA 시작
                        if ((AVRIO.bmsio.byBMSStatus != 3))// && (QCDV.ManualControl.CurrentBatStatus == 0) )
                        {
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                        }
                    }

                    if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) == 0) || (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) == 0))
                    {
                        //AVRIO.bmsio.byBMSStatus   off
                        if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 0) && (QCDV.ManualControl.CurrentBatStatus == 0))
                        {
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                            // AVRIO.avrio.WaitFualtFlag = true;
                        }
                        if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 1) && (QCDV.ManualControl.CurrentBatStatus == 1))
                        {
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                        }
                    }
                }
                //}
                else
                {
                    AVRIO.avrio.AutobatteryDischarge = false;
                }
            }
            #endregion

            #region cbs Edit JFE적용 일단보류  테스트 완료 201811

            //if (AVRIO.avrio.IsBMSCanComm == true &&
            //   AVRIO.avrio.IsControlBDComm == true &&
            //   AVRIO.bmsio.bBMSFaultFlag == false &&
            //   AVRIO.bmsio.bBMSCanCommErrorFlag == false)
            //{
            //    //cbs TsBatteryFinish 안보내도  Dsp에서 SOC가 87% 되면 알아서 멈춘다고 한다..??? -김철호

            //    if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal && AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
            //    {
            //        if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
            //        {
            //            //AVRIO.bmsio.byBMSStatus // 충전 시작
            //            if ((AVRIO.bmsio.byBMSStatus == 3) &&
            //               (!AVRIO.avrio.OnebatteryCharge) &&
            //                (AVRIO.avrio.DspLgBatteryStatus == 1) && (AVRIO.bmsio.bySOC < AVRIO.avrio.ChargeStartSOC))
            //            {
            //                if (/*((AVRIO.avrio.sCommandValue & 16) > 0) && */((AVRIO.avrio.rCommandValue & 8) > 0))
            //                {
            //                    AVRIO.avrio.OnebatteryCharge = false;
            //                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryStart;
            //                    AVRIO.avrio.EventMsg = "Auto Charging TsBatteryStart";// +AVRIO.avrio.TsCommand.ToString();
            //                }
            //                else
            //                {
            //                    //cbs  DspLgBatteryStatus 가 충전중일때는 status가 2라 finish를 날릴수 없다..
            //                    AVRIO.avrio.EventMsg = "Auto Charging TsBatteryFinish";
            //                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
            //                }
            //            }
            //            //cbs DSP 에서 배터리에 충전중일때  셋팅한 ChargeBattSOCLimit 보다크면 batteryFinish
            //            if ((AVRIO.bmsio.byBMSStatus == 3)
            //                && AVRIO.avrio.DspLgBatteryStatus == 2 && AVRIO.bmsio.bySOC >= AVRIO.avrio.ChargeBattSOCLimit)
            //            {
            //                AVRIO.avrio.EventMsg = "Auto Charging TsBatteryFinish";
            //                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryFinish;
            //            }
            //        }
            //        if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
            //        {
            //            //AVRIO.bmsio.byBMSStatus // pcs ready
            //            if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 0) && (AVRIO.avrio.DspLgBatteryStatus == 0) && (AVRIO.bmsio.bySOC < AVRIO.avrio.ChargeStartSOC))
            //            {
            //                AVRIO.avrio.OnebatteryCharge = false;
            //                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBatteryReady;
            //            }
            //        }
            //        if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0) && (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) != 0))
            //        {
            //            //AVRIO.bmsio.byBMSStatus // 준비 완료 PRA 시작
            //            if ((AVRIO.bmsio.byBMSStatus != 3))// && (QCDV.ManualControl.CurrentBatStatus == 0) )
            //            {
            //                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
            //                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            //            }
            //        }

            //        if ((Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) == 0) || (Convert.ToInt32(AVRIO.bmsio.nAverageCellVoltage) == 0))
            //        {
            //            //AVRIO.bmsio.byBMSStatus   off
            //            if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 0) && (QCDV.ManualControl.CurrentBatStatus == 0))
            //            {
            //                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
            //                // AVRIO.avrio.WaitFualtFlag = true;
            //            }
            //            if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 1) && (QCDV.ManualControl.CurrentBatStatus == 1))
            //            {
            //                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
            //            }
            //        }
            //    }
            //    //}
            //    else
            //    {
            //        AVRIO.avrio.AutobatteryDischarge = false;
            //    }
            //}

            #endregion
        }

        #endregion

        #region 관리자 login

        private System.Timers.Timer movePasswordTimer = null;

        private void PassWordRect_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.RunMode = AVRIO.RunningMode.Admin; // RunMode 변경
            
          //  if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal || AVRIO.avrio.RunMode == AVRIO.RunningMode.Fault)
            {
                  if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysFault || AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                {
                    if (movePasswordTimer == null)
                    {
                        movePasswordTimer = new System.Timers.Timer(3000);
                        movePasswordTimer.Elapsed += new ElapsedEventHandler(movePasswordTimer_Elapsed);
                        movePasswordTimer.AutoReset = false;
                        movePasswordTimer.Start();
                    }
                }
            }
        }
        private void PassWordRect_1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (movePasswordTimer != null)
            {
                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
               
                movePasswordTimer.Stop();
                movePasswordTimer = null;
            }
        }

        void movePasswordTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (movePasswordTimer != null)
            {
                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
               
                movePasswordTimer.Stop();
                movePasswordTimer = null;
            }
            try
            {
                this.Dispatcher.Invoke((ThreadStart)delegate()
                {
                    NavigationService nav = NavigationService.GetNavigationService(this);
                    nav.Navigate(PageManager.GetPage(PageId._09_패스워드입력));
                });
            }
            catch { };
        }

        #endregion
      
    }
}