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
using System.Timers;
using System.Threading;

namespace QuickChargeDisplay
{
    /// <summary>
    /// FaultDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FaultDialog : Window
    {
        private System.Timers.Timer WatchDog;
        public FaultDialog()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AVRIO.avrio.FaultCode == "315")  //내부배터리 SOC 5% 미만
                {
                    if (WatchDog != null) WatchDog.Enabled = false;
                    AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                    AVRIO.avrio.FaultCode = "";
                    // AVRIO.avrio.nonefaultcheck = true; 이거 ModbusThread.cs에서 TsResetFault명령 보낼수있다
                    AVRIO.avrio.nSelectCommand56 = 1;
                    AVRIO.avrio.nSelectCommand78 = 1;
                    AVRIO.avrio.nSelectCommand910 = 1;
                   // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                    AVRIO.avrio.Fualtstop = true;
                    AVRIO.avrio.WaitFualtFlag = true;
                    AVRIO.avrio.Fualtstop = false;
                    AVRIO.avrio.nonefaultcheck = false;
                    AVRIO.avrio.IsFaultDialog = false;
                    AVRIO.avrio.ChargeFaultCheck = true;
                    AVRIO.avrio.FualtstopFalg = false;
                    CloseTimer();

                    this.Dispatcher.Invoke((ThreadStart)delegate ()
                    {
                        this.Close();
                    });
                    return;
                }

                AVRIO.avrio.EventMsg = "FaultDialog loaded -> TouchScreen STOP btn";

