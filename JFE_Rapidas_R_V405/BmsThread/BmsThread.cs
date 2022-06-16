using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using Multimedia;
using VCI_CAN_DotNET;

namespace BmsThread
{
    public class DC_BMS_Thread
    {
        public byte CAN_No;
        public byte CanPortNo = 0;

        private UInt32 bSendCmdCnt = 0;

        private bool bSendRecieveWorking = true;
        private bool bSendRecieveWorking_1 = true;

        private bool bSendWorking = false;

        private long StartTime = 0;

        private UInt16 pollCount = 0;
        int SRet;
        byte SMode = 0;
        byte SRTR = 0;
        byte SDLC = 8;

        byte[] SData = new byte[8];
        byte[] SData2 = new byte[8];
        byte[] SReadData = new byte[8];

        String SstrLog = null;

        int RRet;
        UInt32 RrxMsgCnt = 0;
        byte[] RReadData = new byte[8];
        String RstrLog = null;
        byte RMode, RRTR, RDLC;
        UInt32 RCANID, RTH, RTL;
        int Ri = 0;

        // private Int16 byPreBMSStatus = 0;        
        //[1] Global Functions

        string[] ErrMsg = 
        {
	        "No_Err",				"DEV_ModName_Err",			"DEV_ModNotExist_Err",
	        "DEV_PortNotExist_Err",	"DEV_PortInUse_Err", 		"DEV_PortNotOpen_Err",		
	        "CAN_ConfigFail_Err",	"CAN_HARDWARE_Err",			"CAN_PortNo_Err",			
	        "CAN_FIDLength_Err",	"CAN_DevDisconnect_Err",	"CAN_TimeOut_Err",			
	        "CAN_ConfigCmd_Err",	"CAN_ConfigBusy_Err",		"CAN_RxBufEmpty",
	        "CAN_TxBufFull",        "CAN_UserDefISRNo_Err" ,    "CAN_HWSendTimerNo_Err"
        };

        public bool CloseCanSocket()
        {
            int Ret;
            Ret = VCI_SDK.VCI_CloseCAN(CanPortNo);
            if (Ret != 0)
                AVRIO.avrio.EventMsg = ErrMsg[Ret];
            else
            {
                AVRIO.avrio.EventMsg = "CloseCAN Success";
            }
            return false;
        }

        public bool OpenCanPort()
        {
            //RegistryKey reg;
            //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("BMSIO");

            RegistryKey regKey;
            RegistryKey reg;
            if (Environment.Is64BitOperatingSystem)
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\BMSIO", true);
            }
            else
            {
                regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\BMSIO", true);
            }

            CanPortNo = Convert.ToByte(reg.GetValue("CanPortNo", "13"));
           
            int Ret = 0;
            while (!AVRIO.avrio.bThreadReady)
            {
                try
                {
                    Ret = VCI_SDK.VCI_OpenCAN_NoStruct(CanPortNo, 1, 250000, 0);
                }
                catch (System.Exception ex)
                {
                    AVRIO.avrio.EventMsg = "[CHARGER <=> BATTERY BMS] CAN PORT OPEN FAIL";
                }

                if (Ret != 0)
                {
                    AVRIO.avrio.EventMsg = "[CHARGER <=> BATTERY BMS] CAN PORT OPEN FAIL";
                }
                else
                {
                    AVRIO.avrio.bThreadReady = true;
                }
                //  Thread.Sleep(100);
                Thread.Sleep(3000);
            }

            InitCanSetting();
            AVRIO.avrio.IsBMSCanComm = true;
            AVRIO.bmsio.b_CanThread = true;

            Thread canSendThread = new Thread(Can_SendThread);
            canSendThread.Start();
            /*
            Thread.Sleep(499);
            Thread canRecvThread = new Thread(Can_RecvThread);
            canRecvThread.Start();
            */
            return true;
        }

