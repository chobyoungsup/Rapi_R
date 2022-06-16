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
//using System.Timers;
using System.Media;
using System.Threading;
using System.Timers;

namespace QuickChargeDisplay
{
    /// <summary>
    /// ScreenSaverPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StopPage2 : Page
    {
        //private DateTime dt;
        private System.Timers.Timer WatchDog;
        private System.Timers.Timer AutoBattryCharge;
        public StopPage2()
        {
            InitializeComponent();

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "StopPage2_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;


            if (AVRIO.avrio.Rapidaslanguage == 0)
            {// 일본어
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/02a01b.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/01bc.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();

            }
            BackImage.StartAnimate();

            AVRIO.avrio.nSelectCommand12 = 8;
            AVRIO.avrio.BusinessTime = true;
        //    AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
            AutoBattryCharge = new System.Timers.Timer(500);
            AutoBattryCharge.Elapsed += new ElapsedEventHandler(timer_AutoCharge);
            AutoBattryCharge.AutoReset = true;
            AutoBattryCharge.Enabled = false;

            AVRIO.avrio.OnebatteryCharge = false;


            
           WatchDog = new System.Timers.Timer(200);
           WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
           WatchDog.AutoReset = true;
           WatchDog.Enabled = true;

            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.DevStop;

           if ((AVRIO.bmsio.byBMSStatus != 3))// && (QCDV.ManualControl.CurrentBatStatus == 0) )
           {
               AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
               AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
           }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            BackImage.StopAnimate();
            BackImage.Dispose();


            if (WatchDog != null)
            {
                WatchDog.Enabled = false;
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;
            }


            if (AutoBattryCharge != null)
            {
                AutoBattryCharge.Enabled = false;
                AutoBattryCharge.Stop();
                AutoBattryCharge.Dispose();
                AutoBattryCharge = null;
            }

            AVRIO.avrio.BusinessTime = false;
        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.AdminPage != AVRIO.AdminPage.AdminPage)
            {
                if ((AutoBattryCharge.Enabled == false) && (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby))
                {
                    AutoBattryCharge.Enabled = true;
                }

                if (AVRIO.avrio.BusinessCheckbar == 1 && AVRIO.avrio.BussinessFalg != 2)
                {
                    //영업 시간 체크
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

                    }
                    else
                    {
                        try
                        {
                            this.Dispatcher.Invoke((ThreadStart)delegate()
                            {
                                NavigationService nav = NavigationService.GetNavigationService(this);
                                nav.Navigate(PageManager.GetPage(PageId._01_대기화면));
                            });
                        }
                        catch { }

                    }
                }
                else if (AVRIO.avrio.BussinessFalg != 2 && AVRIO.avrio.BusinessCheckbar != 1)
                
                {
                    try
                    {
                        this.Dispatcher.Invoke((ThreadStart)delegate()
                        {
                            NavigationService nav = NavigationService.GetNavigationService(this);
                            nav.Navigate(PageManager.GetPage(PageId._01_대기화면));
                        });
                    }
                    catch { }
                }
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
            
    
            if (AVRIO.bmsio.bySOC == 0 && (AVRIO.bmsio.byBMSStatus != 3))
            {

                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsNone;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            }
        }

        #region 자동충전

        private void timer_AutoCharge(object sender, ElapsedEventArgs e)
        {
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
                            (AVRIO.avrio.DspLgBatteryStatus == 1) && (AVRIO.bmsio.bySOC < 0x55)) 
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
                        if ((AVRIO.bmsio.byBMSStatus == 3) && (QCDV.ManualControl.CurrentBatStatus == 0) && (AVRIO.avrio.DspLgBatteryStatus == 0) && (AVRIO.bmsio.bySOC < 0x55))
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
        }

        #endregion

        #region 관리자 login

        private System.Timers.Timer movePasswordTimer = null;

        private void PassWordRect_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
          //  if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Normal || AVRIO.avrio.RunMode == AVRIO.RunningMode.Fault)
            {
                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysFault || AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby )
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
                movePasswordTimer.Stop();
                movePasswordTimer = null;
            }
        }

        void movePasswordTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (movePasswordTimer != null)
            {
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
            catch { }
        }
        #endregion

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
         //   AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
        //    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
        }

    }
}
