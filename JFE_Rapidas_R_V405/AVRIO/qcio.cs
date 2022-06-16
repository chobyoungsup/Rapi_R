using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVRIO
{
    public class qcio
    {
        private ushort readRegisters = 6;
        private ushort totalReadRegisters = 14 + 8;  //cbs 전압전류설정 추가

        public ushort TotalReadRegisters
        {
            get { return totalReadRegisters; }
            set { totalReadRegisters = value; }
        }

        public ushort ReadRegisters
        {
            get { return readRegisters; }
            set { readRegisters = value; }
        }
        private ushort status;
        private ushort rev_status;
        private ushort fault;
        private ushort sw;      // switch

        public enum READADDRESS
        {
            Status = 400,
            FaultCode = 401,
            Ref_Vout = 402,
            Ref_Iout = 403,
            FeedBack_Vout = 404,
            FeedBack_Iout = 405,
            FeedBack_Vbat = 406,
            FeedBack_IbatAvg = 407,
            Power_Out = 408,
            Power_Bat = 409,
            RemainTime_sec = 410,
            RemainTime_min = 411,
            BmsBatteryCapacity = 412,
            SbcDspVersion = 413,

            VoltageSlope_1 = 414,
            VoltageSlope_2 = 415,

            VoltageOffset_1 = 416,
            VoltageOffset_2 = 417,

            CurrentSlope_1 = 418,
            CurrentSlope_2 = 419,

            CurrentOffset_1 = 420,
            CurrentOffset_2 = 421,

        };

        private ushort writeRegisters = 11 + 8;  
        private ushort writeSetRegisters = 9;

        public ushort WriteRegisters
        {
            get { return writeRegisters; }
            set { writeRegisters = value; }
        }

        //public ushort WriteSetRegisters
        //{
        //    get { return writeSetRegisters; }
        //    set { writeSetRegisters = value; }
        //}

        private ushort command;
        private ushort rev_command;
        private ushort[] wr_analog = new ushort[3];

        public enum WRITEADDRESS
        {
            Command = 200,
            BMS_SetData = 201,

            GridInputPowerMax = 209,
            OutputCurrentMaxMin = 210,
        };

        /// </summary>
        public string[] msgFault =
        {
            "No_Fault",
            "Over-current of Ia ( > 150A )",
            "Over-current of Ib ( > 150A )",
            "Over-current of Idc ( > 300A )",
            "Over-current of Iout ( > 150A )",
            "Over-current of Ibat ( > 150A )",
            "Gate Driver Error",
            "Thermal-Switch 1 Open(Over_Temp1)",
            "Thermal-Switch 2 Open(Over_Temp2)",
            "Reserved",
            "Emergency Switch Open(EMGBtnPushed)",
            "Rectified Voltage Error ( <240V )",
            "DC-Link Voltage Error ( <320V or >420V )",
            "Over-current of Ia or Ib ( > 135A )",
            "Touch-Screen Fault ( from SBC )",
            "Negative Seq. Line Input",
            "Grid Freauency Miss Match",
            "Output/Battery/DC-Link Fuse Open",
            "Output Leakage Current Detected(GroundFaultDetected)",
            "Conn.Lock Released while Charging",
            "Output MC Connection Fault",
            "InsulationVerificationFail_VoltageMismatch",
            "InsulationVerificationFail_OutputShort",
            "VehicleStartReceivedBeforeSysReady",
            "ModBus Comm. Terminated",
        };

        public string[] msgFault100 =
        {
            "InitialTransmissionOfCanDataTimeOut",
            "ChargingEnale_TimeOut",
            "VehicleContactAbnormal",
            "Relay(c)ON-TimeOut",
            "CurrentReq-TimeOut",
            "CurrentStop-TimeOut",
            "Relay(c)OFF-TimeOut",
            "ChargingProhibitionBeforeCharging",
            "CanDataTransmissionTerminationTimeOut",
            "CanDataTransmissionTerminated",
            "ChargingCurrentRequestExceedsAvailableOutputCurrent"
        };

        public string[] msgFault200 =
        {
            "BatteryEmergencyFault",
            "BatteryReadyTimeOut",
            "BatteryStart Received before SysBatteryReady",
            "BatteryVoltageDeviationFault(>15V)",
            "Relay Welding"
        };

        public string[] msgBMSFault =
        {   "NULL",
            "CHVW_CellHighVoltageWarningFlag",//discharge 301
            "CLVW_CellLowVoltageWarningFalg", // 무시302
            "CHVF_CellHighVoltageFaultFalg", //303
            "CLVF_CellLowVoltageFaultFlag ",  //304
           // "HTW_HighTemperatureWarningFlag",  //Warning 305
            "HTW_HighTemperatureFaultFlag",  //2020.04.01 Warning-> Fault 305
            "LTW_LowTemperatureWarningFlag ",   // Discharge 306
            "HTF_HighTemperatureFaultFlag",     //307
            "LTF_LowTemperatureFaultFlag",      //308
            "CVIW_CellVoltageImbalanceWarningFlag",//Warning 309
            "CVIF_CellVoltageImbalanceFaultFlag", // 310
            "HCCW_HighChargeCurrentWarningFalg", // Warning 311
            "HDCW_HighDischargeCurrentWarningFlag", // Warning 312
            "HCCF_HighChargeCurrentFaultFlag",  //313
            "HDCF_HighDischargeCurrentFaultFlag",  //314
            //"PVDW_PackVoltageDeviationWarningFlag",// 무시315
            "SLF_Less than 5% battery",// 2020.04.01    315  추가
            "PVDF_PackVoltageDeviationFaultFlag",// 무시316
            "PCF_PrechargeFailFlag",            //317
            "SHW_SocHighWarningFlag",           //318
            "SHF_SocHighFaultFlag ",            //319
            "ICLF_InternalCimmunicationLossFaultFlag", //320
            "CANCOMMUNICATIONFaultFlag", //321
        };
    }
}



