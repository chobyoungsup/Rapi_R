using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Timers;


namespace AVRIO
{
    public enum SwBitOffset : int
    {
        Run = 0,
        Stop = 1,
    }

    public enum SwStatus : int
    {
        Run = 1,
        Stop = 2,
        Run_Stop = 3,
    }

    public enum TsCommand : int
    {
        TsNone = 0,
        TsStandby = 1,
        TsVehicleReady = 2,
        TsVehicleStart = 4,
        TsVehicleFinish = 8,
        TsBatteryReady = 16,
        TsBatteryStart = 32,
        TsBatteryFinish = 64,
        TsFault = 128,
        TsBMSStatus_1 = 256, //TsBatteryStatus-Precharge1
        TsBMSStatus_2 = 512, //TsBatteryStatus-Precharge2
        TsBMSStatus_3 = 768, //TsBatteryStatusReady
        TsGridOnlyOperation = 1024,
        TsBMSBatteryWarning = 2048, // 배터리  Warning
        TsBMSBatteryDischarge = 4096, // 배터리  DisCharge
        TsResetFault = 8192,
        TsBatteryPRAON = 16384,
        TsCalibration = 32768,
    }

    public enum SysBatteryStatus : int
    {
        SysBatteryStandby = 0,
        SysBatteryReady = 1,
        SysBatteryRunning = 2,
    }

    public enum SysStatus : int
    {
        SysStandby = 0, // 대기화면
        SysReady = 1,   // 커넥터 연결화면
        SysPreRun = 2,  // 절연측정 화면        
        SysRunning = 3, // 충전중 화면
        SysStopping = 4,// 충전종료중 화면
        SysFinish = 5,  // 충전완료 및 커넥터 분리
        SysBatterySystanby = 8,      //0
        SysBatterySysReady = 16,    //1
        SysBatteryRunning = 24,       //2
        SysFault = 32,
        SysPCSReady = 64,
        SysBMSReady = 128,
        SysStartButton = 256,
        SysStopButton = 512,
        SysGridOnlyOperation = 1024,
        // 화면 표시를 위한 부분
        SysConectionFinish = 2048,  // 충전금액
        SysPayCheck = 4096,         // 결재화면
        SysWorkDone = 8192,         // 충전완료화면
    }

    public enum RunningMode : int
    {
        Normal = 0,
        Charging = 1,
        Admin = 2,
        Fault = 4,
    }

    public enum AdminPage : int
    {
        Ready = 0,
        Language,
        Moment,
        AdminPage,            // 09_0_AdminMenuPage.xaml
        ChargeHistory,        // 09_6_ChargeHistoryPage.xaml
        FaultHistory,         // 09_7_TroubleHistoryPage.xaml
        Password,
        Managerment,
        ManualControl,
        DeviceSetting,
        CompulsionCharge,
        SetupMaxiumCurrent,
        LogSampling,
        SetupVoltage
    }

    public enum BILLMETHOD : int
    {
        None = 0,
        Card = 1,
        TMoney = 2,
        Notice = 3,
    }
    public enum ChargingMode : int
    {
        CC = 0,
        CV = 1,
        CP = 2,
        OTHERS = 3,
    }
    public enum WARNNINGTYPE : int
    {
        None = 0,
        Battery_Full = 1,
        Charge_Money = 2,
        Charge_Watt = 3,
        Charge_Soc = 4,
        Charge_CarSoc = 5,
        Charge_Ready = 6,
    }
    public enum HSCMD : byte
    {
        None = 0x00,
        ReadAccount = 0xD1,     // 계좌읽기
        EncPassword = 0xD5,     // 비밀번호 암호화
        CancelCommand = 0xD6,   // 명령취소
        InitDevice = 0xD7,      // 초기화
        CheckStatus = 0xD8,     // 상태체크
        RfdEmv = 0xA3,          // EMV 신용정보 요청
        RfdCardInfo = 0xD0,     // Card 정보읽기
    }
    public enum FAULT_DLG_TYPE : int
    {
        None = 0,
        Charger = 1,            // 충전기 에러
        BMS = 2,                // 차량 BMS 에러
        Charger_BMS = 3,        // 충전중 BMS 에러
        ServerCost_Fail = 4,    // 충전단가 받기 에러
        RfpadOpen_Fail = 5,     // RFPAD 열기 실패
        ControlBoardOpen_Fail = 6,       // AVR 열기 실패
        CDMAModem_Fail = 7,     // CDMA 모뎀 에러
        BMS_CANFail = 9,        // BMS 캔 통신 에러
        BMS_CellVoltageError = 10,  // BMS Cell 에러의 경우 수동모드로 충전을 진행해야한다.
        BMS_Fault = 11,         // BMS Fault 발생
        BMS_Warnning = 12,      // BMS Warnning 발생
        BATTERY_BMS = 13,       // LG 배터리 BMS 에러
    }
    public class SysStatusChangeEventArgs : EventArgs
    {
        private SysStatus currentSysStatus;
        private SysStatus previewSysStatus;

        public SysStatusChangeEventArgs(SysStatus currentSysStatus, SysStatus previewSysStatus)
        {
            this.currentSysStatus = currentSysStatus;
            this.previewSysStatus = previewSysStatus;
        }

        public SysStatus CurrentSysStatus
        {
            get { return currentSysStatus; }
            set { currentSysStatus = value; }
        }

        public SysStatus PreviewSysStatus
        {
            get { return previewSysStatus; }
            set { previewSysStatus = value; }
        }
    }

    public delegate void SysStatusChangedEvent(SysStatusChangeEventArgs args);
    public delegate void SysDataChangedEvent(string dataName, object value);
    public delegate void SysOrderEvent(TsCommand command, params object[] list);
    public delegate void AdminModeChangedEvent(AdminPage page);
    public delegate void CSOSOrderEvent(CSOSCMD command, params object[] list);
    public delegate void HyosungCardOrderEvent(HSCMD command, params object[] list);


    public class ChargeWattPerUnitPrice
    {
        private double unitPrice;
        private double chargStartWatt;

        public ChargeWattPerUnitPrice()
        {
        }

        public ChargeWattPerUnitPrice(double watt)
        {
            unitPrice = 0;
            chargStartWatt = watt;
        }

        public ChargeWattPerUnitPrice(double u, double c)
        {
            unitPrice = u;
            chargStartWatt = c;
        }

        public double UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public double ChargStartWatt
        {
            get { return chargStartWatt; }
            set { chargStartWatt = value; }
        }
    }

    public enum CSOSCMD : int
    {
        None = 0x00,
        ChargingStat = 0x10,
        DevStop = 0x11,             // 충전기정지
        DevCtrl = 0x12,             // 충전기제어
        ChargeStart = 0x20,         // 충전시작
        ChargeFinish = 0x21,        // 충전종료
        ChargeCancel = 0x22,        // 충전취소       
        ChargeCurrent = 0x27,       // 누적전력량 


        Chademo5608 = 0x51,
        Chademo5610 = 0x52,
        Chademo5620 = 0x53,
        Chademo5640 = 0x54,
        Chademo5680 = 0x55,

        ChargeFualt7801 = 0x60,
        ChargeFualt7804 = 0x61,
        ChargeFualt7808 = 0x62,
        ChargeFualt7810 = 0x63,
        ChargeFualt7820 = 0x64,
        ChargeFualt7840 = 0x65,
        ChargeFualt7880 = 0x66,
        ChargeFualt0001 = 0x17,

        CarStop91002 = 0x71,
        CarStop91004 = 0x72,
        CarStop91008 = 0x73,
        CarStop91010 = 0x74,
        CarStop91020 = 0x75,
        CarStop91040 = 0x76,
        CarStop91080 = 0x77,
    }

    public class RemindWattPerTimes
    {
        private string ymd;
        private int hh;
        private int interval;
        private double kwh;
        private int kwhBill;
        private int infraBill;
        private int serviceBill;
        private int kwhBillUCost;
        private int infraBillUCost;
        private int serviceBillUCost;
        private DateTime updateTime;

        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }

        public int ServiceBillUCost
        {
            get { return serviceBillUCost; }
            set { serviceBillUCost = value; }
        }

        public int InfraBillUCost
        {
            get { return infraBillUCost; }
            set { infraBillUCost = value; }
        }

