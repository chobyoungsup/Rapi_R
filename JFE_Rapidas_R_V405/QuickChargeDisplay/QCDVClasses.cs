using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace QuickChargeDisplay
{
    public class DisplayVariablesBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 멤버

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


    /// <summary>
    /// 충전금액 화면 디스플레이 변수
    /// 03_ChargeMoneyPage.xaml
    /// </summary>
    public class ChargeMoney : DisplayVariablesBase
    {
        private double currentUnitPrice;
        private double chargedSoc;
       // private ChargeMode chargeMode = ChargeMode.Money;
        private double chargeValue;

        public double CurrentUnitPrice
        {
            get { return currentUnitPrice; }
            set { currentUnitPrice = value; OnPropertyChanged("CurrentUnitPrice"); }
        }

        public double ChargedSoc
        {
            get { return chargedSoc; }
            set { chargedSoc = value; OnPropertyChanged("ChargedSoc"); }
        }

        //public ChargeMode ChargeMode
        //{
        //    get { return chargeMode; }
        //    set { chargeMode = value; OnPropertyChanged("ChargeMode"); }
        //}
        
        public double ChargeValue
        {
            get { return chargeValue; }
            set { chargeValue = value; OnPropertyChanged("ChargeValue"); }
        }
    }

    /// <summary>
    /// 충전중 화면 디스플레이 변수
    /// 04_ChargingPage.xaml
    /// </summary>
    public class Charging : DisplayVariablesBase
    {
        private int remainTime;
        private double chargingMoney;
        private double chargingWatt;
        private double chargingSoc;
        private int chargingCurrent;

        public int RemainTime
        {
            get { return remainTime; }
            set { remainTime = value; OnPropertyChanged("RemainTime"); }
        }

        public double ChargingMoney
        {
            get { return chargingMoney; }
            set { chargingMoney = value; OnPropertyChanged("ChargingMoney"); }
        }

        public double ChargingWatt
        {
            get { return chargingWatt; }
            set { chargingWatt = value; OnPropertyChanged("ChargingWatt"); }
        }

        public double ChargingSoc
        {
            get { return chargingSoc; }
            set { chargingSoc = value; OnPropertyChanged("ChargingSoc"); }
        }

        public int ChargingCurrent
        {
            get { return chargingCurrent; }
            set { chargingCurrent = value; OnPropertyChanged("ChargingCurrent"); }
        }
    }

    /// <summary>
    /// 결제확인 디스플레이 변수
    /// 06_ConfirmChargePage.xaml
    /// </summary>
    public class ConfirmCharge : DisplayVariablesBase
    {
        private double payment;
        private double chargeWatt;

        public double Payment
        {
            get { return payment; }
            set { payment = value; OnPropertyChanged("Payment"); }
        }
        
        public double ChargeWatt
        {
            get { return chargeWatt; }
            set { chargeWatt = value; OnPropertyChanged("ChargeWatt"); }
        }
    }

    /// <summary>
    /// 충전완료 디스플레이 변수
    /// 06_CompleteChargePage.xaml
    /// </summary>
    public class CompleteCharge : DisplayVariablesBase
    {
        private int nCurrentBatStatus;

        public int CurrentBatStatus
        {
            get { return nCurrentBatStatus; }
            set { nCurrentBatStatus = value; OnPropertyChanged("CurrentBatStatus"); }
        }
    }

    /// <summary>
    /// 충전내역 및 고장내역 레코드의 Base Class
    /// 09_1_ChargeHistoryPage.xaml, 09_2_TroubleHistoryPage.xaml
    /// </summary>
    public class HistoryRecord : DisplayVariablesBase
    {
        private int    number;  // No.
        private string date;    // 일자
        private string time;    // 시간
        private string details; // 내역
        private bool   check;   // Check 여부

        public HistoryRecord(int number, string date, string time, string details, bool check)
        {
            this.number = number;
            this.date = date;
            this.time = time;
            this.details = details;
            this.check = check;
        }
        
        public int Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged("Number"); }
        }
        
        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }
        
        public string Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }
        
        public string Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged("Details"); }
        }
        
        public bool Check
        {
            get { return check; }
            set { check = value; OnPropertyChanged("Check"); }
        }
    }

    public class FaultRecord : DisplayVariablesBase
    {
        private int number;     // No.
        private string date;    // 일자
        private string time;    // 고장시간
        private string time2;   // 복귀시간
        private int code;       // 고장 코드
        private string details; // 내역
        private bool check;     // Check 여부

        public FaultRecord(int number, string date, string time, string time2, int code, string details, bool check)
        {
            this.number = number;
            this.date = date;
            this.time = time;
            this.time2 = time2;
            this.code = code;
            this.details = details;
            this.check = check;
        }

        public int Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged("Number"); }
        }

        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        public string Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }

        public string Time2
        {
            get { return time2; }
            set { time2 = value; OnPropertyChanged("Time2"); }
        }

        public int Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged("Code"); }
        }

        public string Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged("Details"); }
        }

        public bool Check
        {
            get { return check; }
            set { check = value; OnPropertyChanged("Check"); }
        }

    }

    /// <summary>
    /// 충전내역 디스플레이 클래스
    /// 09_1_ChargeHistoryPage.xaml
    /// </summary>
    public class ChargeHistory : DisplayVariablesBase
    {
        private ObservableCollection<HistoryRecord> list = new ObservableCollection<HistoryRecord>();

        public ObservableCollection<HistoryRecord> List
        {
            get { return list; }
            set { list = value; }
        }

        public HistoryRecord this[int index]
        {
            get { return list[index]; }
            set
            {
                if (index < list.Count)
                    list[index] = value;
                else
                    list.Add(value);

                OnPropertyChanged("HistoryList");
            }
        }

        public void Add(HistoryRecord record)
        {
            list.Add(record);
            OnPropertyChanged("HistoryList");
        }

        public void Clear()
        {
            list.Clear();
            OnPropertyChanged("HistoryList");
        }
    }

    /// <summary>
    /// 고장내역 레코드 디스플레이 클래스
    /// 09_2_TroubleHistoryPage.xaml
    /// </summary>
    public class TroubleHistoryRecord : HistoryRecord
    {
        private string returnTime;

        public string ReturnTime
        {
            get { return returnTime; }
            set { returnTime = value; OnPropertyChanged("ReturnTime"); }
        }

        public TroubleHistoryRecord(int number, string date, string time, string returnTime, string details, bool check)
            : base(number, date, time, details, check)
        {
            this.returnTime = returnTime;
        }
    }

    /// <summary>
    /// 고장내역 디스플레이 클래스
    /// 09_1_ChargeHistoryPage.xaml, 09_2_TroubleHistoryPage.xaml
    /// </summary>
    public class TroubleHistory : DisplayVariablesBase
    {
        private ObservableCollection<TroubleHistoryRecord> list = new ObservableCollection<TroubleHistoryRecord>();

        public ObservableCollection<TroubleHistoryRecord> List
        {
            get { return list; }
            set { list = value; }
        }

        public TroubleHistoryRecord this[int index]
        {
            get { return list[index]; }
            set
            {
                if (index < list.Count)
                    list[index] = value;
                else
                    list.Add(value);

                OnPropertyChanged("TroubleHistoryList");
            }
        }

        public void Add(TroubleHistoryRecord record)
        {
            list.Add(record);
            OnPropertyChanged("TroubleHistoryList");
        }

        public void Clear()
        {
            list.Clear();
            OnPropertyChanged("TroubleHistoryList");
        }
    }

    /// <summary>
    /// 기기설정
    /// 09_4_SetupEquipmentPage.xaml
    /// </summary>
    public class SetupEquipment : DisplayVariablesBase
    {
        private int mode = 0;            // 0: 자동제어, 1: 수동제어
        private bool sound = true;       // 사운드 ON, OFF
        private int autoStartTimeHour;   // 시작시간 시간
        private int autoStartTimeMinute; // 시작시간 분
        private int autoEndTimeHour;     // 종료시간 시간
        private int autoEndTimeMinute;   // 종료시간 분

        public int Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged("Mode"); }
        }

        public bool Sound
        {
            get { return sound; }
            set { sound = value; OnPropertyChanged("Sound"); }
        }

        public int AutoStartTimeHour
        {
            get { return autoStartTimeHour; }
            set { autoStartTimeHour = value; OnPropertyChanged("AutoStartTimeHour"); }
        }
        
        public int AutoStartTimeMinute
        {
            get { return autoStartTimeMinute; }
            set { autoStartTimeMinute = value; OnPropertyChanged("AutoStartTimeMinute"); }
        }
                
        public int AutoEndTimeHour
        {
            get { return autoEndTimeHour; }
            set { autoEndTimeHour = value; OnPropertyChanged("AutoEndTimeHour"); }
        }
        
        public int AutoEndTimeMinute
        {
            get { return autoEndTimeMinute; }
            set { autoEndTimeMinute = value; OnPropertyChanged("AutoEndTimeMinute"); }
        }
    }

    /// <summary>
    /// 수동제어
    /// </summary>
    public class ManualCotrol : DisplayVariablesBase
    {
        private int currentBatStatus;
        private bool bBMSReady;
        private string nBMSStatus;
        private string nPCSStatus;
        private string nAverageCellVoltage;
        private string nAverageTemperature;
        private string nBatteryVoltage;
        private string nBatteryCurrent;
        private string bySOC;
        private string nChargeVoltageSetPoint;
       

        public string ChargeVoltageSetPoint
        {
            get { return nChargeVoltageSetPoint; }
            set { nChargeVoltageSetPoint = value; OnPropertyChanged("ChargeVoltageSetPoint"); }
        }

        public string SOC
        {
            get { return bySOC; }
            set { bySOC = value; OnPropertyChanged("SOC"); }
        }

        public string BatteryCurrent
        {
            get { return nBatteryCurrent; }
            set { nBatteryCurrent = value; OnPropertyChanged("BatteryCurrent"); }
        }

        public string BatteryVoltage
        {
            get { return nBatteryVoltage; }
            set { nBatteryVoltage = value; OnPropertyChanged("BatteryVoltage"); }
        }

        public string AverageTemperature
        {
            get { return nAverageTemperature; }
            set { nAverageTemperature = value; OnPropertyChanged("AverageTemperature"); }
        }

        public string AverageCellVoltage
        {
            get { return nAverageCellVoltage; }
            set { nAverageCellVoltage = value; OnPropertyChanged("AverageCellVoltage"); }
        }

        public string BMSStatus
        {
            get { return nBMSStatus; }
            set { nBMSStatus = value; OnPropertyChanged("BMSStatus"); }
        }

        public string PCSStatus
        {
            get { return nPCSStatus; }
            set { nPCSStatus = value; OnPropertyChanged("PCSStatus"); }
        }

        public int CurrentBatStatus
        {
            get { return currentBatStatus; }
            set { currentBatStatus = value; OnPropertyChanged("CurrentBatStatus"); }
        }

        public bool BMSReady
        {
            get { return bBMSReady; }
            set { bBMSReady = value; OnPropertyChanged("BMSReady"); }
        }
    }
   

    /// <summary>
    /// 공장설정 디스플레이 메인
    /// 09_7_FactorySetupPage.xaml
    /// </summary>
    public class FactorySetup : DisplayVariablesBase
    {
        private string hardwareVersion; // Hardware Version
        private int mcOffTime;          // MC Off Time
        private int fanOffTime;         // Fan Off Time

        public string HardwareVersion
        {
            get { return hardwareVersion; }
            set { hardwareVersion = value; OnPropertyChanged("HardwareVersion"); }
        }

        public int McOffTime
        {
            get { return mcOffTime; }
            set { mcOffTime = value; OnPropertyChanged("McOffTime"); QCDV.OnAdminValueChanged(209, 0, mcOffTime, 4); }
        }

        public int FanOffTime
        {
            get { return fanOffTime; }
            set { fanOffTime = value; OnPropertyChanged("FanOffTime"); QCDV.OnAdminValueChanged(210, 0, fanOffTime, 4); }
        }
    }

    

    /// <summary>
    /// 전압/전류 교정
    /// </summary>
    public class CorrectSetup : DisplayVariablesBase
    {
        private bool c_200_01;
        private bool c_400_06;
        private bool c_200_07;
        private bool c_400_05;
        private double c_424;
        private double c_428;
        private double c_426;
        private bool c_200_05;
        private double c_214;
        private double c_216;
        private bool c_203_00;
        private double c_218;
        private double c_220;
        private bool c_203_01;
        private double c_222;
        private double c_224;
        private double c_226;
        private double c_228;

        private bool c_203_02;
        private bool c_203_03;

        private bool c_403_00;
        private bool c_403_04;

        public bool C_403_00
        {
            get { return c_403_00; }
            set { c_403_00 = value; OnPropertyChanged("C_403_00"); }
        }

        public bool C_403_04
        {
            get { return c_403_04; }
            set { c_403_04 = value; OnPropertyChanged("C_403_04"); }
        }

        public bool C_400_06
        {
            get { return c_400_06; }
            set { c_400_06 = value; OnPropertyChanged("C_400_06"); }
        }

        public bool C_200_01
        {
            get { return c_200_01; }
            set
            {
                c_200_01 = value; OnPropertyChanged("C_200_01");
                QCDV.OnAdminValueChanged(200, 1, c_200_01, 1);
            }
        }

        public bool C_200_07
        {
            get { return c_200_07; }
            set
            {
                c_200_07 = value; OnPropertyChanged("C_200_07");
                QCDV.OnAdminValueChanged(200, 7, c_200_07, 1);
            }
        }

        public bool C_400_05
        {
            get { return c_400_05; }
            set { c_400_05 = value; OnPropertyChanged("C_400_05"); }
        }

        public double C_424
        {
            get { return c_424; }
            set { c_424 = value; OnPropertyChanged("C_424"); }
        }

        public double C_428
        {
            get { return c_428; }
            set { c_428 = value; OnPropertyChanged("C_428"); }
        }

        public double C_426
        {
            get { return c_426; }
            set { c_426 = value; OnPropertyChanged("C_426"); }
        }

        public bool C_200_05
        {
            get { return c_200_05; }
            set
            {
                c_200_05 = value; OnPropertyChanged("C_200_05");
                QCDV.OnAdminValueChanged(200, 5, c_200_05, 1);
            }
        }

        public double C_214
        {
            get { return c_214; }
            set
            {
                c_214 = value; OnPropertyChanged("C_214");
                QCDV.OnAdminValueChanged(214, 0, c_214, 5);
            }
        }

        public double C_216
        {
            get { return c_216; }
            set
            {
                c_216 = value; OnPropertyChanged("C_216");
                QCDV.OnAdminValueChanged(216, 0, c_216, 5);
            }
        }

        public bool C_203_00
        {
            get { return c_203_00; }
            set
            {
                c_203_00 = value; OnPropertyChanged("C_203_00");
                QCDV.OnAdminValueChanged(203, 0, c_203_00, 1);
            }
        }

        public double C_218
        {
            get { return c_218; }
            set
            {
                c_218 = value; OnPropertyChanged("C_218");
                QCDV.OnAdminValueChanged(218, 0, c_218, 5);
            }
        }

        public double C_220
        {
            get { return c_220; }
            set
            {
                c_220 = value; OnPropertyChanged("C_220");
                QCDV.OnAdminValueChanged(220, 0, c_220, 5);
            }
        }

        public bool C_203_01
        {
            get { return c_203_01; }
            set
            {
                c_203_01 = value; OnPropertyChanged("C_203_01");
                QCDV.OnAdminValueChanged(203, 1, c_203_01, 1);
            }
        }

        public double C_222
        {
            get { return c_222; }
            set
            {
                c_222 = value; OnPropertyChanged("C_222");
                QCDV.OnAdminValueChanged(222, 0, c_222, 5);
            }
        }


        public double C_224
        {
            get { return c_224; }
            set
            {
                c_224 = value; OnPropertyChanged("C_224");
                QCDV.OnAdminValueChanged(224, 0, c_224, 5);
            }
        }

        public double C_226
        {
            get { return c_226; }
            set
            {
                c_226 = value; OnPropertyChanged("C_226");
                QCDV.OnAdminValueChanged(226, 0, c_226, 5);
            }
        }

        public double C_228
        {
            get { return c_228; }
            set
            {
                c_228 = value; OnPropertyChanged("C_228");
                QCDV.OnAdminValueChanged(228, 0, c_228, 5);
            }
        }

        public bool C_203_02
        {
            get { return c_203_02; }
            set
            {
                c_203_02 = value;
                OnPropertyChanged("C_203_02");
                QCDV.OnAdminValueChanged(203, 2, c_203_02, 1);
            }
        }

        public bool C_203_03
        {
            get { return c_203_03; }
            set
            {
                c_203_03 = value;
                OnPropertyChanged("C_203_03");
                QCDV.OnAdminValueChanged(203, 3, c_203_03, 1);
            }
        }
    }

    /// <summary>
    /// 장비 내부 설정
    /// </summary>
    public class InnerSetup : DisplayVariablesBase
    {
        private int year;
        private int month;
        private int day;
        
        private int hour;
        private int minute;
        private int seconds;

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        
        public int Day
        {
            get { return day; }
            set { day = value; }
        }
                
        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }
        
        public int Minute
        {
            get { return minute; }
            set { minute = value; }
        }

        public int Seconds
        {
            get { return seconds; }
            set { seconds = value; }
        }
    }


    public class BmsInfo : DisplayVariablesBase
    {
        private System.Windows.Visibility visible = System.Windows.Visibility.Visible;

        public System.Windows.Visibility Visible
        {
            get { return visible; }
            set { visible = value; OnPropertyChanged("Visible"); }
        }


        private string data1 = "Data1 테스트";
        private string data2 = "Data2 테스트";
        private string data3 = "Data3 테스트";
        private string data4 = "Data4 테스트";
        private string data5 = "Data5 테스트";
        private string data6 = "Data6 테스트";
        

        public string SOC
        {
            get { return data6; }
            set { data6 = value; OnPropertyChanged("SOC"); }
        }

        public string RemainTime
        {
            get { return data5; }
            set { data5 = value; OnPropertyChanged("RemainTime"); }
        }

        public string TargetAmpare
        {
            get { return data4; }
            set { data4 = value; OnPropertyChanged("TargetAmpare"); }
        }

        public string TargetVoltage
        {
            get { return data3; }
            set { data3 = value; OnPropertyChanged("TargetVoltage"); }
        }

        public string OutAmpare
        {
            get { return data2; }
            set { data2 = value; OnPropertyChanged("OutAmpare"); }
        }

        public string OutVoltage
        {
            get { return data1; }
            set { data1 = value; OnPropertyChanged("OutVoltage"); }
        }

    }

    //cbs 2019.10.27 
    /// <summary>
    /// 전압 전류 교정값 설정
    /// </summary>
    public class DSPCalibration : DisplayVariablesBase
    {
        private string voltage_Slope = "";
        private string voltage_Offset = "";
        private string current_Slope = "";
        private string current_Offset = "";    

        public string Voltage_Slope
        {
            get { return voltage_Slope; }
            set { voltage_Slope = value; OnPropertyChanged("Voltage_Slope"); }
        }
        public string Voltage_Offset
        {
            get { return voltage_Offset; }
            set { voltage_Offset = value; OnPropertyChanged("Voltage_Offset"); }
        }
        public string Current_Slope
        {
            get { return current_Slope; }
            set { current_Slope = value; OnPropertyChanged("Current_Slope"); }
        }
        public string Current_Offset
        {
            get { return current_Offset; }
            set { current_Offset = value; OnPropertyChanged("Current_Offset"); }
        }
    }
}