        public void InitCanSetting()
        {
            // Default CanModule 은 1번
            CAN_No = 1;
            AVRIO.bmsio.SendData_300.byPCS_Heartbeat_300 = 0;
            AVRIO.bmsio.SendData_301.byPCS_Heartbeat_301 = 0;
            AVRIO.bmsio.SendData_302.byPCS_Heartbeat_302 = 0;
            AVRIO.bmsio.SendData_303.byPCS_Heartbeat_303 = 0;
            AVRIO.bmsio.SendData_304.byPCS_Heartbeat_304 = 0;
            //AVRIO.bmsio.SendData_410.byPCS_Heartbeat_410 = 0;                
            VCI_SDK.VCI_Clr_RxMsgBuf(CAN_No);
        }
        

        public DC_BMS_Thread()
        {
        }

        private void Can_SendThread()
        {
            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;
            int pollCount = 0;
            while (AVRIO.avrio.bThreadReady)
            {

                //if (bSendRecieveWorking)
                //{
                //    Can_Send();
                //    Thread.Sleep(500);
                //}
                //if(bSendRecieveWorking_1)
                //{
                //    Can_Recv();
                //    Thread.Sleep(500);
                //}
                //  AVRIO.avrio.EventCanMsg = pollCount.ToString();

                pollCount %= 2;
                if (pollCount == 0)
                {
                    // Thread.Sleep(250);
                    Can_Send();
                    //  Thread.Sleep(500);
                    Thread.Sleep(500);

                }
                else
                {
                    Can_Recv();
                    Thread.Sleep(500);
                }

                pollCount++;
            }
        }



