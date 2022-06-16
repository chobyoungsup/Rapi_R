using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using QuickChargeDisplay;
using Microsoft.Win32;
using System.Timers;
using System.Runtime.InteropServices;
using AVRIO;

namespace ModbusThread
{
    public class SysFaultList
    {
        private DateTime eventTime;
        private int num;
        private string content;

        public SysFaultList(DateTime dt, int n, string s)
        {
            eventTime = dt;
            num = n;
            content = s;
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public DateTime EventTime
        {
            get { return eventTime; }
            set { eventTime = value; }
        }
    }

    public enum ADMINCMDTYPE
    {
        Bit = 1,
        Long = 2,
        Short = 3,
        Int = 4,
        Double = 5,
    };

    public enum QCCODE : int
    {
        KEPCO = 1,
        PSTEK = 2,
        LS_Industrial_Sys = 3,
        LS_Cable = 4,
        KODS = 5,
        HASE_TECH = 6,
        DBT = 7,
        PNE = 8,
    }

    public enum VCODE : int
    {
        HYUNDAI_MOTORS = 1,
        KIA_MOTORS = 2,
        GM_DAEWOO = 3,
        RENAULT_SAMSUNG = 4,
        CTnT = 5,
        LEO_MOTORS = 6,
        TESLAR_MOTORS = 7,
    }

    public struct AdminOrder
    {
        public int address;
        public int offset;
        public object value;
        public int type;
    }

    public class ModbusQC
    {

        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);
        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern bool QueryPerformanceCounter(ref long PerformanceCount);

        private System.Timers.Timer WaitFualt;
        public System.Timers.Timer WaitFualt1;
        public System.Timers.Timer WaitFualt2;

        public System.Timers.Timer rebootTimer;

        private static Object obj = new Object();
        private SerialPort sp = new SerialPort();
        private BmsThread.DC_BMS_Thread Dc_Bms_Thread = new BmsThread.DC_BMS_Thread();
        public string modbusStatus;
        System.Timers.Timer timer = new System.Timers.Timer();
        bool isPolling = true;
        private static int pollCount;
        private static ushort currentStatus = 0;
        private static int chargingCount = 0;
        private static int nFaultSeqNum;
        private static int nChargeHistorySeqNum;

        private static int PreSysCommand = 0;

        // fault 리스트 관리용
        private static List<SysFaultList> faultList = new List<SysFaultList>();

        private AVRIO.qcio qcio = new AVRIO.qcio();
        private AVRIO.amiio amiio = new AVRIO.amiio();
        private int SendCommand;
        private int SendHSCommand;
        private bool bAdminCommand = false;
        private bool isFault = false;


        private int currentMinPrev = -1;
        private int currentHourPrev = -1;

        private AdminOrder adminCommand;

        public AdminOrder AdminCommand
        {
            get { return adminCommand; }
            set { adminCommand = value; }
        }
        bool bRtn = false;
        private byte modbusCommand;
        private ushort startAddress = 0;
        private ushort requestRegisters = 0;
        private byte mcuid = 1;
        private byte amiid = 7;
        private byte currentId = 0;

        private string subRoot = "AVRIO";
        private string portNum = "COM2";
        private int boudrate = 38400;

        private int MaxTimeOutCnt = 50;                     // 대략 10s
        private bool bInitVehicleBatteryEnergy = false;     // 차량에서 남은 배터리 용량값을 가져와서 충전량을 계산
        private bool bStopBtn = false;
        private bool bStartBtn = false;
        private bool bDcUse = false;      // 전력량계 없이 DC값을 이용해서 충전량을 계산
        private bool bAmiLineUse = false;
        private int nDspCommChk = -1;
        private int nSendCnt = 0;
        RebootFunc _RebootFunc;

        public bool IsFault
        {
            get { return isFault; }
            set { isFault = value; }
        }

        private static object thislock = new object();
        #region 생성자
        public ModbusQC()
        {
            AVRIO.avrio.fault321check = true;

            AVRIO.avrio.SendSysOrder += new AVRIO.SysOrderEvent(avrio_SendSysOrder);
            AVRIO.avrio.SendHSOrder += new AVRIO.HyosungCardOrderEvent(avrio_SendHSOrder);

            AVRIO.bmsio.bBMSCanCommErrorFlag = false;
            AVRIO.avrio.WaitFualtFlag = false;
            AVRIO.avrio.FaultStopCheck = false;
            WaitFualt = new System.Timers.Timer(12000); // 2초간 대기 (테스트)
            WaitFualt.Elapsed += new ElapsedEventHandler(timer_WaitFualt);
            WaitFualt.AutoReset = false;
            WaitFualt.Start();

            WaitFualt1 = new System.Timers.Timer(1000 * 20); // 15초간 대기 (테스트)
            WaitFualt1.Elapsed += new ElapsedEventHandler(timer_WaitFualt1);
            WaitFualt1.AutoReset = false;
            WaitFualt1.Start();

            WaitFualt2 = new System.Timers.Timer(500); // 15초간 대기 (테스트)
            WaitFualt2.Elapsed += new ElapsedEventHandler(timer_WaitFualt2);
            WaitFualt2.AutoReset = true;
            WaitFualt2.Start();

            rebootTimer = new System.Timers.Timer(1000 * 59); // 15초간 대기 (테스트)
            rebootTimer.Elapsed += new ElapsedEventHandler(timer_reboot);
            rebootTimer.AutoReset = true;
            // rebootTimer.Start();           

            /*
            PollFunctionQCTimer = new Multimedia.Timer();
            PollFunctionQCTimer.Mode = Multimedia.TimerMode.Periodic;
            PollFunctionQCTimer.Period = 50;
            PollFunctionQCTimer.Resolution = 1;
            PollFunctionQCTimer.Tick += new System.EventHandler(PollFunctionQC);
            */
        }
        #endregion

        private void timer_WaitFualt(object sender, ElapsedEventArgs e) //12초 후에 리셋인데..왜 하는건지..  모르겠다 
        {
            AVRIO.avrio.nonefaultcheck = true;
            AVRIO.avrio.IsFaultDialog = false;
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
            AVRIO.avrio.FaultCode = "";
            AVRIO.avrio.ChargeFaultCheck = true;
            AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
            AVRIO.avrio.Fualtstop = false;
            AVRIO.avrio.FualtstopFalg = false;
        }

        public void timer_WaitFualt1(object sender, ElapsedEventArgs e)  //20초 후에 진짜 Fault체크를 시작한다는 뜻인거 같다
        {
            //AVRIO.bmsio.bBMSCanCommErrorFlag = true;
            AVRIO.avrio.FaultStopCheck = true;
            AVRIO.avrio.WaitFualtFlag = true;
        }

        public void timer_reboot(object sender, ElapsedEventArgs e)
        {
            if ((AVRIO.avrio.nSelectCommand12 == 5) && (AVRIO.avrio.nOneRebootNum == 2))
            {
                //cbs  
                if (AVRIO.avrio.UseReboot) //대기 상태에서 재부팅 사용자 체크일떄 만
                {
                    AVRIO.avrio.EventMsg = "OS Reboot Now----> " + System.DateTime.Now.ToString();
                    System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");
                }
            }
        }

        public void timer_WaitFualt2(object sender, ElapsedEventArgs e)
        {
            if (AVRIO.avrio.nonefaultcheck)  //<- DSP에서 정의안된 FaultCode가 올라 올경우 초기화 하는 기능인것 같다
            {
                AVRIO.avrio.faultCount++;
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            }
            else AVRIO.avrio.faultCount = 0;


            if (AVRIO.avrio.faultCount >= 4)
            {
                AVRIO.avrio.WaitFualtFlag = true;
                AVRIO.avrio.Fualtstop = false;
                AVRIO.avrio.nonefaultcheck = false;
                AVRIO.avrio.IsFaultDialog = false;
                AVRIO.avrio.ChargeFaultCheck = true;
                AVRIO.avrio.FualtstopFalg = false;
            }
        }


        #region 관리자화면 명령처리
        void QCDV_AdminValueChanged(int address, int offset, object value, int type)
        {
            adminCommand.address = address;
            adminCommand.offset = offset;
            adminCommand.value = value;
            adminCommand.type = type;
            bAdminCommand = true;
        }
        #endregion

        #region AVIIO events
        void avrio_SendSysOrder(AVRIO.TsCommand command, params object[] list)
        {
            try
            {
                SendCommand = (int)command;
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        void avrio_SendHSOrder(AVRIO.HSCMD command, params object[] list)
        {
            try
            {
                SendHSCommand = (int)command;
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        #endregion

        #region Thread Run/Ready ...
        public void ThreadRun()
        {
            // 1. AVR 보드와 통신 연결
            // 2. RF_PAD와 통신 연결
            bool bThreadReady = false;
            ThreadReady();

            //portNum = "COM6";
            while (!bThreadReady)
            // while (bThreadReady)
            {
                if (Open(portNum, boudrate, 8, Parity.None, StopBits.One) == false)
                {
                    AVRIO.avrio.EventMsg = "[DSP PORT OPEN FAIL] SBC COM PORT OPEN FAIL!";
                    Thread.Sleep(3000);
                    // Thread.Sleep(100);
                }
                else
                {
                    AVRIO.avrio.EventMsg = "[DSP PORT OPEN] SBC COM PORT OPEN SUCCES!";
                    bThreadReady = true;
                }
            }

            AVRIO.avrio.IsRfpadWork = true;
            AVRIO.avrio.IsControlBDComm = true;
            AVRIO.avrio.bDSPCalibration = true; 

            while (bThreadReady)
            //  while (!bThreadReady)
            {
                // 
                // if(AVRIO.avrio.IsFaultDialog) // 버튼 주기 떄문에 변경
                Thread.Sleep(200);

                if (AVRIO.avrio.fault321check)
                    PollFunctionQC();

                Thread.Sleep(150);

                if ((rebootTimer.Enabled == false) && (AVRIO.avrio.nSelectCommand12 == 5) && (AVRIO.avrio.nOneRebootNum == 2)) //대기상태에서 만...
                    rebootTimer.Start();
                else if ((rebootTimer.Enabled == true) && (AVRIO.avrio.nSelectCommand12 != 5) && (AVRIO.avrio.nOneRebootNum == 2))
                    rebootTimer.Stop();


                //cbs 2020.03.30 재부팅기능 추가
                if (AVRIO.avrio.UseReboot && !AVRIO.avrio.RebootSelectDay) // 기존대로 일주일에 한번 재부팅
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    switch (currentDay)
                    {
                        case DayOfWeek.Sunday: //일
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 1)
                                {

                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();
                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }
                            }
                            break;

                        case DayOfWeek.Monday: //월
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 2)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();


                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }
                            }
                            break;

                        case DayOfWeek.Tuesday: //화
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 3)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();


                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }
                            }
                            break;