                if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError)
                {
                    AVRIO.avrio.EventMsg = "[CAN] SBC => BMS : FaultReset cmd";
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                }
                else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Fault)
                {
                    AVRIO.avrio.EventMsg = "[CAN] SBC => BMS : FaultReset cmd";
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                }


                //else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.RfpadOpen_Fail)
                //{
                //    AVRIO.bmsio.bBMSCanCommErrorFlag = false;
                //}


                else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail)
                {
                }
                else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.CDMAModem_Fail)
                {
                }
                else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CANFail)
                {

                    AVRIO.bmsio.bBMSCanCommErrorFlag = false;
                }
                else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Warnning)
                {
                }
                else
                {
                    AVRIO.avrio.EventMsg = "SBC => DSP : TsResetFault cmd try";
                }
                if (WatchDog != null) WatchDog.Enabled = false;

                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;


                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;

                AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;

                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;

                if (AVRIO.avrio.FaultCode == "321")
                {
                    AVRIO.avrio.fault321check = false;
                }
                AVRIO.avrio.FaultCode = "";
                // AVRIO.avrio.Fualtstop = false;
                // AVRIO.avrio.FualtstopFalg = false;
                AVRIO.avrio.nonefaultcheck = true;
                AVRIO.avrio.nSelectCommand56 = 1;
                AVRIO.avrio.nSelectCommand78 = 1;
                AVRIO.avrio.nSelectCommand910 = 1;
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                AVRIO.avrio.Fualtstop = true;

                CloseTimer();

                this.Dispatcher.Invoke((ThreadStart)delegate ()
                {
                    this.Close();
                });
            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = err.ToString();
            }
        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (AVRIO.avrio.Fualtstop)
                {
                    AVRIO.avrio.EventMsg = "FaultDialog loaded -> STOP btn";
                    //cbs 20200711
                    if (AVRIO.avrio.FaultCode == "315")
                    {
                        AVRIO.avrio.ChargeFaultCheck = true;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                        AVRIO.avrio.FaultCode = "";
                        //AVRIO.avrio.nonefaultcheck = true;
                        AVRIO.avrio.nSelectCommand56 = 1;
                        AVRIO.avrio.nSelectCommand78 = 1;
                        AVRIO.avrio.nSelectCommand910 = 1;

                        AVRIO.avrio.WaitFualtFlag = true;
                        AVRIO.avrio.Fualtstop = false;
                        AVRIO.avrio.nonefaultcheck = false;
                        AVRIO.avrio.IsFaultDialog = false;
                        AVRIO.avrio.ChargeFaultCheck = true;
                        AVRIO.avrio.FualtstopFalg = false;
                    }
                    else
                    {
                        if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError)
                        {
                            // AVRIO.avrio.EventMsg = "[CAN] SBC => BMS : FaultReset cmd";
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                        }
                        else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Fault)
                        {
                            //  AVRIO.avrio.EventMsg = "[CAN] SBC => BMS : FaultReset cmd";
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                        }
                        else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail)
                        {
                        }
                        else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.CDMAModem_Fail)
                        {
                        }
                        else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CANFail)
                        {

                            AVRIO.bmsio.bBMSCanCommErrorFlag = false;
                        }
                        else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Warnning)
                        {
                        }
                        else
                        {
                            // AVRIO.avrio.EventMsg = "SBC => DSP : TsResetFault cmd try";
                        }

                        AVRIO.avrio.ChargeFaultCheck = true;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        // AVRIO.avrio.IsFaultDialog = false;
                        // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;

                        if (AVRIO.avrio.FaultCode == "321" && AVRIO.avrio.FaulStopButton)
                        {
                            AVRIO.avrio.fault321check = false;
                        }

                        AVRIO.avrio.FaultCode = "";

                        AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;

                        //   AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;

                        AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.nonefaultcheck = true;
                        AVRIO.avrio.nSelectCommand56 = 1;
                        AVRIO.avrio.nSelectCommand78 = 1;
                        AVRIO.avrio.nSelectCommand910 = 1;
                        // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        //WatchDog.Enabled = false;
                    }

                    CloseTimer();

                    this.Dispatcher.Invoke((ThreadStart)delegate ()
                    {
                        this.Close();
                    });
                }
            }
            catch
            {
                CloseTimer();
                this.Dispatcher.Invoke((ThreadStart)delegate ()
                {
                    this.Close();
                });
            }

        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (WatchDog != null)
            {
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;
                AVRIO.avrio.EventMsg = "FaultDialog Watch";
            }

            AVRIO.avrio.EventMsg = "FaultDialog Unloaded";

        }

        private void CloseTimer()
        {
            if (WatchDog != null)
            {
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;

                AVRIO.avrio.EventMsg = "FaultDialog Dispose";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //cbs 재수없으면 AVRIO.avrio.FaultCode가 아래 조건 타는 도중에  "" 값으로 바뀔가능성이있다.
            // 복사해서 써야될거 같은데....

            AVRIO.avrio.EventMsg = "FaultDialog_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            try
            {
                if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                    if (WatchDog != null) WatchDog.Enabled = false;

                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                    AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                    AVRIO.avrio.FaultCode = "";
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                    AVRIO.avrio.nonefaultcheck = true;
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                    AVRIO.avrio.Fualtstop = true;

                    CloseTimer();

                    this.Dispatcher.Invoke((ThreadStart)delegate ()
                    {
                        this.Close();
                    });
                }

                AVRIO.avrio.Fualtstop = false;
                WatchDog = new System.Timers.Timer(100);
                WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
                WatchDog.AutoReset = true;
                WatchDog.Enabled = true;
                AVRIO.avrio.FualtstopFalg = true;

                if (AVRIO.avrio.Rapidaslanguage == 0)
                {
                    #region 일본어 버전 
                    if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                    {

                        // 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                        if (WatchDog != null) WatchDog.Enabled = false;

                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                        AVRIO.avrio.FaultCode = "";
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.nonefaultcheck = true;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.Fualtstop = true;

                        CloseTimer();

                        this.Dispatcher.Invoke((ThreadStart)delegate ()
                        {
                            this.Close();
                        });

                    }
                    if (10 == Convert.ToInt32(AVRIO.avrio.FaultCode))
                    {
                        this.strTitle.Text = "非常停止中 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                        this.strInfo1.Text = "非常停止ボタンが作動中です。解除される場合は、";
                        this.strInfo2.Text = "非常停止ボタンを時計方向に廻し ロックを解除してください。";
                        this.strInfo3.Text = "STOPボタンを押してください。";
                    }
                    else
                        if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.Charger)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS)
                    {
                        if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 320)
                            AVRIO.avrio.FaultCode = "14";
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;


                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.Charger_BMS)
                    {
                        if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 320)
                            AVRIO.avrio.FaultCode = "14";
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ServerCost_Fail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    //else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.RfpadOpen_Fail)
                    //{
                    //    // this.strTitle.Text = "Stop charging [charger]";
                    //    this.strTitle.Text = "異常発生[S]";
                    //    this.strInfo1.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                    //    // this.strInfo1.Text = "Failed to communicate with rf-pad.";
                    //    // this.strInfo2.Text = "Restart the charger.";
                    //}
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail)
                    {
                        if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 320)
                            AVRIO.avrio.FaultCode = "14";
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }

                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CANFail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError)
                    {
                        if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 320)
                            AVRIO.avrio.FaultCode = "14";
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Fault)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);
                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Warnning)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);
                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if(Select == 315) //cbs 20200711추가
                        {
                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }                        
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BATTERY_BMS)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();
                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    else
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);
                        if (Select == 11)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 15)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "電源入力の相順が逆となっている可能性。";
                            this.strInfo2.Text = "があります。確認をお願いします。";
                        }
                        else if (Select == 21 || Select == 22)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "絶縁異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 23 || Select == 24 || Select == 101 || Select == 103
                            || Select == 104 || Select == 105 || Select == 106 || Select == 107 || Select == 110
                            )
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "充電器異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電器に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";
                        }
                        else if (Select == 100 || Select == 108 || Select == 109)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください ";
                            this.strInfo1.Text = " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo2.Text = "充電コネクタを抜いて、はじめからやり直してください。";
                            this.strInfo3.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 102)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "コネクタ接続を確認してください。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if (Select == 257 || Select == 258 || Select == 259 || Select == 260 || Select == 261 || Select == 263)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰には、STOPボタンを押してください。";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "車両側の異常発生 " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両側に異常が発生しました。";
                            this.strInfo2.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "充電コネクタを抜いて、もう一度、 ";
                            this.strInfo2.Text = "はじめから充電をやり直してください。 ";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "もう一度、はじめから充電をやり直してください。" + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "車両のシフトを”P”に戻し、";
                            this.strInfo2.Text = "はじめから充電をやり直してください。";
                            this.strInfo3.Text = "復帰時にはSTOPボタンを押してください。";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }

                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "充電器を再起動してください";
                            }

                        }
                    }
                    #endregion
                }
                else
                {
                    if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                    {

                        // 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                        if (WatchDog != null) WatchDog.Enabled = false;

                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                        AVRIO.avrio.FaultCode = "";
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                        AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.nonefaultcheck = true;
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                        AVRIO.avrio.Fualtstop = true;

                        CloseTimer();
                        this.Dispatcher.Invoke((ThreadStart)delegate ()
                        {
                            this.Close();
                        });

                    }

                    if (10 == Convert.ToInt32(AVRIO.avrio.FaultCode))
                    {
                        this.strTitle.Text = "Emergency stop " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                        this.strInfo1.Text = "Emergency stop, button clockwise to unlock please.";
                        this.strInfo2.Text = "please to press the STOP button.";
                    }
                    else
                        if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.Charger)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생
                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";
                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();
                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }

                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);
                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생
                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";
                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";
                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();
                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.Charger_BMS)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ServerCost_Fail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();
                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.RfpadOpen_Fail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생
                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";
                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";
                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생
                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.ControlBoardOpen_Fail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();


                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CANFail)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Fault)
                    {
                        this.strTitle.Text = "Stop charging [batteryF]";
                        this.strInfo1.Text = "Problem occured during the charging process";
                        this.strInfo2.Text = "Check the state of BMS.";
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BMS_Warnning)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 315) //cbs 20200711
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                        }
                    }
                    else if (AVRIO.avrio.FaultDlgType == AVRIO.FAULT_DLG_TYPE.BATTERY_BMS)
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                            // this.strInfo1.Text = "Problem occured during the charging process";
                            // this.strInfo2.Text = "Check the state of BMS.";
                        }
                    }
                    else
                    {
                        int Select = Convert.ToInt32(AVRIO.avrio.FaultCode);

                        if ((1 <= Select && Select <= 26) || (200 <= Select && Select <= 204))
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Chargers side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur charger side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";
                        }
                        else if ((100 <= Select && Select <= 106) || (108 <= Select && Select <= 110) || (401 <= Select && Select <= 405) || (407 == Select))
                        {// 차량측 이상 발생

                            this.strTitle.Text = "Vehicle side error " + " FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Occur vehicle side error.";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button. .";

                        }
                        else if (Select == 107)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please unplug the charger connector from the Inlet in the vehicle. ";
                            this.strInfo2.Text = "Please restart from the beginging. If you want to return, please to press the STOP button.";

                        }
                        else if (Select == 406)
                        {// 충전기 이상 발생

                            this.strTitle.Text = "Please restart from the beginging " + "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                            this.strInfo1.Text = "Please check the charger connector with vehicle inlet connections. ";
                            this.strInfo2.Text = "If you want to return, please to press the STOP button.";

                        }
                        else
                        {
                            if (AVRIO.avrio.FaultCode == "" || Convert.ToInt32(AVRIO.avrio.FaultCode) > 321)
                            {// 예외 Fault 없애기 위해 만듬 어떤 증상이 일어 날지 테스트 필요
                                if (WatchDog != null) WatchDog.Enabled = false;

                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
                                AVRIO.avrio.FaultCode = "";
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
                                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.nonefaultcheck = true;
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                                AVRIO.avrio.Fualtstop = true;

                                CloseTimer();

                                this.Dispatcher.Invoke((ThreadStart)delegate ()
                                {
                                    this.Close();
                                });
                            }
                            else
                            {
                                this.strTitle.Text = "FaultCode [ " + AVRIO.avrio.FaultCode + " ]";
                                this.strInfo2.Text = "Stop charging";
                            }
                            // this.strInfo1.Text = "Problem occured during the charging process";
                            // this.strInfo2.Text = "Check the state of BMS.";
                        }
                    }
                }

            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = err.ToString();
            }
        }
    }

}





