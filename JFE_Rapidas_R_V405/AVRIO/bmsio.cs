using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AVRIO
{
    public class bmsio
    {
        public static bool b_CanThread = false;             // Thread 종료 플래그
        public static bool b_CanSendThread = true;         // Send Thread 동작유무
        public static bool b_CanBatteryChk = false;         // Battery Check
        public static uint nCanCommChkCnt = 0;              // Can 통신확인
        public static uint nCanArriveChk = 0;               // Can 확인
        public static bool bWorking = false;                // 통신 확인

        public static bool bBMSFaultFlag = false;           // BMS Fault 유무
        public static bool bBMSCellVoltageFaultFlag = false;// BMS Cell 전압관련 폴트
        public static bool bBMSWarnFlag = false;            // BMS Warnning 유무
        public static bool bBMSCanCommErrorFlag = false;    // BMS CanComm Err.
         
        #region SendData
        // 300
        public struct _SendData_ID300
        {
            public UInt16 nYear;
            public Byte byMonth;
            public Byte byDay;
            public Byte byHour;
            public Byte byMinute;
            public Byte byPCS_Heartbeat_300;
        };

        // 301
        public struct _SendData_ID301
        {
            public UInt16 nBatteryCurrent;
            public UInt16 nBatteryVoltage;
            public Byte byPCS_Heartbeat_301;
        };

        // 302
        public struct _SendData_ID302
        {
            public UInt16 nBatteryInstantaneousPower;
            public UInt16 nCarInstantaneousPower;
            public UInt16 nUPSModeInstantaneousPower;
            public Byte byPCS_Heartbeat_302;
        };

        // 303
        public struct _SendData_ID303
        {
            public Byte byPSC_Status;
            public Byte byPCS_Heartbeat_303;
        };

        //  304
        [StructLayout(LayoutKind.Explicit)]
        public struct CanID304
        {
            [FieldOffsetAttribute(0)]
            public Byte ID304;
            [FieldOffsetAttribute(0)]
            public Byte byReserve0;
            [FieldOffsetAttribute(1)]
            public Byte byReserve1;
            [FieldOffsetAttribute(2)]
            public Byte byReserve2;
            [FieldOffsetAttribute(3)]
            public Byte byReserve3;
            [FieldOffsetAttribute(4)]
            public Byte byBMSWakeSleepModeControl;
            [FieldOffsetAttribute(5)]
            public Byte byBMSFaultFlagResetControl;
            [FieldOffsetAttribute(6)]
            public Byte byBMSMainContactor;
        };

       

        public struct _SendData_ID304
        {
            public CanID304 Control_Status;
            public Byte byPCS_Heartbeat_304;
        };

        public static _SendData_ID300 SendData_300 = new _SendData_ID300();
        public static _SendData_ID301 SendData_301 = new _SendData_ID301();
        public static _SendData_ID302 SendData_302 = new _SendData_ID302();
        public static _SendData_ID303 SendData_303 = new _SendData_ID303();
        public static _SendData_ID304 SendData_304 = new _SendData_ID304();
       // public static _SendData_ID305 SendData_305 = new _SendData_ID305();
        #endregion

        #region RecvData

        // 200
        public static UInt16 nMaxCellVoltage;
        public static UInt16 nMinCellVoltage;
        public static UInt16 nAverageCellVoltage;
        public static Byte byPCS_Heartbeat_200;

        // 201
        public static UInt16 nMaxTemperature;
        public static UInt16 nMinTemperature;
        public static UInt16 nAverageTemperature;
        public static Byte byFanControl;
        public static Byte byPCS_Heartbeat_201;

        // 202
        public static UInt32 nChargePowerLimit;        
        public static UInt32 nDischargePowerLimit;     
        public static UInt32 n30MinuteDischargePowerLimit;
        public static Byte bySOC = 0X75;
        public static Byte byPCS_Heartbeat_202;

        // 203
        public static bool bLowTempWarnFlag;
        public static bool bHighTempWarnFlag;
        public static bool bCellVoltageImbalanceWarnFlag;
        public static bool bCellLowVoltageWarnFlag;
        public static bool bCellHighVoltageWarnFlag;
        public static bool bHighDischargeCurrentWarnFlag;
        public static bool bHighChargeCurrentWarnFlag;

        public static bool bHighDischargePowerWarnFlag;
        public static bool bHighChargePowerWarnFlag;
        public static bool bPackVoltageDeviationWarnFlag;
        public static bool bSOCHighWarnFlag;
        public static bool bSOCLowWarnFlag;

        public static bool bFaultFlag;
        public static bool bInternalCommuLossFaultFlag;
        public static bool bLowTempFaultFlag;
        public static bool bHighTempFaultFlag;
        public static bool bCellVoltageImbalanceFaultFlag;
        public static bool bCellLowVoltageFaultFlag;
        public static bool bCellHighVoltageFaultFlag;
        public static bool bHighDischargeCurrentFaultFlag;
        public static bool bHighChargeCurrentFaultFlag;
        public static bool bHighDischargePowerFaultFlag;
        public static bool bHighChargePowerFaultFlag;
        public static bool bPackVoltageDeviationFaultFlag;
        public static bool bPrechargeFailFlag;
        public static bool bPrechargeContactorStatus;
        public static bool bLowSideMainContactorStatus;
        public static bool bHighSideMainContactorStatus;
        public static bool bSOCHighFaultFlag;
        //public static bool bSOCLowFaultFlag;

        public static Byte byBMSStatus;
        public static Byte byBMSDataEnable;
        public static Byte byPCS_Heartbeat_203;

        // 204
        public static UInt16 nChargeVoltageSetPoint;        // Scale 0.01
        public static UInt16 nCutOffVoltageSetPoint;        // R : Scale 0.01
        public static Byte byRBMSSWVersion;
        public static Byte byRBMSHWVersion;
        public static Byte bySOCLowValueSetPoint;
        public static Byte bySOCHighValueSetPoint;
        public static Byte byPCS_Heartbeat_204;

        // 205
        public static Byte byMaxCellVoltageNumber;
        public static Byte byMinCellVoltageNumber;
        public static Byte byMaxModuleTemperatureNumber;
        public static Byte byMinModuleTemperatureNumber;


        #endregion
    }
}