        public int KwhBillUCost
        {
            get { return kwhBillUCost; }
            set { kwhBillUCost = value; }
        }

        public int ServiceBill
        {
            get { return serviceBill; }
            set { serviceBill = value; }
        }

        public int InfraBill
        {
            get { return infraBill; }
            set { infraBill = value; }
        }

        public int KwhBill
        {
            get { return kwhBill; }
            set { kwhBill = value; }
        }

        public double Kwh
        {
            get { return kwh; }
            set { kwh = value; }
        }

        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public int Hh
        {
            get { return hh; }
            set { hh = value; }
        }

        public string Ymd
        {
            get { return ymd; }
            set { ymd = value; }
        }
    }

    public class UnitPricePerTimes
    {
        private int hour;
        private double ucost;
        private int interval;       // 30분단위 0,1
        private string date;        // YYYYMMDD

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public UnitPricePerTimes()
        {
        }

        public UnitPricePerTimes(int h, double u)
        {
            hour = h;
            ucost = u;
        }

        public UnitPricePerTimes(int h, int i, double u, string d)
        {
            hour = h;
            interval = i;
            ucost = u;
            date = d;
        }

        public double Ucost
        {
            get { return ucost; }
            set { ucost = value; }
        }

        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }
    }

    public class ChargeListPerTimes
    {
        private DateTime startTime;
        private DateTime endTime;
        private double chargeWatt;
        private double chargePrice;
        private double unitPrice;

        public double UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public double ChargePrice
        {
            get { return chargePrice; }
            set { chargePrice = value; }
        }

        public double ChargeWatt
        {
            get { return chargeWatt; }
            set { chargeWatt = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
    }

    //cbs
    public class RebootFunc
    {
        public bool IsEnable { get; set; }
        public int TimeHH { get; set; }
        public int Timemm { get; set; }
    }

    public static class avrio
    {

        public static int Fault315Cnt { get; set; }
        //cbs
        public static bool TEST_MODE { get; set; }

        //cbs
        public static bool RebootSelectDay = false;
        public static Dictionary<DayOfWeek, RebootFunc> _RebootFunc;

        public static bool bWarnningWindowClosed = true;
        public static byte ChargeStartSOC;
        public static byte ChargeBattSOCLimit;
        public static bool bDSPCalibration { get; set; }
        public static int nSET_Voltage_Slope { get; set; }
        public static int nSET_Voltage_Offset { get; set; }
        public static int nSET_Current_Slope { get; set; }
        public static int nSET_Current_Offset { get; set; }





        // TEST용 변수들
        private static double chargeTestUnitPrice = 300;      // TEST 용 고정단가정보
        private static double chargeTestWatt = 5.18;
        private static double chargeTestStartWatt = 5.18;
        private static double chargeTestCost = 0;


        private static Queue<string> qLogDataList = new Queue<string>();
        private static int currentLogDataInQueue = 0;


        private static Queue<string> qCanLogDataList = new Queue<string>();
        private static int currentCanLogDataInQueue = 0;

        private static Queue<string> qDSPLogDataList = new Queue<string>();
        private static int currentDSPLogDataInQueue = 0;

        private static int CountDeletelog = 0;
        private static int CountDeleteBMS = 0;
        // 1. 로그 동작 플래그

        private static bool bLogEnable_Type0 = false;
        private static bool bLogEnable_Type1 = false;// rapidas
        private static bool bLogEnable_Type2 = false;// rapidas

        private static string strLog = null;
        private static string strLog1 = null;

        #region SystemLog관련함수


        public static void SystemLog(string data, int nType)       // 0:SystemLog, 1:CanLog
        {

            try
            {
                strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , " + data;
                qLogDataList.Enqueue(strLog);
                currentLogDataInQueue++;
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
            try
            {
                if (currentLogDataInQueue == 0)
                {
                    return;
                }

                string logPath = logPath = @"D:\QuickChargeApp";
                string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                if (!logPath.EndsWith(@"\"))
                {
                    logPath += @"\";
                    logPath += @"LOG\";
                    int i = 0;
                    MakeLogPath(logPath);
                    if (bLogEnable_Type0 == true)
                        return;
                    bLogEnable_Type0 = true;

                    StreamWriter sw1 = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);
                    try
                    {

                        bLogEnable_Type0 = true;
                        for (i = 0; i < currentLogDataInQueue; i++)
                        {
                            strLog = qLogDataList.Dequeue();
                            sw1.WriteLine(string.Format("{0}", strLog));
                            currentLogDataInQueue--;
                        }
                        bLogEnable_Type0 = false;
                        if (sw1 != null)
                            sw1.Close();
                    }
                    catch (Exception err)
                    {
                        if (sw1 != null)
                            sw1.Close();
                        bLogEnable_Type0 = false;
                        AVRIO.Log.Eventlog = err.ToString();
                    }
                }
                CountDeletelog++;
                if (CountDeletelog > 18000)
                {
                    DeleteLogFile(logPath);
                    CountDeletelog = 0;
                }

            }
            catch
            {
                bLogEnable_Type0 = false;
            }

        }
        public static void SystemLog1(string data, int nType)       // 0:SystemLog, 1:CanLog
        {
            try
            {
                strLog1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , " + data;
                qCanLogDataList.Enqueue(strLog1);
                currentCanLogDataInQueue++;
            }
            catch
            {

            }

            try
            {
                if (currentCanLogDataInQueue == 0)
                {
                    return;
                }

                string logPath = logPath = @"D:\QuickChargeApp";
                string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                if (!logPath.EndsWith(@"\"))
                {
                    logPath += @"\";
                    logPath += @"BMS\";
                    int i = 0;
                    MakeLogPath(logPath);
                    if (bLogEnable_Type1 == true)
                        return;
                    bLogEnable_Type1 = true;
                    StreamWriter sw = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);
                    try
                    {

                        bLogEnable_Type1 = true;
                        for (i = 0; i < currentCanLogDataInQueue; i++)
                        {
                            strLog1 = qCanLogDataList.Dequeue();
                            sw.WriteLine(string.Format("{0}", strLog1));
                            currentCanLogDataInQueue--;
                        }
                        bLogEnable_Type1 = false;
                        if (sw != null)
                            sw.Close();
                    }
                    catch
                    {
                        if (sw != null)
                            sw.Close();
                        bLogEnable_Type1 = false;
                    }
                }
                CountDeleteBMS++;
                if (CountDeleteBMS > 18000)
                {
                    DeleteLogFile3(logPath);
                    CountDeleteBMS = 0;
                }
            }
            catch
            {
                bLogEnable_Type1 = false;
            }
        }
        public static void SystemLog2(string data, int nType)       // 0:SystemLog, 1:CanLog
        {
            try
            {
                string strLog = null;
                if (nType == 2)
                {

                    if (bLogEnable_Type2 == false)
                    {
                        bLogEnable_Type2 = true;
                        strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , " + data;
                        qDSPLogDataList.Enqueue(strLog);
                        currentDSPLogDataInQueue++;
                        WriteLog(nType);
                    }
                }
            }
            catch
            {
                bLogEnable_Type2 = false;
            }
        }

        public static void WriteLog(int nType)
        {
            try
            {
                if (nType == 0)
                {
                    CountDeletelog++;

                    if (currentLogDataInQueue == 0)
                    {
                        return;
                    }

                    string logPath = @"D:\QuickChargeApp";
                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log";
                    if (!logPath.EndsWith(@"\"))
                    {
                        logPath += @"\";
                        logPath += @"LOG\";
                        int i = 0;
                        string strLog = null;

                        MakeLogPath(logPath);
                        StreamWriter sw = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);

                        try
                        {

                            if (CountDeletelog > 18000)
                            {
                                DeleteLogFile(logPath);
                                CountDeletelog = 0;
                            }
                            for (i = 0; i < currentLogDataInQueue; i++)
                            {
                                strLog = qLogDataList.Dequeue();
                                sw.WriteLine(string.Format("{0}", strLog));
                                currentLogDataInQueue--;
                            }
                            if (sw != null)
                                sw.Close();
                        }
                        catch
                        {
                            if (sw != null)
                                sw.Close();
                            bLogEnable_Type0 = false;
                        }

                    }
                }
                bLogEnable_Type0 = false;
            }
            catch (System.Exception ex)
            {
                bLogEnable_Type0 = false;
                AVRIO.avrio.EventMsg = "WriteLog : " + ex.ToString();
            }
        }
        public static void WriteLog1(int nType)
        {

            try
            {
                if (nType == 1)
                {
                    CountDeleteBMS++;
                    if (currentCanLogDataInQueue == 0)
                    {
                        return;
                    }

                    string logPath = logPath = @"D:\QuickChargeApp";
                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                    if (!logPath.EndsWith(@"\"))
                    {
                        logPath += @"\";
                        logPath += @"BMS\";
                        int i = 0;
                        string strLog = null;

                        MakeLogPath(logPath);
                        StreamWriter sw1 = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);

                        try
                        {
                            if (CountDeleteBMS > 18000)
                            {
                                DeleteLogFile3(logPath);
                                CountDeleteBMS = 0;
                            }
                            for (i = 0; i < currentCanLogDataInQueue; i++)
                            {
                                strLog = qCanLogDataList.Dequeue();
                                sw1.WriteLine(string.Format("{0}", strLog));
                                currentCanLogDataInQueue--;
                            }
                            if (sw1 != null)
                                sw1.Close();
                        }
                        catch
                        {
                            if (sw1 != null)
                                sw1.Close();
                            bLogEnable_Type1 = false;
                        }
                    }
                }
                bLogEnable_Type1 = false;
            }
            catch (System.Exception ex)
            {
                bLogEnable_Type1 = false;
                AVRIO.avrio.EventMsg = "WriteLog1 : " + ex.ToString();
            }
        }
        public static void WriteLog2(int nType)
        {
            try
            {
                if (nType == 2)
                {
                    if (currentDSPLogDataInQueue == 0)
                    {
                        return;
                    }

                    string logPath = logPath = @"D:\QuickChargeApp";
                    string logFile = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".LOG";
                    if (!logPath.EndsWith(@"\"))
                    {
                        logPath += @"\";
                        logPath += @"DSP\";
                        int i = 0;
                        string strLog = null;

                        MakeLogPath(logPath);
                        StreamWriter sw2 = new StreamWriter(logPath + logFile, true, System.Text.Encoding.UTF8);

                        try
                        {
                            DeleteLogFile2(logPath);
                            for (i = 0; i < currentDSPLogDataInQueue; i++)
                            {
                                strLog = qDSPLogDataList.Dequeue();
                                sw2.WriteLine(string.Format("{0}", strLog));
                                currentDSPLogDataInQueue--;
                            }
                            if (sw2 != null)
                                sw2.Close();
                        }
                        catch
                        {
                            if (sw2 != null)
                                sw2.Close();
                            bLogEnable_Type2 = false;
                        }
                    }
                }
                bLogEnable_Type2 = false;
            }
            catch (System.Exception ex)
            {
                bLogEnable_Type2 = false;
                AVRIO.avrio.EventMsg = "WriteLog2 : " + ex.ToString();
            }
        }

        public static void DeleteLogFile(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.log"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            return;
        }
        public static void DeleteLogFile1(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.txt"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            return;
        }
        public static void DeleteLogFile2(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.csv"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
                return;
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        public static void DeleteLogFile3(string strLogFilePath)
        {
            try
            {
                DateTime Now = DateTime.Now;

                DirectoryInfo LogDirectory = new DirectoryInfo(strLogFilePath);

                foreach (FileInfo FileName in LogDirectory.GetFiles("*.log"))
                {
                    string LogFile = FileName.Name;

                    string LogYearDelete = LogFile.Substring(0, 4);
                    string LogMonthDelete = LogFile.Substring(5, 2);
                    string LogDayDelete = LogFile.Substring(8, 2);

                    DateTime logDate = new DateTime(Convert.ToInt32(LogYearDelete), Convert.ToInt32(LogMonthDelete), Convert.ToInt32(LogDayDelete));
                    TimeSpan offset = Now - logDate;
                    int DeleteDay = offset.Days;

                    if (DeleteDay > 30)
                    {
                        FileName.Delete();
                    }
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            return;
        }

        public static void MakeLogPath(string path)
        {
            try
            {
                // 디렉토리 체크
                string basePath = path.IndexOf(@"\\") > -1 ? path.Substring(0, path.IndexOf(@"\", 3)) : path.Substring(0, 3);
                string[] dirList = path.Substring(basePath.Length).Split('\\');
                int cnt = 0;
                for (int i = 0; i < dirList.Length; i++)
                {
                    if (dirList[i].Trim().Equals("")) continue;

                    if (!basePath.EndsWith(@"\")) basePath = basePath + @"\";
                    basePath = basePath + dirList[i];

                    if (path.IndexOf(@"\\") > -1 && cnt++ <= 0) continue;
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }


        #endregion

        public static double ChargeTestCost
        {
            get { return avrio.chargeTestCost; }
            set { avrio.chargeTestCost = value; }
        }

        public static double ChargeTestStartWatt
        {
            get { return avrio.chargeTestStartWatt; }
            set { avrio.chargeTestStartWatt = value; }
        }

        public static double ChargeTestWatt
        {
            get { return avrio.chargeTestWatt; }
            set { avrio.chargeTestWatt = value; }
        }

        public static double ChargeTestUnitPrice
        {
            get { return avrio.chargeTestUnitPrice; }
            set { avrio.chargeTestUnitPrice = value; }
        }

        // JFE


        private static bool nAdminManuClose;
        private static int nBusinessCheckbar = 0;
        private static int RapidasLanguage = 0;
        private static int RapidasCardCheck = 0;
        private static int RapidasWeekReboot = 1;
        private static int RapidasPassWord = 0;
        private static int rapidaslogintype = 1;   //1 이면 admin , 2 이면 Manual
        private static byte choice1 = 0;// 1의 자리
        private static byte choice2 = 0;// 10의 자리
        private static byte choice3 = 0;// 100 의 자리
        private static byte choice4 = 0;// 1000 의 자리

        private static double businessstimehh = 9; //영업 시작 시간
        private static double businessstimemm = 0; //영업 시작 시간
        private static double businessetimehh = 18; //영업 종료 시간
        private static double businessetimemm = 0; //영업 종료 시간
        private static double gridplay = 0;     // 배터리 사용 할건지..
        private static string adminpassword;// = "0";
        private static string manualpassword;// = "0";
        private static string chargepassword;// = "0";
        private static string nOutAmpare;// = "0";


        private static double nTimeWattTime_0_0 = 22;
        private static double nTimeWattTime_0_1 = 00;
        private static double nTimeWattTime_0_2 = 10;
        private static double nTimeWattTime_0_3 = 00;
        private static double nTimeWattTime_0_4 = 28;

        private static double nTimeWattTime_1_0 = 22;
        private static double nTimeWattTime_1_1 = 00;
        private static double nTimeWattTime_1_2 = 10;
        private static double nTimeWattTime_1_3 = 00;
        private static double nTimeWattTime_1_4 = 28;

        private static double nTimeWattTime_2_0 = 22;
        private static double nTimeWattTime_2_1 = 00;
        private static double nTimeWattTime_2_2 = 10;
        private static double nTimeWattTime_2_3 = 00;
        private static double nTimeWattTime_2_4 = 28;

        private static double nTimeWattTime_3_0 = 22;
        private static double nTimeWattTime_3_1 = 00;
        private static double nTimeWattTime_3_2 = 10;
        private static double nTimeWattTime_3_3 = 00;
        private static double nTimeWattTime_3_4 = 28;

        private static double nTimeWattTime_4_0 = 22;
        private static double nTimeWattTime_4_1 = 00;
        private static double nTimeWattTime_4_2 = 10;
        private static double nTimeWattTime_4_3 = 00;
        private static double nTimeWattTime_4_4 = 28;

        private static double nTimeWattSoc_0_0 = 125;
        private static double nTimeWattSoc_0_1 = 100;
        private static double nTimeWattSoc_0_2 = 60;

        private static double nTimeWattSoc_1_0 = 90;
        private static double nTimeWattSoc_1_1 = 90;
        private static double nTimeWattSoc_1_2 = 55;

        private static double nTimeWattSoc_2_0 = 75;
        private static double nTimeWattSoc_2_1 = 75;
        private static double nTimeWattSoc_2_2 = 40;

        private static double nTimeWattSoc_3_0 = 65;
        private static double nTimeWattSoc_3_1 = 65;
        private static double nTimeWattSoc_3_2 = 30;

        private static double nTimeWattSoc_4_0 = 55;
        private static double nTimeWattSoc_4_1 = 55;
        private static double nTimeWattSoc_4_2 = 20;

        private static double nTimeWattSoc_5_0 = 30;
        private static double nTimeWattSoc_5_1 = 25;
        private static double nTimeWattSoc_5_2 = 5;



        private static bool bmscanportcheck = false;
        private static bool threadready = false;

        public static bool bThreadReady
        {
            get { return avrio.threadready; }
            set { avrio.threadready = value; }
        }

        public static bool BmsCanportCheck
        {
            get { return avrio.bmscanportcheck; }
            set { avrio.bmscanportcheck = value; }
        }




        private static int currentTime = 0;
        private static double[] chargeUnitPriceArray = new double[24]; // 충전단가
        private static double currentUnitPrice = 300;             // 현재적용중 충전단가
        private static int chargeUnitPriceMode = 0;               // 0:Hour, 1:Half(30min)
        private static List<double> chargeUnitPriceList = new List<double>(); // 충전단가 저장
        private static List<UnitPricePerTimes> unitPriceList = new List<UnitPricePerTimes>();
        private static List<UnitPricePerTimes> unitPriceListNext = new List<UnitPricePerTimes>();
        private static double chargeWatt;                         // 사용(충전)전력량
        private static uint chargePrice;                        // 사용(충전)금액
        private static double chargeSOC;                          // SOC

        private static double nStartchargeSOC;                          // Start SOC
        private static double nStopchargeSOC;                          // Stop SOC

        private static BILLMETHOD billMethod;                     // 결제방법
        private static int remindPrice;                           // 카드잔액(티머니)
        private static int cpStat = 1;                            // 충전기 상태 
        private static int chargeRemainTime;                      // 충전남은시간
        private static int chargeRemainTimemm;                      // 충전남은시간
        private static int chargeRemainTimess;                      // 충전남은시간
        private static double currentUnitPricePrev;               // 현재적용중 충전단가
        private static bool preSendStat;                          // 상태값 중복 방지
        private static bool bTsbatteryWarning;                          // 배터리 상태
        private static bool bTsbatteryDischarge;                          //배터리 상태
        private static bool bAutobatteryDischarge;                          //자동충전
        private static bool bStopButton;
        private static bool bStartButton;
        private static bool bStopButtonPlay;
        private static bool bStartButtonPlay;
        private static bool nPageStanByStop;
        private static bool nBusinessTime;
        private static bool nChargeFaultCheck;
        private static bool nDisChargeFalg;
        private static bool nWarrningFalg;
        private static int nBussinessFalg;    //대기 <=> 중지 체인지
        private static ushort nVehiclePower;
        private static ushort nBatteryPower;
        private static bool nWaitFualtFlag;
        private static int SelectCommand12;
        private static int SelectCommand34;
        private static int SelectCommand56;
        private static int SelectCommand78;
        private static int SelectCommand910;
        private static bool nFaultStopCheck;
        private static bool nFaultStopCheck1;
        private static ushort sCommandvalue;
        private static ushort rCommandvalue;
        private static int OneRebootNum;
        private static bool Fault321Check = true;



        public static bool fault321check
        {
            get { return avrio.Fault321Check; }
            set { avrio.Fault321Check = value; }


        }
        public static int nOneRebootNum
        {
            get { return avrio.OneRebootNum; }
            set { avrio.OneRebootNum = value; }
        }

        public static ushort rCommandValue
        {
            get { return avrio.rCommandvalue; }
            set { avrio.rCommandvalue = value; }
        }

        public static ushort sCommandValue
        {
            get { return avrio.sCommandvalue; }
            set { avrio.sCommandvalue = value; }
        }

        public static bool FaultStopCheck1
        {
            get { return avrio.nFaultStopCheck1; }
            set { avrio.nFaultStopCheck1 = value; }
        }

        public static bool FaultStopCheck
        {
            get { return avrio.nFaultStopCheck; }
            set { avrio.nFaultStopCheck = value; }
        }

        public static int nSelectCommand12
        {
            get { return avrio.SelectCommand12; }
            set { avrio.SelectCommand12 = value; }
        }

        public static int nSelectCommand34
        {
            get { return avrio.SelectCommand34; }
            set { avrio.SelectCommand34 = value; }
        }
        public static int nSelectCommand56
        {
            get { return avrio.SelectCommand56; }
            set { avrio.SelectCommand56 = value; }
        }
        public static int nSelectCommand78
        {
            get { return avrio.SelectCommand78; }
            set { avrio.SelectCommand78 = value; }
        }
        public static int nSelectCommand910
        {
            get { return avrio.SelectCommand910; }
            set { avrio.SelectCommand910 = value; }
        }
        public static bool AdminManuClose
        {
            get { return avrio.nAdminManuClose; }
            set { avrio.nAdminManuClose = value; }
        }
        public static bool WaitFualtFlag
        {
            get { return avrio.nWaitFualtFlag; }
            set { avrio.nWaitFualtFlag = value; }
        }

        public static ushort VehiclePower
        {
            get { return avrio.nVehiclePower; }
            set { avrio.nVehiclePower = value; }
        }
        public static ushort BatteryPower
        {
            get { return avrio.nBatteryPower; }
            set { avrio.nBatteryPower = value; }
        }

        public static int BussinessFalg
        {
            get { return avrio.nBussinessFalg; }
            set { avrio.nBussinessFalg = value; }
        }

        public static bool DisChargeFalg
        {
            get { return avrio.nDisChargeFalg; }
            set { avrio.nDisChargeFalg = value; }
        }

        public static bool WarrningFalg
        {
            get { return avrio.nWarrningFalg; }
            set { avrio.nWarrningFalg = value; }
        }

        public static bool BusinessTime
        {
            get { return avrio.nBusinessTime; }
            set { avrio.nBusinessTime = value; }
        }

        public static bool PageStanByStop
        {
            get { return avrio.nPageStanByStop; }
            set { avrio.nPageStanByStop = value; }
        }

        public static bool ChargeFaultCheck
        {
            get { return avrio.nChargeFaultCheck; }
            set { avrio.nChargeFaultCheck = value; }
        }

        public static bool StopButton
        {
            get { return avrio.bStopButton; }
            set { avrio.bStopButton = value; }
        }
        public static bool StartButton
        {
            get { return avrio.bStartButton; }
            set { avrio.bStartButton = value; }
        }

        public static bool StopButtonPlay
        {
            get { return avrio.bStopButtonPlay; }
            set { avrio.bStopButtonPlay = value; }
        }
        public static bool StartButtonPlay
        {
            get { return avrio.bStartButtonPlay; }
            set { avrio.bStartButtonPlay = value; }
        }

        public static double TimeWattTime_0_0
        {
            get { return avrio.nTimeWattTime_0_0; }
            set { avrio.nTimeWattTime_0_0 = value; }
        }
        public static double TimeWattTime_0_1
        {
            get { return avrio.nTimeWattTime_0_1; }
            set { avrio.nTimeWattTime_0_1 = value; }
        }
        public static double TimeWattTime_0_2
        {
            get { return avrio.nTimeWattTime_0_2; }
            set { avrio.nTimeWattTime_0_2 = value; }
        }
        public static double TimeWattTime_0_3
        {
            get { return avrio.nTimeWattTime_0_3; }
            set { avrio.nTimeWattTime_0_3 = value; }
        }
        public static double TimeWattTime_0_4
        {
            get { return avrio.nTimeWattTime_0_4; }
            set { avrio.nTimeWattTime_0_4 = value; }
        }

        public static double TimeWattTime_1_0
        {
            get { return avrio.nTimeWattTime_1_0; }
            set { avrio.nTimeWattTime_1_0 = value; }
        }
        public static double TimeWattTime_1_1
        {
            get { return avrio.nTimeWattTime_1_1; }
            set { avrio.nTimeWattTime_1_1 = value; }
        }
        public static double TimeWattTime_1_2
        {
            get { return avrio.nTimeWattTime_1_2; }
            set { avrio.nTimeWattTime_1_2 = value; }
        }
        public static double TimeWattTime_1_3
        {
            get { return avrio.nTimeWattTime_1_3; }
            set { avrio.nTimeWattTime_1_3 = value; }
        }
        public static double TimeWattTime_1_4
        {
            get { return avrio.nTimeWattTime_1_4; }
            set { avrio.nTimeWattTime_1_4 = value; }
        }

        public static double TimeWattTime_2_0
        {
            get { return avrio.nTimeWattTime_2_0; }
            set { avrio.nTimeWattTime_2_0 = value; }
        }
        public static double TimeWattTime_2_1
        {
            get { return avrio.nTimeWattTime_2_1; }
            set { avrio.nTimeWattTime_2_1 = value; }
        }

        public static double TimeWattTime_2_2
        {
            get { return avrio.nTimeWattTime_2_2; }
            set { avrio.nTimeWattTime_2_2 = value; }
        }
        public static double TimeWattTime_2_3
        {
            get { return avrio.nTimeWattTime_2_3; }
            set { avrio.nTimeWattTime_2_3 = value; }
        }
        public static double TimeWattTime_2_4
        {
            get { return avrio.nTimeWattTime_2_4; }
            set { avrio.nTimeWattTime_2_4 = value; }
        }

        public static double TimeWattTime_3_0
        {
            get { return avrio.nTimeWattTime_3_0; }
            set { avrio.nTimeWattTime_3_0 = value; }
        }
        public static double TimeWattTime_3_1
        {
            get { return avrio.nTimeWattTime_3_1; }
            set { avrio.nTimeWattTime_3_1 = value; }
        }
        public static double TimeWattTime_3_2
        {
            get { return avrio.nTimeWattTime_3_2; }
            set { avrio.nTimeWattTime_3_2 = value; }
        }
        public static double TimeWattTime_3_3
        {
            get { return avrio.nTimeWattTime_3_3; }
            set { avrio.nTimeWattTime_3_3 = value; }
        }
        public static double TimeWattTime_3_4
        {
            get { return avrio.nTimeWattTime_3_4; }
            set { avrio.nTimeWattTime_3_4 = value; }
        }

        public static double TimeWattTime_4_0
        {
            get { return avrio.nTimeWattTime_4_0; }
            set { avrio.nTimeWattTime_4_0 = value; }
        }
        public static double TimeWattTime_4_1
        {
            get { return avrio.nTimeWattTime_4_1; }
            set { avrio.nTimeWattTime_4_1 = value; }
        }
        public static double TimeWattTime_4_2
        {
            get { return avrio.nTimeWattTime_4_2; }
            set { avrio.nTimeWattTime_4_2 = value; }
        }
        public static double TimeWattTime_4_3
        {
            get { return avrio.nTimeWattTime_4_3; }
            set { avrio.nTimeWattTime_4_3 = value; }
        }
        public static double TimeWattTime_4_4
        {
            get { return avrio.nTimeWattTime_4_4; }
            set { avrio.nTimeWattTime_4_4 = value; }
        }


        public static double TimeWattSoc_0_0
        {
            get { return avrio.nTimeWattSoc_0_0; }
            set { avrio.nTimeWattSoc_0_0 = value; }
        }
        public static double TimeWattSoc_0_1
        {
            get { return avrio.nTimeWattSoc_0_1; }
            set { avrio.nTimeWattSoc_0_1 = value; }
        }
        public static double TimeWattSoc_0_2
        {
            get { return avrio.nTimeWattSoc_0_2; }
            set { avrio.nTimeWattSoc_0_2 = value; }
        }

        public static double TimeWattSoc_1_0
        {
            get { return avrio.nTimeWattSoc_1_0; }
            set { avrio.nTimeWattSoc_1_0 = value; }
        }
        public static double TimeWattSoc_1_1
        {
            get { return avrio.nTimeWattSoc_1_1; }
            set { avrio.nTimeWattSoc_1_1 = value; }
        }
        public static double TimeWattSoc_1_2
        {
            get { return avrio.nTimeWattSoc_1_2; }
            set { avrio.nTimeWattSoc_1_2 = value; }
        }
        public static double TimeWattSoc_2_0
        {
            get { return avrio.nTimeWattSoc_2_0; }
            set { avrio.nTimeWattSoc_2_0 = value; }
        }
        public static double TimeWattSoc_2_1
        {
            get { return avrio.nTimeWattSoc_2_1; }
            set { avrio.nTimeWattSoc_2_1 = value; }
        }
        public static double TimeWattSoc_2_2
        {
            get { return avrio.nTimeWattSoc_2_2; }
            set { avrio.nTimeWattSoc_2_2 = value; }
        }
        public static double TimeWattSoc_3_0
        {
            get { return avrio.nTimeWattSoc_3_0; }
            set { avrio.nTimeWattSoc_3_0 = value; }
        }
        public static double TimeWattSoc_3_1
        {
            get { return avrio.nTimeWattSoc_3_1; }
            set { avrio.nTimeWattSoc_3_1 = value; }
        }
        public static double TimeWattSoc_3_2
        {
            get { return avrio.nTimeWattSoc_3_2; }
            set { avrio.nTimeWattSoc_3_2 = value; }
        }

        public static double TimeWattSoc_4_0
        {
            get { return avrio.nTimeWattSoc_4_0; }
            set { avrio.nTimeWattSoc_4_0 = value; }
        }
        public static double TimeWattSoc_4_1
        {
            get { return avrio.nTimeWattSoc_4_1; }
            set { avrio.nTimeWattSoc_4_1 = value; }
        }
        public static double TimeWattSoc_4_2
        {
            get { return avrio.nTimeWattSoc_4_2; }
            set { avrio.nTimeWattSoc_4_2 = value; }
        }

        public static double TimeWattSoc_5_0
        {
            get { return avrio.nTimeWattSoc_5_0; }
            set { avrio.nTimeWattSoc_5_0 = value; }
        }
        public static double TimeWattSoc_5_1
        {
            get { return avrio.nTimeWattSoc_5_1; }
            set { avrio.nTimeWattSoc_5_1 = value; }
        }
        public static double TimeWattSoc_5_2
        {
            get { return avrio.nTimeWattSoc_5_2; }
            set { avrio.nTimeWattSoc_5_2 = value; }
        }
        public static double BusinessStimeHH
        {
            get { return avrio.businessstimehh; }
            set { avrio.businessstimehh = value; }
        }
        public static double BusinessStimeMM
        {
            get { return avrio.businessstimemm; }
            set { avrio.businessstimemm = value; }
        }
        public static double BusinessEtimeHH
        {
            get { return avrio.businessetimehh; }
            set { avrio.businessetimehh = value; }
        }
        public static double BusinessEtimeMM
        {
            get { return avrio.businessetimemm; }
            set { avrio.businessetimemm = value; }
        }
        public static double GridPlay
        {
            get { return avrio.gridplay; }
            set { avrio.gridplay = value; }
        }

        public static string AdminPassWord
        {
            get { return avrio.adminpassword; }
            set { avrio.adminpassword = value; }
        }
        public static string ManualPassWord
        {
            get { return avrio.manualpassword; }
            set { avrio.manualpassword = value; }
        }


        public static string OutAmpare
        {
            get { return avrio.nOutAmpare; }
            set { avrio.nOutAmpare = value; }
        }

        public static string ChargePassWord
        {
            get { return avrio.chargepassword; }
            set { avrio.chargepassword = value; }
        }

        public static byte Choice1
        {
            get { return avrio.choice1; }
            set { avrio.choice1 = value; }
        }
        public static byte Choice2
        {
            get { return avrio.choice2; }
            set { avrio.choice2 = value; }
        }
        public static byte Choice3
        {
            get { return avrio.choice3; }
            set { avrio.choice3 = value; }
        }
        public static byte Choice4
        {
            get { return avrio.choice4; }
            set { avrio.choice4 = value; }
        }
        public static bool PreSendStat
        {
            get { return avrio.preSendStat; }
            set { avrio.preSendStat = value; }
        }
        public static bool TsbatteryWarning
        {
            get { return avrio.bTsbatteryWarning; }
            set { avrio.bTsbatteryWarning = value; }
        }
        public static bool TsbatteryDischarge
        {
            get { return avrio.bTsbatteryDischarge; }
            set { avrio.bTsbatteryDischarge = value; }
        }
        public static bool AutobatteryDischarge
        {
            get { return avrio.bAutobatteryDischarge; }
            set { avrio.bAutobatteryDischarge = value; }
        }

        public static double CurrentUnitPricePrev
        {
            get { return avrio.currentUnitPricePrev; }
            set { avrio.currentUnitPricePrev = value; }
        }

        public static List<UnitPricePerTimes> UnitPriceListNext
        {
            get { return avrio.unitPriceListNext; }
            set { avrio.unitPriceListNext = value; }
        }

        public static List<UnitPricePerTimes> UnitPriceList
        {
            get { return avrio.unitPriceList; }
            set { avrio.unitPriceList = value; }
        }


        public static int ChargeRemainTime
        {
            get { return avrio.chargeRemainTime; }
            set { avrio.chargeRemainTime = value; }
        }

        public static int ChargeRemainTimemm
        {
            get { return avrio.chargeRemainTimemm; }
            set { avrio.chargeRemainTimemm = value; }
        }

        public static int ChargeRemainTimess
        {
            get { return avrio.chargeRemainTimess; }
            set { avrio.chargeRemainTimess = value; }
        }

        public static int CpStat
        {
            get { return avrio.cpStat; }
            set { avrio.cpStat = value; }
        }

        private static bool isLocking;                              // 충전중 잠금
        public static double CurrentUnitPrice
        {
            get { return avrio.currentUnitPrice; }
            set { avrio.currentUnitPrice = value; OnSysDataChanged("CurrentUnitPrice", value); }
        }

        public static BILLMETHOD BillMethod
        {
            get { return avrio.billMethod; }
            set { avrio.billMethod = value; }
        }

        public static bool IsLocking
        {
            get { return avrio.isLocking; }
            set { avrio.isLocking = value; OnSysDataChanged("IsLocking", value); }
        }

        private static byte chargeStartWatt;                      // 충전시작 전력량
        private static double vaildWatt;                              // 현재유효전력량
        private static double remindBaseWatt;                       // 누적전력량계산용 기준값
        private static float inputVoltage;                          // 입력 전압
        private static float inputCurrent;                          // 입력 전류
        private static uint chargeStartWattN;                       // watt

        public static float InputVoltage
        {
            get { return avrio.inputVoltage; }
            set { avrio.inputVoltage = value; }
        }

        public static float InputCurrent
        {
            get { return avrio.inputCurrent; }
            set { avrio.inputCurrent = value; }
        }

        public static uint ChargeStartWattN
        {
            get { return avrio.chargeStartWattN; }
            set { avrio.chargeStartWattN = value; }
        }

        public static double RemindBaseWatt
        {
            get { return avrio.remindBaseWatt; }
            set { avrio.remindBaseWatt = value; }
        }

        public static double VaildWatt
        {
            get { return avrio.vaildWatt; }
            set { avrio.vaildWatt = value; OnSysDataChanged("VaildWatt", value); }
        }

        public static byte ChargeStartWatt
        {
            get { return avrio.chargeStartWatt; }
            set { avrio.chargeStartWatt = value; }
        }

        private static List<ChargeWattPerUnitPrice> chargingPriceList = new List<ChargeWattPerUnitPrice>(); // 단가와 충전 시작 Watt를 저장
        private static List<ChargeListPerTimes> chargeHistoryList = new List<ChargeListPerTimes>();
        private static List<RemindWattPerTimes> remindWattList = new List<RemindWattPerTimes>();
        private static List<RemindWattPerTimes> remindChargingWattList = new List<RemindWattPerTimes>();

        public static List<RemindWattPerTimes> RemindChargingWattList
        {
            get { return avrio.remindChargingWattList; }
            set { avrio.remindChargingWattList = value; }
        }

        public static List<ChargeWattPerUnitPrice> ChargingPriceList
        {
            get { return avrio.chargingPriceList; }
            set { avrio.chargingPriceList = value; }
        }

        public static List<ChargeListPerTimes> ChargeHistoryList
        {
            get { return avrio.chargeHistoryList; }
            set { avrio.chargeHistoryList = value; }
        }
        public static List<RemindWattPerTimes> RemindWattList
        {
            get { return avrio.remindWattList; }
            set { avrio.remindWattList = value; }
        }

        private static string rapidasnumber = "00000000";                    // Rapidas기기 벊호

        private static DateTime chargeStartTime;                    // 충전시작시간
        private static DateTime chargeEndTime;                      // 충전종료시간
        private static byte[] userInfo = new byte[16];              // 사용자정보
        private static string cardNo = "0000000000000000";          // 카드넘버
        private static string cardDate = "0000";                    // 카드유효기간
        private static string userNo;                               // 사용자아이디
        private static int ctrlCode;                                // 충전기 제어코드
        private static bool devCtrl;                                // 장치제어
        private static int devType;                                 // 장치타입
        private static bool userConfirm;                            // 사용자확인
        private static byte bybmsRemainingBatteryEnergy;
        private static byte bybmsTotalBatteryCapacity;



        public static byte byBmsRemainingBatteryEnergy
        {
            get { return avrio.bybmsRemainingBatteryEnergy; }
            set { avrio.bybmsRemainingBatteryEnergy = value; }
        }

        public static byte byBmsTotalBatteryCapacity
        {
            get { return avrio.bybmsTotalBatteryCapacity; }
            set { avrio.bybmsTotalBatteryCapacity = value; }
        }


        public static bool UserConfirm
        {
            get { return avrio.userConfirm; }
            set { avrio.userConfirm = value; }
        }

        private static uint seqNumFault;                            // 고장이력시퀀스넘버
        private static uint seqNumHistory;                          // 충전이력시퀀스넘버
        private static uint seqNumPrice;                            // 단가이력시퀀스넘버

        public static uint SeqNumPrice
        {
            get { return avrio.seqNumPrice; }
            set { avrio.seqNumPrice = value; }
        }

        public static uint SeqNumHistory
        {
            get { return avrio.seqNumHistory; }
            set { avrio.seqNumHistory = value; }
        }

        public static uint SeqNumFault
        {
            get { return avrio.seqNumFault; }
            set { avrio.seqNumFault = value; }
        }

        public static int DevType
        {
            get { return avrio.devType; }
            set { avrio.devType = value; }
        }

        public static bool DevCtrl
        {
            get { return avrio.devCtrl; }
            set { avrio.devCtrl = value; }
        }

        // 16:즉시제어, 1:충전가능
        public static int CtrlCode
        {
            get { return avrio.ctrlCode; }
            set { avrio.ctrlCode = value; }
        }
        public static HSCMD TsHSCommand
        {
            get { return tsHSCommand; }
            set
            {
                tsHSCommand = value;
                if (SendHSOrder != null)
                {
                    SendHSOrder(tsHSCommand);
                }
            }
        }
        public static string CardNo
        {
            get { return avrio.cardNo; }
            set { avrio.cardNo = value; }
        }
        public static string RapiDasNumber
        {
            get { return avrio.rapidasnumber; }
            set { avrio.rapidasnumber = value; }
        }

        public static string CardDate
        {
            get { return avrio.cardDate; }
            set { avrio.cardDate = value; }
        }

        public static string UserNo
        {
            get { return avrio.userNo; }
            set { avrio.userNo = value; }
        }

        private static object thislock = new object();
        private static bool isFaultDialog = false;
        private static bool faultDialogDelay = false;
        private static int bCarBMSFault = 0;        // 0:정상, 1:자동차에서 폴트로 충전을 종료한 경우

        public static int CarBMSFault
        {
            get { return avrio.bCarBMSFault; }
            set { avrio.bCarBMSFault = value; }
        }

        //public static bool IsFaultDialog
        //{
        //    get { return avrio.isFaultDialog; }
        //    set { avrio.isFaultDialog = value; OnSysDataChanged("FaultDialog", value); }
        //}
        public static bool IsFaultDialog
        {
            get
            {
                lock (thislock)
                {
                    return avrio.isFaultDialog;
                }
            }
            set
            {
                lock (thislock)
                {
                    avrio.isFaultDialog = value;
                    OnSysDataChanged("FaultDialog", value);
                }
            }
        }

        // 1. 충전단가를 받아올때 발생하는 창
        private static bool isServerChargeCostDialog = false;


        public static bool IsServerChargeCostDialog
        {
            get { return avrio.isServerChargeCostDialog; }
            set { avrio.isServerChargeCostDialog = value; OnSysDataChanged("ServerChargeCostDialog", value); }
        }

        // 1. 사용자 인식 과정중 발생하는 창
        private static bool isServerConfirmDialog = false;

        public static bool IsServerConfirmDialog
        {
            get { return avrio.isServerConfirmDialog; }
            set { avrio.isServerConfirmDialog = value; OnSysDataChanged("ServerConfirmDialog", value); }
        }

        public static bool FaultDialogDelay
        {
            get { return avrio.faultDialogDelay; }
            set { avrio.faultDialogDelay = value; }
        }

        //public enum ChargeMode
        //{
        //    Money,
        //    Watt,
        //    Soc,
        //}

        private static int chargeMode;                           // 충전요청 모드 Soc Watt Money
        private static int charging_Mode;                        // 충전중 모드 CC CV CP
        private static bool isRfpadWork = false;
        private static double chargeValue;                       // 실제값
        private static int sendCommand;                          // 전송명령
        private static bool isChargeCheck = false;               // 결제승인여부
        private static string fanOffTimeSelect = "000000000000000000000000";
        private static int fanOffTime = 10;
        //   public static bool BatteryChacrge = false;
        // 1. Default 설정값은 true => 실패시 false로 변환

        private static bool isControlBDComm = false;
        private static bool isBMSCanComm = false;

        public static bool IsControlBDComm
        {
            get { return avrio.isControlBDComm; }
            set { avrio.isControlBDComm = value; }
        }
        public static bool IsRfpadWork
        {
            get { return avrio.isRfpadWork; }
            set { avrio.isRfpadWork = value; }
        }
        public static bool IsBMSCanComm
        {
            get { return avrio.isBMSCanComm; }
            set { avrio.isBMSCanComm = value; }
        }

        private static bool bShowWarnningDlg = false;

        public static bool ShowWarnningDlg
        {
            get { return avrio.bShowWarnningDlg; }
            set { avrio.bShowWarnningDlg = value; }
        }

        public static int FanOffTime
        {
            get { return avrio.fanOffTime; }
            set { avrio.fanOffTime = value; }
        }

        public static string FanOffTimeSelect
        {
            get { return avrio.fanOffTimeSelect; }
            set { avrio.fanOffTimeSelect = value; }
        }

        public static bool IsChargeCheck
        {
            get { return avrio.isChargeCheck; }
            set { avrio.isChargeCheck = value; }
        }

        public static int SendCommand
        {
            get { return avrio.sendCommand; }
            set { avrio.sendCommand = value; }
        }

        // 운영모드
        private static RunningMode runMode = RunningMode.Normal;
        private static AdminPage adminPage = AdminPage.Ready;
        private static TsCommand tsCommand = TsCommand.TsNone;
        private static TsCommand lastCommand = TsCommand.TsNone;
        private static HSCMD tsHSCommand = HSCMD.None;
        private static bool chargeStart = false;
        private static bool chargingStart = false;

        public static bool ChargingStart
        {
            get { return avrio.chargingStart; }
            set { avrio.chargingStart = value; }
        }

        public static bool ChargeStart
        {
            get { return avrio.chargeStart; }
            set { avrio.chargeStart = value; }
        }

        private static SysStatus currentStatus = SysStatus.SysStandby;
        private static SysStatus previousStatus = SysStatus.SysStandby;
        private static SysBatteryStatus currentBatStatus = SysBatteryStatus.SysBatteryStandby;
        public static event SysDataChangedEvent SysDataChanged;
        public static event SysStatusChangedEvent SysStatusChanged;
        public static event SysOrderEvent SendSysOrder;
        public static event AdminModeChangedEvent AdminModeChanged;
        public static event HyosungCardOrderEvent SendHSOrder;
        public static int DspLgBatteryStatus = 0;


        private static int nRapidasOutputw;
        private static int FaultCount = 0;
        private static string receiveData;
        private static string receiveSmart;
        private static string receiveServer;
        private static string eventMsg;
        private static string eventCanMsg;
        private static string eventDSPMsg;
        private static ushort changeStatus;
        private static string soundPlayCS;
        private static string sendMessage;
        private static bool isReadyCancel = false;
        private static string priceMsg;
        private static string soundPlayQS;
        private static WARNNINGTYPE warnningType = WARNNINGTYPE.None;
        private static FAULT_DLG_TYPE faultDlgType = FAULT_DLG_TYPE.None;
        private static ushort nGridInputPowerMax;
        private static Byte nOutCurrentMax;
        private static Byte nChargingTimeMax;

        private static int nreboottimehh;
        private static int nreboottimemm;
        private static ushort nfanoffset;

        private static string faultCode;
        private static bool isGridOnlyMode = false;
        private static bool startRunSeq = false;        // BATTERY 가 충전중일 경우 충전을 시작
        public static bool MouseCuser = false;
        public static bool Fualtstop = false;
        public static bool FualtstopFalg = false;
        public static bool CardCheckFlag = false;
        public static bool CardCheckFlag4 = false;
        public static bool CardCheckFlag5 = false;
        public static bool CardCheckFlag6 = false;
        public static int nRapidasModFlag = 0;
        private static bool onebatteryCharge = false;
        public static string nDspVersion;
        public static int nSbcVersion;
        private static bool onefaultcheck = false;
        private static bool windowsClose = false;
        private static bool windowsClose1 = false;


        private static bool faulstopbutton = false;
        private static bool useReboot = false;
        public static bool UseReboot
        {
            get { return avrio.useReboot; }
            set { avrio.useReboot = value; }
        }


        public static bool FaulStopButton
        {
            get { return avrio.faulstopbutton; }
            set { avrio.faulstopbutton = value; }
        }
        public static bool WwindowsClose1
        {
            get { return avrio.windowsClose1; }
            set { avrio.windowsClose1 = value; }
        }
        public static bool WwindowsClose
        {
            get { return avrio.windowsClose; }
            set { avrio.windowsClose = value; }
        }

        public static bool nonefaultcheck
        {
            get { return avrio.onefaultcheck; }
            set { avrio.onefaultcheck = value; }
        }

        public static string DspVersion
        {
            get { return nDspVersion; }
            set { nDspVersion = value; }
        }
        public static int SbcVersion
        {
            get { return nSbcVersion; }
            set { nSbcVersion = value; }
        }
        public static int faultCount
        {
            get { return FaultCount; }
            set { FaultCount = value; }
        }

        public static int RapidasModFlag
        {
            get { return avrio.nRapidasModFlag; }
            set { avrio.nRapidasModFlag = value; }
        }

        public static int RapidasOutputw
        {
            get { return avrio.nRapidasOutputw; }
            set { avrio.nRapidasOutputw = value; }
        }

        public static bool nMouseCuser
        {
            get { return avrio.MouseCuser; }
            set { avrio.MouseCuser = value; }
        }
        public static bool StartRunSeq
        {
            get { return avrio.startRunSeq; }
            set { avrio.startRunSeq = value; }
        }
        public static bool OnebatteryCharge
        {
            get { return avrio.onebatteryCharge; }
            set { avrio.onebatteryCharge = value; }
        }

        public static string FaultCode
        {
            get { return avrio.faultCode; }
            set { avrio.faultCode = value; }
        }

        public static FAULT_DLG_TYPE FaultDlgType
        {
            get { return avrio.faultDlgType; }
            set { avrio.faultDlgType = value; }
        }

        public static WARNNINGTYPE WarnningType
        {
            get { return avrio.warnningType; }
            set { avrio.warnningType = value; }
        }

        public static string SoundPlayQS
        {
            get { return avrio.soundPlayQS; }
            set
            {
                avrio.soundPlayQS = value;
                OnSysDataChanged("SoundPlayQS", value);
                EventMsg = value;
            }
        }
        public static bool GridOnlyMode
        {
            get { return isGridOnlyMode; }
            set { isGridOnlyMode = value; }
        }
        public static string PriceMsg
        {
            get { return avrio.priceMsg; }
            set { avrio.priceMsg = value; OnSysDataChanged("PriceMsg", value); }
        }

        public static bool IsReadyCancel
        {
            get { return avrio.isReadyCancel; }
            set { avrio.isReadyCancel = value; }
        }

        public static string SendMessage
        {
            get { return avrio.sendMessage; }
            set
            {
                avrio.sendMessage = value;
                OnSysDataChanged(value, value);
                EventMsg = value;
            }
        }

        public static string SoundPlayCS
        {
            get { return avrio.soundPlayCS; }
            set
            {
                avrio.soundPlayCS = value;
                OnSysDataChanged("SoundPlay", value);
                EventMsg = value;
            }
        }

        public static ushort ChangeStatus
        {
            get { return avrio.changeStatus; }
            set
            {
                avrio.changeStatus = value;
                OnSysDataChanged("ChangeStatus", value);
            }
        }

        public static string EventMsg
        {
            get { return avrio.eventMsg; }
            set
            {
                avrio.eventMsg = value;
                SystemLog(value, 0);
                System.Console.WriteLine(value);
            }
        }

        public static string EventCanMsg
        {
            get { return avrio.eventMsg; }
            set
            {
                avrio.eventCanMsg = value;
                SystemLog1(value, 1);
                // System.Console.WriteLine(value);
            }
        }

        public static string EventDSPMsg
        {
            get { return avrio.eventDSPMsg; }
            set
            {
                avrio.eventDSPMsg = value;
                SystemLog2(value, 2);
                // System.Console.WriteLine(value);
            }
        }

        public static string ReceiveServer
        {
            get { return avrio.receiveServer; }
            set { avrio.receiveServer = value; OnSysDataChanged("ReceiveServer", value); }
        }

        public static string ReceiveSmart
        {
            get { return avrio.receiveSmart; }
            set { avrio.receiveSmart = value; OnSysDataChanged("ReceiveSmart", value); }
        }

        public static string ReceiveData
        {
            get { return avrio.receiveData; }
            set { avrio.receiveData = value; OnSysDataChanged("ReceiveData", value); }
        }

        public static RunningMode RunMode
        {
            get { return avrio.runMode; }
            set { avrio.runMode = value; }
        }

        public static AdminPage AdminPage
        {
            get { return avrio.adminPage; }
            set
            {
                avrio.adminPage = value;
                if (AdminModeChanged != null)
                    AdminModeChanged(avrio.adminPage);
            }
        }

        public static int ChargeMode
        {
            get { return avrio.chargeMode; }
            set { avrio.chargeMode = value; }
        }

        public static int Charging_Mode
        {
            get { return avrio.charging_Mode; }
            set { avrio.charging_Mode = value; }
        }

        public static double ChargeValue
        {
            get { return avrio.chargeValue; }
            set { avrio.chargeValue = value; }
        }
        private static CSOSCMD csosCommand = CSOSCMD.None;

        public static CSOSCMD CsosCommand
        {
            get { return avrio.csosCommand; }
            set
            {
                avrio.csosCommand = value;
                if (SendCSOSOrder != null)
                {
                    switch (csosCommand)
                    {
                        default:
                            SendCSOSOrder(csosCommand);
                            break;
                    }
                }
            }
        }
        public static int CurrentTime
        {
            get { return avrio.currentTime; }
            set { avrio.currentTime = value; OnSysDataChanged("CurrentTime", value); }
        }


        public static int RapidasLoginType
        {
            get { return avrio.rapidaslogintype; }
            set { avrio.rapidaslogintype = value; }
        }
        public static int Rapidaslanguage
        {
            get { return avrio.RapidasLanguage; }
            set { avrio.RapidasLanguage = value; }
        }
        public static int BusinessCheckbar
        {
            get { return avrio.nBusinessCheckbar; }
            set { avrio.nBusinessCheckbar = value; }
        }

        public static int Rapidascardcheck
        {
            get { return avrio.RapidasCardCheck; }
            set { avrio.RapidasCardCheck = value; }
        }
        public static int Rapidasweekreboot
        {
            get { return avrio.RapidasWeekReboot; }
            set { avrio.RapidasWeekReboot = value; }
        }



        public static int Rapidaspassword
        {
            get { return avrio.RapidasPassWord; }
            set { avrio.RapidasPassWord = value; }
        }

        public static double[] ChargeUnitPriceArray
        {
            get { return avrio.chargeUnitPriceArray; }
            set { avrio.chargeUnitPriceArray = value; OnSysDataChanged("ChargeUnitPrice", value); }
        }

        public static double ChargeWatt
        {
            get { return avrio.chargeWatt; }
            set { avrio.chargeWatt = value; OnSysDataChanged("ChargeWatt", value); }
        }

        public static uint ChargePrice
        {
            get { return avrio.chargePrice; }
            set { avrio.chargePrice = value; OnSysDataChanged("ChargePrice", value); }
        }

        public static double ChargeSOC
        {
            get { return avrio.chargeSOC; }
            set { avrio.chargeSOC = value; OnSysDataChanged("ChargeSOC", value); }
        }

        public static double StartchargeSOC
        {
            get { return avrio.nStartchargeSOC; }
            set { avrio.nStartchargeSOC = value; }
        }
        public static double StopchargeSOC
        {
            get { return avrio.nStopchargeSOC; }
            set { avrio.nStopchargeSOC = value; }
        }

        public static DateTime ChargeStartTime
        {
            get { return avrio.chargeStartTime; }
            set { avrio.chargeStartTime = value; OnSysDataChanged("ChargeStartTime", value); }
        }

        public static DateTime ChargeEndTime
        {
            get { return avrio.chargeEndTime; }
            set { avrio.chargeEndTime = value; OnSysDataChanged("ChargeEndTime", value); }
        }

        public static byte[] UserInfo
        {
            get { return avrio.userInfo; }
            set { avrio.userInfo = value; OnSysDataChanged("UserInfo", avrio.userInfo); }
        }
        public static SysBatteryStatus CurrentBatStatus
        {
            get { return currentBatStatus; }
            set { currentBatStatus = value; }
        }
        public static ushort GridInputPowerMax
        {
            get { return nGridInputPowerMax; }
            set { nGridInputPowerMax = value; }
        }
        public static Byte OutCurrentMax
        {
            get { return nOutCurrentMax; }
            set { nOutCurrentMax = value; }
        }
        public static Byte ChargingTimeMax
        {
            get { return nChargingTimeMax; }
            set { nChargingTimeMax = value; }
        }

        public static int RebootTimehh
        {
            get { return nreboottimehh; }
            set { nreboottimehh = value; }
        }
        public static int RebootTimemm
        {
            get { return nreboottimemm; }
            set { nreboottimemm = value; }
        }
        public static ushort FanOffset
        {
            get { return nfanoffset; }
            set { nfanoffset = value; }
        }


        public static SysStatus CurrentStatus
        {
            get { return currentStatus; }
            set
            {
                previousStatus = currentStatus;
                currentStatus = value;

                OnSysStatusChanged();
            }
        }

        public static SysStatus PreviousStatus
        {
            get { return previousStatus; }
            set
            {
                previousStatus = value;
            }
        }

        public static TsCommand TsCommand
        {
            get { return tsCommand; }
            set
            {
                tsCommand = value;
                if (SendSysOrder != null)
                {
                    switch (tsCommand)
                    {
                        case TsCommand.TsVehicleStart:
                            SendSysOrder(TsCommand.TsVehicleStart);
                            ChargingPriceList.Clear();
                            break;
                        case TsCommand.TsVehicleFinish:
                            SendSysOrder(TsCommand.TsVehicleFinish);
                            break;
                        case TsCommand.TsVehicleReady:
                            SendSysOrder(TsCommand.TsVehicleReady);
                            break;
                        default:
                            SendSysOrder(tsCommand);
                            break;
                    }
                }
            }
        }

        private static void OnSysStatusChanged()
        {
            if (SysStatusChanged != null)
                SysStatusChanged(new SysStatusChangeEventArgs(currentStatus, previousStatus));
        }

        private static void OnSysDataChanged(string dataName, object value)
        {
            if (SysDataChanged != null)
                SysDataChanged(dataName, value);
        }
        public static event CSOSOrderEvent SendCSOSOrder;
        //public virtual void SetOrder(ushort order);
        //public virtual ushort GetOrder();

    }
}
