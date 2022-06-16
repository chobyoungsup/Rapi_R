using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using System.IO.Ports;
using System.Threading;
using QuickChargeConfig;
using AVRIO;
using System.Timers;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections;
using Microsoft.Win32;
using System.Diagnostics;

//AVRIO.avrio.ChargePrice 충전금액
//AVRIO.avrio.ChargeWatt 충전 양

namespace CSOS
{

    public class Rapidas
    {

        [DllImport("kernel32.dll")]
        private static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        private static extern bool GetSystemTime(ref SYSTEMTIME lpSystemTime);

        string command12;
        string command34;
        string command56;
        string command78;
        string command910;
        string command1112;

        int CountCheck;
        public string modbusStatus;
        // int SelectCommand12 = 0;
        int SelectCommand34 = 0;
        int SelectCommand56 = 0;
        int SelectCommand78 = 0;
        int SelectCommand910 = 0;
        bool onemore = true;

        private SerialPort sp = new SerialPort();
        string portNum;
        string ftp_ip;
        string ftp_id;
        string ftp_pass;
        int boudrate;
        public byte[] sendData = new byte[1024]; // 보낼 데이타 저장
        public byte[] buffer = new byte[1024];   // 받은 데이타 저장

        OSVersionInfo _OSVersionInfo;
        string OSName;
        // private System.Timers.Timer timerrec;

        // public static string EventCSVMsg
        // public static string EventTXTsg
        // AVRIO.avrio.EventMsg = "[CHARGER <=> BATTERY BMS] CAN PORT OPEN FAIL";

        //private System.Timers.Timer timer;
        //private UInt16 nSeqNum = 0;

        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        private void SetTime(string i_Time)
        {
            try
            {
                SYSTEMTIME systime = new SYSTEMTIME();

                systime.wYear = (ushort)Convert.ToInt16("22");
                systime.wMonth = (ushort)Convert.ToInt16("22");
                systime.wDay = (ushort)Convert.ToInt16("22");
                systime.wHour = (ushort)Convert.ToInt16("22");
                systime.wMinute = (ushort)Convert.ToInt16("22");
                systime.wSecond = (ushort)Convert.ToInt16("22");
                systime.wHour = (ushort)(systime.wHour - 9);
                SetSystemTime(ref systime);
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }

        #region initial

        void avrio_SendCSOSOrder(CSOSCMD command, params object[] list)
        {

            SendCSOSCommand(command);
            return;
        }

        public Rapidas()
        {
            try
            {
                //RegistryKey reg;
                //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("CSOS");

                RegistryKey regKey;
                RegistryKey reg;
                if (Environment.Is64BitOperatingSystem)
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\CSOS", true);
                    AVRIO.avrio.EventMsg = "DSP Registry->64";
                }
                else
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\CSOS", true);
                    AVRIO.avrio.EventMsg = "DSP Registry->32";
                }

                portNum = Convert.ToString(reg.GetValue("RAPIDAS_PORT", "COM5"));
                boudrate = Convert.ToInt32(reg.GetValue("BAURATE", "19200"));

                AVRIO.avrio.SendCSOSOrder += new CSOSOrderEvent(avrio_SendCSOSOrder);


                AVRIO.avrio.EventMsg = $"JFE PortNum->{portNum}";
                AVRIO.avrio.EventMsg = $"JFE Boudrate->{boudrate}";

                OSVersionInfo _OSVersionInfo = OSVersionInfo.GetOSVersionInfo();
                OSName = _OSVersionInfo.Name;
                AVRIO.avrio.EventMsg = $"!!!Rapidas OSName = {OSName}";
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }

        private void ConfigInit()
        {

        }

        #endregion


        private int GetResponse(ref byte[] response)
        {
            int i = 0;
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (i = 0; i < response.Length; i++)
            {
                response[i] = (byte)(sp.ReadByte());
            }
            return i;
        }


        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;
                
                // >>> 20200401 SMJ  422 카드단말기 연결시 Rapidas(충전기) 에서 응답이 없을 경우가 있는데 이 경우 RtsEnable을 변경해서 사용함. true일때 리턴값이 들어옴.
                //sp.RtsEnable = true;
                
                // cbs 20200711 윈도우 버젼에 따라서 옵션설정 
                if(OSName == "Windows 10")
                    sp.RtsEnable = true; 
                else
                    sp.RtsEnable = false;