                        case DayOfWeek.Wednesday: //수
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 4)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();


                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }

                            }
                            break;

                        case DayOfWeek.Thursday: //목
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 5)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();


                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }

                            }
                            break;


                        case DayOfWeek.Friday: //금
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 6)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();
                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();
                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }
                            }
                            break;

                        case DayOfWeek.Saturday: //토
                            {
                                if (AVRIO.avrio.Rapidasweekreboot == 7)
                                {
                                    string qqwtime = System.DateTime.Now.ToString("HHmmss");
                                    string reboottime = "";
                                    string reboottime1 = "";
                                    string reboottime2 = "";

                                    if (AVRIO.avrio.RebootTimehh < 10)
                                    {
                                        reboottime1 = "0";
                                        reboottime1 += AVRIO.avrio.RebootTimehh.ToString();


                                    }
                                    else
                                    {
                                        reboottime1 = AVRIO.avrio.RebootTimehh.ToString();

                                    }
                                    if (AVRIO.avrio.RebootTimemm < 10)
                                    {
                                        reboottime2 = "0";
                                        reboottime2 += AVRIO.avrio.RebootTimemm.ToString();


                                    }
                                    else
                                    {
                                        reboottime2 = AVRIO.avrio.RebootTimemm.ToString();

                                    }

                                    reboottime = reboottime1 + reboottime2 + "00";

                                    if (qqwtime == reboottime)
                                        AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                                    qqwtime = null;
                                    reboottime = null;
                                    reboottime1 = null;
                                    reboottime2 = null;
                                }
                            }
                            break;
                    }
                }
                else if (AVRIO.avrio.UseReboot && AVRIO.avrio.RebootSelectDay) //cbs 2020.03.30 하루에 한번 재부팅 추가 선택요일 재부팅
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;

                    if (AVRIO.avrio.AdminPage != AVRIO.AdminPage.Ready) //cbs 2020.06.11 조건수정
                        continue;

                    if (AVRIO.avrio._RebootFunc.TryGetValue(currentDay, out _RebootFunc))
                    {
                        if (_RebootFunc.IsEnable)
                        {
                            string qqwtime = System.DateTime.Now.ToString("HHmmss");
                            string reboottime = "";
                            string reboottime1 = "";
                            string reboottime2 = "";

                            if (_RebootFunc.TimeHH < 10)
                            {
                                reboottime1 = "0";
                                reboottime1 += _RebootFunc.TimeHH.ToString();
                            }
                            else
                            {
                                reboottime1 = _RebootFunc.TimeHH.ToString();

                            }
                            if (_RebootFunc.Timemm < 10)
                            {
                                reboottime2 = "0";
                                reboottime2 += _RebootFunc.Timemm.ToString();
                            }
                            else
                            {
                                reboottime2 = _RebootFunc.Timemm.ToString();
                            }

                            reboottime = reboottime1 + reboottime2 + "00";

                            if (qqwtime == reboottime)
                                AVRIO.avrio.nOneRebootNum = 2;// nOneRebootNum = 1;

                            qqwtime = null;
                            reboottime = null;
                            reboottime1 = null;
                            reboottime2 = null;
                        }
                    }
                } 
            }
        }

        public void ThreadReady()
        {
            RegistryKey regKey;
            RegistryKey reg;
            if (Environment.Is64BitOperatingSystem)
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
                AVRIO.avrio.EventMsg = "DSP Registry->64";
            }
            else
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\AVRIO", true);
                AVRIO.avrio.EventMsg = "DSP Registry->32";
            }

            AVRIO.avrio.ChargingTimeMax = Convert.ToByte(reg.GetValue("CHARGETIME", "60"));
            string pn = Convert.ToString(reg.GetValue("Port", "COM2"));
            string br = Convert.ToString(reg.GetValue("Boudrate", "38400"));
            string mid = Convert.ToString(reg.GetValue("MCUID", "1"));
            string aid = Convert.ToString(reg.GetValue("AMIID", "7"));

            bDcUse = Convert.ToBoolean(reg.GetValue("DC_USE", "False"));
            bAmiLineUse = Convert.ToBoolean(reg.GetValue("AMI_LINE_USE", "False"));
            portNum = pn;

            boudrate = Convert.ToInt32(br);
            mcuid = (byte)Convert.ToInt32(mid);
            amiid = (byte)Convert.ToInt32(aid);

            AVRIO.avrio.EventMsg = $"DSP PortNum->{portNum}";
            AVRIO.avrio.EventMsg = $"DSP Boudrate->{boudrate}";

            if (AVRIO.avrio.CurrentStatus != AVRIO.SysStatus.SysRunning)
            {
                // reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("TIMEWATT");
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\TIMEWATT", true);

                AVRIO.avrio.TimeWattTime_0_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time00", "22"));
                AVRIO.avrio.TimeWattTime_0_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time01", "00"));
                AVRIO.avrio.TimeWattTime_0_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time02", "10"));
                AVRIO.avrio.TimeWattTime_0_3 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time03", "00"));
                AVRIO.avrio.TimeWattTime_0_4 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time04", "28"));

                reg.SetValue("Time00", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time00", "22"));
                reg.SetValue("Time01", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time01", "00"));
                reg.SetValue("Time02", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time02", "10"));
                reg.SetValue("Time03", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time03", "00"));
                reg.SetValue("Time04", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time04", "28"));

                AVRIO.avrio.TimeWattTime_1_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time10", "22"));
                AVRIO.avrio.TimeWattTime_1_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time11", "00"));
                AVRIO.avrio.TimeWattTime_1_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time12", "10"));
                AVRIO.avrio.TimeWattTime_1_3 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time13", "00"));
                AVRIO.avrio.TimeWattTime_1_4 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time14", "28"));

                reg.SetValue("Time10", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time10", "22"));
                reg.SetValue("Time11", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time11", "00"));
                reg.SetValue("Time12", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time12", "10"));
                reg.SetValue("Time13", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time13", "00"));
                reg.SetValue("Time14", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time14", "28"));

                AVRIO.avrio.TimeWattTime_2_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time20", "22"));
                AVRIO.avrio.TimeWattTime_2_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time21", "00"));
                AVRIO.avrio.TimeWattTime_2_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time22", "10"));
                AVRIO.avrio.TimeWattTime_2_3 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time23", "00"));
                AVRIO.avrio.TimeWattTime_2_4 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time24", "28"));

                reg.SetValue("Time20", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time20", "22"));
                reg.SetValue("Time21", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time21", "00"));
                reg.SetValue("Time22", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time22", "10"));
                reg.SetValue("Time23", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time23", "00"));
                reg.SetValue("Time24", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time24", "28"));

                AVRIO.avrio.TimeWattTime_3_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time30", "22"));
                AVRIO.avrio.TimeWattTime_3_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time31", "00"));
                AVRIO.avrio.TimeWattTime_3_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time32", "10"));
                AVRIO.avrio.TimeWattTime_3_3 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time33", "00"));
                AVRIO.avrio.TimeWattTime_3_4 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time34", "28"));

                reg.SetValue("Time30", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time30", "22"));
                reg.SetValue("Time31", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time31", "00"));
                reg.SetValue("Time32", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time32", "10"));
                reg.SetValue("Time33", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time33", "00"));
                reg.SetValue("Time34", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time34", "28"));

                AVRIO.avrio.TimeWattTime_4_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time40", "22"));
                AVRIO.avrio.TimeWattTime_4_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time41", "00"));
                AVRIO.avrio.TimeWattTime_4_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time42", "10"));
                AVRIO.avrio.TimeWattTime_4_3 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time43", "00"));
                AVRIO.avrio.TimeWattTime_4_4 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time44", "28"));

                reg.SetValue("Time40", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time40", "22"));
                reg.SetValue("Time41", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time41", "00"));
                reg.SetValue("Time42", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time42", "10"));
                reg.SetValue("Time43", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time43", "00"));
                reg.SetValue("Time44", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Time44", "28"));

                AVRIO.avrio.TimeWattSoc_0_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc00", "125"));
                AVRIO.avrio.TimeWattSoc_0_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc01", "100"));
                AVRIO.avrio.TimeWattSoc_0_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc02", "60"));

                reg.SetValue("Soc00", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc00", "125"));
                reg.SetValue("Soc01", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc01", "100"));
                reg.SetValue("Soc02", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc02", "60"));

                AVRIO.avrio.TimeWattSoc_1_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc10", "90"));
                AVRIO.avrio.TimeWattSoc_1_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc11", "90"));
                AVRIO.avrio.TimeWattSoc_1_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc12", "55"));

                reg.SetValue("Soc10", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc10", "90"));
                reg.SetValue("Soc11", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc11", "90"));
                reg.SetValue("Soc12", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc12", "55"));

                AVRIO.avrio.TimeWattSoc_2_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc20", "75"));
                AVRIO.avrio.TimeWattSoc_2_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc21", "75"));
                AVRIO.avrio.TimeWattSoc_2_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc22", "40"));

                reg.SetValue("Soc20", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc20", "75"));
                reg.SetValue("Soc21", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc21", "75"));
                reg.SetValue("Soc22", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc22", "40"));

                AVRIO.avrio.TimeWattSoc_3_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc30", "65"));
                AVRIO.avrio.TimeWattSoc_3_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc31", "65"));
                AVRIO.avrio.TimeWattSoc_3_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc32", "30"));

                reg.SetValue("Soc30", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc30", "65"));
                reg.SetValue("Soc31", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc31", "65"));
                reg.SetValue("Soc32", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc32", "30"));

                AVRIO.avrio.TimeWattSoc_4_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc40", "55"));
                AVRIO.avrio.TimeWattSoc_4_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc41", "55"));
                AVRIO.avrio.TimeWattSoc_4_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc42", "20"));

                reg.SetValue("Soc40", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc40", "55"));
                reg.SetValue("Soc41", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc41", "55"));
                reg.SetValue("Soc42", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc42", "20"));

                AVRIO.avrio.TimeWattSoc_5_0 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc50", "30"));
                AVRIO.avrio.TimeWattSoc_5_1 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc51", "25"));
                AVRIO.avrio.TimeWattSoc_5_2 = Convert.ToInt32(QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc52", "5"));

                reg.SetValue("Soc50", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc50", "30"));
                reg.SetValue("Soc51", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc51", "25"));
                reg.SetValue("Soc52", QuickChargeConfig.ChargeConfig.GetConfig1(subRoot, "Soc52", "5"));
            }
        }
        #endregion

        #region AMI 읽어오기
        //public void AmiReadValues(AVRIO.RunningMode runMode)
        //{
        //    //Create array to accept read values:
        //    ushort[] values = new ushort[amiio.ReadRegisters];
        //    ushort pollStart = (ushort)AVRIO.amiio.READADDRESS.Start;
        //    ushort pollLength = amiio.ReadRegisters;

        //    //Read registers and display data in desired format:
        //    try
        //    {
        //        bool ret = SendFc4((byte)amiid, pollStart, pollLength, ref values);
        //        if (ret)
        //        {
        //            ParserAmiReadData(values, runMode);
        //        }
        //        else
        //        {
        //            modbusStatus = "AmiReadValues SendFc4 Error";
        //            // System.Console.WriteLine(modbusStatus);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        AVRIO.avrio.EventMsg = "AmiReadValues Error in modbus read: " + err.Message;
        //        // System.Console.WriteLine(modbusStatus);
        //    }
        //}

        //public void ParserAmiReadData(ushort[] values, AVRIO.RunningMode runMode)
        //{
        //    byte[] byTemp1 = BitConverter.GetBytes(values[0]);
        //    byte[] byTemp2 = BitConverter.GetBytes(values[1]);
        //    byte[] byTemp = new byte[4];
        //    byTemp[0] = byTemp1[0];
        //    byTemp[1] = byTemp1[1];
        //    byTemp[2] = byTemp2[0];
        //    byTemp[3] = byTemp2[1];
        //    AVRIO.avrio.VaildWatt = BitConverter.ToUInt32(byTemp, 0);

        //    #region QC Amdin PowerMeter
        //    if (AVRIO.avrio.RunMode == AVRIO.RunningMode.Admin)
        //    {
        //        for (int i = 0; i < (amiio.ReadRegisters / 2); i += 2)
        //        {
        //            byte[] byTemp3 = BitConverter.GetBytes(values[i * 2]);
        //            byte[] byTemp4 = BitConverter.GetBytes(values[i * 2 + 1]);
        //            byte[] byTemp0 = new byte[4];
        //            byTemp0[0] = byTemp3[0];
        //            byTemp0[1] = byTemp3[1];
        //            byTemp0[2] = byTemp4[0];
        //            byTemp0[3] = byTemp4[1];
        //            if (i == 0 || i == 1)
        //            {
        //                amiio.readLongData[i] = BitConverter.ToUInt32(byTemp0, 0);
        //            }
        //            else
        //            {
        //                amiio.readFloatData[i - 2] = BitConverter.ToSingle(byTemp0, 0);
        //            }
        //        } // for
        //    }
        //    #endregion PowerMeter

        //}

        #endregion

        public void SendCommandQC()
        {
            //cbs 2020.02.19
            string Temp1 = "null";
            string Temp2 = "null";
            string Temp3 = "null";
            string Temp4 = "null";

            //  if (AVRIO.avrio.fault321check)
            {
                bool ret = false;
                byte[] byTemp_2 = new byte[2];
                int sendSysCommand = SendCommand;
                //Create array to accept read values:
                ushort[] values = new ushort[qcio.WriteRegisters];
                ushort pollStart = (ushort)AVRIO.qcio.WRITEADDRESS.Command;
                ushort pollLength = qcio.WriteRegisters;
                // ushort maxvalue =0;
                short threehundred = 200, fourhundred = 100, fivehundred = 0;
                ////////////////////////////////////////////////////////
                //LEE 여기 에다가 Status 정보를 더해서 보낸다......
                // TsBMSBatteryDischarge 도 여기서 더해서 보낸다.....
                // 여기에서 전부 연산 해서 보내면 됨..........
                //////////////////////////////////////////////////////

                try
                {
                    if (AVRIO.avrio.DisChargeFalg)
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBMSBatteryDischarge;
                    }
                    if (AVRIO.avrio.WarrningFalg || AVRIO.bmsio.bBMSWarnFlag)
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBMSBatteryWarning;
                    }
                    if (AVRIO.bmsio.byBMSStatus == 1)
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBMSStatus_1;
                    }
                    else if (AVRIO.bmsio.byBMSStatus == 2)
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBMSStatus_2;
                    }
                    else if (AVRIO.bmsio.byBMSStatus == 3) //TsBatteryStatusReady
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBMSStatus_3;//+ (int)AVRIO.TsCommand.TsBatteryPRAON; 
                    }
                    //if (AVRIO.bmsio.byFanControl == 1)
                    //{
                    //    SendCommand = SendCommand + (int)AVRIO.TsCommand.TsBatteryFanOn;
                    //}
                    if (AVRIO.avrio.GridOnlyMode == true)
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsGridOnlyOperation;
                    }

                    if (SendCommand == 800)
                        AVRIO.avrio.OnebatteryCharge = true;

                    //cbs 2020.02.17
                    if (AVRIO.avrio.bDSPCalibration && nDspCommChk == 0) //관리자 Calibration Set 눌렀을때 + 통신이 정상
                    {
                        SendCommand = SendCommand + (int)AVRIO.TsCommand.TsCalibration;
                        nSendCnt++;
                        if (nSendCnt > 3) //혹시나 DSP에서 초기화 안될때...2020.06.19  
                            AVRIO.avrio.bDSPCalibration = false;
                    }

                    values[0] = (UInt16)SendCommand;
                    // AVRIO.bmsio.nAverageCellVoltage = 3751;

                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.nAverageCellVoltage);
                    values[1] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    QuickChargeDisplay.QCDV.ManualControl.BatteryVoltage = string.Format("{0:F3}", ((double)AVRIO.bmsio.nAverageCellVoltage / 1000 * 56));
                    QuickChargeDisplay.QCDV.ManualControl.AverageCellVoltage = string.Format("{0:F3}", ((double)AVRIO.bmsio.nAverageCellVoltage / 1000));

                    //  string.Format("{0:F1}", (AVRIO.bmsio.SendData_301.nBatteryCurrent / 10 - 300));
                    //  QuickChargeDisplay.QCDV.ManualControl.BatteryVoltage = Math.Floor((((double)AVRIO.bmsio.nAverageCellVoltage / 1000 * 56)* 1000) / 1000).ToString();
                    //  QuickChargeDisplay.QCDV.ManualControl.AverageCellVoltage = Math.Floor((((double)AVRIO.bmsio.nAverageCellVoltage / 1000) * 1000) / 1000).ToString();
                    //  AVRIO.bmsio.nMaxTemperature = 1255;

                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.nMaxTemperature + ((ushort)AVRIO.avrio.FanOffset * 10));
                    values[2] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    //  QuickChargeDisplay.QCDV.ManualControl.AverageTemperature = Math.Floor((((double)AVRIO.bmsio.nMaxTemperature / 10 - 100) * 10) / 10).ToString(); // LEE
                    // AverageTemperature 에서 nMaxTemperature 변경
                    QuickChargeDisplay.QCDV.ManualControl.AverageTemperature = string.Format("{0:F1}", ((double)AVRIO.bmsio.nMaxTemperature / 10 - 100));

                    //AVRIO.bmsio.nChargePowerLimit = 2500;
                    //2014-12-16 Scale 조정 ####/10
                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.nChargePowerLimit / 10);
                    values[3] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    // AVRIO.bmsio.nDischargePowerLimit = 3000;
                    //2014-12-16 Scale 조정 ###/10
                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.nDischargePowerLimit / 10);
                    values[4] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    // AVRIO.bmsio.n30MinuteDischargePowerLimit = 3000;
                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.n30MinuteDischargePowerLimit);
                    values[5] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    // AVRIO.bmsio.bySOC = 50;

                    //if (AVRIO.bmsio.byBMSStatus == 3)
                    //{

                    byTemp_2 = BitConverter.GetBytes((UInt16)AVRIO.bmsio.bySOC);
                    values[6] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    //}
                    //else
                    //{
                    //    byTemp_2 = BitConverter.GetBytes((UInt16)AVRIO.bmsio.bySOC);
                    //    values[6] = (ushort)((((ushort)(byTemp_2[1] << 8)) | (ushort)0x00));
                    //}


                    QuickChargeDisplay.QCDV.ManualControl.SOC = AVRIO.bmsio.bySOC.ToString();

                    byTemp_2 = BitConverter.GetBytes(AVRIO.bmsio.nChargeVoltageSetPoint);
                    values[7] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    QuickChargeDisplay.QCDV.ManualControl.ChargeVoltageSetPoint = Math.Floor((((double)AVRIO.bmsio.nChargeVoltageSetPoint / 100) * 1000) / 1000).ToString();

                    // AVRIO.bmsio.bySOCLowValueSetPoint = 5;
                    // AVRIO.bmsio.bySOCHighValueSetPoint = 95;

                    byTemp_2 = BitConverter.GetBytes((UInt16)AVRIO.bmsio.bySOCLowValueSetPoint);
                    byTemp_2[1] = AVRIO.bmsio.bySOCHighValueSetPoint;
                    values[8] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    if (AVRIO.avrio.CurrentStatus != AVRIO.SysStatus.SysRunning)
                    {
                        if (AVRIO.avrio.GridInputPowerMax >= (ushort)500 && AVRIO.avrio.GridInputPowerMax <= (ushort)1000)
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.GridInputPowerMax + threehundred);

                        else if (AVRIO.avrio.GridInputPowerMax > (ushort)1000 && AVRIO.avrio.GridInputPowerMax <= (ushort)2000)
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.GridInputPowerMax + fourhundred);

                        else if (AVRIO.avrio.GridInputPowerMax > (ushort)2000 && AVRIO.avrio.GridInputPowerMax <= (ushort)2800)
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.GridInputPowerMax + fivehundred);
                    }


                    byTemp_2 = BitConverter.GetBytes(AVRIO.avrio.GridInputPowerMax);
                    values[9] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);

                    //nChargingTimeMax , nOutCurrentMax

                    byTemp_2 = BitConverter.GetBytes((UInt16)AVRIO.avrio.OutCurrentMax);
                    byTemp_2[1] = AVRIO.avrio.ChargingTimeMax;
                    //byTemp_2[1] = AVRIO.avrio.OutCurrentMax;
                    values[10] = (ushort)((ushort)(byTemp_2[1] << 8) | (ushort)byTemp_2[0]);



                    //cbs 2020.02.18 DSP 연산정확도 떄문에 1000을 추가로 곱해서 보냄 
                    int Temp = AVRIO.avrio.nSET_Voltage_Slope * 1000;
                    byte[] nTemp = BitConverter.GetBytes(Temp);
                    values[12] = (ushort)((ushort)(nTemp[1] << 8) | (ushort)nTemp[0]);
                    values[11] = (ushort)((ushort)(nTemp[3] << 8) | (ushort)nTemp[2]);
                    Temp1 = Temp.ToString();

                    Temp = AVRIO.avrio.nSET_Voltage_Offset * 1000;
                    nTemp = BitConverter.GetBytes(Temp);
                    values[14] = (ushort)((ushort)(nTemp[1] << 8) | (ushort)nTemp[0]);
                    values[13] = (ushort)((ushort)(nTemp[3] << 8) | (ushort)nTemp[2]);
                    Temp2 = Temp.ToString();

                    Temp = AVRIO.avrio.nSET_Current_Slope * 1000;
                    nTemp = BitConverter.GetBytes(Temp);
                    values[16] = (ushort)((ushort)(nTemp[1] << 8) | (ushort)nTemp[0]);
                    values[15] = (ushort)((ushort)(nTemp[3] << 8) | (ushort)nTemp[2]);
                    Temp3 = Temp.ToString();

                    Temp = AVRIO.avrio.nSET_Current_Offset * 1000;
                    nTemp = BitConverter.GetBytes(Temp);
                    values[18] = (ushort)((ushort)(nTemp[1] << 8) | (ushort)nTemp[0]);
                    values[17] = (ushort)((ushort)(nTemp[3] << 8) | (ushort)nTemp[2]);
                    Temp4 = Temp.ToString();

                }
                catch (Exception err)
                {
                    AVRIO.Log.Eventlog = err.ToString();
                }

                try
                {
                    ret = SendFc16((byte)mcuid, pollStart, pollLength, values);
                }
                catch (Exception err)
                {
                    AVRIO.avrio.EventMsg = "SendCommandQC Error in modbus write : " + err.Message;
                    return;
                }

                if (ret)
                {
                    nDspCommChk = 0;
                    /*
                    AVRIO.avrio.EventDSPMsg = "[SEND], " + values[0].ToString() + " , " + AVRIO.bmsio.nAverageCellVoltage.ToString() + " , " + AVRIO.bmsio.nMaxTemperature.ToString() + " , " + AVRIO.bmsio.nChargePowerLimit.ToString() + " , "
                            + AVRIO.bmsio.nDischargePowerLimit.ToString() + " , " + AVRIO.bmsio.n30MinuteDischargePowerLimit.ToString() + " , " + AVRIO.bmsio.bySOC.ToString() + " , "
                            + AVRIO.bmsio.nChargeVoltageSetPoint.ToString() + " , " + AVRIO.bmsio.bySOCLowValueSetPoint.ToString() + " , " + AVRIO.bmsio.bySOCHighValueSetPoint.ToString() + ", "
                            + AVRIO.avrio.GridInputPowerMax.ToString();
                    */

                    AVRIO.avrio.EventMsg = "[SEND], " + values[0].ToString() + " , " + AVRIO.bmsio.nAverageCellVoltage.ToString() + " , " + AVRIO.bmsio.nMaxTemperature.ToString() + " , " + (AVRIO.bmsio.nChargePowerLimit / 10).ToString() + " , "
                             + (AVRIO.bmsio.nDischargePowerLimit / 10).ToString() + " , " + AVRIO.bmsio.n30MinuteDischargePowerLimit.ToString() + " , " + AVRIO.bmsio.bySOC.ToString() + " , "
                             + AVRIO.bmsio.nChargeVoltageSetPoint.ToString() + " , " + AVRIO.bmsio.bySOCLowValueSetPoint.ToString() + " , " + AVRIO.bmsio.bySOCHighValueSetPoint.ToString() + ", "
                             + AVRIO.avrio.GridInputPowerMax.ToString() + " , " + AVRIO.avrio.OutCurrentMax.ToString()
                            /*cbs 2020.02.17*/
                            + " , " + Temp1
                            + " , " + Temp2
                            + " , " + Temp3
                            + " , " + Temp4;


                    //AVRIO.avrio.EventMsg = "Voltage_Slope -> " + AVRIO.avrio.nSET_Voltage_Slope.ToString();
                    //AVRIO.avrio.EventMsg = "Voltage_Offset -> " + AVRIO.avrio.nSET_Voltage_Offset.ToString();
                    //AVRIO.avrio.EventMsg = "Current_Slope -> " + AVRIO.avrio.nSET_Current_Slope.ToString();
                    //AVRIO.avrio.EventMsg = "Current_Offset -> " + AVRIO.avrio.nSET_Current_Offset.ToString();


                    AVRIO.avrio.sCommandValue = values[0];

                    if (PreSysCommand != sendSysCommand)
                    {
                        AVRIO.avrio.EventMsg = "sendSysCommand :::" + ((AVRIO.TsCommand)sendSysCommand).ToString();

                        PreSysCommand = sendSysCommand;

                        switch (sendSysCommand)
                        {
                            case (int)AVRIO.TsCommand.TsNone:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsStandby:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsVehicleReady:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsVehicleStart:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsVehicleFinish:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsBatteryStart:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsBatteryFinish:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsFault:
                                {
                                    break;
                                }
                            case (int)AVRIO.TsCommand.TsResetFault:
                                {
                                    try
                                    {
                                        DateTime dt = DateTime.Now;
                                        foreach (SysFaultList sfl in faultList)
                                        {
                                            string msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + sfl.Num + "," + sfl.Content + ",N";
                                            QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, sfl.EventTime);
                                        }
                                        faultList.Clear();
                                    }
                                    catch (Exception err)
                                    {
                                        AVRIO.Log.Eventlog = err.ToString();
                                    }
                                    //AVRIO.avrio.IsFaultDialog = false;
                                    break;
                                }
                        }
                    }
                    SendCommand = (int)AVRIO.TsCommand.TsNone;
                }
                //2020.02.17 cbs 주석처리
                // else AVRIO.avrio.EventMsg = "SendCommandQC Error in modbus write :";

                //2020.02.17 cbs
                else
                {
                    nDspCommChk--;
                    // AVRIO.avrio.EventMsg = "SendCommandQC Error in modbus write :";
                }
            }
        }

        #region AdminCommand 를 위해서 남겨준 코드
        /*
        public void SendAdminCommandQC()
        {
            bool ret = false;
//Create array to accept read values:
#if ONE_REGIST
            ER
            ushort[] values = new ushort[qcio.WriteAdminRegisters];
            ushort pollStart = (ushort)AdminCommand.address;
            ushort pollLength = qcio.WriteAdminRegisters;
#else
            ushort[] values = new ushort[qcio.WriteAdminRegisters];
            ushort pollStart = (ushort)AVRIO.qcio.WRITEADDRESS.Command;
            ushort pollLength = qcio.WriteAdminRegisters;
#endif

            if (QCDV.ManualControl.M_200_10)
            {
                values[0] = (ushort)((ushort)1 << 10);
            }

            switch (adminCommand.type)
            {
                case (int)ADMINCMDTYPE.Bit:
                    if ((bool)adminCommand.value)
                    {
#if ONE_REGISTER
                        values[0] = (ushort)((ushort)1 << adminCommand.offset);
                        pollLength = 1;
#else
                        values[adminCommand.address - (int)AVRIO.qcio.WRITEADDRESS.Command] = (ushort)((ushort)1 << adminCommand.offset);
#endif
                    }
                    break;
                case (int)ADMINCMDTYPE.Long:
                    {
                        int address = adminCommand.address - (int)AVRIO.qcio.WRITEADDRESS.Command;
                        byte[] ba = BitConverter.GetBytes((long)AVRIO.avrio.ChargePrice);

#if ONE_REGISTER
                        pollLength = 2;
                        // 배열순서 1032로 재배치
                        values[0] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        values[1] = (ushort)((ushort)(ba[3] << 8) | (ushort)ba[2]);
#else
                        // 배열순서 1032로 재배치
                        values[0 + address] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        values[1 + address] = (ushort)((ushort)(ba[3] << 8) | (ushort)ba[2]);
#endif
                    }
                    break;
                case (int)ADMINCMDTYPE.Short:
                    {
#if ONE_REGISTER
                        pollLength = 1;
                        try
                        {
                            int n = (int)adminCommand.value;
                            byte[] ba = BitConverter.GetBytes((ushort)n);
                            values[0] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        }
                        catch (Exception e)
                        {
                            ushort n = (ushort)adminCommand.value;
                            byte[] ba = BitConverter.GetBytes((ushort)n);
                            values[0] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        }
#else
                        try
                        {
                            int n = (int)adminCommand.value;
                            byte[] ba = BitConverter.GetBytes((ushort)n);
                            values[adminCommand.address - (int)AVRIO.qcio.WRITEADDRESS.Command] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        }
                        catch (Exception e)
                        {
                            ushort n = (ushort)adminCommand.value;
                            byte[] ba = BitConverter.GetBytes((ushort)n);
                            values[adminCommand.address - (int)AVRIO.qcio.WRITEADDRESS.Command] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        }
#endif
                    }
                    break;
                case (int)ADMINCMDTYPE.Double:
                    {
                        int address = adminCommand.address - (int)AVRIO.qcio.WRITEADDRESS.Command;
                        double d = (double)adminCommand.value;
                        byte[] ba = BitConverter.GetBytes((float)d);

#if ONE_REGISTER
                        pollLength = 2;
                        // 배열순서 1032로 재배치
                        values[0] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        values[1] = (ushort)((ushort)(ba[3] << 8) | (ushort)ba[2]);
#else
                        // 배열순서 1032로 재배치
                        values[0 + address] = (ushort)((ushort)(ba[1] << 8) | (ushort)ba[0]);
                        values[1 + address] = (ushort)((ushort)(ba[3] << 8) | (ushort)ba[2]);
#endif
                    }
                    break;
            }

            try
            {
                ret = SendFc16((byte)mcuid, pollStart, pollLength, values);
            }
            catch (Exception err)
            {
                modbusStatus = "Error in modbus write: " + err.Message;
                // System.Console.WriteLine(modbusStatus);
                return;
            }
            if (ret)
            {
                bAdminCommand = false;
                if (adminCommand.address == 201)
                {
                    switch (adminCommand.offset)
                    {
                        case 0:
                        case 2:
                        case 3:
                            {
                                if ((bool)adminCommand.value)
                                {
                                    adminCommand.value = false;
                                    bAdminCommand = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (adminCommand.address == 202)
                {
                    switch (adminCommand.offset)
                    {
                        case 0:
                        case 2:
                            {
                                if ((bool)adminCommand.value)
                                {
                                    adminCommand.value = false;
                                    bAdminCommand = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        */
        #endregion

        // 급속 폴링 함수
        // private void PollFunctionQC(object sender, System.EventArgs e)

        public void PollFunctionQC()
        {
            try
            {
                DateTime dt = DateTime.Now;
                int currentHour;
                int currentMin;
                currentHour = dt.Hour;
                currentMin = dt.Minute;
                AVRIO.RunningMode runMode = AVRIO.avrio.RunMode;


                #region 수전전력 init
                string firsttime, secondtime;
                string firsttime1, secondtime1;
                string firsttime2, secondtime2;
                string firsttime3, secondtime3;
                string firsttime4, secondtime4;


                double NowTime = Convert.ToDouble(System.DateTime.Now.ToString("HHmm"));
                ///////////////////////////////////////////////////////////////////////////////////////////
                if (AVRIO.avrio.TimeWattTime_0_0 < 10)
                {
                    firsttime = "0";
                    firsttime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_0);
                }
                else firsttime = Convert.ToString(AVRIO.avrio.TimeWattTime_0_0);

                if (AVRIO.avrio.TimeWattTime_0_1 < 10)
                {
                    firsttime += "0";
                    firsttime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_1);
                }
                else firsttime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_1);


                double TimeWattTime_1 = Convert.ToDouble(firsttime);
                if (AVRIO.avrio.TimeWattTime_0_2 < 10)
                {
                    secondtime = "0";
                    secondtime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_2);
                }
                else secondtime = Convert.ToString(AVRIO.avrio.TimeWattTime_0_2);

                if (AVRIO.avrio.TimeWattTime_0_3 < 10)
                {
                    secondtime += "0";
                    secondtime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_3);
                }
                else secondtime += Convert.ToString(AVRIO.avrio.TimeWattTime_0_3);
                double TimeWattTime_2 = Convert.ToDouble(secondtime);

                /////////////////////////////////////////////////////////////////////////////////////////////

                if (AVRIO.avrio.TimeWattTime_1_0 < 10)
                {
                    firsttime1 = "0";
                    firsttime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_0);
                }
                else firsttime1 = Convert.ToString(AVRIO.avrio.TimeWattTime_1_0);
                if (AVRIO.avrio.TimeWattTime_1_1 < 10)
                {
                    firsttime1 += "0";
                    firsttime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_1);
                }
                else firsttime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_1);
                double TimeWattTime_3 = Convert.ToDouble(firsttime1);

                if (AVRIO.avrio.TimeWattTime_1_2 < 10)
                {
                    secondtime1 = "0";
                    secondtime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_2);
                }
                else secondtime1 = Convert.ToString(AVRIO.avrio.TimeWattTime_1_2);

                if (AVRIO.avrio.TimeWattTime_1_3 < 10)
                {
                    secondtime1 += "0";
                    secondtime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_3);
                }
                else secondtime1 += Convert.ToString(AVRIO.avrio.TimeWattTime_1_3);
                double TimeWattTime_4 = Convert.ToDouble(secondtime1);

                /////////////////////////////////////////////////////////////////////////////////////////////

                if (AVRIO.avrio.TimeWattTime_2_0 < 10)
                {
                    firsttime2 = "0";
                    firsttime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_0);
                }
                else firsttime2 = Convert.ToString(AVRIO.avrio.TimeWattTime_2_0);

                if (AVRIO.avrio.TimeWattTime_2_1 < 10)
                {
                    firsttime2 += "0";
                    firsttime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_1);
                }
                else firsttime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_1);
                double TimeWattTime_5 = Convert.ToDouble(firsttime2);

                if (AVRIO.avrio.TimeWattTime_2_2 < 10)
                {
                    secondtime2 = "0";
                    secondtime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_2);
                }
                else secondtime2 = Convert.ToString(AVRIO.avrio.TimeWattTime_2_2);

                if (AVRIO.avrio.TimeWattTime_2_3 < 10)
                {
                    secondtime2 += "0";
                    secondtime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_3);
                }
                else secondtime2 += Convert.ToString(AVRIO.avrio.TimeWattTime_2_3);
                double TimeWattTime_6 = Convert.ToDouble(secondtime2);

                /////////////////////////////////////////////////////////////////////////////////////////////
                if (AVRIO.avrio.TimeWattTime_3_0 < 10)
                {
                    firsttime3 = "0";
                    firsttime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_0);
                }
                else firsttime3 = Convert.ToString(AVRIO.avrio.TimeWattTime_3_0);

                if (AVRIO.avrio.TimeWattTime_3_1 < 10)
                {
                    firsttime3 += "0";
                    firsttime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_1);
                }
                else firsttime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_1);
                double TimeWattTime_7 = Convert.ToDouble(firsttime3);

                if (AVRIO.avrio.TimeWattTime_3_2 < 10)
                {
                    secondtime3 = "0";
                    secondtime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_2);
                }
                else secondtime3 = Convert.ToString(AVRIO.avrio.TimeWattTime_3_2);

                if (AVRIO.avrio.TimeWattTime_3_3 < 10)
                {
                    secondtime3 += "0";
                    secondtime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_3);
                }
                else secondtime3 += Convert.ToString(AVRIO.avrio.TimeWattTime_3_3);
                double TimeWattTime_8 = Convert.ToDouble(secondtime3);
                ////////////////////////////////////////////////////////////////////////////////////////////

                if (AVRIO.avrio.TimeWattTime_4_0 < 10)
                {
                    firsttime4 = "0";
                    firsttime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_0);
                }
                else firsttime4 = Convert.ToString(AVRIO.avrio.TimeWattTime_4_0);

                if (AVRIO.avrio.TimeWattTime_4_1 < 10)
                {
                    firsttime4 += "0";
                    firsttime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_1);
                }
                else firsttime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_1);
                double TimeWattTime_9 = Convert.ToDouble(firsttime4);

                if (AVRIO.avrio.TimeWattTime_4_2 < 10)
                {
                    secondtime4 = "0";
                    secondtime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_2);
                }
                else secondtime4 = Convert.ToString(AVRIO.avrio.TimeWattTime_4_2);

                if (AVRIO.avrio.TimeWattTime_4_3 < 10)
                {
                    secondtime4 += "0";
                    secondtime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_3);
                }
                else secondtime4 += Convert.ToString(AVRIO.avrio.TimeWattTime_4_3);
                double TimeWattTime_10 = Convert.ToDouble(secondtime4);
                ////////////////////////////////////////////////////////////////////////////////////////////

                #endregion

                pollCount %= 2;
                if (pollCount == 0)
                {
                    if (AVRIO.avrio.CurrentStatus != AVRIO.SysStatus.SysRunning)
                    {

                        #region 수전전력 Select
                        if ((TimeWattTime_3 < NowTime) && (NowTime <= TimeWattTime_4))
                        {
                            #region 수전전력 Select 1
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.TimeWattTime_1_4 * 100);
                            if ((0x0A <= AVRIO.avrio.TimeWattTime_1_4) && (0x0E >= AVRIO.avrio.TimeWattTime_1_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_2;
                                }
                            }
                            else if ((0X19 <= AVRIO.avrio.TimeWattTime_1_4) && (0X1B >= AVRIO.avrio.TimeWattTime_1_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_2;
                                }
                            }
                            else if ((0X14 <= AVRIO.avrio.TimeWattTime_1_4) && (0X18 >= AVRIO.avrio.TimeWattTime_1_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_2;
                                }
                            }
                            else if ((0X0F <= AVRIO.avrio.TimeWattTime_1_4) && (0X13 >= AVRIO.avrio.TimeWattTime_1_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_2;
                                }
                            }
                            else if ((1 <= AVRIO.avrio.TimeWattTime_1_4) && (9 >= AVRIO.avrio.TimeWattTime_1_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_2;
                                }
                            }
                            else
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_2;
                                }
                            }
                            #endregion
                        }
                        else if ((TimeWattTime_5 < NowTime) && (NowTime <= TimeWattTime_6))
                        {
                            #region 수전전력 Select 2
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.TimeWattTime_2_4 * 100);
                            if ((0x0A <= AVRIO.avrio.TimeWattTime_2_4) && (0x0E >= AVRIO.avrio.TimeWattTime_2_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_2;
                                }
                            }
                            else if ((0X19 <= AVRIO.avrio.TimeWattTime_2_4) && (0X1B >= AVRIO.avrio.TimeWattTime_2_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_2;
                                }
                            }
                            else if ((0X14 <= AVRIO.avrio.TimeWattTime_2_4) && (0X18 >= AVRIO.avrio.TimeWattTime_2_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_2;
                                }
                            }
                            else if ((0X0F <= AVRIO.avrio.TimeWattTime_2_4) && (0X13 >= AVRIO.avrio.TimeWattTime_2_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_2;
                                }
                            }
                            else if ((1 <= AVRIO.avrio.TimeWattTime_2_4) && (9 >= AVRIO.avrio.TimeWattTime_2_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_2;
                                }
                            }
                            else
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_2;
                                }
                            }
                            #endregion
                        }
                        else if ((TimeWattTime_7 < NowTime) && (NowTime <= TimeWattTime_8))
                        {
                            #region 수전전력 Select 3
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.TimeWattTime_3_4 * 100);
                            if ((0x0A <= AVRIO.avrio.TimeWattTime_3_4) && (0x0E >= AVRIO.avrio.TimeWattTime_3_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_2;
                                }
                            }
                            else if ((0X19 <= AVRIO.avrio.TimeWattTime_3_4) && (0X1B >= AVRIO.avrio.TimeWattTime_3_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_2;
                                }
                            }
                            else if ((0X14 <= AVRIO.avrio.TimeWattTime_3_4) && (0X18 >= AVRIO.avrio.TimeWattTime_3_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_2;
                                }
                            }
                            else if ((0X0F <= AVRIO.avrio.TimeWattTime_3_4) && (0X13 >= AVRIO.avrio.TimeWattTime_3_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_2;
                                }
                            }
                            else if ((1 <= AVRIO.avrio.TimeWattTime_3_4) && (9 >= AVRIO.avrio.TimeWattTime_3_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_2;
                                }
                            }
                            else
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_2;
                                }
                            }
                            #endregion
                        }

                        else if ((TimeWattTime_9 < NowTime) && (NowTime <= TimeWattTime_10))
                        {
                            #region 수전전력 Select 4
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.TimeWattTime_4_4 * 100);
                            if ((0x0A <= AVRIO.avrio.TimeWattTime_4_4) && (0x0E >= AVRIO.avrio.TimeWattTime_4_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_2;
                                }
                            }
                            else if ((0X19 <= AVRIO.avrio.TimeWattTime_4_4) && (0X1B >= AVRIO.avrio.TimeWattTime_4_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_2;
                                }
                            }
                            else if ((0X14 <= AVRIO.avrio.TimeWattTime_4_4) && (0X18 >= AVRIO.avrio.TimeWattTime_4_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_2;
                                }
                            }
                            else if ((0X0F <= AVRIO.avrio.TimeWattTime_4_4) && (0X13 >= AVRIO.avrio.TimeWattTime_4_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_2;
                                }
                            }
                            else if ((1 <= AVRIO.avrio.TimeWattTime_4_4) && (9 >= AVRIO.avrio.TimeWattTime_4_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_2;
                                }
                            }
                            else
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_2;
                                }
                            }
                            #endregion
                        }
                        else

                        #region 수전전력 Select 5
                        //if ((TimeWattTime_1 <= NowTime) && ((NowTime <= TimeWattTime_2) || (TimeWattTime_1 <= NowTime)))
                        {
                            AVRIO.avrio.GridInputPowerMax = (ushort)(AVRIO.avrio.TimeWattTime_0_4 * 100);

                            if ((0x0A <= AVRIO.avrio.TimeWattTime_0_4) && (0x0E >= AVRIO.avrio.TimeWattTime_0_4))
                            {

                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_4_2;
                                }
                            }
                            else if ((0X19 <= AVRIO.avrio.TimeWattTime_0_4) && (0X1B >= AVRIO.avrio.TimeWattTime_0_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_1_2;
                                }
                            }
                            else if ((0X14 <= AVRIO.avrio.TimeWattTime_0_4) && (0X18 >= AVRIO.avrio.TimeWattTime_0_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_1;
                                }
                                else
                                {

                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_2_2;
                                }
                            }
                            else if ((0X0F <= AVRIO.avrio.TimeWattTime_0_4) && (0X13 >= AVRIO.avrio.TimeWattTime_0_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_3_2;
                                }
                            }
                            else if ((1 <= AVRIO.avrio.TimeWattTime_0_4) && (9 >= AVRIO.avrio.TimeWattTime_0_4))
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_5_2;
                                }
                            }
                            else
                            {
                                if (AVRIO.bmsio.bySOC > 0x46)
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_0;
                                }
                                else if ((AVRIO.bmsio.bySOC <= 0x46) && (AVRIO.bmsio.bySOC >= 0x32))
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_1;
                                }
                                else
                                {
                                    AVRIO.avrio.OutCurrentMax = (Byte)AVRIO.avrio.TimeWattSoc_0_2;
                                }
                            }
                            #endregion
                        }

                        #endregion

                    }
                    else
                    {

                        AVRIO.avrio.GridInputPowerMax = AVRIO.avrio.GridInputPowerMax;

                    }
                    SendCommandQC();
                }
                else
                {
                    NormalStatusRequest(runMode);
                }
                pollCount++;
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }
        private void NormalStatusRequest(AVRIO.RunningMode runMode)
        {
            ushort[] values = new ushort[qcio.TotalReadRegisters];
            ushort pollStart = (ushort)AVRIO.qcio.READADDRESS.Status;
            ushort pollLength = qcio.TotalReadRegisters;
            UInt16 pout_rapidas;
            UInt16 iout_rapidas;

            //Read registers and display data in desired format:
            try
            {
                bool ret = SendFc4((byte)mcuid, pollStart, pollLength, ref values);
                if (!ret) return;

                String strLog = null;
                byte[] byData = new byte[2];

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Ref_Vout - AVRIO.qcio.READADDRESS.Status]);
                strLog = BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                //AVRIO.avrio.EventMsg = "[Ref_Vout] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                //cbs
                QCDV.BmsInfo.TargetVoltage = BitConverter.ToUInt16(byData, 0).ToString();


                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Ref_Iout - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                //AVRIO.avrio.EventMsg = "[Ref_Iout] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                AVRIO.avrio.OutAmpare = BitConverter.ToUInt16(byData, 0).ToString();
                //cbs
                QCDV.BmsInfo.TargetAmpare = BitConverter.ToUInt16(byData, 0).ToString();


                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FeedBack_Vout - AVRIO.qcio.READADDRESS.Status]);
                // iout_rapidas = (UInt16)(byData[0]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                // AVRIO.avrio.EventMsg = "[FeedBack_Vout] : , " +                    
                pout_rapidas = Convert.ToUInt16(BitConverter.ToUInt16(byData, 0).ToString());
                //cbs
                QCDV.BmsInfo.OutVoltage = BitConverter.ToUInt16(byData, 0).ToString();


                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FeedBack_Iout - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                //  AVRIO.avrio.EventMsg = "[FeedBack_Iout] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                //cbs
                QCDV.BmsInfo.OutAmpare = BitConverter.ToUInt16(byData, 0).ToString();

                iout_rapidas = Convert.ToUInt16(BitConverter.ToUInt16(byData, 0).ToString());
                AVRIO.avrio.RapidasOutputw = (int)((int)(pout_rapidas * iout_rapidas));

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FeedBack_Vbat - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                // AVRIO.avrio.EventMsg = "[FeedBack_Vbat] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                AVRIO.bmsio.SendData_301.nBatteryVoltage = BitConverter.ToUInt16(byData, 0);

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FeedBack_IbatAvg - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                // AVRIO.avrio.EventMsg = "[FeedBack_IbatAvg] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                AVRIO.bmsio.SendData_301.nBatteryCurrent = BitConverter.ToUInt16(byData, 0);
                // QuickChargeDisplay.QCDV.ManualControl.BatteryCurrent = ((double)AVRIO.bmsio.SendData_301.nBatteryCurrent / 10 - 300).ToString();

                QuickChargeDisplay.QCDV.ManualControl.BatteryCurrent = string.Format("{0:F1}", (AVRIO.bmsio.SendData_301.nBatteryCurrent / 10 - 300));

                // QuickChargeDisplay.QCDV.ManualControl.BatteryCurrent = ((double)AVRIO.bmsio.SendData_301.nBatteryCurrent / 10 - 300).ToString();
                // QuickChargeDisplay.QCDV.ManualControl.BatteryCurrent = AVRIO.bmsio.SendData_301.nBatteryCurrent.ToString("000.00");


                ///////////////////////////////////////LEE////////////////////////////////////////////////////////////////////////////
                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Power_Out - AVRIO.qcio.READADDRESS.Status]);
                // AVRIO.avrio.EventMsg = "[Power_Out] : , " + BitConverter.ToUInt16(byData, 0).ToString();
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                AVRIO.bmsio.SendData_302.nCarInstantaneousPower = AVRIO.avrio.VehiclePower = BitConverter.ToUInt16(byData, 0); ;

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Power_Bat - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";
                AVRIO.bmsio.SendData_302.nBatteryInstantaneousPower = AVRIO.avrio.BatteryPower = BitConverter.ToUInt16(byData, 0);
                //////////////////////////////////////LEE/////////////////////////////////////////////////////

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.RemainTime_sec - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.RemainTime_min - AVRIO.qcio.READADDRESS.Status]);
                strLog = strLog + BitConverter.ToUInt16(byData, 0).ToString() + " , ";


                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.BmsBatteryCapacity - AVRIO.qcio.READADDRESS.Status]);
                AVRIO.avrio.byBmsTotalBatteryCapacity = byData[0];
                AVRIO.avrio.byBmsRemainingBatteryEnergy = byData[1];

                // strLog = strLog + AVRIO.avrio.byBmsTotalBatteryCapacity.ToString() + AVRIO.avrio.byBmsRemainingBatteryEnergy.ToString() + " , ";
                strLog = strLog + AVRIO.avrio.byBmsTotalBatteryCapacity.ToString() + " , ";
                strLog = strLog + AVRIO.avrio.byBmsRemainingBatteryEnergy.ToString() + " , ";
                byte ttemp;

                byData = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.SbcDspVersion - AVRIO.qcio.READADDRESS.Status]);

                ttemp = byData[0];
                //소숫점 2 까지 표시
                AVRIO.avrio.nDspVersion = "V" + string.Format("{0:F2}", (Convert.ToDouble(ttemp) / 100));
                //     AVRIO.avrio.nDspVersion = (Convert(ttemp))/100;

                strLog = strLog + byData[0].ToString();
                strLog = strLog + byData[1].ToString() + " , ";

                //cbs 2020.02.18   2020.            
                int nVoltageSlope = (values[14] << 16) | values[15];
                QCDV.DSPCalibration.Voltage_Slope = string.Format("{0:f4}", (nVoltageSlope * 0.000001));
                strLog += nVoltageSlope.ToString() + " , ";

                int nVoltageOffset = (values[16] << 16) | values[17];
                QCDV.DSPCalibration.Voltage_Offset = string.Format("{0:f4}", (nVoltageOffset * 0.000001));
                strLog += nVoltageOffset.ToString() + " , ";

                int nCurrentSlope = (values[18] << 16) | values[19];
                QCDV.DSPCalibration.Current_Slope = string.Format("{0:f4}", (nCurrentSlope * 0.000001));
                strLog += nCurrentSlope.ToString() + " , ";

                int nCurrentOffset = (values[20] << 16) | values[21];
                QCDV.DSPCalibration.Current_Offset = string.Format("{0:f4}", (nCurrentOffset * 0.000001));
                strLog += nCurrentOffset.ToString();

                int nDspLgBatteryStatus = 0;
                byte BatteryStatus;
                BatteryStatus = (byte)values[0];
                BatteryStatus = (byte)(BatteryStatus & 0x18);
                BatteryStatus = (byte)(BatteryStatus >> 3);
                //   BatteryStatus = (byte)(BatteryStatus >> 13);

                if ((BatteryStatus & (int)1) > 0)
                {
                    AVRIO.avrio.DspLgBatteryStatus = nDspLgBatteryStatus = nDspLgBatteryStatus + 1;
                }
                else
                    if ((BatteryStatus & (int)2) > 0)
                {
                    AVRIO.avrio.DspLgBatteryStatus = nDspLgBatteryStatus = nDspLgBatteryStatus + 2;
                }
                else AVRIO.avrio.DspLgBatteryStatus = 0;

                AVRIO.avrio.EventMsg = "[RECV], " + values[0].ToString() + " , " + values[1].ToString() + " , " + strLog;
                // 1. 자동차BMS 에서 FAULT 발생시 해당 어드레스에 값을 마킹.
                AVRIO.avrio.rCommandValue = values[0];
                byte[] byFault = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FaultCode - AVRIO.qcio.READADDRESS.Status]);

                AVRIO.avrio.CarBMSFault = (int)byFault[1];

                if ((values[0] & (ushort)AVRIO.SysStatus.SysBMSReady) > 0)
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSReady = true;
                }
                else
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSReady = false;
                }

                // 1. Battery 상태 확인
                int nBatteryStatus = 0;
                if ((values[0] & (ushort)AVRIO.SysStatus.SysBatterySystanby) > 0) { nBatteryStatus = nBatteryStatus + 1; }
                if ((values[0] & (ushort)AVRIO.SysStatus.SysBatterySysReady) > 0) { nBatteryStatus = nBatteryStatus + 2; }

                //       AVRIO.avrio.EventMsg = "AVRIO.avrio.CurrentBatStatus=> " + QuickChargeDisplay.QCDV.ManualControl.CurrentBatStatus.ToString();

                AVRIO.avrio.CurrentBatStatus = (AVRIO.SysBatteryStatus)nBatteryStatus;
                QuickChargeDisplay.QCDV.ManualControl.CurrentBatStatus = nBatteryStatus;
                QuickChargeDisplay.QCDV.CompleteCharge.CurrentBatStatus = nBatteryStatus;

                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby
                    && AVRIO.avrio.CurrentBatStatus == AVRIO.SysBatteryStatus.SysBatteryStandby)
                {
                    if ((values[0] & (ushort)AVRIO.SysStatus.SysPCSReady) > 0)
                    {
                        AVRIO.bmsio.SendData_303.byPSC_Status = 1;
                    }
                    else
                    {
                        AVRIO.bmsio.SendData_303.byPSC_Status = 0;
                    }
                }
                else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady
                    && AVRIO.avrio.CurrentBatStatus == AVRIO.SysBatteryStatus.SysBatteryReady)
                {
                    AVRIO.bmsio.SendData_303.byPSC_Status = 2;
                }
                else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                {
                    AVRIO.bmsio.SendData_303.byPSC_Status = 3;
                }
                else if (AVRIO.avrio.CurrentBatStatus == AVRIO.SysBatteryStatus.SysBatteryRunning)
                {
                    AVRIO.bmsio.SendData_303.byPSC_Status = 4;
                }

                // 1. 수동모드에서 화면에 표시되는 정보
                // 2. BMS, PCS 캔 신호

                if (AVRIO.bmsio.byBMSStatus == 0)
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSStatus = "Initial";
                }
                else if (AVRIO.bmsio.byBMSStatus == 1)
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSStatus = "PreCharging1";
                }
                else if (AVRIO.bmsio.byBMSStatus == 2)
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSStatus = "PreCharging2";
                }
                else if (AVRIO.bmsio.byBMSStatus == 3)
                {
                    QuickChargeDisplay.QCDV.ManualControl.BMSStatus = "Ready";
                }

                if (AVRIO.bmsio.bBMSCellVoltageFaultFlag || AVRIO.bmsio.bBMSFaultFlag || ((values[0] & (ushort)AVRIO.SysStatus.SysFault) != 0))
                {
                    QuickChargeDisplay.QCDV.ManualControl.PCSStatus = "Fault";
                }
                else if (AVRIO.avrio.DspLgBatteryStatus == 0)
                {
                    QuickChargeDisplay.QCDV.ManualControl.PCSStatus = "Battery Standby";
                }
                else if (AVRIO.avrio.DspLgBatteryStatus == 1)
                {
                    QuickChargeDisplay.QCDV.ManualControl.PCSStatus = "Battery Ready";
                }

                else if (AVRIO.avrio.DspLgBatteryStatus == 2)
                {
                    QuickChargeDisplay.QCDV.ManualControl.PCSStatus = "Battery Running";
                }

                if (AVRIO.avrio.RunMode != AVRIO.RunningMode.Admin)
                {
                    DateTime dt = DateTime.Now;
                    string msg;

                    if (AVRIO.bmsio.bBMSCellVoltageFaultFlag == true)
                    {
                        if (AVRIO.avrio.IsFaultDialog == false)
                        {
                            if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                            {
                                AVRIO.avrio.EventMsg = "BMSCellVoltageFaultFlag : ON";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                            {
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                                msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[BMS] BMSCellVoltageFaultFlag => TsVehicleFinish" + ",N";
                                QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                            }
                            else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                            {
                                if (AVRIO.avrio.IsFaultDialog == false)
                                {
                                    if (AVRIO.avrio.FaultStopCheck)
                                    {
                                        ushort idx = 0;
                                        if (AVRIO.bmsio.bCellVoltageImbalanceWarnFlag)
                                        { // CVIF
                                            idx = 310;
                                        }
                                        else if (AVRIO.bmsio.bCellLowVoltageFaultFlag == true)
                                        {//CLVF                                       
                                            idx = 304;
                                        }
                                        else if (AVRIO.bmsio.bCellHighVoltageFaultFlag == true)
                                        {//CHVF
                                            idx = 303;
                                        }
                                        else if (AVRIO.bmsio.bSOCHighFaultFlag == true)
                                        {//SHF
                                            idx = 319;
                                        }
                                        else if (AVRIO.bmsio.bInternalCommuLossFaultFlag == true)
                                        {// ICLF
                                            idx = 320;
                                        }
                                        else if (AVRIO.bmsio.bLowTempFaultFlag == true)
                                        {//LTF                                         
                                            idx = 308;
                                        }
                                        else if (AVRIO.bmsio.bHighTempFaultFlag == true)
                                        {//HTF                                           
                                            idx = 307;
                                        }
                                        else if (AVRIO.bmsio.bCellVoltageImbalanceFaultFlag == true)
                                        {//CVIF                                          
                                            idx = 310;
                                        }
                                        else if (AVRIO.bmsio.bHighDischargeCurrentFaultFlag == true)
                                        {//HDCF                                         
                                            idx = 314;
                                        }
                                        else if (AVRIO.bmsio.bHighChargeCurrentFaultFlag == true)
                                        {//HCCF
                                            idx = 313;
                                        }
                                        else if (AVRIO.bmsio.bHighTempWarnFlag)
                                        {
                                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                            idx = 305;
                                        }
                                        else if (AVRIO.bmsio.bPackVoltageDeviationFaultFlag == true)
                                        {//PVDF                                            
                                            idx = 316;
                                        }
                                        else if (AVRIO.bmsio.bPrechargeFailFlag == true)
                                        {//PCF
                                            idx = 317;
                                        }
                                        else
                                        {
                                            idx = 321;
                                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt0001;
                                        }
                                        AVRIO.avrio.FaultCode = idx.ToString();
                                        SetFaultLog(idx);
                                    }
                                    //  AVRIO.avrio.IsFaultDialog = true;
                                }
                            }
                        }
                    }
                    else if (AVRIO.bmsio.bBMSFaultFlag == true)
                    {
                        if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                        {
                            AVRIO.avrio.EventMsg = "BMSFaultFlag : ON";
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                        }
                        else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                        {
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                            msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[BMS] BMSFaultFlag => TsVehicleFinish" + ",N";
                            QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                        }
                        else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                        {
                            if (AVRIO.avrio.IsFaultDialog == false)
                            {
                                if (AVRIO.avrio.FaultStopCheck)
                                {
                                    ushort idx = 0;
                                    if (AVRIO.bmsio.bCellVoltageImbalanceWarnFlag)
                                    { // CVIF
                                        idx = 310;
                                    }
                                    else if (AVRIO.bmsio.bCellLowVoltageFaultFlag == true)
                                    {//CLVF                                       
                                        idx = 304;
                                    }
                                    else if (AVRIO.bmsio.bCellHighVoltageFaultFlag == true)
                                    {//CHVF
                                        idx = 303;
                                    }
                                    else if (AVRIO.bmsio.bSOCHighFaultFlag == true)
                                    {//SHF
                                        idx = 319;
                                    }
                                    else if (AVRIO.bmsio.bInternalCommuLossFaultFlag == true)
                                    {// ICLF
                                        idx = 320;
                                    }
                                    else if (AVRIO.bmsio.bLowTempFaultFlag == true)
                                    {//LTF                                         
                                        idx = 308;
                                    }
                                    else if (AVRIO.bmsio.bHighTempFaultFlag == true)
                                    {//HTF                                           
                                        idx = 307;
                                    }
                                    else if (AVRIO.bmsio.bCellVoltageImbalanceFaultFlag == true)
                                    {//CVIF                                          
                                        idx = 310;
                                    }
                                    else if (AVRIO.bmsio.bHighDischargeCurrentFaultFlag == true)
                                    {//HDCF                                         
                                        idx = 314;
                                    }
                                    else if (AVRIO.bmsio.bHighChargeCurrentFaultFlag == true)
                                    {//HCCF
                                        idx = 313;
                                    }


                                    //cbs 20191014 주석
                                    //else if (AVRIO.bmsio.bPackVoltageDeviationWarnFlag == true)
                                    //{//PVDW
                                    //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                    //    idx = 315;
                                    //}

                                    //cbs 2020.04.01 추가////////////////
                                    else if (AVRIO.bmsio.bHighTempWarnFlag)
                                    {
                                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                        idx = 305;
                                    }
                                    //cbs 20200711 주석처리
                                    //else if (AVRIO.bmsio.bSOCLowFaultFlag == true)
                                    //{
                                    //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                    //    idx = 315;
                                    //}
                                    ///////////////////////////////////////

                                    else if (AVRIO.bmsio.bPackVoltageDeviationFaultFlag == true)
                                    {//PVDF                                            
                                        idx = 316;
                                    }
                                    else if (AVRIO.bmsio.bPrechargeFailFlag == true)
                                    {//PCF
                                        idx = 317;
                                    }
                                    else
                                    {
                                        idx = 321;
                                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt0001;
                                    }
                                    AVRIO.avrio.FaultCode = idx.ToString();
                                    SetFaultLog(idx);
                                }
                            }
                        }
                    }
                    else if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                    {
                        if (!AVRIO.avrio.GridOnlyMode)
                        {
                            if (AVRIO.avrio.IsFaultDialog == false)
                            {
                                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                                {
                                    AVRIO.avrio.EventMsg = "BMSCanCommErrorFlag : ON";
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                }
                                ////cbs 2020.08.07 테스트용으로 임시로 추가 
                                /// 양산 테스트시 CAN COVERTER CABLE 제거 테스트항목 떄문에 임시로 넣음
                                /// 테스트 항목을 변경하든지 아래 주석해지든지 해야됨
                                /// 이전 소스에는 케이블제거 관련하여 내용없음
                                //else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                                //{
                                //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsFault;
                                //    SetFaultLog(14); 
                                //}

                                else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                                {
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsFault;
                                    SetFaultLog(321);

                                }
                                if (AVRIO.avrio.FaultStopCheck)
                                {
                                    ushort idx = 0;

                                    if (AVRIO.bmsio.bCellVoltageImbalanceWarnFlag)
                                    { // CVIF
                                        idx = 310;
                                    }
                                    else if (AVRIO.bmsio.bCellLowVoltageFaultFlag == true)
                                    {//CLVF                                       
                                        idx = 304;
                                    }
                                    else if (AVRIO.bmsio.bCellHighVoltageFaultFlag == true)
                                    {//CHVF
                                        idx = 303;
                                    }
                                    else if (AVRIO.bmsio.bSOCHighFaultFlag == true)
                                    {//SHF
                                        idx = 319;
                                    }
                                    else if (AVRIO.bmsio.bInternalCommuLossFaultFlag == true)
                                    {// ICLF
                                        idx = 320;
                                    }
                                    else if (AVRIO.bmsio.bLowTempFaultFlag == true)
                                    {//LTF                                         
                                        idx = 308;
                                    }
                                    else if (AVRIO.bmsio.bHighTempFaultFlag == true)
                                    {//HTF                                           
                                        idx = 307;
                                    }
                                    else if (AVRIO.bmsio.bCellVoltageImbalanceFaultFlag == true)
                                    {//CVIF                                          
                                        idx = 310;
                                    }
                                    else if (AVRIO.bmsio.bHighDischargeCurrentFaultFlag == true)
                                    {//HDCF                                         
                                        idx = 314;
                                    }
                                    else if (AVRIO.bmsio.bHighChargeCurrentFaultFlag == true)
                                    {//HCCF
                                        idx = 313;
                                    }
                                    //else if (AVRIO.bmsio.bPackVoltageDeviationWarnFlag == true)
                                    //{//PVDW
                                    //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                    //    idx = 315;
                                    //}

                                    //cbs 20191014 주석
                                    //else if (AVRIO.bmsio.bPackVoltageDeviationWarnFlag == true)
                                    //{//PVDW
                                    //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                    //    idx = 315;
                                    //}

                                    //cbs 2020.04.01 추가////////////////
                                    else if (AVRIO.bmsio.bHighTempWarnFlag)
                                    {
                                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                        idx = 305;
                                    }
                                    //cbs 20200711 주석처리
                                    //else if (AVRIO.bmsio.bSOCLowFaultFlag == true)
                                    //{
                                    //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                    //    idx = 315;
                                    //}
                                    ///////////////////////////////////////

                                    else if (AVRIO.bmsio.bPackVoltageDeviationFaultFlag == true)
                                    {//PVDF                                            
                                        idx = 316;
                                    }
                                    else if (AVRIO.bmsio.bPrechargeFailFlag == true)
                                    {//PCF
                                        idx = 317;
                                    }
                                    else
                                    {
                                        idx = 321;
                                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt0001;
                                    }
                                    AVRIO.avrio.FaultCode = idx.ToString();
                                    SetFaultLog(idx);

                                }
                            }
                        }
                    }
                    else if (AVRIO.bmsio.bySOC < 0x05) //cbs 20200711 SOC 5% 이하를 일반 FAULT처럼 처리를 하면 TsReset 명령떄문에  내부 배터리에 충전을 할 수 없어 Fault Dialog만 띄우고 JFE 과금 멤버에 업데이트만 함 
                    {                          
                        if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                        {
                        }
                        else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                        {
                            AVRIO.avrio.EventMsg = "AVRIO.bmsio.bySOC < 0x05  Charging Stop";
                            AVRIO.avrio.EventMsg = "[Normal] Less than 5% battery => TsVehicleFinish";
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

                            if (!AVRIO.avrio.IsFaultDialog)
                            {
                                int idx = 315;
                                msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + idx.ToString() + "," + qcio.msgBMSFault[idx - 300] + ",N";
                                QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);

                                //1
                                AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                //2
                                AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_Warnning;
                                //3
                                AVRIO.avrio.FaultCode = idx.ToString();
                                //4
                                AVRIO.avrio.IsFaultDialog = true;

                                AVRIO.avrio.EventMsg = "[Normal] Fault 315 ";
                            }
                        }
                        else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby)
                        {
                            if (AVRIO.avrio.IsFaultDialog == false)
                            {
                                if (AVRIO.avrio.FaultStopCheck) //폴트 체킹이 끝나면
                                {
                                    AVRIO.avrio.Fault315Cnt++;
                                    if (AVRIO.avrio.Fault315Cnt > 1) //다시 Fault Dialog 띄우는 속도가 빨라서 늦춤
                                    {
                                        AVRIO.avrio.Fault315Cnt = 0;
                                        int idx = 315;

                                        msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + idx.ToString() + "," + qcio.msgBMSFault[idx - 300] + ",N";
                                        QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);

                                        //1
                                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                                        //2
                                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_Warnning;
                                        //3
                                        AVRIO.avrio.FaultCode = idx.ToString();
                                        //4
                                        AVRIO.avrio.IsFaultDialog = true;

                                        AVRIO.avrio.EventMsg = "[Normal] Fault 315 ";
                                    }
                                }
                            }
                        }
                    }
                }

                if (runMode == AVRIO.RunningMode.Normal)
                {
                    AVRIO.avrio.ChargeFaultCheck = false;
                    ReadQCNormalData(values);
                }
                else if (runMode == AVRIO.RunningMode.Charging)
                {
                    AVRIO.avrio.ChargeFaultCheck = false;
                    ReadQCChargingFullData(values);
                }
                else if (runMode == AVRIO.RunningMode.Fault)
                {
                    AVRIO.avrio.ChargeFaultCheck = false;
                    ReadQCFaultData(values);
                }
            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = "NormalStatusRequest " + err.ToString();
                AVRIO.Log.Eventlog = err.ToString();
            }

            try
            {
                byte[] byStatus = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Status - AVRIO.qcio.READADDRESS.Status]);
                int sw = (int)byStatus[1];

                // 1. 스위치가 반복해서 눌리는 것을 방지            
                if (((sw & (int)AVRIO.SwStatus.Run) > 0) && ((sw & (int)AVRIO.SwStatus.Stop) > 0))
                {
                    // 1. 스위치가 동시에 눌릴 경우
                    return;
                }
                else if ((sw & (int)AVRIO.SwStatus.Run) > 0)
                {
                    if (bStartBtn == true)
                    {
                    }
                    else
                    {
                        bStartBtn = true;
                    }
                }
                else if ((sw & (byte)AVRIO.SwStatus.Stop) > 0)
                {
                    if (bStopBtn == true)
                    {
                    }
                    else
                    {
                        bStopBtn = true;
                    }
                }
                if (AVRIO.avrio.FualtstopFalg && bStopBtn)
                {
                    AVRIO.avrio.FaulStopButton = true;
                    AVRIO.avrio.Fualtstop = true;
                    bStopBtn = false;
                    bStartBtn = false;
                }
                else
                {
                    if ((AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysStandby) && bStartBtn)
                    {
                        Thread.Sleep(300);
                        AVRIO.avrio.StartButton = bStartBtn;
                        AVRIO.avrio.StopButton = bStopBtn;
                        bStartBtn = false;
                        bStopBtn = false;
                    }

                    else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady && bStopBtn)
                    {
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                        bStartBtn = false;
                        bStopBtn = false;
                        Thread.Sleep(200);
                    }
                    else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady && bStartBtn)
                    {
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;
                        bStartBtn = false;
                        bStopBtn = false;
                        Thread.Sleep(200);
                    }
                    else if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysPayCheck && bStopBtn)
                    {
                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysWorkDone;
                        bStartBtn = false;
                        bStopBtn = false;
                        Thread.Sleep(200);
                    }
                    else
                    {
                        AVRIO.avrio.StartButton = bStartBtn;
                        AVRIO.avrio.StopButton = bStopBtn;
                        bStartBtn = false;
                        bStopBtn = false;
                    }
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }
        private void SetChargeHistory(double price, double watt, bool confirm, string emsg)
        {//LEE JFE 충전량 SOC 넣어 달라면 계산해서 넣어줘야함

            try
            {
                string temp, temp1;
                temp = string.Format("{0:F2}", AVRIO.avrio.StartchargeSOC);
                AVRIO.avrio.StartchargeSOC = Convert.ToDouble(temp);


                temp1 = string.Format("{0:F2}", AVRIO.avrio.StopchargeSOC);
                AVRIO.avrio.StopchargeSOC = Convert.ToDouble(temp1);
                // AVRIO.avrio.StartchargeSOC = AVRIO.avrio.StartchargeSOC;
                //AVRIO.avrio.StopchargeSOC = AVRIO.avrio.StopchargeSOC ;
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
            try
            {
                string msg = 1 + "," + watt + "," + (confirm ? "Y" : "N") + "," + AVRIO.avrio.StartchargeSOC + "," + AVRIO.avrio.StopchargeSOC + "," + emsg;
                // string msg = price + "," + watt + "," + (confirm ? "Y" : "N") + "," + emsg;
                QuickChargeConfig.ChargeConfig.SetLog("History", AVRIO.avrio.SeqNumHistory++, msg, DateTime.Now);
                // QuickChargeConfig.ChargeConfig.SetLog("HISTORY", nChargeHistorySeqNum++, msg, DateTime.Now);
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }

            AVRIO.avrio.StartchargeSOC = 0;// 07 , 16
            AVRIO.avrio.StopchargeSOC = 0;// 07 , 16
        }
        private bool SetFaultLog(UInt16 idx)
        {
            if (AVRIO.avrio.FaultStopCheck)
            {
                DateTime dt = DateTime.Now;
                // AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;   
                // LG BMS Fault 로그 저장 부분 

                if (idx == 203)
                {
                    //  return true;
                }
                if (idx == 14)
                {
                    return true;
                }

                if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                {
                    if (!AVRIO.avrio.GridOnlyMode)
                    {
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsFault;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CANFail;

                        if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                        {
                            if (idx > 321)
                            {
                                return true;
                            }
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        return true;
                    }
                }
                else if (AVRIO.bmsio.bBMSWarnFlag == true || AVRIO.avrio.WarrningFalg || AVRIO.avrio.DisChargeFalg)
                {
                    if (AVRIO.bmsio.bLowTempWarnFlag)//LTW///////////////////////////////////
                    {
                        idx = 306;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }
                    // cbs 2020.04.01 주석처리 warnning -> Fault로 변경
                    //else if (AVRIO.bmsio.bHighTempWarnFlag)//HTW//////////////////////////////////////////////
                    //{
                    //    idx = 305;
                    //    try
                    //    {
                    //        faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                    //    }
                    //    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    //}


                    else if (AVRIO.bmsio.bCellLowVoltageWarnFlag)//CLVW///////////////////////////////////////////
                    {
                        idx = 304;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }
                    else if (AVRIO.bmsio.bCellHighVoltageWarnFlag)//CHVW//////////////////////////////////////
                    {
                        idx = 301;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }
                    else if (AVRIO.bmsio.bHighDischargeCurrentWarnFlag)//HDCW//////////////////////////////////
                    {
                        idx = 312;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }
                    else if (AVRIO.bmsio.bHighChargeCurrentWarnFlag)//HCCW////////////////////////////////////////////////////////////////////////////////
                    {
                        idx = 311;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }
                    else if (AVRIO.bmsio.bSOCHighWarnFlag)//SHW/////////////////////////////////////////////////////////////////////////
                    {
                        idx = 319;
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        }
                        catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    }

                    AVRIO.avrio.FaultCode = idx.ToString();
                }
                else if (AVRIO.bmsio.bBMSCellVoltageFaultFlag || AVRIO.bmsio.bBMSFaultFlag)
                {
                    if (idx > 321 || idx == 0)
                    {
                        return true;
                    }
                    try
                    {
                        //cbs 베터리 Fault창을 띄우려고 DSP에게 TsFault 를 보내는 것 같다
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsFault;
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.BMS_CellVoltageError;

                        if (AVRIO.bmsio.bCellVoltageImbalanceWarnFlag)
                        { // CVIF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7804;
                            idx = 310;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bCellLowVoltageFaultFlag == true)
                        {//CLVF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7804;
                            idx = 304;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bCellHighVoltageFaultFlag == true)
                        {//CHVF

                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7804;
                            idx = 303;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bSOCHighFaultFlag == true)
                        {//SHF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 319;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }

                        //cbs 20191014 주석처리
                        //else if (AVRIO.bmsio.bSOCLowFaultFlag == true)
                        //{//
                        //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                        //    idx = 323;
                        //    AVRIO.avrio.FaultCode = idx.ToString();
                        //    try
                        //    {
                        //        faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        //    }
                        //    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        //}

                        else if (AVRIO.bmsio.bInternalCommuLossFaultFlag == true)
                        {// ICLF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 320;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bLowTempFaultFlag == true)
                        {//LTF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 308;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bHighTempFaultFlag == true)
                        {//HTF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 307;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bCellVoltageImbalanceFaultFlag == true)
                        {//CVIF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 310;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bHighDischargeCurrentFaultFlag == true)
                        {//HDCF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 314;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bHighChargeCurrentFaultFlag == true)
                        {//HCCF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 313;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }



                        //cbs 20191014 주석처리
                        //else if (AVRIO.bmsio.bPackVoltageDeviationWarnFlag == true)
                        //{//PVDW
                        //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                        //    idx = 315;
                        //    AVRIO.avrio.FaultCode = idx.ToString();
                        //    try
                        //    {
                        //        faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        //    }
                        //    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        //}


                        //cbs 20200711 주석처리
                        ////cbs 20191014 추가  //fault List에 2번 들어감  NormalStatusRequest()에서 1번 여기서 1번
                        //else if (AVRIO.bmsio.bSOCLowFaultFlag == true)
                        //{//PVDW
                        //    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                        //    idx = 315;
                        //    AVRIO.avrio.FaultCode = idx.ToString();
                        //    try
                        //    {
                        //        faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                        //    }
                        //    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        //}

                        //cbs 2020.04.01 추가 warnning -> Fault로 변경  msgBMSFault 내용 추가
                        else if (AVRIO.bmsio.bHighTempWarnFlag)//HTW//////////////////////////////////////////////
                        {
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 305;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bPackVoltageDeviationFaultFlag == true)
                        {//PVDF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 316;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (AVRIO.bmsio.bPrechargeFailFlag == true)
                        {//PCF
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                            idx = 317;
                            AVRIO.avrio.FaultCode = idx.ToString();
                            try
                            {
                                faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                            }
                            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                        }
                        else if (idx == 321)
                            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt0001;


                        AVRIO.avrio.FaultCode = idx.ToString();
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

                    if (idx == 23 || idx == 101 || idx == 103 || idx == 107 || idx == 110)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5608;
                    }
                    else if (idx == 100 || idx == 108 || idx == 109)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5610;
                    }
                    else if (idx == 21 || idx == 22)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5620;
                    }
                    else if (idx == 102)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5640;
                    }
                    else if (idx == 1 || idx == 3 || idx == 5 || idx == 6 || idx == 9 ||
                        idx == 11 || idx == 14 || idx == 17 || idx == 19 || idx == 20 ||
                        idx == 25 || idx == 26 || idx == 200 || idx == 201 || idx == 202 || idx == 203 || idx == 204
                        )
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7804;
                    }
                    else if (idx == 10)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7808;
                    }
                    else if (idx == 18)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7810;
                    }
                    else if (idx == 7 || idx == 8)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                    }
                    else if (idx == 4)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7840;
                    }
                    else if (idx == 11 || idx == 15)
                    {
                        AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7880;
                    }

                    return true;
                }

                if (idx == 23 || idx == 101 || idx == 103 || idx == 107 || idx == 110)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5608;
                }
                else if (idx == 100 || idx == 108 || idx == 109)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5610;
                }
                else if (idx == 21 || idx == 22)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5620;
                }
                else if (idx == 102)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.Chademo5640;
                }
                else if (idx == 1 || idx == 3 || idx == 5 || idx == 6 || idx == 9 ||
                    idx == 11 || idx == 14 || idx == 17 || idx == 19 || idx == 20 ||
                    idx == 25 || idx == 26 || idx == 200 || idx == 201 || idx == 202 || idx == 203 || idx == 204
                    )
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7804;
                }
                else if (idx == 10)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7808;
                }
                else if (idx == 18)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7810;
                }
                else if (idx == 7 || idx == 8)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7820;
                }
                else if (idx == 4)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7840;
                }
                else if (idx == 11 || idx == 15)
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFualt7880;
                }


                if (idx < qcio.msgFault.Length)
                {
                    try
                    {
                        if (idx == 0x0b)
                        {
                            //AVRIO.avrio.EventMsg = "BMSWakeSleepModeControl_0";
                            //AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0; 

                            // Fail Safe Design 변경 2013_11_19 
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                        }
                        else
                        {
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                        }
                        AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.Charger;
                        AVRIO.avrio.FaultCode = idx.ToString();
                        try
                        {
                            faultList.Add(new SysFaultList(dt, idx, qcio.msgFault[idx]));
                        }
                        catch (Exception err)
                        {
                            AVRIO.Log.Eventlog = err.ToString();
                        }
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

                }
                else if ((idx - 100) < qcio.msgFault100.Length)
                {
                    try
                    {
                        faultList.Add(new SysFaultList(dt, idx, qcio.msgFault100[idx - 100]));
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

                    AVRIO.avrio.FaultCode = idx.ToString();
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                    return true;
                }
                else if ((idx - 200) < qcio.msgFault200.Length)
                {
                    try
                    {
                        faultList.Add(new SysFaultList(dt, idx, qcio.msgFault200[idx - 200]));
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

                    AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                    AVRIO.avrio.FaultCode = idx.ToString();
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                    return true;
                }
                else
                {
                    //AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
                    //AVRIO.avrio.EventMsg = AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl.ToString();
                    //AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;
                    //faultList.Add(new SysFaultList(dt, idx, qcio.msgBMSFault[idx - 300]));
                    //AVRIO.avrio.FaultCode = idx.ToString();
                    return true;
                }
                return true;
            }
            else return true;
        }
        private void ChargeFinish()
        {
            try
            {
                AVRIO.avrio.ChargeEndTime = DateTime.Now;
                QuickChargeDisplay.QCDV.ConfirmCharge.ChargeWatt = AVRIO.avrio.ChargeWatt;
                QuickChargeDisplay.QCDV.ConfirmCharge.Payment = (double)AVRIO.avrio.ChargePrice;
                SetChargeHistory(QuickChargeDisplay.QCDV.ConfirmCharge.Payment, QuickChargeDisplay.QCDV.ConfirmCharge.ChargeWatt, true, "");
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        private void ReadQCFaultData(ushort[] values)
        {
            try
            {
                // Fault처리
                if ((values[0] & (ushort)AVRIO.SysStatus.SysFault) == 0 || (values[0] & (ushort)AVRIO.SysStatus.SysFault) == 203)
                {
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                    // 폴트가 해결이 되었을 때 상태값을 정상으로 돌린 후 Return.
                    AVRIO.avrio.EventMsg = "CHARGER FAULT -> NORMAL";
                    AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                    return;
                }
                else
                {
                    if (AVRIO.avrio.IsFaultDialog == true)
                    {
                        return;
                    }
                    else if (AVRIO.avrio.FaultStopCheck)
                    {
                        if (AVRIO.avrio.WaitFualtFlag)
                            AVRIO.avrio.IsFaultDialog = true;
                        else
                        {
                            WaitFualt.Start();
                            return;
                        }

                        byte[] byFault = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FaultCode - AVRIO.qcio.READADDRESS.Status]);
                        // UInt16 nFaultNum = BitConverter.ToUInt16(byFault, 0);
                        UInt16 nFaultNum = (UInt16)byFault[0];

                        if (nFaultNum == 14)
                        {
                            return;
                        }
                        SetFaultLog(nFaultNum);

                        if (nFaultNum == 0xcb)
                        {
                            return;
                        }
                    }
                    // else return;                    
                }

                AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;

                int nTempStatus = 0;

                if ((values[0] & (int)1) > 0)
                {
                    nTempStatus = nTempStatus + 1;
                }
                if ((values[0] & (int)2) > 0)
                {
                    nTempStatus = nTempStatus + 2;
                }
                if ((values[0] & (int)4) > 0)
                {
                    nTempStatus = nTempStatus + 4;
                }

                // byte aaa = (values[0] 
                // Fault 상태에 빠졌을경우 시퀀스 변경
                // Fault 다이얼로그를 띄우면서 한번만 실행

                switch (AVRIO.avrio.CurrentStatus)
                {
                    case AVRIO.SysStatus.SysStandby:
                        {
                        }
                        break;
                    case AVRIO.SysStatus.SysReady:
                        {
                            if (nTempStatus == 0)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysReady => SysStandby Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
                            }
                            else
                            {
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            }
                        }
                        break;
                    case AVRIO.SysStatus.SysPreRun:
                        {
                            if (nTempStatus == 4)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysPreRun => SysStopping Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStopping;
                            }
                            else if (nTempStatus == 5)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysPreRun => SysFinish Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                ChargeFinish();
                            }
                            else
                            {
                                AVRIO.avrio.EventMsg = "[Fault] TsVehicleFinish";
                                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                            }
                        }
                        break;

                    case AVRIO.SysStatus.SysRunning:
                        if (nTempStatus == 4)
                        {
                            AVRIO.avrio.EventMsg = "[FAULT] SysRunning => SysStopping Status";
                            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStopping;
                        }
                        else if (nTempStatus == 5)
                        {
                            AVRIO.avrio.EventMsg = "[FAULT] SysRunning => SysFinish Status";
                            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                            ChargeFinish();
                        }
                        else
                        {
                            AVRIO.avrio.EventMsg = "[Fault] TsFinish";
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
                        }
                        break;

                    case AVRIO.SysStatus.SysStopping:
                        {
                            if (nTempStatus == 5)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysStopping => SysFinish Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                ChargeFinish();
                            }
                            else if (nTempStatus == 0)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysStopping => SysFinish Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                ChargeFinish();
                            }
                        }
                        break;

                    case AVRIO.SysStatus.SysFinish:
                        {

                        }
                        break;
                    case AVRIO.SysStatus.SysPayCheck:
                        {

                        }
                        break;

                    case AVRIO.SysStatus.SysWorkDone:
                        {
                            if (nTempStatus == 0)
                            {
                                AVRIO.avrio.EventMsg = "[FAULT] SysPayCheck,SysWorkDone => SysStandby Status";
                                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
                            }
                        }
                        break;
                    default:
                        {
                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
                        }
                        break;
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        private void ReadQCChargingFullData(ushort[] values)
        {
            try
            {
                byte[] byStatus = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Status - AVRIO.qcio.READADDRESS.Status]);

                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                {
                    // BMS 정보 표시                
                    if (AVRIO.avrio.byBmsTotalBatteryCapacity != 0)
                    {
                        AVRIO.avrio.ChargeSOC = (double)(((float)AVRIO.avrio.byBmsRemainingBatteryEnergy / (float)AVRIO.avrio.byBmsTotalBatteryCapacity) * 100);

                        if (bInitVehicleBatteryEnergy == false)
                        {
                            bInitVehicleBatteryEnergy = true;
                            AVRIO.avrio.ChargeStartWatt = AVRIO.avrio.byBmsRemainingBatteryEnergy;
                            AVRIO.avrio.EventMsg = "RemainBatteryEnergy : " + AVRIO.avrio.byBmsRemainingBatteryEnergy.ToString();
                        }

                        QuickChargeDisplay.QCDV.Charging.ChargingSoc = AVRIO.avrio.ChargeSOC;
                    }

                    if (bInitVehicleBatteryEnergy == true)
                    {
                        AVRIO.avrio.ChargeWatt = (double)(AVRIO.avrio.byBmsRemainingBatteryEnergy - AVRIO.avrio.ChargeStartWatt) / (double)10;
                    }
                    else
                    {
                        AVRIO.avrio.ChargeWatt = 0;
                    }

                    AVRIO.avrio.ChargePrice = (uint)AVRIO.avrio.ChargeWatt;// (uint)(AVRIO.avrio.ChargeWatt * AVRIO.avrio.CurrentUnitPrice);

                    if (AVRIO.avrio.ChargePrice < 1)
                    {
                        AVRIO.avrio.ChargePrice = 1;
                    }

                    // 충전 시간 계산
                    byte[] byTime = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.RemainTime_sec - AVRIO.qcio.READADDRESS.Status]);
                    ushort remainTime_Sec = BitConverter.ToUInt16(byTime, 0);

                    if (remainTime_Sec != 588)
                        AVRIO.avrio.ChargeRemainTimess = remainTime_Sec;

                    byTime = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.RemainTime_min - AVRIO.qcio.READADDRESS.Status]);
                    ushort remainTime_Min = BitConverter.ToUInt16(byTime, 0);

                    if (remainTime_Min != 588)
                        AVRIO.avrio.ChargeRemainTimemm = remainTime_Min;

                    if (remainTime_Sec != 255)
                    {
                        // AVRIO.avrio.ChargeRemainTime = (ushort)((remainTime_Sec * 10) / 60) + 1;
                        AVRIO.avrio.ChargeRemainTime = (ushort)((remainTime_Sec * 10) / 60);
                    }
                    else
                    {
                        AVRIO.avrio.ChargeRemainTime = remainTime_Min;
                    }
                    //  AVRIO.avrio.EventMsg = "remainTime_Sec=> " + remainTime_Sec.ToString();
                    //  AVRIO.avrio.EventMsg = "remainTime_Min=> " + remainTime_Min.ToString();
                    //  AVRIO.avrio.EventMsg = "AVRIO.avrio.ChargeRemainTime=> " + AVRIO.avrio.ChargeRemainTime.ToString();


                    QuickChargeDisplay.QCDV.Charging.RemainTime = AVRIO.avrio.ChargeRemainTime;
                    QuickChargeDisplay.QCDV.Charging.ChargingWatt = AVRIO.avrio.ChargeWatt;
                    QuickChargeDisplay.QCDV.Charging.ChargingMoney = (double)AVRIO.avrio.ChargePrice;
                    QuickChargeDisplay.QCDV.Charging.ChargingCurrent = Convert.ToInt32(QuickChargeDisplay.QCDV.BmsInfo.OutAmpare);

                    //cbs
                    QCDV.BmsInfo.RemainTime = AVRIO.avrio.ChargeRemainTime.ToString();
                    QCDV.BmsInfo.SOC = string.Format("{0:F1}", AVRIO.avrio.ChargeSOC);


                    if (AVRIO.bmsio.bCellLowVoltageWarnFlag)
                    {
                        AVRIO.avrio.EventMsg = " Warring bCellLowVoltageWarnFlag Charging Stop";
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

                    }

                    if (AVRIO.bmsio.bCellHighVoltageWarnFlag)
                    {
                        AVRIO.avrio.EventMsg = " Warring bCellHighVoltageWarnFlag Charging Stop";
                        AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

                    }

                    //if (AVRIO.bmsio.bSOCHighWarnFlag)
                    //{
                    //    AVRIO.avrio.EventMsg = " Warring bSOCHighWarnFlag Charging Stop";
                    //    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

                    //}
                }

                // Fault처리

                if ((values[0] & (ushort)AVRIO.SysStatus.SysFault) != 0)
                {
                    try
                    {
                        AVRIO.avrio.EventMsg = "CHARGER RUN -> FAULT";

                        AVRIO.avrio.RunMode = AVRIO.RunningMode.Fault;

                        if (!AVRIO.avrio.IsFaultDialog || AVRIO.avrio.FaultStopCheck)
                        {
                            if (AVRIO.avrio.FaultDialogDelay == false)
                            {
                                byte[] byFault = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FaultCode - AVRIO.qcio.READADDRESS.Status]);

                                //    UInt16 nFaultNum = BitConverter.ToUInt16(byFault, 0);
                                UInt16 nFaultNum = (UInt16)byFault[0];

                                SetFaultLog(nFaultNum);
                                if (nFaultNum == 0x0b)
                                {

                                }
                                else
                                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;

                                if (AVRIO.avrio.WaitFualtFlag)
                                    AVRIO.avrio.IsFaultDialog = true;
                                else
                                {
                                    WaitFualt.Start();
                                    return;
                                }

                                if (nFaultNum == 0xcb)
                                {
                                    return;
                                }
                            }

                        }
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                    return;
                }
                else
                {
                    int nTempStatus = 0;
                    if ((values[0] & (int)1) > 0)
                    {
                        nTempStatus = nTempStatus + 1;
                    }
                    if ((values[0] & (int)2) > 0)
                    {
                        nTempStatus = nTempStatus + 2;
                    }
                    if ((values[0] & (int)4) > 0)
                    {
                        nTempStatus = nTempStatus + 4;
                    }
                    switch (AVRIO.avrio.CurrentStatus)
                    {
                        case AVRIO.SysStatus.SysStandby:
                            {
                                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                            }
                            break;
                        case AVRIO.SysStatus.SysReady:
                            {
                                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                            }
                            break;
                        case AVRIO.SysStatus.SysStopping:
                            {
                                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                            }
                            break;
                        case AVRIO.SysStatus.SysFinish:
                            {
                                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                            }
                            break;
                        case AVRIO.SysStatus.SysPreRun:
                            {
                                if (nTempStatus == 3)
                                {
                                    AVRIO.avrio.EventMsg = "[Normal] SysPreRun => SysRunning";
                                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysRunning;
                                }
                                else if (nTempStatus == 4)
                                {
                                    AVRIO.avrio.EventMsg = "[Normal] SysPreRun => SysStopping";
                                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStopping;
                                }
                                else if (nTempStatus == 5)
                                {
                                    AVRIO.avrio.EventMsg = "[Normal] SysPreRun => SysFinish";
                                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                    ChargeFinish();
                                }
                            }
                            break;
                        case AVRIO.SysStatus.SysStartButton:
                            {
                                AVRIO.avrio.StartButton = true;
                            }
                            break;

                        case AVRIO.SysStatus.SysStopButton:
                            {

                                AVRIO.avrio.StopButton = true;
                            }
                            break;
                        case AVRIO.SysStatus.SysRunning:
                            {
                                if (nTempStatus == 4)
                                {
                                    AVRIO.avrio.EventMsg = "[Normal] SysRunning => SysStopping";
                                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStopping;
                                }
                                else if (nTempStatus == 5)
                                {
                                    AVRIO.avrio.EventMsg = "[Normal] SysRunning => SysFinish";
                                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                    ChargeFinish();
                                }
                            }
                            break;
                    }
                    if (!((AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysPreRun)
                        || (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)))
                    {
                        AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                    }
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }
        private void ReadQCNormalData(ushort[] values)
        {
            try
            {
                byte[] byStatus = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.Status - AVRIO.qcio.READADDRESS.Status]);

                // Fault처리
                if ((values[0] & (ushort)AVRIO.SysStatus.SysFault) != 0 && AVRIO.avrio.FaultStopCheck)
                {
                    try
                    {
                        AVRIO.avrio.EventMsg = "CHARGER RUN -> FAULT";
                        AVRIO.avrio.RunMode = AVRIO.RunningMode.Fault;

                        if (!AVRIO.avrio.IsFaultDialog)
                        {
                            if (AVRIO.avrio.FaultDialogDelay == false)
                            {
                                byte[] byFault = BitConverter.GetBytes(values[AVRIO.qcio.READADDRESS.FaultCode - AVRIO.qcio.READADDRESS.Status]);
                                // UInt16 nFaultNum = BitConverter.ToUInt16(byFault, 0);
                                UInt16 nFaultNum = (UInt16)byFault[0];

                                if (nFaultNum == 14 || nFaultNum < 1)
                                    return;

                                SetFaultLog(nFaultNum);

                                if (nFaultNum == 0x0b)
                                {

                                }
                                else
                                    AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 1;

                                if (AVRIO.avrio.WaitFualtFlag)
                                    AVRIO.avrio.IsFaultDialog = true;
                                else
                                {
                                    WaitFualt.Start();
                                    return;
                                }
                                if (nFaultNum == 0xcb)
                                {
                                    return;
                                }

                            }
                        }
                        return;
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                }
                else
                {
                    try
                    {
                        int nTempStatus = 0;
                        if ((values[0] & (int)1) > 0)
                        {
                            nTempStatus = nTempStatus + 1;
                        }

                        if ((values[0] & (int)2) > 0)
                        {
                            nTempStatus = nTempStatus + 2;
                        }

                        if ((values[0] & (int)4) > 0)
                        {
                            nTempStatus = nTempStatus + 4;
                        }

                        switch (AVRIO.avrio.CurrentStatus)
                        {
                            case AVRIO.SysStatus.SysStandby:
                                {
                                    if (AVRIO.avrio.CurrentBatStatus == AVRIO.SysBatteryStatus.SysBatteryStandby)
                                    {
                                        if (nTempStatus == 5)
                                        {
                                            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                                            AVRIO.avrio.EventMsg = "[SysBatteryRunning => SysBatteryStandby] SysFinish : TsStandby";
                                        }
                                    }

                                    if (nTempStatus == 1)
                                    {
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysReady;
                                    }
                                    else if (nTempStatus == 2)
                                    {
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysReady;
                                    }
                                }
                                break;
                            case AVRIO.SysStatus.SysReady:
                                {
                                    if (nTempStatus == 0)
                                    {
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
                                    }
                                    else if (nTempStatus == 2)
                                    {
                                        bInitVehicleBatteryEnergy = false;
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysPreRun;
                                    }
                                    else if (nTempStatus == 3)
                                    {
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysRunning;
                                    }
                                }
                                break;
                            case AVRIO.SysStatus.SysPreRun:
                                {
                                    if (nTempStatus == 3)
                                    {
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysRunning;
                                    }
                                }
                                break;
                            case AVRIO.SysStatus.SysStopping:
                                {
                                    if (nTempStatus == 5)
                                    {
                                        AVRIO.avrio.EventMsg = "[Normal] SysStopping => SysFinish";
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                        ChargeFinish();
                                    }
                                    else if (nTempStatus == 0)
                                    {
                                        AVRIO.avrio.EventMsg = "[Normal] SysStopping => SysStandby";
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysFinish;
                                        ChargeFinish();
                                    }
                                }
                                break;
                            case AVRIO.SysStatus.SysFinish:
                                {

                                }
                                break;
                            case AVRIO.SysStatus.SysPayCheck:
                                {

                                }
                                break;

                            case AVRIO.SysStatus.SysWorkDone:
                                {
                                    if (nTempStatus == 0)
                                    {
                                        AVRIO.avrio.EventMsg = "[Normal] SysPayCheck, SysWorkDone => SysStandby";
                                        AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
                                    }
                                }
                                break;
                        }
                        if ((AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysPreRun)
                            || (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning))
                        {
                            AVRIO.avrio.RunMode = AVRIO.RunningMode.Charging;
                        }
                    }
                    catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {  //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = databits;
                sp.Parity = parity;
                sp.StopBits = stopBits;

                sp.RtsEnable = false;
                //These timeouts are default and cannot be editted through the class at this point:
                sp.ReadTimeout = 300;
                sp.WriteTimeout = 300;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }

        }

        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            try
            {
                if (sp.IsOpen)
                {
                    try
                    {
                        sp.Close();
                    }
                    catch (Exception err)
                    {
                        modbusStatus = "Error closing " + sp.PortName + ": " + err.Message;
                        return false;
                    }
                    modbusStatus = sp.PortName + " closed successfully";
                    return true;
                }
                else
                {
                    modbusStatus = sp.PortName + " is not open";
                    return false;
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:
            try
            {
                ushort CRCFull = 0xFFFF;
                byte CRCHigh = 0xFF, CRCLow = 0xFF;
                char CRCLSB;

                for (int i = 0; i < (message.Length) - 2; i++)
                {
                    CRCFull = (ushort)(CRCFull ^ message[i]);

                    for (int j = 0; j < 8; j++)
                    {
                        CRCLSB = (char)(CRCFull & 0x0001);
                        CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                        if (CRCLSB == 1)
                            CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
                CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
                CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }

        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            try
            {
                //Array to receive CRC bytes:
                byte[] CRC = new byte[2];
                message[0] = address;
                message[1] = type;
                message[2] = (byte)(start >> 8);
                message[3] = (byte)start;
                message[4] = (byte)(registers >> 8);
                message[5] = (byte)registers;

                GetCRC(message, ref CRC);
                message[message.Length - 2] = CRC[0];
                message[message.Length - 1] = CRC[1];
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            try
            {
                //Perform a basic CRC check:
                byte[] CRC = new byte[2];
                GetCRC(response, ref CRC);
#if DEBUG
                //return true;
#endif
                if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                return false;
            }
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.

            try
            {
                for (int i = 0; i < response.Length; i++)
                {
                    response[i] = (byte)(sp.ReadByte());
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }

        }
        #endregion

        #region Function 16 - Write Multiple Registers

        public bool SendFc16(byte address, ushort start, ushort registers, ushort[] values)
        {
            try
            {
                //Ensure port is open:
                if (sp.IsOpen)
                {
                    Monitor.Enter(ModbusQC.obj);
                    //Clear in/out buffers:
                    sp.DiscardOutBuffer();
                    sp.DiscardInBuffer();
                    //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                    byte[] message = new byte[9 + 2 * registers];
                    //Function 16 response is fixed at 8 bytes
                    byte[] response = new byte[8];

                    //Add bytecount to message:

                    message[6] = (byte)(registers * 2);
                    //Put write values into message prior to sending:
                    for (int i = 0; i < registers; i++)
                    {
                        message[7 + 2 * i] = (byte)(values[i] >> 8);
                        message[8 + 2 * i] = (byte)(values[i]);
                    }

                    //Build outgoing message:

                    BuildMessage(address, (byte)16, start, registers, ref message);

                    //Send Modbus message to Serial Port:

                    try
                    {
                        // sp.RtsEnable = true;

                        //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.
                        // Sleep4uSec(1300);

                        //AVRIO.avrio.EventMsg = "SendFc16 Request";

                        sp.Write(message, 0, message.Length);

                        //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.

                        Sleep4uSec(1300);

                        // sp.RtsEnable = false;

                        //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.
                        // Sleep4uSec(1300);

                        GetResponse(ref response);

                        //Evaluate message:

                        if (CheckResponse(response))
                        {
                            modbusStatus = "Write successful";
                            //AVRIO.avrio.EventMsg = "SendFc16 Request -> Write successful";
                            return true;
                        }
                        else
                        {

                            modbusStatus = "SendFc16 CRC error";
                            //AVRIO.avrio.EventMsg = "SendFc16 Request -> SendFc16 CRC error";
                            // System.Console.WriteLine(modbusStatus);
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        modbusStatus = "SendFc16 Error in write event: " + err.Message;

                        // System.Console.WriteLine(modbusStatus);
                        return false;
                    }
                    finally
                    {
                        Monitor.Exit(ModbusQC.obj);
                    }
                }
                else
                {
                    modbusStatus = "SendFc16 Serial port not open";
                    // System.Console.WriteLine(modbusStatus);                
                    return false;
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                return false;
            }
        }
        #endregion

        #region Function 4 - Read Registers
        public bool SendFc4(byte address, ushort start, ushort registers, ref ushort[] values)
        {
            try
            {
                //Ensure port is open:
                if (sp.IsOpen)
                {
                    lock (this)
                    {
                        //Monitor.Enter(Modbus.obj);

                        //Clear in/out buffers:
                        sp.DiscardOutBuffer();
                        sp.DiscardInBuffer();
                        //Function 3 request is always 8 bytes:
                        byte[] message = new byte[8];
                        //Function 3 response buffer:
                        byte[] response = new byte[5 + 2 * registers];
                        //Build outgoing modbus message:
                        BuildMessage(address, (byte)4, start, registers, ref message);
                        //Send modbus message to Serial Port:

                        try
                        {
                            // sp.RtsEnable = true;

                            //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.
                            // Sleep4uSec(1300);

                            //AVRIO.avrio.EventMsg = "SendFc4 Request";
                            sp.Write(message, 0, message.Length);

                            //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.
                            Sleep4uSec(1300);

                            // sp.RtsEnable = false;

                            //wait 350us for문 8개가 350us 정도 딜레이 역할을 함.
                            // Sleep4uSec(1300);

                            //Thread recvThread = new Thread();
                            GetResponse(ref response);

                            //Evaluate message:
                            if (CheckResponse(response))
                            {
                                //Return requested register values:
                                int j = 0;
                                for (j = 0; j < (response.Length - 5) / 2; j++)
                                {
                                    values[j] = response[2 * j + 3];
                                    values[j] <<= 8;
                                    values[j] += response[2 * j + 4];
                                }
                                modbusStatus = "Read successful";
                                // AVRIO.avrio.EventMsg = "SendFc4 Request -> Read successful";

                                return true;
                            }
                            else
                            {
                                modbusStatus = "SendFc4 CRC error";
                                //AVRIO.avrio.EventMsg = "SendFc4 Request -> SendFc4 CRC error";
                                // System.Console.WriteLine(modbusStatus);
                                return false;
                            }
                        }
                        catch (Exception err)
                        {
                            modbusStatus = "SendFc4 Error in read event: " + err.Message;
                            AVRIO.Log.Eventlog = err.ToString();
                            // System.Console.WriteLine(modbusStatus);                
                            return false;
                        }
                        finally
                        {
                            //Monitor.Exit(Modbus.obj);
                        }

                    } // lock
                }
                else
                {
                    modbusStatus = "SendFc4 Serial port not open";
                    // System.Console.WriteLine(modbusStatus);
                    return false;
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                return false;
            }

        }
        #endregion

        #region MicroSleep
        public void Sleep4uSec(long usec)  // delay(ms), frequency(clock)
        {
            try
            {
                long start = 0;
                long end = 0;
                long frequency = 0;
                long differance;
                long duration;

                QueryPerformanceFrequency(ref frequency);
                QueryPerformanceCounter(ref start);
                for (; ; )
                {
                    QueryPerformanceCounter(ref end);
                    differance = end - start;
                    duration = (long)(differance * 1000000 / frequency);
                    if (usec < duration)
                    {
                        return;
                    }
                }
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }
        #endregion

        #region 변환함수

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));

            return sb.ToString().ToUpper();
        }
        #endregion
    }
}