        private void Can_Send()
        {
            try
            {
                if (AVRIO.bmsio.b_CanThread == false)
                {
                    return;
                }


                SData = MakeCanMsg_300();
                SRet = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(CAN_No, SMode, SRTR, SDLC, 0x300, SData);
                if (SRet != 0)
                {
                    AVRIO.avrio.EventMsg = ErrMsg[SRet];                    
                }
                else
                {
                    SstrLog = "[SEND] CMD:0x300 ==== " + ByteArrayToHexString(SData);
                }

                SData = MakeCanMsg_301();
                SRet = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(CAN_No, SMode, SRTR, SDLC, 0x301, SData);
                if (SRet != 0)
                {
                    AVRIO.avrio.EventMsg = ErrMsg[SRet];
                }
                else
                {
                    SstrLog = SstrLog + ", [SEND] CMD:0x301 ==== " + ByteArrayToHexString(SData);
                }

                SData = MakeCanMsg_302();
                SRet = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(CAN_No, SMode, SRTR, SDLC, 0x302, SData);
                if (SRet != 0)
                {
                    AVRIO.avrio.EventMsg = ErrMsg[SRet];
                }
                else
                {
                    SstrLog = SstrLog + ", [SEND] CMD:0x302 ==== " + ByteArrayToHexString(SData);
                }

                SData = MakeCanMsg_303();
                SRet = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(CAN_No, SMode, SRTR, SDLC, 0x303, SData);
                if (SRet != 0)
                {
                    AVRIO.avrio.EventMsg = ErrMsg[SRet];
                }
                else
                {
                    SstrLog = SstrLog + ", [SEND] CMD:0x303 ==== " + ByteArrayToHexString(SData);
                }

                SData = MakeCanMsg_304();
                SRet = VCI_CAN_DotNET.VCI_SDK.VCI_SendCANMsg_NoStruct(CAN_No, SMode, SRTR, SDLC, 0x304, SData);
                if (SRet != 0)
                {
                    AVRIO.avrio.EventMsg = ErrMsg[SRet];
                }
                else
                {
                    SstrLog = SstrLog + ", [SEND] CMD:0x304 ==== " + ByteArrayToHexString(SData);
                    AVRIO.avrio.EventCanMsg = SstrLog;
                }

                // 1. BMS에 CAN으로 FaultReset 신호를 한번 보낸 후 상태값을 0으로 초기화 해준다.
                if (AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl == 1)
                {
                    AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 0;
                }

                AVRIO.bmsio.SendData_300.byPCS_Heartbeat_300++;
                if (AVRIO.bmsio.SendData_300.byPCS_Heartbeat_300 > 3)
                {
                    AVRIO.bmsio.SendData_300.byPCS_Heartbeat_300 = 0;
                }
                AVRIO.bmsio.SendData_301.byPCS_Heartbeat_301++;
                if (AVRIO.bmsio.SendData_301.byPCS_Heartbeat_301 > 3)
                {
                    AVRIO.bmsio.SendData_301.byPCS_Heartbeat_301 = 0;
                }
                AVRIO.bmsio.SendData_302.byPCS_Heartbeat_302++;
                if (AVRIO.bmsio.SendData_302.byPCS_Heartbeat_302 > 3)
                {
                    AVRIO.bmsio.SendData_302.byPCS_Heartbeat_302 = 0;
                }
                AVRIO.bmsio.SendData_303.byPCS_Heartbeat_303++;
                if (AVRIO.bmsio.SendData_303.byPCS_Heartbeat_303 > 3)
                {
                    AVRIO.bmsio.SendData_303.byPCS_Heartbeat_303 = 0;
                }
                AVRIO.bmsio.SendData_304.byPCS_Heartbeat_304++;
                if (AVRIO.bmsio.SendData_304.byPCS_Heartbeat_304 > 3)
                {
                    AVRIO.bmsio.SendData_304.byPCS_Heartbeat_304 = 0;
                }
                //AVRIO.bmsio.SendData_305.byPCS_Heartbeat_305++;
                //if (AVRIO.bmsio.SendData_304.byPCS_Heartbeat_305 > 3)
                //{
                //    AVRIO.bmsio.SendData_304.byPCS_Heartbeat_305 = 0;
                //}

            }
            catch(Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
        public void Can_Recv()
        {

            RstrLog = "";
            RrxMsgCnt = 0;
            Ri = 0;
            RMode = RRTR = RDLC = 0;
            RCANID = RTH = RTL = 0;
            if (AVRIO.bmsio.b_CanThread == false)
            {
                return;
            }

            try
            {
                VCI_CAN_DotNET.VCI_SDK.VCI_Get_RxMsgCnt(CAN_No, ref RrxMsgCnt);
            }
            catch(Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

            //AVRIO.avrio.EventMsg = $"RrxMsgCnt {RrxMsgCnt}";

            ////cbs 2020.08.07 테스트용으로 임시로 추가 
            /// 양산 테스트시 CAN COVERTER CABLE 제거 테스트항목 떄문에 임시로 넣음
            /// 테스트 항목을 변경하든지 아래 주석해지든지 해야됨
            /// 이전 소스에는 케이블제거 관련하여 내용없음
            if (RrxMsgCnt == 65535)
            {
                //USB케이블을 뺴버리면 RrxMsgCnt 65535값을 업데이트 해버림
                RrxMsgCnt = 0;
            }

            if (RrxMsgCnt != 0)
            {
                AVRIO.bmsio.nCanArriveChk = 0;
                if (AVRIO.bmsio.bBMSCanCommErrorFlag == true)
                {
                    AVRIO.avrio.EventMsg = "[CHARGER <=> BATTERY BMS] Can Arrive signal Fail => Recv.";
                    if (!AVRIO.avrio.GridOnlyMode)
                        AVRIO.bmsio.bBMSCanCommErrorFlag = false;
                }
                for (Ri = 0; Ri < RrxMsgCnt; Ri++)
                {
                    try
                    {
                        RRet = VCI_CAN_DotNET.VCI_SDK.VCI_RecvCANMsg_NoStruct(CAN_No, ref RMode, ref RRTR, ref RDLC, ref RCANID, ref RTL, ref RTH, RReadData);
                        if (RRet != 0)
                        {
                            AVRIO.avrio.EventMsg = "Can open Error : " + ErrMsg[RRet];
                        }
                        else
                        {
                            switch (RCANID)
                            {
                                case 0x200:
                                    {
                                        byte[] byTemp = new byte[2];
                                        byTemp[0] = RReadData[1];
                                        byTemp[1] = RReadData[0];
                                        AVRIO.bmsio.nMaxCellVoltage = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[3];
                                        byTemp[1] = RReadData[2];
                                        AVRIO.bmsio.nMinCellVoltage = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[5];
                                        byTemp[1] = RReadData[4];
                                        AVRIO.bmsio.nAverageCellVoltage = BitConverter.ToUInt16(byTemp, 0);

                                        AVRIO.bmsio.byPCS_Heartbeat_200 = RReadData[7];
                                        RstrLog = "[RECV] CMD:0x200 ==== " + ByteArrayToHexString(RReadData);

                                    }
                                    break;
                                case 0x201:
                                    {
                                        byte[] byTemp = new byte[2];
                                        byTemp[0] = RReadData[1];
                                        byTemp[1] = RReadData[0];
                                        AVRIO.bmsio.nMaxTemperature = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[3];
                                        byTemp[1] = RReadData[2];
                                        AVRIO.bmsio.nMinTemperature = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[5];
                                        byTemp[1] = RReadData[4];
                                        AVRIO.bmsio.nAverageTemperature = BitConverter.ToUInt16(byTemp, 0);

                                        AVRIO.bmsio.byFanControl = RReadData[6];
                                        AVRIO.bmsio.byPCS_Heartbeat_201 = RReadData[7];
                                        RstrLog = RstrLog + ", [RECV] CMD:0x201 ==== " + ByteArrayToHexString(RReadData);
                                    }
                                    break;
                                case 0x202:
                                    {
                                        byte[] byTemp = new byte[2];
                                        byTemp[0] = RReadData[1];
                                        byTemp[1] = RReadData[0];
                                        AVRIO.bmsio.nChargePowerLimit = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[3];
                                        byTemp[1] = RReadData[2];
                                        AVRIO.bmsio.nDischargePowerLimit = BitConverter.ToUInt16(byTemp, 0);

                                        byTemp[0] = RReadData[5];
                                        byTemp[1] = RReadData[4];
                                        AVRIO.bmsio.n30MinuteDischargePowerLimit = BitConverter.ToUInt16(byTemp, 0);

                                        AVRIO.bmsio.bySOC = RReadData[6];

                                        //AVRIO.bmsio.bySOC = 0x34;
                                        // AVRIO.bmsio.bySOC = 0x55;
                                        AVRIO.bmsio.byPCS_Heartbeat_202 = RReadData[7];

                                        RstrLog = RstrLog + ", [RECV] CMD:0x202 ==== " + ByteArrayToHexString(RReadData);
                                    }
                                    break;

                                case 0x203:
                                    {
                                        bool bBatteryMcFlag = false;
                                        bool bWarnFlag = false;
                                        bool bFaultFlag = false;
                                        bool discharge = false;
                                        bool bCellVoltageFaultFlag = false;

                                        if ((RReadData[0] & 2) > 0)
                                        {
                                            AVRIO.bmsio.bLowTempWarnFlag = true;//LTW
                                            discharge = true;
                                            bBatteryMcFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bLowTempWarnFlag = false;//LTW
                                        }

                                        //cbs 2020.04.01 주석처리 Warning -> Fault 처리
                                        //if ((RReadData[0] & 4) > 0)
                                        //{
                                        //    AVRIO.bmsio.bHighTempWarnFlag = true;//HTW
                                        //    bWarnFlag = true;
                                        //    bBatteryMcFlag = true;
                                        //}
                                        //else
                                        //{
                                        //    AVRIO.bmsio.bHighTempWarnFlag = false;//HTW
                                        //}

                                        //cbs 2020.04.01  Warning -> Fault 처리
                                        if ((RReadData[0] & 4) > 0)
                                        {
                                            AVRIO.bmsio.bHighTempWarnFlag = true;//HTW
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bHighTempWarnFlag -> Fault";
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighTempWarnFlag = false;//HTW
                                        }

                                        if ((RReadData[0] & 8) > 0)
                                        {
                                            AVRIO.bmsio.bCellVoltageImbalanceWarnFlag = true;//CVIF
                                            bFaultFlag = true;
                                            bBatteryMcFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bCellVoltageImbalanceWarnFlag -> Fault";
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellVoltageImbalanceWarnFlag = false;
                                        }


                                        if ((RReadData[0] & 16) > 0)
                                        {
                                            AVRIO.bmsio.bCellLowVoltageWarnFlag = true;
                                            bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellLowVoltageWarnFlag = false;
                                        }


                                        if ((RReadData[0] & 32) > 0)
                                        {
                                            AVRIO.bmsio.bCellHighVoltageWarnFlag = true;
                                            discharge = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellHighVoltageWarnFlag = false;
                                        }


                                        if ((RReadData[0] & 64) > 0)
                                        {
                                            AVRIO.bmsio.bHighDischargeCurrentWarnFlag = true;
                                            bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighDischargeCurrentWarnFlag = false;
                                        }


                                        if ((RReadData[0] & 128) > 0)
                                        {
                                            AVRIO.bmsio.bHighChargeCurrentWarnFlag = true;
                                            bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighChargeCurrentWarnFlag = false;
                                        }

                                        if ((RReadData[1] & 1) > 0)
                                        {
                                            //  AVRIO.bmsio.bHighDischargePowerWarnFlag = true;
                                            //  bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighDischargePowerWarnFlag = false;
                                        }

                                        if ((RReadData[1] & 2) > 0)
                                        {
                                            // AVRIO.bmsio.bHighChargePowerWarnFlag = true;
                                            // bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighChargePowerWarnFlag = false;
                                        }

                                        if ((RReadData[1] & 8) > 0)
                                        {
                                            // AVRIO.bmsio.bPackVoltageDeviationWarnFlag = true;
                                            //bWarnFlag = true;
                                            //  bFaultFlag = true;
                                        }
                                        else
                                        {
                                            // AVRIO.bmsio.bPackVoltageDeviationWarnFlag = false;
                                        }

                                        if ((RReadData[1] & 16) > 0)
                                        {
                                            // AVRIO.bmsio.bSOCLowWarnFlag = true;
                                            // bWarnFlag = true;
                                        }
                                        else
                                        {
                                            // AVRIO.bmsio.bSOCLowWarnFlag = false;
                                        }

                                        if ((RReadData[1] & 32) > 0)
                                        {
                                           //   AVRIO.bmsio.bSOCHighWarnFlag = true;
                                            //  bWarnFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bSOCHighWarnFlag = false;
                                        }

                                        AVRIO.bmsio.bBMSWarnFlag = bWarnFlag;

                                        if ((RReadData[2] & 1) > 0)
                                        {
                                            AVRIO.bmsio.bInternalCommuLossFaultFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bInternalCommuLossFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bInternalCommuLossFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 2) > 0)
                                        {
                                        //    AVRIO.bmsio.bLowTempFaultFlag = true;
                                         //   bFaultFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bLowTempFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 4) > 0)
                                        {
                                            AVRIO.bmsio.bHighTempFaultFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bHighTempFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighTempFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 8) > 0)
                                        {
                                            AVRIO.bmsio.bCellVoltageImbalanceFaultFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bCellVoltageImbalanceFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellVoltageImbalanceFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 16) > 0)
                                        {
                                            AVRIO.bmsio.bCellLowVoltageFaultFlag = true;
                                            bCellVoltageFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bCellLowVoltageFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellLowVoltageFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 32) > 0)
                                        {
                                            AVRIO.bmsio.bCellHighVoltageFaultFlag = true;
                                            bCellVoltageFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bCellHighVoltageFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bCellHighVoltageFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 64) > 0)
                                        {
                                            AVRIO.bmsio.bHighDischargeCurrentFaultFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bHighDischargeCurrentFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighDischargeCurrentFaultFlag = false;
                                        }

                                        if ((RReadData[2] & 128) > 0)
                                        {
                                            AVRIO.bmsio.bHighChargeCurrentFaultFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bHighChargeCurrentFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighChargeCurrentFaultFlag = false;
                                        }

                                        if ((RReadData[3] & 1) > 0)
                                        {
                                            //  AVRIO.bmsio.bHighDischargePowerFaultFlag = true;
                                            // bFaultFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighDischargePowerFaultFlag = false;
                                        }

                                        if ((RReadData[3] & 2) > 0)
                                        {
                                            //  AVRIO.bmsio.bHighChargePowerFaultFlag = true;
                                            //  bFaultFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighChargePowerFaultFlag = false;
                                        }

                                        if ((RReadData[3] & 8) > 0)
                                        {
                                            // AVRIO.bmsio.bPackVoltageDeviationFaultFlag = true;
                                            //  bFaultFlag = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bPackVoltageDeviationFaultFlag = false;
                                        }

                                        if ((RReadData[3] & 16) > 0)
                                        {
                                            AVRIO.bmsio.bPrechargeFailFlag = true;
                                            bFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bPrechargeFailFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bPrechargeFailFlag = false;
                                        }

                                        if ((RReadData[4] & 1) > 0)
                                        {
                                            AVRIO.bmsio.bPrechargeContactorStatus = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bPrechargeContactorStatus = false;
                                        }

                                        if ((RReadData[4] & 2) > 0)
                                        {
                                            AVRIO.bmsio.bLowSideMainContactorStatus = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bLowSideMainContactorStatus = false;
                                        }

                                        if ((RReadData[4] & 4) > 0)
                                        {
                                            AVRIO.bmsio.bHighSideMainContactorStatus = true;
                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bHighSideMainContactorStatus = false;
                                        }

                                        //cbs 20200711 주석처리
                                        ////cbs soclow flag .LG BMS에서 읽힐때만 처리 하기로함 김철호 전무 구두로 협의                                    
                                        //if ((RReadData[4] & 16) > 0)  //FAULT CODE 315
                                        //{
                                        //    AVRIO.bmsio.bSOCLowFaultFlag = true;
                                        //    bFaultFlag = true;
                                        //    AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS SOC Low flag -> Fault";
                                        //}
                                        //else
                                        //{
                                        //    AVRIO.bmsio.bSOCLowFaultFlag = false;
                                        //}
                                        //cbs  20200711 SOC가 2%까지 떨져도 Flag가 오질않음                            
                                        if ((RReadData[4] & 16) > 0)  //FAULT CODE 315
                                        {
                                            //AVRIO.bmsio.bSOCLowFaultFlag = true;
                                            //bFaultFlag = true;
                                            //AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS SOC Low flag -> Fault";
                                        }
                                        else
                                        {
                                            //AVRIO.bmsio.bSOCLowFaultFlag = false;
                                        }


                                        if ((RReadData[4] & 32) > 0)
                                        {
                                            AVRIO.bmsio.bSOCHighFaultFlag = true;
                                            bCellVoltageFaultFlag = true;
                                            AVRIO.avrio.EventMsg = "[0x203] !!!Check BMS bSOCHighFaultFlag -> Fault";

                                        }
                                        else
                                        {
                                            AVRIO.bmsio.bSOCHighFaultFlag = false;
                                        }

                                        if (discharge)
                                        {
                                            AVRIO.avrio.DisChargeFalg = true;
                                            //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBMSBatteryDischarge;
                                        }
                                        else
                                            AVRIO.avrio.DisChargeFalg = false;


                                        if (bWarnFlag)
                                        {
                                            AVRIO.avrio.WarrningFalg = true;
                                            // AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsBMSBatteryWarning;
                                        }
                                        else
                                            AVRIO.avrio.WarrningFalg = false;

                                        AVRIO.bmsio.bBMSCellVoltageFaultFlag = bCellVoltageFaultFlag;
                                        AVRIO.bmsio.bBMSFaultFlag = bFaultFlag;
                                        AVRIO.bmsio.byBMSStatus = RReadData[5];
                                        AVRIO.bmsio.byBMSDataEnable = RReadData[6];
                                        AVRIO.bmsio.byPCS_Heartbeat_203 = RReadData[7];
                                        RstrLog = RstrLog + ", [RECV] CMD:0x203 ==== " + ByteArrayToHexString(RReadData);                                        
                                    }
                                    break;
                                case 0x204:
                                    {
                                        byte[] byTemp = new byte[2];
                                        byTemp[0] = RReadData[1];
                                        byTemp[1] = RReadData[0];
                                        AVRIO.bmsio.nChargeVoltageSetPoint = BitConverter.ToUInt16(byTemp, 0);

                                        AVRIO.bmsio.byRBMSSWVersion = RReadData[2];
                                        AVRIO.bmsio.byRBMSHWVersion = RReadData[3];

                                        AVRIO.bmsio.bySOCLowValueSetPoint = RReadData[4];
                                        AVRIO.bmsio.bySOCHighValueSetPoint = RReadData[5];
                                        AVRIO.bmsio.byPCS_Heartbeat_204 = RReadData[7];

                                        RstrLog = RstrLog + ", [RECV] CMD:0x204 ==== " + ByteArrayToHexString(RReadData);
                                        //   AVRIO.avrio.EventCanMsg = RstrLog;
                                    }
                                    break;

                                case 0x205:
                                    {

                                        AVRIO.bmsio.byMaxCellVoltageNumber = RReadData[0];
                                        AVRIO.bmsio.byMinCellVoltageNumber = RReadData[1];

                                        AVRIO.bmsio.byMaxModuleTemperatureNumber = RReadData[2];
                                        AVRIO.bmsio.byMinModuleTemperatureNumber = RReadData[3];


                                        RstrLog = RstrLog + ", [RECV] CMD:0x205 ==== " + ByteArrayToHexString(RReadData);
                                        AVRIO.avrio.EventCanMsg = RstrLog;
                                    }
                                    break;
                            
                            }
                        }


                        bSendRecieveWorking = true;
                        bSendRecieveWorking_1 = true;
                    }
                    catch (System.Exception ex)
                    {

                        AVRIO.avrio.EventMsg = "CAN_Recv thread : " + ex.ToString();
                        AVRIO.Log.Eventlog = ex.ToString();
                    }
                }
            }
            else
            {                
                if (!AVRIO.avrio.GridOnlyMode)
                {
                    AVRIO.bmsio.nCanArriveChk++;

                    if (AVRIO.bmsio.nCanArriveChk > 2)
                    {
                        AVRIO.bmsio.nCanArriveChk = 0;

                        if (AVRIO.bmsio.bBMSCanCommErrorFlag == false)
                        {
                            AVRIO.avrio.EventMsg = "[CHARGER <=> BATTERY BMS] Can Arrive signal Fail";

                            // 1. LG BMS 와의 통신이 끊겼을 경우 처리해야 할 예외 처리 부분
                            AVRIO.bmsio.bBMSCanCommErrorFlag = true;
                        }
                    }
                }
            }
        }

        #region MakeSendCanMsg
        public static byte[] MakeCanMsg_300()
        {   
            byte[] sendData = new byte[8];

            byte[] byData = new byte[2];
            byte[] byTemp = new byte[2];

            DateTime dt;
            dt = DateTime.Now;

            AVRIO.bmsio.SendData_300.nYear = Convert.ToUInt16(dt.Year);
            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_300.nYear);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 0, byTemp.Length);

            AVRIO.bmsio.SendData_300.byMonth = Convert.ToByte(dt.Month);
            sendData[2] = (byte)AVRIO.bmsio.SendData_300.byMonth;

            AVRIO.bmsio.SendData_300.byDay = Convert.ToByte(dt.Day);
            sendData[3] = (byte)AVRIO.bmsio.SendData_300.byDay;

            AVRIO.bmsio.SendData_300.byHour = Convert.ToByte(dt.Hour);
            sendData[4] = (byte)AVRIO.bmsio.SendData_300.byHour;

            AVRIO.bmsio.SendData_300.byMinute = Convert.ToByte(dt.Minute);
            sendData[5] = (byte)AVRIO.bmsio.SendData_300.byMinute;

            sendData[7] = (byte)AVRIO.bmsio.SendData_300.byPCS_Heartbeat_300;
            return sendData;
        }

        public static byte[] MakeCanMsg_301()
        {
            byte[] sendData = new byte[8];
            byte[] byData = new byte[2];
            byte[] byTemp = new byte[2];

            if (AVRIO.bmsio.SendData_301.nBatteryCurrent == 0)
            {
                AVRIO.bmsio.SendData_301.nBatteryCurrent = 3000;
            }
            else if (AVRIO.bmsio.SendData_301.nBatteryCurrent == 2999)
            {
                AVRIO.bmsio.SendData_301.nBatteryCurrent = 3000;
            }

            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_301.nBatteryCurrent);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 0, byTemp.Length);

            if (AVRIO.bmsio.SendData_301.nBatteryVoltage == 0)
            {
                AVRIO.bmsio.SendData_301.nBatteryVoltage = 22886;
            }
            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_301.nBatteryVoltage);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 2, byTemp.Length);

            sendData[7] = (byte)AVRIO.bmsio.SendData_301.byPCS_Heartbeat_301;
            return sendData;
        }

        public static byte[] MakeCanMsg_302()
        {
            byte[] sendData = new byte[8];
            byte[] byData = new byte[2];
            byte[] byTemp = new byte[2];

            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_302.nBatteryInstantaneousPower);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 0, byTemp.Length);

            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_302.nCarInstantaneousPower);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 2, byTemp.Length);

            byData = BitConverter.GetBytes(AVRIO.bmsio.SendData_302.nUPSModeInstantaneousPower);
            byTemp[0] = byData[1];
            byTemp[1] = byData[0];
            Array.Copy(byTemp, 0, sendData, 4, byTemp.Length);

            sendData[7] = (byte)AVRIO.bmsio.SendData_302.byPCS_Heartbeat_302;
            return sendData;
        }

        public static byte[] MakeCanMsg_303()
        {
            byte[] sendData = new byte[8];

            sendData[0] = AVRIO.bmsio.SendData_303.byPSC_Status;
            sendData[7] = AVRIO.bmsio.SendData_303.byPCS_Heartbeat_303;

            return sendData;
        }

        public static byte[] MakeCanMsg_304()
        {
            byte[] sendData = new byte[8];
            BitArray bits = new BitArray(8);
            byte[] byData = new byte[1];

            if (AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl > 0)
            {
                bits[4] = true;
            }
            if (AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl > 0)
            {
                bits[5] = true;
            }

            if (AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor > 0)
            {
                bits[6] = true;
            }
            else
            {
                bits[6] = false;
            }

            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 0;
            bits.CopyTo(byData, 0);
            sendData[0] = byData[0];

            sendData[7] = AVRIO.bmsio.SendData_304.byPCS_Heartbeat_304;
            return sendData;
        }
        #endregion

#region UTIL
        public static byte[] HexStringToByteArray(string Hex)
        {
            // Hex = "20110116010101";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:yyyyMMddhhmmss}", DateTime.Now);

            string[] str = sb.ToString().Split(',');

            byte[] Bytes = new byte[Hex.Length / 2];
            int[] HexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,  
                                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D,  
                                 0x0E, 0x0F };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
            {
                Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                    HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
            }
            
            return Bytes;
        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder();
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }
            return Result.ToString();
        }
        
        public static string ByteToHexString(byte _Byte)
        {
            StringBuilder Result = new StringBuilder();
            string HexAlphabet = "0123456789ABCDEF";
            Result.Append(HexAlphabet[(int)(_Byte >> 4)]);
            Result.Append(HexAlphabet[(int)(_Byte & 0xF)]);

            return Result.ToString();
        }
    }
#endregion
}