                AVRIO.avrio.EventMsg = $"JFE RTS Enable {sp.RtsEnable}";

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
                // modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                // modbusStatus = portName + " already opened";
                return false;
            }

        }

        public void PollFunction()
        {

            bool bThreadReady = false;
            int count = 0;

            //portNum = "COM2";


            while (!bThreadReady)
            {
                if (Open(portNum, boudrate, 8, Parity.None, StopBits.One) == false)
                {
                    AVRIO.avrio.EventMsg = "[( PAY ) PORT OPEN FAIL] SBC COM PORT OPEN FAIL!";
                    Thread.Sleep(3000);
                }
                else
                {
                    AVRIO.avrio.EventMsg = "[( PAY ) PORT OPEN] SBC COM PORT OPEN SUCCES!";
                    bThreadReady = true;
                }
            }

            AVRIO.avrio.IsRfpadWork = true;
            AVRIO.avrio.IsControlBDComm = true;

            while (bThreadReady)
            {
                byte[] response = new byte[64];

                Thread.Sleep(300);
                try
                {
                    count = GetResponse(ref response);
                }
                catch
                {
                    if (response[0] != 0x00)
                    {
                        StringBuilder hex = new StringBuilder(response.Length);
                        for (int ii = 0; ii < response.Length; ii++)
                            hex.Append(response[ii].ToString("X2"));
                        hex.ToString();
                        AVRIO.avrio.EventMsg = "[Rapidas REC]=> : " + hex.ToString();
                        hex = null;
                        byte[] Check = new byte[26];

                        Array.Copy(response, 1, Check, 0, Check.Length - 3);

                        int Checksum = CHECKSUM(Check, Check.Length);
                        byte crc1 = (byte)h2n((byte)((Checksum & 0x000000f0) >> 4));
                        byte crc2 = (byte)h2n((byte)(Checksum & 0x0000000f));
                        Check = null;
                        ReceiveOder(response);
                    }
                }
                response = null;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                buffer = Encoding.UTF8.GetBytes(indata);

                StringBuilder hex = new StringBuilder(buffer.Length);
                for (int ii = 0; ii < buffer.Length; ii++)
                    hex.Append(buffer[ii].ToString("X2"));
                hex.ToString();
                AVRIO.avrio.EventMsg = "[Rapidas REC]=> : " + hex.ToString();


                byte[] Check = new byte[26];

                Array.Copy(buffer, 1, Check, 0, Check.Length - 3);

                int Checksum = CHECKSUM(Check, Check.Length);
                byte crc1 = (byte)h2n((byte)((Checksum & 0x000000f0) >> 4));
                byte crc2 = (byte)h2n((byte)(Checksum & 0x0000000f));

            }
            catch { }
        }

        public void ReceiveOder(byte[] buff)
        {
            try
            {
                CountCheck = 0;

                if (0 == Find_STX(buff, 5) && buff.Length > 5)
                {
                    AVRIO.avrio.Choice4 = buff[1]; AVRIO.avrio.Choice3 = buff[2];
                    AVRIO.avrio.Choice2 = buff[3]; AVRIO.avrio.Choice1 = buff[4];

                    AVRIO.avrio.EventMsg = "Rapidas Receive : " + AVRIO.avrio.Choice4.ToString() + AVRIO.avrio.Choice3.ToString() + AVRIO.avrio.Choice2.ToString() + AVRIO.avrio.Choice1.ToString();

                    if (AVRIO.avrio.Choice1 == 0x30 && AVRIO.avrio.Choice2 == 0x30 && AVRIO.avrio.Choice3 == 0x30 && AVRIO.avrio.Choice4 == 0x30)
                    {

                        AVRIO.avrio.RapidasModFlag = 0;
                        Packet_Send();

                    }
                    else
                    {
                        if (AVRIO.avrio.Choice1 == 0x31)
                        {// 1의 자리 충전기 재기동 리부팅
                            AVRIO.avrio.EventMsg = "0001";
                            //cbs
                            Packet_Send();
                            System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");

                            //cbs 주석처리
                            //Process.Start("shutdown -r -f -t 0");
                            //Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice1 == 0x32)
                        {// 현재 시각 수정
                            if (AVRIO.avrio.RapidasModFlag != 1)
                            {
                                AVRIO.avrio.RapidasModFlag = 1;

                                byte[] timeyy = new byte[4];
                                byte[] timeMM = new byte[2];
                                byte[] timeDD = new byte[2];
                                byte[] timehh = new byte[2];
                                byte[] timemm = new byte[2];
                                byte[] timess = new byte[2];

                                timeyy[0] = buff[5]; timeyy[1] = buff[6];
                                timeyy[2] = buff[7]; timeyy[3] = buff[8];
                                timeMM[0] = buff[9]; timeMM[1] = buff[10];
                                timeDD[0] = buff[11]; timeDD[1] = buff[12];
                                timehh[0] = buff[13]; timehh[1] = buff[14];
                                timemm[0] = buff[15]; timemm[1] = buff[16];
                                timess[0] = buff[17]; timess[1] = buff[18];

                                try
                                {
                                    SYSTEMTIME systime = new SYSTEMTIME();
                                    systime.wYear = (ushort)Convert.ToInt16(BitConverter.ToString(timeyy));
                                    systime.wMonth = (ushort)Convert.ToInt16(BitConverter.ToString(timeMM));
                                    systime.wDay = (ushort)Convert.ToInt16(BitConverter.ToString(timeDD));
                                    systime.wHour = (ushort)Convert.ToInt16(BitConverter.ToString(timehh));
                                    systime.wMinute = (ushort)Convert.ToInt16(BitConverter.ToString(timemm));
                                    systime.wSecond = (ushort)Convert.ToInt16(BitConverter.ToString(timess));
                                    systime.wHour = (ushort)(systime.wHour - 9);
                                    SetSystemTime(ref systime);
                                }
                                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
                                Packet_Send();
                            }
                        }
                        else if (AVRIO.avrio.Choice1 == 0x34)
                        {// 아직 명확하지가 않음
                            if (AVRIO.avrio.RapidasModFlag != 9000)
                            {
                                AVRIO.avrio.RapidasModFlag = 9000;
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


                                AVRIO.avrio.Rapidascardcheck = 1;
                                reg.SetValue("CARDCHECK", 1);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CARDCHECK", "1");
                                AVRIO.avrio.Rapidaspassword = 0;
                                reg.SetValue("PASSWORD", 0);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "PASSWORD", "0");
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice2 == 0x31)
                        {// 10의 자리 중지중으로부터 대기 모드로 이행
                            //   AVRIO.avrio.EventMsg = "0010";

                            if (AVRIO.avrio.RapidasModFlag != 2)
                            {
                                AVRIO.avrio.RapidasModFlag = 2;
                                AVRIO.avrio.BussinessFalg = 1;
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice2 == 0x32)
                        {// 대기중으로 부터 중지 모드 이행
                            if (AVRIO.avrio.RapidasModFlag != 3)
                            {
                                AVRIO.avrio.RapidasModFlag = 3;
                                AVRIO.avrio.BussinessFalg = 2;
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice2 == 0x34)
                        {// 자립 운전모드로 이행(인증시스템 사용불가)
                            if (AVRIO.avrio.RapidasModFlag != 4)
                            {
                                AVRIO.avrio.RapidasModFlag = 4;
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

                                AVRIO.avrio.Rapidascardcheck = 0;
                                reg.SetValue("CARDCHECK", 0);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CARDCHECK", "0");
                                AVRIO.avrio.Rapidaspassword = 0;
                                reg.SetValue("PASSWORD", 0);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "PASSWORD", "0");
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice2 == 0x34)
                        {// 카드 운전모드로 이행(인증시스템 사용불가)
                            if (AVRIO.avrio.RapidasModFlag != 9000)
                            {
                                AVRIO.avrio.RapidasModFlag = 9000;
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

                                AVRIO.avrio.Rapidascardcheck = 1;
                                reg.SetValue("CARDCHECK", 1);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CARDCHECK", "1");
                                AVRIO.avrio.Rapidaspassword = 0;
                                reg.SetValue("PASSWORD", 0);
                                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "PASSWORD", "0");
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice2 == 0x38)
                        {//인증해제(충전허가)
                            if (AVRIO.avrio.Rapidascardcheck == 1)
                            {
                                if (AVRIO.avrio.RapidasModFlag != 5)
                                {
                                    AVRIO.avrio.RapidasModFlag = 5;
                                    //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleReady;
                                    if (!AVRIO.avrio.CardCheckFlag)
                                        AVRIO.avrio.CardCheckFlag = true;
                                }

                                Packet_Send();
                            }
                            else
                            {
                                Packet_Send1();
                                //   AVRIO.avrio.EventMsg = "0080";
                            }
                        }

                        if (AVRIO.avrio.Choice3 == 0x31)
                        {// 100의 자리  인증에러(미등록)
                            if (AVRIO.avrio.Rapidascardcheck == 1)
                            {
                                if (AVRIO.avrio.RapidasModFlag != 6)
                                {
                                    AVRIO.avrio.RapidasModFlag = 6;
                                    if (!AVRIO.avrio.CardCheckFlag4)
                                        AVRIO.avrio.CardCheckFlag4 = true;
                                }
                                //  AVRIO.avrio.EventMsg = "0100";
                                Packet_Send();
                            }
                            else
                            {
                                Packet_Send1();
                                //   AVRIO.avrio.EventMsg = "0100";
                            }
                        }
                        else if (AVRIO.avrio.Choice3 == 0x32)
                        {// 인증에러 (등록 없음)
                            if (AVRIO.avrio.Rapidascardcheck == 1)
                            {
                                if (AVRIO.avrio.RapidasModFlag != 7)
                                {
                                    AVRIO.avrio.RapidasModFlag = 7;
                                    if (!AVRIO.avrio.CardCheckFlag5)
                                        AVRIO.avrio.CardCheckFlag5 = true;
                                }
                                //   AVRIO.avrio.EventMsg = "0200";
                                Packet_Send();
                            }
                            else
                            {
                                Packet_Send1();
                                //   AVRIO.avrio.EventMsg = "0200";
                            }
                        }
                        else if (AVRIO.avrio.Choice3 == 0x34)
                        {// 인증에러 (예약완료)
                            if (AVRIO.avrio.Rapidascardcheck == 1)
                            {
                                if (AVRIO.avrio.RapidasModFlag != 8)
                                {
                                    AVRIO.avrio.RapidasModFlag = 8;
                                    if (!AVRIO.avrio.CardCheckFlag6)
                                        AVRIO.avrio.CardCheckFlag6 = true;
                                }

                                Packet_Send();
                            }
                            else
                            {

                                Packet_Send1();

                            }
                        }

                        if (AVRIO.avrio.Choice4 == 0x31)
                        {// 1000의 자리  충전시작(충전 스타트 버튼 누르면 같은 동작)
                            AVRIO.avrio.EventMsg = "1000";
                            if (AVRIO.avrio.RapidasModFlag != 9)
                            {
                                AVRIO.avrio.RapidasModFlag = 9;
                                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysReady)
                                {
                                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleStart;

                                }
                                else
                                    if (!AVRIO.avrio.StartButtonPlay)
                                    AVRIO.avrio.StartButtonPlay = true;
                            }
                            Packet_Send();
                        }
                        else if (AVRIO.avrio.Choice4 == 0x32)
                        {// 충전 멈춤(충전 멈춤 버튼 누르면 같은 동작)
                            //AVRIO.avrio.EventMsg = "2000";
                            if (AVRIO.avrio.RapidasModFlag != 10)
                            {
                                if (AVRIO.avrio.FualtstopFalg)
                                {
                                    AVRIO.avrio.Fualtstop = true;
                                }
                                else
                                {
                                    AVRIO.avrio.RapidasModFlag = 10;

                                    if (!AVRIO.avrio.StopButtonPlay)
                                        AVRIO.avrio.StopButtonPlay = true;
                                }
                            }
                            //  AVRIO.avrio.EventMsg = "[Rapidas SEND]=> : " + buff.ToString();
                            Packet_Send();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = "[Rapidas REC]=> : ERROR ";
                AVRIO.Log.Eventlog = err.ToString();
            }
        }

        public void SendCSOSCommand(CSOSCMD cmd)
        {
            //   CSOSCMD.CARDCheck

            AVRIO.avrio.EventMsg = "****JFE CMD UPDATE--> " + cmd.ToString() + "****";
            switch (cmd)
            {
                // 충전중 1-2 04
                case CSOSCMD.ChargeStart:
                    {
                        // SelectCommand12 = 3;
                    }
                    break;

                // 충전종료1-2 02
                case CSOSCMD.ChargeFinish:
                    {
                        // SelectCommand12 = 5;
                        AVRIO.avrio.nSelectCommand34 = 1;
                    }
                    break;

                // 정지 처리중 1-2 01
                case CSOSCMD.ChargeCancel:
                    // 없음
                    {
                        // SelectCommand12 = 2;
                    }
                    break;

                case CSOSCMD.DevCtrl:
                    {

                    }
                    break;
                //충전 대기중 1-2 10
                case CSOSCMD.ChargingStat:
                    {
                        AVRIO.avrio.nSelectCommand56 = 1;
                        AVRIO.avrio.nSelectCommand78 = 1;
                        AVRIO.avrio.nSelectCommand910 = 1;
                    }
                    break;

                // 중지중 StopPage 1-2 80
                case CSOSCMD.DevStop:
                    {
                        AVRIO.avrio.nSelectCommand34 = 5;

                    }
                    break;

                // 컨넥트 도어 해제중 3-4 01
                case CSOSCMD.ChargeCurrent:
                    {
                        // SelectCommand12 = 4;
                        AVRIO.avrio.nSelectCommand34 = 1;
                    }
                    break;

                case CSOSCMD.Chademo5608:
                    {
                        AVRIO.avrio.nSelectCommand56 = 4;

                    }
                    break;

                case CSOSCMD.Chademo5610:// = 0x52,
                    {
                        AVRIO.avrio.nSelectCommand56 = 5;
                    }
                    break;
                case CSOSCMD.Chademo5620:
                    {
                        AVRIO.avrio.nSelectCommand56 = 6;
                    }
                    break;


                case CSOSCMD.Chademo5640:// = 0x54,
                    {
                        AVRIO.avrio.nSelectCommand56 = 7;
                    }
                    break;
                case CSOSCMD.Chademo5680:// = 0x55,
                    {
                        AVRIO.avrio.nSelectCommand56 = 8;
                    }
                    break;

                case CSOSCMD.ChargeFualt7801:
                    {
                        AVRIO.avrio.nSelectCommand78 = 1;
                    }
                    break;
                case CSOSCMD.ChargeFualt7804:
                    {
                        AVRIO.avrio.nSelectCommand78 = 3;
                    }
                    break;
                case CSOSCMD.ChargeFualt7808:
                    {
                        AVRIO.avrio.nSelectCommand78 = 4;
                    }
                    break;

                case CSOSCMD.ChargeFualt7810:
                    {
                        AVRIO.avrio.nSelectCommand78 = 5;
                    }
                    break;

                case CSOSCMD.ChargeFualt7820:
                    {
                        AVRIO.avrio.nSelectCommand78 = 6;

                    }
                    break;

                case CSOSCMD.ChargeFualt7840:
                    {
                        AVRIO.avrio.nSelectCommand78 = 7;

                    }
                    break;

                case CSOSCMD.ChargeFualt7880:
                    {
                        AVRIO.avrio.nSelectCommand78 = 8;

                    }
                    break;

                case CSOSCMD.ChargeFualt0001:
                    {
                        AVRIO.avrio.nSelectCommand78 = 9;

                    }
                    break;

                case CSOSCMD.CarStop91002:
                    {
                        AVRIO.avrio.nSelectCommand910 = 2;
                    }
                    break;

                case CSOSCMD.CarStop91004:
                    {
                        AVRIO.avrio.nSelectCommand910 = 3;
                    }
                    break;
                case CSOSCMD.CarStop91008:
                    {
                        AVRIO.avrio.nSelectCommand910 = 4;
                    }
                    break;
                case CSOSCMD.CarStop91010:
                    {
                        AVRIO.avrio.nSelectCommand910 = 5;
                    }
                    break;
                case CSOSCMD.CarStop91020:
                    {
                        AVRIO.avrio.nSelectCommand910 = 6;
                    }
                    break;
                case CSOSCMD.CarStop91040:
                    {
                        AVRIO.avrio.nSelectCommand910 = 7;
                    }
                    break;
                case CSOSCMD.CarStop91080:
                    {
                        AVRIO.avrio.nSelectCommand910 = 8;
                    }
                    break;
            }
        }

        public void Packet_Send1()// Nak
        {

            try
            {

                byte[] sendData = new byte[128];
                byte[] Checksum = new byte[4];

                int i = 0;
                int a = 0;
                sendData[i++] = 0x02;

                Checksum[a++] = sendData[i++] = (byte)'R';
                Checksum[a++] = sendData[i++] = (byte)'P';
                Checksum[a++] = sendData[i++] = (byte)'N';
                Checksum[a++] = sendData[i++] = (byte)'G';

                int Check = CHECKSUM(Checksum, a);
                byte crc1 = (byte)((Check & 0x000000f0) >> 4);
                byte crc2 = (byte)(Check & 0x0000000f);

                sendData[i++] = (byte)h2n(crc1);
                sendData[i++] = (byte)h2n(crc2);

                sendData[i++] = 0x03;
                sp.Write(sendData, 0, i);
            }
            catch { }
        }

        //cbs 임시
        //public void Packet_Send2()// Rapidas  Start
        //{
        //    try
        //    {
        //        byte[] vdsend = new byte[1024];
        //        int i = 0;
        //        StringBuilder vData = new StringBuilder();
        //        String vsData = String.Empty;

        //        string Chargingtime = "";
        //        string Evsoc = "";
        //        string Inputelectricity = "";
        //        string Ouputelectricity = "";

        //        string newtime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        //        byte[] timetemp = new byte[14];

        //        timetemp = Encoding.UTF8.GetBytes(newtime);
        //        sendData[i++] = 0x02;
        //        sendData[i++] = timetemp[0]; sendData[i++] = timetemp[1]; sendData[i++] = timetemp[2];
        //        sendData[i++] = timetemp[3]; sendData[i++] = timetemp[4]; sendData[i++] = timetemp[5]; sendData[i++] = timetemp[6];
        //        sendData[i++] = timetemp[7]; sendData[i++] = timetemp[8]; sendData[i++] = timetemp[9]; sendData[i++] = timetemp[10];
        //        sendData[i++] = timetemp[11]; sendData[i++] = timetemp[12]; sendData[i++] = timetemp[13]; //sendData[i++] = timetemp[10];

        //        command12 = "20";

        //        command34 = "02";

        //        command56 = "01";

        //        command78 = "01";

        //        command910 = "01";

        //        Chargingtime = "00000000";
        //        Evsoc = "001";
        //        Inputelectricity = "00000000";
        //        // 두놈을 더한 수 인데 정확한지 모르 겠음
        //        Ouputelectricity = "00000000";


        //        /// LEE 명확한 조건이 필요 함
        //        // 수전 전력 또는 AVRIO.avrio.VehiclePower 작은놈?
        //        //  Inputelectricity = "000000"; Inputelectricity += (AVRIO.avrio.VehiclePower + AVRIO.avrio.BatteryPower).ToString();
        //        // 두놈을 더한 수 인데 정확한지 모르 겠음
        //        //  Ouputelectricity = "000000"; Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
        //        command1112 = "00";

        //        vData.Append(Evsoc)//"000"
        //            .Append(Chargingtime)//"0000 0000"
        //            .Append(Inputelectricity)//"0000 0000"
        //            .Append(Ouputelectricity)//"0000 0000"                
        //            .Append(command12)
        //            .Append(command34)
        //            .Append(command56)
        //            .Append(command78)
        //            .Append(command910)
        //            .Append(command1112);

        //        vsData = vData.ToString();
        //        vdsend = Encoding.UTF8.GetBytes(vsData);
        //        Array.Copy(vdsend, 0, sendData, i, vdsend.Length);
        //        int Checksum = CHECKSUM(sendData, i + vdsend.Length);
        //        byte crc1 = (byte)((Checksum & 0x000000f0) >> 4);
        //        byte crc2 = (byte)(Checksum & 0x0000000f);
        //        sendData[i + vdsend.Length] = (byte)h2n(crc1);
        //        sendData[i + vdsend.Length + 1] = (byte)h2n(crc2);
        //        sendData[i + vdsend.Length + 2] = 0x03;

        //        sp.Write(sendData, 0, ((i + vdsend.Length) + 3));

        //        StringBuilder hex = new StringBuilder(((i + vdsend.Length) + 3) * 2);
        //        for (int ii = 0; ii < ((i + vdsend.Length) + 2); ii++)
        //            hex.Append(sendData[ii].ToString("X2"));
        //        // hex.ToString();
        //        AVRIO.avrio.EventMsg = "[Rapidas SEND]=> : " + hex.ToString();
        //        command12 = "10";
        //    }
        //    catch { }
        //}

        public void Packet_Send()
        {
            try
            {
                byte[] vdsend = new byte[1024];
                int i = 0;
                StringBuilder vData = new StringBuilder();
                String vsData = String.Empty;
                int aaaaa = 0;
                string Chargingtime = "";
                string Evsoc = "";
                string Inputelectricity = "";
                string Ouputelectricity = "";

                string newtime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                byte[] timetemp = new byte[14];

                timetemp = Encoding.UTF8.GetBytes(newtime);
                sendData[i++] = 0x02;
                sendData[i++] = timetemp[0]; sendData[i++] = timetemp[1]; sendData[i++] = timetemp[2];
                sendData[i++] = timetemp[3]; sendData[i++] = timetemp[4]; sendData[i++] = timetemp[5]; sendData[i++] = timetemp[6];
                sendData[i++] = timetemp[7]; sendData[i++] = timetemp[8]; sendData[i++] = timetemp[9]; sendData[i++] = timetemp[10];
                sendData[i++] = timetemp[11]; sendData[i++] = timetemp[12]; sendData[i++] = timetemp[13]; //sendData[i++] = timetemp[10];

                if (AVRIO.avrio.nSelectCommand12 == 1) { command12 = "01"; }       //
                else if (AVRIO.avrio.nSelectCommand12 == 2) { command12 = "02"; }
                else if (AVRIO.avrio.nSelectCommand12 == 3) { command12 = "04"; }
                else if (AVRIO.avrio.nSelectCommand12 == 4) { command12 = "08"; }
                else if (AVRIO.avrio.nSelectCommand12 == 5) { command12 = "10"; }
                else if (AVRIO.avrio.nSelectCommand12 == 6) { command12 = "20"; }
                else if (AVRIO.avrio.nSelectCommand12 == 8) { command12 = "80"; }
                else { command12 = "40"; }

                // == AVRIO.SysStatus.SysStandby

                if (AVRIO.avrio.nSelectCommand34 == 1) { command34 = "01"; }
                else if (AVRIO.avrio.nSelectCommand34 == 2) { command34 = "02"; }
                else if (AVRIO.avrio.nSelectCommand34 == 3) { command34 = "04"; }
                else if (AVRIO.avrio.nSelectCommand34 == 4) { command34 = "08"; }
                else if (AVRIO.avrio.nSelectCommand34 == 5) { command34 = "10"; }
                else if (AVRIO.avrio.nSelectCommand34 == 6) { command34 = "20"; }
                else if (AVRIO.avrio.nSelectCommand34 == 7) { command34 = "40"; }
                else if (AVRIO.avrio.nSelectCommand34 == 8) { command34 = "80"; }
                else { command34 = "02"; }

                if (AVRIO.avrio.nSelectCommand56 == 2) { command56 = "02"; }
                else if (AVRIO.avrio.nSelectCommand56 == 3) { command56 = "04"; }
                else if (AVRIO.avrio.nSelectCommand56 == 4) { command56 = "08"; }
                else if (AVRIO.avrio.nSelectCommand56 == 5) { command56 = "10"; }
                else if (AVRIO.avrio.nSelectCommand56 == 6) { command56 = "20"; }
                else if (AVRIO.avrio.nSelectCommand56 == 7) { command56 = "40"; }
                else if (AVRIO.avrio.nSelectCommand56 == 8) { command56 = "80"; }
                else { command56 = "01"; }

                if (AVRIO.avrio.nSelectCommand78 == 2) { command78 = "02"; }
                else if (AVRIO.avrio.nSelectCommand78 == 3) { command78 = "04"; }
                else if (AVRIO.avrio.nSelectCommand78 == 4) { command78 = "08"; }
                else if (AVRIO.avrio.nSelectCommand78 == 5) { command78 = "10"; }
                else if (AVRIO.avrio.nSelectCommand78 == 6) { command78 = "20"; }
                else if (AVRIO.avrio.nSelectCommand78 == 7) { command78 = "40"; }
                else if (AVRIO.avrio.nSelectCommand78 == 8) { command78 = "80"; }
                else if (AVRIO.avrio.nSelectCommand78 == 9) { command78 = "04"; }

                else { command78 = "01"; }

                if (AVRIO.avrio.nSelectCommand910 == 2) { command910 = "02"; }
                else if (AVRIO.avrio.nSelectCommand910 == 3) { command910 = "04"; }
                else if (AVRIO.avrio.nSelectCommand910 == 4) { command910 = "08"; }
                else if (AVRIO.avrio.nSelectCommand910 == 5) { command910 = "10"; }
                else if (AVRIO.avrio.nSelectCommand910 == 6) { command910 = "20"; }
                else if (AVRIO.avrio.nSelectCommand910 == 7) { command910 = "40"; }
                else if (AVRIO.avrio.nSelectCommand910 == 8) { command910 = "80"; }
                else { command910 = "01"; }


                if (AVRIO.avrio.CurrentStatus == AVRIO.SysStatus.SysRunning)
                {

                    if (AVRIO.avrio.ChargeRemainTimess != 255)
                    {
                        AVRIO.avrio.ChargeRemainTime = (ushort)((AVRIO.avrio.ChargeRemainTimess * 10) / 60) + 1;
                    }
                    else
                    {
                        AVRIO.avrio.ChargeRemainTime = AVRIO.avrio.ChargeRemainTimemm;
                    }

                    aaaaa = AVRIO.avrio.ChargeRemainTimemm * 60;

                    //if (aaaaa > 0 && aaaaa < 10)
                    //{
                    //    Chargingtime = "00000000"; //Chargingtime += (AVRIO.avrio.ChargeRemainTimemm * 60).ToString();
                    //}
                    //else if (aaaaa >= 10 && aaaaa < 100)
                    //{
                    //    Chargingtime = "000000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    //}
                    //else if (aaaaa >= 100 && aaaaa < 1000)
                    //{
                    //    Chargingtime = "00000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    //}
                    //else if (aaaaa >= 1000 && aaaaa < 10000)
                    //{
                    //    Chargingtime = "0000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    //}
                    //else if (aaaaa >= 10000 && aaaaa < 100000)
                    //{
                    //    Chargingtime = "000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    //}
                    //else
                    //{
                    //    Chargingtime = "00000000"; //Chargingtime += (AVRIO.avrio.ChargeRemainTimemm * 60).ToString();
                    //}

                    Chargingtime = "";
                    if (aaaaa > 0 && aaaaa < 10)
                    {
                        Chargingtime = "00000000"; //Chargingtime += (AVRIO.avrio.ChargeRemainTimemm * 60).ToString();
                    }
                    else if (aaaaa >= 10 && aaaaa < 100)
                    {
                        Chargingtime = "000000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 100 && aaaaa < 1000)
                    {
                        Chargingtime = "00000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 1000 && aaaaa < 10000)
                    {
                        Chargingtime = "0000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 10000 && aaaaa < 100000)
                    {
                        Chargingtime = "000"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 100000 && aaaaa < 1000000)
                    {
                        Chargingtime = "00"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 100000 && aaaaa < 1000000)
                    {
                        Chargingtime = "0"; Chargingtime += (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else if (aaaaa >= 1000000 && aaaaa < 10000000)
                    {
                        Chargingtime = (AVRIO.avrio.ChargeRemainTime * 60).ToString();
                    }
                    else
                    {
                        Chargingtime = "00000000"; //Chargingtime += (AVRIO.avrio.ChargeRemainTimemm * 60).ToString();
                    }


                    int tempSoc = Convert.ToInt32(AVRIO.avrio.ChargeSOC);

                    if (tempSoc < 10)
                    {
                        Evsoc = "00"; Evsoc += tempSoc.ToString();
                    }
                    else
                    {
                        Evsoc = "0"; Evsoc += tempSoc.ToString();
                    }
                }
                else
                {
                    Chargingtime = "00000000";
                    Evsoc = "001";
                }

                /// LEE 명확한 조건이 필요 함
                // 수전 전력 또는 AVRIO.avrio.VehiclePower 작은놈?
                Inputelectricity = "FFFFFFFF"; //Inputelectricity += (AVRIO.avrio.VehiclePower + AVRIO.avrio.BatteryPower).ToString();
                // 두놈을 더한 수 인데 정확한지 모르 겠음

                AVRIO.avrio.RapidasOutputw = AVRIO.avrio.RapidasOutputw / 10;
                if (AVRIO.avrio.RapidasOutputw < 10)
                {
                    Ouputelectricity = "0000000"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 100)
                {
                    Ouputelectricity = "000000"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 1000)
                {
                    Ouputelectricity = "00000"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 10000)
                {
                    Ouputelectricity = "0000"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 100000)
                {
                    Ouputelectricity = "000"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 1000000)
                {
                    Ouputelectricity = "00"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else if (AVRIO.avrio.RapidasOutputw < 10000000)
                {
                    Ouputelectricity = "0"; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                else
                {
                    Ouputelectricity = ""; Ouputelectricity += AVRIO.avrio.RapidasOutputw.ToString();   //Inputelectricity += (AVRIO.avrio.GridInputPowerMax).ToString();
                }
                command1112 = "00";


                vData.Append(Evsoc)//"000"
                    .Append(Chargingtime)//"0000 0000"
                    .Append(Inputelectricity)//"0000 0000"
                    .Append(Ouputelectricity)//"0000 0000"                
                    .Append(command12)
                    .Append(command34)
                    .Append(command56)
                    .Append(command78)
                    .Append(command910)
                    .Append(command1112);

                vsData = vData.ToString();
                vdsend = Encoding.UTF8.GetBytes(vsData);


                Array.Copy(vdsend, 0, sendData, i, vdsend.Length);

                byte[] Check = new byte[100];
                Array.Copy(sendData, 1, Check, 0, i + vdsend.Length);


                int Checksum = CHECKSUM(Check, i - 1 + vdsend.Length);
                byte crc1 = (byte)((Checksum & 0x000000f0) >> 4);
                byte crc2 = (byte)(Checksum & 0x0000000f);
                sendData[i + vdsend.Length] = (byte)h2n(crc1);
                sendData[i + vdsend.Length + 1] = (byte)h2n(crc2);
                sendData[i + vdsend.Length + 2] = 0x03;
                sp.Write(sendData, 0, ((i + vdsend.Length) + 3));

                AVRIO.avrio.EventMsg = "[Rapidas SEND]=> : " + vsData;

            }
            catch { AVRIO.avrio.EventMsg = "[Rapidas SEND]=> : ERROR "; }
        }

        private byte h2n(byte c)
        {
            if (0x00 <= c && c <= 0x09) return (byte)(c + 0x30);
            else
            {
                return (byte)((byte)(c + 0x41) - 0x0A);
            }

        }

        public int Find_STX(byte[] Dat, int Sz)
        {
            try
            {
                for (int i = 0; i < Sz; i++)
                {	// Find STX and Command code 
                    if (Dat[i] == 0x02)
                    {// 
                        return (i);
                    }
                }
            }
            catch { }
            return (-1);
        }

        public int CHECKSUM(byte[] _bytBuffer, int datacnt)
        {
            int checksum = 0x00;
            for (int i = 0; i < datacnt; ++i)
            {
                checksum += _bytBuffer[i];
            }
            return checksum;
        }
    }
    #region ftp class

    //private void timer_RecCheck(object sender, ElapsedEventArgs e)
    //   {
    //       FtpClient ftp = new FtpClient(ftp_ip, ftp_id, ftp_pass);
    //       ftp.Login();
    //       ftp.Upload(@"D:\ttt\qqq\ttt.log");
    //       ftp.Close();

    //   }

    public class FtpClient
    {
        public class FtpException : Exception
        {
            public FtpException(string message) : base(message) { }
            public FtpException(string message, Exception innerException) : base(message, innerException) { }
        }

        private static int BUFFER_SIZE = 512;
        private static Encoding ASCII = Encoding.ASCII;

        private bool verboseDebugging = false;

        // defaults
        public string server = "192.168.0.16";
        private string remotePath = ".";
        public string username = "anonymous";
        public string password = "anonymous@anonymous.net";
        private string message = null;
        private string result = null;

        private int port = 21;
        private int bytes = 0;
        private int resultCode = 0;

        private bool loggedin = false;
        private bool binMode = false;

        private Byte[] buffer = new Byte[BUFFER_SIZE];
        private Socket clientSocket = null;

        private int timeoutSeconds = 10;

        /// <summary>
        /// Default contructor
        /// </summary>
        public FtpClient()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public FtpClient(string server, string username, string password)
        {
            this.server = server;
            this.username = username;
            this.password = password;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="port"></param>
        public FtpClient(string server, string username, string password, int timeoutSeconds, int port)
        {
            this.server = server;
            this.username = username;
            this.password = password;
            this.timeoutSeconds = timeoutSeconds;
            this.port = port;
        }

        /// <summary>
        /// Display all communications to the debug log
        /// </summary>
        public bool VerboseDebugging
        {
            get
            {
                return this.verboseDebugging;
            }
            set
            {
                this.verboseDebugging = value;
            }
        }
        /// <summary>
        /// Remote server port. Typically TCP 21
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        /// <summary>
        /// Timeout waiting for a response from server, in seconds.
        /// </summary>
        public int Timeout
        {
            get
            {
                return this.timeoutSeconds;
            }
            set
            {
                this.timeoutSeconds = value;
            }
        }
        /// <summary>
        /// Gets and Sets the name of the FTP server.
        /// </summary>
        /// <returns></returns>
        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }
        /// <summary>
        /// Gets and Sets the port number.
        /// </summary>
        /// <returns></returns>
        public int RemotePort
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        /// <summary>
        /// GetS and Sets the remote directory.
        /// </summary>
        public string RemotePath
        {
            get
            {
                return this.remotePath;
            }
            set
            {
                this.remotePath = value;
            }

        }
        /// <summary>
        /// Gets and Sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }
        /// <summary>
        /// Gets and Set the password.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// If the value of mode is true, set binary mode for downloads, else, Ascii mode.
        /// </summary>
        public bool BinaryMode
        {
            get
            {
                return this.binMode;
            }
            set
            {
                if (this.binMode == value) return;

                if (value)
                    sendCommand("TYPE I");

                else
                    sendCommand("TYPE A");

                if (this.resultCode != 200) throw new FtpException(result.Substring(4));
            }
        }
        /// <summary>
        /// Login to the remote server.
        /// </summary>
        public void Login()
        {
            if (this.loggedin) this.Close();

            //  //Debug.WriteLine("Opening connection to " + this.server, "FtpClient");

            IPAddress addr = null;
            IPEndPoint ep = null;

            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                addr = Dns.Resolve(this.server).AddressList[0];
                ep = new IPEndPoint(addr, this.port);
                this.clientSocket.Connect(ep);
            }
            catch (Exception ex)
            {
                // doubtfull
                // if (this.clientSocket != null && this.clientSocket.Connected) this.clientSocket.Close();

                //  throw new FtpException("Couldn't connect to remote server", ex);
            }

            this.readResponse();

            if (this.resultCode != 220)
            {
                this.Close();
                throw new FtpException(this.result.Substring(4));
            }

            this.sendCommand("USER " + username);

            if (!(this.resultCode == 331 || this.resultCode == 230))
            {
                this.cleanup();
                throw new FtpException(this.result.Substring(4));
            }

            if (this.resultCode != 230)
            {
                this.sendCommand("PASS " + password);

                if (!(this.resultCode == 230 || this.resultCode == 202))
                {
                    this.cleanup();
                    throw new FtpException(this.result.Substring(4));
                }
            }

            this.loggedin = true;

            //Debug.WriteLine("Connected to " + this.server, "FtpClient");

            this.ChangeDir(this.remotePath);
        }

        /// <summary>
        /// Close the FTP connection.
        /// </summary>
        public void Close()
        {
            //Debug.WriteLine("Closing connection to " + this.server, "FtpClient");

            if (this.clientSocket != null)
            {
                this.sendCommand("QUIT");
            }

            this.cleanup();
        }

        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList()
        {
            return this.GetFileList("*.*");
        }

        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public string[] GetFileList(string mask)
        {
            if (!this.loggedin) this.Login();

            Socket cSocket = createDataSocket();

            this.sendCommand("NLST " + mask);

            if (!(this.resultCode == 150 || this.resultCode == 125)) throw new FtpException(this.result.Substring(4));

            this.message = "";

            DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

            while (timeout > DateTime.Now)
            {
                int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                this.message += ASCII.GetString(buffer, 0, bytes);

                if (bytes < this.buffer.Length) break;
            }

            string[] msg = this.message.Replace("\r", "").Split('\n');

            cSocket.Close();

            if (this.message.IndexOf("No such file or directory") != -1)
                msg = new string[] { };

            this.readResponse();

            if (this.resultCode != 226)
                msg = new string[] { };
            //	throw new FtpException(result.Substring(4));

            return msg;
        }

        /// <summary>
        /// Return the size of a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public long GetFileSize(string fileName)
        {
            if (!this.loggedin) this.Login();

            this.sendCommand("SIZE " + fileName);
            long size = 0;

            if (this.resultCode == 213)
                size = long.Parse(this.result.Substring(4));

            else
                throw new FtpException(this.result.Substring(4));

            return size;
        }


        /// <summary>
        /// Download a file to the Assembly's local directory,
        /// keeping the same file name.
        /// </summary>
        /// <param name="remFileName"></param>
        public void Download(string remFileName)
        {
            this.Download(remFileName, "", false);
        }

        /// <summary>
        /// Download a remote file to the Assembly's local directory,
        /// keeping the same file name, and set the resume flag.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="resume"></param>
        public void Download(string remFileName, Boolean resume)
        {
            this.Download(remFileName, "", resume);
        }

        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path. The local file name will be created or overwritten,
        /// but the path must exist.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="locFileName"></param>
        public void Download(string remFileName, string locFileName)
        {
            this.Download(remFileName, locFileName, false);
        }

        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path, and set the resume flag. The local file name will be
        /// created or overwritten, but the path must exist.
        /// </summary>
        /// <param name="remFileName"></param>
        /// <param name="locFileName"></param>
        /// <param name="resume"></param>
        public void Download(string remFileName, string locFileName, Boolean resume)
        {
            if (!this.loggedin) this.Login();

            this.BinaryMode = true;

            //Debug.WriteLine("Downloading file " + remFileName + " from " + server + "/" + remotePath, "FtpClient");

            if (locFileName.Equals(""))
            {
                locFileName = remFileName;
            }

            FileStream output = null;

            if (!File.Exists(locFileName))
                output = File.Create(locFileName);

            else
                output = new FileStream(locFileName, FileMode.Open);

            Socket cSocket = createDataSocket();

            long offset = 0;

            if (resume)
            {
                offset = output.Length;

                if (offset > 0)
                {
                    this.sendCommand("REST " + offset);
                    if (this.resultCode != 350)
                    {
                        //Server dosnt support resuming
                        offset = 0;
                        //Debug.WriteLine("Resuming not supported:" + result.Substring(4), "FtpClient");
                    }
                    else
                    {
                        //Debug.WriteLine("Resuming at offset " + offset, "FtpClient");
                        output.Seek(offset, SeekOrigin.Begin);
                    }
                }
            }

            this.sendCommand("RETR " + remFileName);

            if (this.resultCode != 150 && this.resultCode != 125)
            {
                throw new FtpException(this.result.Substring(4));
            }

            DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

            while (timeout > DateTime.Now)
            {
                this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
                output.Write(this.buffer, 0, this.bytes);

                if (this.bytes <= 0)
                {
                    break;
                }
            }

            output.Close();

            if (cSocket.Connected) cSocket.Close();

            this.readResponse();

            if (this.resultCode != 226 && this.resultCode != 250)
                throw new FtpException(this.result.Substring(4));
        }


        /// <summary>
        /// Upload a file.
        /// </summary>
        /// <param name="fileName"></param>
        public void Upload(string fileName)
        {
            this.Upload(fileName, false);
        }


        /// <summary>
        /// Upload a file and set the resume flag.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resume"></param>
        public void Upload(string fileName, bool resume)
        {
            if (!this.loggedin) this.Login();

            Socket cSocket = null;
            long offset = 0;

            if (resume)
            {
                try
                {
                    this.BinaryMode = true;

                    offset = GetFileSize(Path.GetFileName(fileName));
                }
                catch (Exception)
                {
                    // file not exist
                    offset = 0;
                }
            }

            // open stream to read file
            FileStream input = new FileStream(fileName, FileMode.Open);

            if (resume && input.Length < offset)
            {
                // different file size
                //Debug.WriteLine("Overwriting " + fileName, "FtpClient");
                offset = 0;
            }
            else if (resume && input.Length == offset)
            {
                // file done
                input.Close();
                //Debug.WriteLine("Skipping completed " + fileName + " - turn resume off to not detect.", "FtpClient");
                return;
            }

            // dont create untill we know that we need it
            cSocket = this.createDataSocket();

            if (offset > 0)
            {
                this.sendCommand("REST " + offset);
                if (this.resultCode != 350)
                {
                    //Debug.WriteLine("Resuming not supported", "FtpClient");
                    offset = 0;
                }
            }

            this.sendCommand("STOR " + Path.GetFileName(fileName));

            if (this.resultCode != 125 && this.resultCode != 150) throw new FtpException(result.Substring(4));

            if (offset != 0)
            {
                //Debug.WriteLine("Resuming at offset " + offset, "FtpClient");

                input.Seek(offset, SeekOrigin.Begin);
            }

            //Debug.WriteLine("Uploading file " + fileName + " to " + remotePath, "FtpClient");

            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                cSocket.Send(buffer, bytes, 0);
            }

            input.Close();

            if (cSocket.Connected)
            {
                cSocket.Close();
            }

            this.readResponse();

            if (this.resultCode != 226 && this.resultCode != 250) throw new FtpException(this.result.Substring(4));
        }

        /// <summary>
        /// Upload a directory and its file contents
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recurse">Whether to recurse sub directories</param>
        public void UploadDirectory(string path, bool recurse)
        {
            this.UploadDirectory(path, recurse, "*.*");
        }

        /// <summary>
        /// Upload a directory and its file contents
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recurse">Whether to recurse sub directories</param>
        /// <param name="mask">Only upload files of the given mask - everything is '*.*'</param>
        public void UploadDirectory(string path, bool recurse, string mask)
        {
            string[] dirs = path.Replace("/", @"\").Split('\\');
            string rootDir = dirs[dirs.Length - 1];

            // make the root dir if it doed not exist
            if (this.GetFileList(rootDir).Length < 1) this.MakeDir(rootDir);

            this.ChangeDir(rootDir);

            foreach (string file in Directory.GetFiles(path, mask))
            {
                this.Upload(file, true);
            }
            if (recurse)
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    this.UploadDirectory(directory, recurse, mask);
                }
            }

            this.ChangeDir("..");
        }

        /// <summary>
        /// Delete a file from the remote FTP server.
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFile(string fileName)
        {
            if (!this.loggedin) this.Login();

            this.sendCommand("DELE " + fileName);

            if (this.resultCode != 250) throw new FtpException(this.result.Substring(4));

            //Debug.WriteLine("Deleted file " + fileName, "FtpClient");
        }

        /// <summary>
        /// Rename a file on the remote FTP server.
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        /// <param name="overwrite">setting to false will throw exception if it exists</param>
        public void RenameFile(string oldFileName, string newFileName, bool overwrite)
        {
            if (!this.loggedin) this.Login();

            this.sendCommand("RNFR " + oldFileName);

            if (this.resultCode != 350) throw new FtpException(this.result.Substring(4));

            if (!overwrite && this.GetFileList(newFileName).Length > 0) throw new FtpException("File already exists");

            this.sendCommand("RNTO " + newFileName);

            if (this.resultCode != 250) throw new FtpException(this.result.Substring(4));

            //Debug.WriteLine("Renamed file " + oldFileName + " to " + newFileName, "FtpClient");
        }

        /// <summary>
        /// Create a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            if (!this.loggedin) this.Login();

            this.sendCommand("MKD " + dirName);

            if (this.resultCode != 250 && this.resultCode != 257) throw new FtpException(this.result.Substring(4));

            //Debug.WriteLine("Created directory " + dirName, "FtpClient");
        }

        /// <summary>
        /// Delete a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public void RemoveDir(string dirName)
        {
            if (!this.loggedin) this.Login();

            this.sendCommand("RMD " + dirName);

            if (this.resultCode != 250) throw new FtpException(this.result.Substring(4));

            //Debug.WriteLine("Removed directory " + dirName, "FtpClient");
        }

        /// <summary>
        /// Change the current working directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName"></param>
        public void ChangeDir(string dirName)
        {
            if (dirName == null || dirName.Equals(".") || dirName.Length == 0)
            {
                return;
            }

            if (!this.loggedin) this.Login();

            this.sendCommand("CWD " + dirName);

            if (this.resultCode != 250) throw new FtpException(result.Substring(4));

            this.sendCommand("PWD");

            if (this.resultCode != 257) throw new FtpException(result.Substring(4));

            // gonna have to do better than this....
            this.remotePath = this.message.Split('"')[1];

            //Debug.WriteLine("Current directory is " + this.remotePath, "FtpClient");
        }

        /// <summary>
        /// 
        /// </summary>
        private void readResponse()
        {
            this.message = "";
            this.result = this.readLine();

            if (this.result.Length > 3)
                this.resultCode = int.Parse(this.result.Substring(0, 3));
            else
                this.result = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string readLine()
        {
            while (true)
            {
                try
                {
                    this.bytes = clientSocket.Receive(this.buffer, this.buffer.Length, 0);
                    this.message += ASCII.GetString(this.buffer, 0, this.bytes);


                    if (this.bytes < this.buffer.Length)
                    {
                        break;
                    }
                }
                catch { }
            }

            string[] msg = this.message.Split('\n');

            if (this.message.Length > 2)
                this.message = msg[msg.Length - 2];

            else
                this.message = msg[0];


            if (this.message.Length > 4 && !this.message.Substring(3, 1).Equals(" ")) return this.readLine();

            if (this.verboseDebugging)
            {
                for (int i = 0; i < msg.Length - 1; i++)
                {
                    //  Debug.Write(msg[i], "FtpClient");
                }
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void sendCommand(String command)
        {
            if (this.verboseDebugging) ; //Debug.WriteLine(command, "FtpClient");

            Byte[] cmdBytes = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
            clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            this.readResponse();
        }

        /// <summary>
        /// when doing data transfers, we need to open another socket for it.
        /// </summary>
        /// <returns>Connected socket</returns>
        private Socket createDataSocket()
        {
            this.sendCommand("PASV");

            if (this.resultCode != 227) throw new FtpException(this.result.Substring(4));

            int index1 = this.result.IndexOf('(');
            int index2 = this.result.IndexOf(')');

            string ipData = this.result.Substring(index1 + 1, index2 - index1 - 1);

            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = "";

            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = char.Parse(ipData.Substring(i, 1));

                if (char.IsDigit(ch))
                    buf += ch;

                else if (ch != ',')
                    throw new FtpException("Malformed PASV result: " + result);

                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = int.Parse(buf);
                        buf = "";
                    }
                    catch (Exception ex)
                    {
                        throw new FtpException("Malformed PASV result (not supported?): " + this.result, ex);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];

            int port = (parts[4] << 8) + parts[5];

            Socket socket = null;
            IPEndPoint ep = null;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
                socket.Connect(ep);
            }
            catch (Exception ex)
            {
                // doubtfull....
                if (socket != null && socket.Connected) socket.Close();

                throw new FtpException("Can't connect to remote server", ex);
            }

            return socket;
        }

        /// <summary>
        /// Always release those sockets.
        /// </summary>
        private void cleanup()
        {
            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
                this.clientSocket = null;
            }
            this.loggedin = false;
        }

        /// <summary>
        /// Destuctor
        /// </summary>
        ~FtpClient()
        {
            this.cleanup();
        }


        /************************************************************************************************************/


        /*
				WinInetApi.FtpClient ftp = new WinInetApi.FtpClient();

				MethodInfo[] methods = ftp.GetType().GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.Public);

				foreach ( MethodInfo method in methods )
				{
					string param = "";
					string values = "";
					foreach ( ParameterInfo i in  method.GetParameters() )
					{
						param += i.ParameterType.Name + " " + i.Name + ",";
						values += i.Name + ",";
					}
					

					//Debug.WriteLine("private delegate " + method.ReturnType.Name + " " + method.Name + "Callback(" + param.TrimEnd(',') + ");");

					//Debug.WriteLine("public System.IAsyncResult Begin" + method.Name + "( " + param + " System.AsyncCallback callback )");
					//Debug.WriteLine("{");
					//Debug.WriteLine("" + method.Name + "Callback ftpCallback = new " + method.Name + "Callback(" + values + " this." + method.Name + ");");
					//Debug.WriteLine("return ftpCallback.BeginInvoke(callback, null);");
					//Debug.WriteLine("}");
					//Debug.WriteLine("public void End" + method.Name + "(System.IAsyncResult asyncResult)");
					//Debug.WriteLine("{");
					//Debug.WriteLine(method.Name + "Callback fc = (" + method.Name + "Callback) ((AsyncResult)asyncResult).AsyncDelegate;");
					//Debug.WriteLine("fc.EndInvoke(asyncResult);");
					//Debug.WriteLine("}");
					////Debug.WriteLine(method);
				}
*/


        private delegate void LoginCallback();
        public System.IAsyncResult BeginLogin(System.AsyncCallback callback)
        {
            LoginCallback ftpCallback = new LoginCallback(this.Login);
            return ftpCallback.BeginInvoke(callback, null);
        }
        private delegate void CloseCallback();
        public System.IAsyncResult BeginClose(System.AsyncCallback callback)
        {
            CloseCallback ftpCallback = new CloseCallback(this.Close);
            return ftpCallback.BeginInvoke(callback, null);
        }
        private delegate String[] GetFileListCallback();
        public System.IAsyncResult BeginGetFileList(System.AsyncCallback callback)
        {
            GetFileListCallback ftpCallback = new GetFileListCallback(this.GetFileList);
            return ftpCallback.BeginInvoke(callback, null);
        }
        private delegate String[] GetFileListMaskCallback(String mask);
        public System.IAsyncResult BeginGetFileList(String mask, System.AsyncCallback callback)
        {
            GetFileListMaskCallback ftpCallback = new GetFileListMaskCallback(this.GetFileList);
            return ftpCallback.BeginInvoke(mask, callback, null);
        }
        private delegate Int64 GetFileSizeCallback(String fileName);
        public System.IAsyncResult BeginGetFileSize(String fileName, System.AsyncCallback callback)
        {
            GetFileSizeCallback ftpCallback = new GetFileSizeCallback(this.GetFileSize);
            return ftpCallback.BeginInvoke(fileName, callback, null);
        }
        private delegate void DownloadCallback(String remFileName);
        public System.IAsyncResult BeginDownload(String remFileName, System.AsyncCallback callback)
        {
            DownloadCallback ftpCallback = new DownloadCallback(this.Download);
            return ftpCallback.BeginInvoke(remFileName, callback, null);
        }
        private delegate void DownloadFileNameResumeCallback(String remFileName, Boolean resume);
        public System.IAsyncResult BeginDownload(String remFileName, Boolean resume, System.AsyncCallback callback)
        {
            DownloadFileNameResumeCallback ftpCallback = new DownloadFileNameResumeCallback(this.Download);
            return ftpCallback.BeginInvoke(remFileName, resume, callback, null);
        }
        private delegate void DownloadFileNameFileNameCallback(String remFileName, String locFileName);
        public System.IAsyncResult BeginDownload(String remFileName, String locFileName, System.AsyncCallback callback)
        {
            DownloadFileNameFileNameCallback ftpCallback = new DownloadFileNameFileNameCallback(this.Download);
            return ftpCallback.BeginInvoke(remFileName, locFileName, callback, null);
        }
        private delegate void DownloadFileNameFileNameResumeCallback(String remFileName, String locFileName, Boolean resume);
        public System.IAsyncResult BeginDownload(String remFileName, String locFileName, Boolean resume, System.AsyncCallback callback)
        {
            DownloadFileNameFileNameResumeCallback ftpCallback = new DownloadFileNameFileNameResumeCallback(this.Download);
            return ftpCallback.BeginInvoke(remFileName, locFileName, resume, callback, null);
        }
        private delegate void UploadCallback(String fileName);
        public System.IAsyncResult BeginUpload(String fileName, System.AsyncCallback callback)
        {
            UploadCallback ftpCallback = new UploadCallback(this.Upload);
            return ftpCallback.BeginInvoke(fileName, callback, null);
        }
        private delegate void UploadFileNameResumeCallback(String fileName, Boolean resume);
        public System.IAsyncResult BeginUpload(String fileName, Boolean resume, System.AsyncCallback callback)
        {
            UploadFileNameResumeCallback ftpCallback = new UploadFileNameResumeCallback(this.Upload);
            return ftpCallback.BeginInvoke(fileName, resume, callback, null);
        }
        private delegate void UploadDirectoryCallback(String path, Boolean recurse);
        public System.IAsyncResult BeginUploadDirectory(String path, Boolean recurse, System.AsyncCallback callback)
        {
            UploadDirectoryCallback ftpCallback = new UploadDirectoryCallback(this.UploadDirectory);
            return ftpCallback.BeginInvoke(path, recurse, callback, null);
        }
        private delegate void UploadDirectoryPathRecurseMaskCallback(String path, Boolean recurse, String mask);
        public System.IAsyncResult BeginUploadDirectory(String path, Boolean recurse, String mask, System.AsyncCallback callback)
        {
            UploadDirectoryPathRecurseMaskCallback ftpCallback = new UploadDirectoryPathRecurseMaskCallback(this.UploadDirectory);
            return ftpCallback.BeginInvoke(path, recurse, mask, callback, null);
        }
        private delegate void DeleteFileCallback(String fileName);
        public System.IAsyncResult BeginDeleteFile(String fileName, System.AsyncCallback callback)
        {
            DeleteFileCallback ftpCallback = new DeleteFileCallback(this.DeleteFile);
            return ftpCallback.BeginInvoke(fileName, callback, null);
        }
        private delegate void RenameFileCallback(String oldFileName, String newFileName, Boolean overwrite);
        public System.IAsyncResult BeginRenameFile(String oldFileName, String newFileName, Boolean overwrite, System.AsyncCallback callback)
        {
            RenameFileCallback ftpCallback = new RenameFileCallback(this.RenameFile);
            return ftpCallback.BeginInvoke(oldFileName, newFileName, overwrite, callback, null);
        }
        private delegate void MakeDirCallback(String dirName);
        public System.IAsyncResult BeginMakeDir(String dirName, System.AsyncCallback callback)
        {
            MakeDirCallback ftpCallback = new MakeDirCallback(this.MakeDir);
            return ftpCallback.BeginInvoke(dirName, callback, null);
        }
        private delegate void RemoveDirCallback(String dirName);
        public System.IAsyncResult BeginRemoveDir(String dirName, System.AsyncCallback callback)
        {
            RemoveDirCallback ftpCallback = new RemoveDirCallback(this.RemoveDir);
            return ftpCallback.BeginInvoke(dirName, callback, null);
        }
        private delegate void ChangeDirCallback(String dirName);
        public System.IAsyncResult BeginChangeDir(String dirName, System.AsyncCallback callback)
        {
            ChangeDirCallback ftpCallback = new ChangeDirCallback(this.ChangeDir);
            return ftpCallback.BeginInvoke(dirName, callback, null);
        }


    }
    #endregion
}
