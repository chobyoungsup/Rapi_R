using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using QuickChargeDisplay;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace QuickCharge
{
    class QuickChargeApplication : Application
    {
       

        private string company;
        private static string Ver;
        private static CSOS.Rapidas RApidas = null;
        private System.Timers.Timer startTimer;     // 충전기 프로그램 시작 타이머 = 시작 delay(Port 오픈 등)
        private System.Timers.Timer canTimer;
        public QuickChargeApplication()
            : base()
        {
           

        }

        /// <summary>
        /// 주 Display Window
        /// </summary>n   
        ///                                                                                                                      
        [STAThread]

        static void Main(string[] args)
        {
            try
            {
                QuickChargeApplication app = new QuickChargeApplication();
                app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                app.Startup += app.AppStartingUp;
                app.Exit += app.AppExit;
                app.Run();
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
                //       QuickChargeApplication app = new QuickChargeApplication();
                //         app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                //         app.Startup += app.AppStartingUp;
                //        app.Exit += app.AppExit;
                //        app.Run();
            }
        }

        private MainWindow mainWindow = new MainWindow();
        private ModbusThread.ModbusQC modbus = new ModbusThread.ModbusQC();
        private BmsThread.DC_BMS_Thread Dc_Bms_Thread = new BmsThread.DC_BMS_Thread();

        void AppExit(object sender, ExitEventArgs e)
        {
            Thread.Sleep(1000);
            System.Console.WriteLine("Exit UI Thread.");
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Thread modbusThread = new Thread(modbus.ThreadRun);
                modbusThread.Priority = ThreadPriority.Normal;
                modbusThread.Start();
                Thread.Sleep(100);
                Dc_Bms_Thread.OpenCanPort();
            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }


        private void timerCadn_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (AVRIO.avrio.BmsCanportCheck)
            //{
            //    AVRIO.avrio.BmsCanportCheck = false;
            //    canTimer.Stop();                
            //    AVRIO.avrio.bThreadReady = false;

            //    Dc_Bms_Thread = null;

            //    Dc_Bms_Thread = new BmsThread.DC_BMS_Thread();
            //    Dc_Bms_Thread.OpenCanPort();
            //    canTimer.Start();
            //}
        }


        void AppStartingUp(object sender, StartupEventArgs e)
        {

            //cbs 2020.04.02
#if DEBUG
            AVRIO.avrio.nMouseCuser = true;
            // AVRIO.avrio.EventMsg = "Debug";
#else
            AVRIO.avrio.nMouseCuser = false;
           // AVRIO.avrio.EventMsg = "Release";
#endif

            try
            {
                AVRIO.avrio.Rapidaslanguage = 1;
                QuickChargeInit();
                // AVRIO 이벤트 핸들러 연결
                AVRIO.avrio.SysStatusChanged += avrio_SysStatusChanged;
                AVRIO.avrio.SysDataChanged += avrio_SysDataChanged;
               // mainWindow.Topmost = true;               

                mainWindow.Show();

              

                startTimer = new System.Timers.Timer(1000 * 3); // 3초간 대기후 프로그램 가동 (테스트)
                startTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                startTimer.AutoReset = false;
                startTimer.Start();
                Thread.Sleep(300);

                // canTimer = new System.Timers.Timer(1000 * 4); // Can 복구용
                //  canTimer.Elapsed += new ElapsedEventHandler(timerCadn_Elapsed);
                //  canTimer.AutoReset = true;
                //  canTimer.Start();



                RApidas = new CSOS.Rapidas();
                Thread JfeRapidas;
                JfeRapidas = new Thread(RApidas.PollFunction);
                JfeRapidas.Start();
                Thread.Sleep(300);

            }
            catch (Exception err)
            {
                AVRIO.Log.Eventlog = err.ToString();
            }
        }

        private static void QuickChargeInit()
        {
            try
            {
                // V1.00 양산1호
                // V1.01 양산 시리얼 번호 표시 추가 디자인 수정
                // V1.02 충전중 수전전력 흔들림 수정
                // V1.03 수전 전력 값 연산식 수정 +300,+400,+500 에서 -300 ,-400,-500
                // V1.04 수전 전력 값 연산식 수정 -300 ,-400,-500 에서 전체 +500 적용
                // V1.05 시간 설정 입력 에러 범위 수정
                // V1.06 마우스 포인트 뜨는 문제로 버전 업후 재 배포
                // V1.07 차징 타임이 레지스트 값으로 설정이되어서 온 오프시 변경이 안됨 D: 파일로 처음 로딩시 동작
                // V1.07 FAULT 메세지 중복 없애기
                // V1.07 차징 타임 255로 변경 
                // V1.07 Normal 상태나 Fault 상태나 통신주기 250ms 고정
                // V1.07 UI 이름 Quick Charger에서 JFE_RAPIDAS 로 변경
                // V1.07 UI 디자인 TextBox 위치 수정
                // V1.07 Fault Reset 명령 가끔 씹히는 문제 수정
                // V1.08  카드 모드 패쓰워드 모드 터치 2번에 실행 되는 문제    2014_06_13
                // V1.09 JFE 요청 이미지 수정 , Normal 상태나 Fault 상태나 통신주기 300ms 고정 
                // V1.09 Charging time + 1분 없앰  2014_09_18
                // V1.10 자동 충전 202번 문제 SBC에서 수정 , BMS 통신 에러 14번 Fault 통신 에러 관련 -> 321 변경 , BMS 통신 패킷 Rec 누수 현상 3회 -> 2회변경  
                //            2014_10_30 
                // V1.11 로그 파일 E: => D: 변경    2014_11_28 
                // V2.00 관리자 모드 MANU 패쓰워드 수정 안되는 문제 , BMS 통신 패킷 Rec 누수 현상 2회 -> 3회변경 , 
                //전력 시간 맨마지막 처음 실행시 오류 수정\
                //       PassWord 충전 화면 재 입장시 전에 입력한 값 나타다던 현상 사라지게 변경 ,
                //Scale 수정 (AVRIO.bmsio.nChargePowerLimit/10) ,(AVRIO.bmsio.nDischargePowerLimit/10)
                //V3.0 리부팅 주 1회 배터리 OFFSET fault 321 시 과금에 17 전송
                //    321 복귀 기능 워닝시 Stop 기능

                //cbs 2018.09.10
                //V3.1.0 
                //    커넥터 연결화면 이미지 변경 「充電STARTボタンをを押す」→「充電スタートを押す」  
                //    충전중 BMS INFO 화면 추가 View  
                //    Modbus.cs -> System Reboot Command 수정   System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");  
                //    StandByPage Led 점등 표시 추가
                //    Rapidas.cs -> PacketSend ,system reboot 전거 바꿈
                //    사용자 재부팅 체크 기능 추가 
                //    StanbyPage BMS 배터리 충전 정지버그 수정 -> 테스트완료 현장에 적용안함
                //    App 시작시 와치독 Load 기능추가
                //    Admin Login시 와치독 kill Logout시 Start 기능추가
                //    StanbyPage 배터리 충전정지 조건 수정
                //    SetupPowerPage.cs  배터리 충전 제한 기능 추가
                //V3.1.1 
                //      내부 배터리 충전시작 85 에서 80% 로 수정  2019.05.13 김철호 상무요청
                //      내부 배터리 충전설정 가능기능  09_2_SetupPowerPage.xaml UI 기능구현(주석처리함) 
                //      내부 배터리 충전설정 가능기능  01_StandByPage.xaml timer_AutoCharge 함수에 기능구현 (주석처리함)
                //V3.1.2 
                //      DSP 전압 전류 설정 기능추가  2019.10
                //      BMS SOC LowFault 추가 FaultCode 315 -> 과금업데이트 7820   2019.10
                //      Modbus.cs  DSP 전압전류 교정값 송수신 기능 추가  2020.02
                //      09_10_SetupVoltagePage 화면추가  2020.02
                //      KeyPadWindow2  화면 추가  2020.02
                //V4.0.0
                //      배은우 부장 주버젼 변경요청 2020.04.01           
                //      리부팅 주, 일 설정기능 추가  ->09_4_SetupLanguagePage.xaml 2020.04.01
                //      ChargeWarnningDialog1.cs 삭제  2020.04.01
                //      BMS HighTempWarnFlag을 FaultFlag로 바꿈 Fault CODE 305 처리 및 과금전송 2020.04.01
                //      BMS SOCLowFaultFlag시  + SOC 5% 미만일때 FAULT 315처리 및 과금전송기능을 ->  BMS SLF Flag 확인하여 체크하도록 변경 김철호 전무
                //V4.0.1
                //      전압전류설정화면에 Output Voltage, Ampere 추가  2020.06. 10
                //      터치버그수정 -> WPF버그 DisableWPFTabletSupport 함수추가  2020.06. 10
                //      SBC 교정값 셋팅 정밀도가 낮으므로 문제될시에는 float -> decimal로 변경해야될것 같음. 2020.06. 10
                //V4.0.2 Release 2020.07.03
                //      관리자모드 로그아웃시 START버튼 인식문제 수정  2020.06.11
                //      SBC 교정값 셋팅 정밀도 float -> decimal로 변경. 2020.06.11                              
                //      과금 시리얼통신 옵션 RtsEnable = true로 변경함  SMJ 차장님꺼 적용  2020.06.19 
                //      전압전류 TsCalibration Flag DSP 통신연결시 1회 ON 수정요청(김철호 부사장님......)  2020.06.19
                //      Registry 64bit 32bit 구분 SMJ 차장님꺼 적용  + .net 4.0 변경 + 빌드 옵션 Any Cpu 적용 2020.06.19
                //      BmsThread.cs VCI_CAN_DotNET.dll 32bit 종속성 문제 때문에 빌드 옵션을 Any cpu-> x86으로 재설정 2020.06.22
                //      x86 설정, JFE 기존 레지스트리 입력된 값으로 인하여  동일경로에서 읽어오도록 수정     2020.06.22
                //      AdminMainWindow 터치입력 부분을 좀더 넓게 사용하기위해    Release/Debug 모드에따라 윈도우 스타일 변경 2020.06.25  
                //****  내부배터리 SOC 5% 미만일때(JFE에서 잘못알고있음 5%미만이 아니라 SLF발생시임) Fault315를 SBC에서 만드는기능-> LGBMS에서  SLF Flag를 전송하지 않아서 발생이 안됨 (SOC 2%까지 떨어져도 발생안됨)
                //V4.0.3 Release
                //      app.manifest WPF 지원운영체제 명시, 관리자 권한실행
                //      CSOS.OSVersionInfo.cs 추가 Win10 일때 만 Serial RtsEnable true 옵션설정
                //      SLF-> SOC 5%미만일떄 Fault315창만 띄움 + 과금멤버 업데이트 JFE요청 
                //      Win10 시작프로그램 등록 및 인스톨 가이드는 PS 사업부에서 진행
                //      Win10 화면터치시 콘솔화면으로 랜덤하게 전환됨 화면 우선순위 변경 및 콘솔화면 하단으로 이동 2020.08.10
                //      SBC 버젼정보 Config/Registery등록방식 최무림차장 수정요청함 배은우부장 승인  2020.08.10  
                //V4.0.4 Release
                //      Mainwindow.xaml.cs 콘솔창 숨김으로 변경  단축키 'S' -> Console Show / 'H' Console Hide 추가 2020.08.18             
                //V4.0.5 Release 
                //      기존 Reg 파일 내용이 사라짐-> SBC 버젼정보 레지스트리에만 수정  2020.10.07 

                Ver = "V" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);               

                AVRIO.avrio.EventMsg = "=================================================";
                AVRIO.avrio.EventMsg = $" JFE EV QuickCharger Rapidas-R  S/W ver {Ver}! ";
                AVRIO.avrio.EventMsg = "=================================================";

                RegistryKey regKey;
                RegistryKey reg;
                if (Environment.Is64BitOperatingSystem)
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\SEQ", true);
                    AVRIO.avrio.EventMsg = "###OperatingSystem 64bit";
                }
                else
                {
                    regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\SEQ", true);
                    AVRIO.avrio.EventMsg = "###OperatingSystem 32bit";
                }

                AVRIO.avrio.EventMsg = "### This App is 32bit only";
                AVRIO.avrio.FanOffTimeSelect = Convert.ToString(reg.GetValue("FanOffTimeSelect", "111111111111111111111111"));
                AVRIO.avrio.FanOffTime = Convert.ToInt32(reg.GetValue("FanOffTime", "10"));
                AVRIO.avrio.SeqNumFault = Convert.ToUInt32(reg.GetValue("Fault", "0"));
                AVRIO.avrio.SeqNumHistory = Convert.ToUInt32(reg.GetValue("History", "0"));
                AVRIO.avrio.SeqNumPrice = Convert.ToUInt32(reg.GetValue("Price", "0"));
                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                //cbs 2020.10.07 SBC 버젼정보수정 
                int nVer = int.Parse(Regex.Replace(Ver, @"\D", ""));
                reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\Admin", true);
                reg.SetValue("SBCVERSION", nVer.ToString());
                AVRIO.avrio.EventMsg = "###Version Setting /Registry";
                               

                //cbs 2020.07.01 주석처리함 안쓰는거임
                //reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("MediaPlayer");
                // reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\MediaPlayer", true);
                // QCDV.SetupEquipment.Mode = Convert.ToInt32(reg.GetValue("Mode", "0"));
                // QCDV.SetupEquipment.Sound = Convert.ToBoolean(reg.GetValue("Mute", "False"));
                // QCDV.SetupEquipment.AutoStartTimeHour = Convert.ToInt32(reg.GetValue("StartHour", "0"));
                // QCDV.SetupEquipment.AutoStartTimeMinute = Convert.ToInt32(reg.GetValue("StartMin", "0"));
                // QCDV.SetupEquipment.AutoEndTimeHour = Convert.ToInt32(reg.GetValue("EndHour", "0"));
                // QCDV.SetupEquipment.AutoEndTimeMinute = Convert.ToInt32(reg.GetValue("EndMin", "0"));
                //// reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("QuickCharger").CreateSubKey("Admin");
                // reg = regKey.OpenSubKey(@"SOFTWARE\QuickCharger\Admin", true);

                AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;

                GetETC_Config();
                AVRIO.avrio.EventMsg = "###Rapidas-R Init Finsh!";
            }
            catch (Exception e)
            {
                AVRIO.avrio.EventMsg = e.Message;
                AVRIO.Log.Eventlog = e.ToString();
            }
            return;
        }

        //cbs 2020.02.01
        static void GetETC_Config()
        {
            string temp = null;
                       
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "UseReboot", ""));
                AVRIO.avrio.UseReboot = Boolean.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.UseReboot = true;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "UseReboot", "true");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "StartSOC", ""));
                AVRIO.avrio.ChargeStartSOC = byte.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.ChargeStartSOC = 80;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "StartSOC", AVRIO.avrio.ChargeStartSOC.ToString());

            }

            AVRIO.avrio.ChargeBattSOCLimit = (byte)(AVRIO.avrio.ChargeStartSOC + (byte)5);


            //try
            //{
            //    temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "ChargeBattLimit", ""));
            //    AVRIO.avrio.ChargeBattSOCLimit = byte.Parse(temp);
            //}
            //catch
            //{
            //    AVRIO.avrio.ChargeBattSOCLimit = 85;
            //    QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "ChargeBattLimit", AVRIO.avrio.ChargeBattSOCLimit.ToString());

            //}
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "VoltageSlope", ""));
                AVRIO.avrio.nSET_Voltage_Slope = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.nSET_Voltage_Slope = 1 * 10000;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "VoltageSlope", AVRIO.avrio.nSET_Voltage_Slope.ToString());
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "VoltageOffset", ""));
                AVRIO.avrio.nSET_Voltage_Offset = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.nSET_Voltage_Offset = 1 * 10000;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "VoltageOffset", AVRIO.avrio.nSET_Voltage_Offset.ToString());
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "CurrentSlope", ""));
                AVRIO.avrio.nSET_Current_Slope = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.nSET_Current_Slope = 1 * 10000;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CurrentSlope", AVRIO.avrio.nSET_Current_Slope.ToString());
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "CurrentOffset", ""));
                AVRIO.avrio.nSET_Current_Offset = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio.nSET_Current_Offset = 1 * 10000;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "CurrentOffset", AVRIO.avrio.nSET_Current_Offset.ToString());
            }
            try
            {
                //WatchDog
                Process[] prs = Process.GetProcesses();
                foreach (Process pr in prs)
                {
                    if (pr.ProcessName == "RapidasWatchDog")
                    {
                        // pr.Exited += new EventHandler(ProcessExited);
                        pr.Kill();
                    }
                }

                string Names = "RapidasWatchDog.exe";
                string fullPathss = @"C:\WatchDog";
                Process ps2 = new Process();

                ps2.StartInfo.FileName = Names;
                ps2.StartInfo.WorkingDirectory = fullPathss;
                ps2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                // ps2.EnableRaisingEvents = true;
                ps2.Start();
            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = err.ToString();
            }

            RebootConfig();
        }

        static void RebootConfig()
        {
            //cbs 2020.03.30 
            string temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "RebootSelect", ""));

            if (temp == "Day")
            {
                AVRIO.avrio.RebootSelectDay = true;
            }
            else if (temp == "Week")
            {
                AVRIO.avrio.RebootSelectDay = false;
            }
            else
            {
                AVRIO.avrio.RebootSelectDay = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "RebootSelect", "Week");
            }


            AVRIO.avrio._RebootFunc = new Dictionary<DayOfWeek, AVRIO.RebootFunc>();
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Sunday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Monday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Tuesday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Wednesday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Thursday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Friday, new AVRIO.RebootFunc());
            AVRIO.avrio._RebootFunc.Add(DayOfWeek.Saturday, new AVRIO.RebootFunc());

            #region 
            ///////////////Sunday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Sunday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Sunday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Sunday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Sunday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Sunday_Enable", "false");
            }


            //////////////Monday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Monday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Monday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Monday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Monday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Monday_Enable", "false");
            }


            //////////////Tuesday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Tuesday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Tuesday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Tuesday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Tuesday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Tuesday_Enable", "false");
            }


            //////////////Wednesday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Wednesday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Wednesday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Wednesday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Wednesday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Wednesday_Enable", "false");
            }

            //////////////Thursday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Thursday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Thursday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Thursday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Thursday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Thursday_Enable", "false");
            }

            //////////////Friday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Friday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Friday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Friday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Friday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Friday_Enable", "false");
            }

            //////////////Saturday
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Saturday_HH", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].TimeHH = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].TimeHH = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_HH", "12");
            }
            try
            {
                temp = Convert.ToString(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Saturday_mm", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].Timemm = int.Parse(temp);
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].Timemm = 12;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_mm", "12");
            }
            try
            {
                bool Ret = Convert.ToBoolean(QuickChargeConfig.ChargeConfig.GetConfig1("AVRIO", "Reboot_Saturday_Enable", ""));
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable = Ret;
            }
            catch
            {
                AVRIO.avrio._RebootFunc[DayOfWeek.Saturday].IsEnable = false;
                QuickChargeConfig.ChargeConfig.SetConfig1("AVRIO", "Reboot_Saturday_Enable", "false");
            }

            #endregion
        }

        private void avrio_SysStatusChanged(AVRIO.SysStatusChangeEventArgs args)
        {
            if (args.CurrentSysStatus == AVRIO.SysStatus.SysStandby)
            {
                try
                {
                    // AVRIO.avrio.nSelectCommand12 = 5;
                    AVRIO.avrio.CpStat = 1;
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._01_대기화면); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }

            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysReady)
            {
                try
                {
                    QuickChargeDisplay.QCDV.Charging.RemainTime = 0;
                    QuickChargeDisplay.QCDV.Charging.ChargingWatt = 0;
                    QuickChargeDisplay.QCDV.Charging.ChargingMoney = 0;
                    QuickChargeDisplay.QCDV.Charging.ChargingCurrent = 0;

                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._03_커넥터연결); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysConectionFinish)
            {
                try
                {
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._03_접속확인중); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysPreRun)
            {
                try
                {
                    AVRIO.avrio.ChargeStartTime = DateTime.Now;
                    AVRIO.avrio.ChargeStartWatt = 0;
                    AVRIO.avrio.ChargeWatt = 0;

                    // AVRIO.avrio.ChargeSOC = 0;

                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._05_0_절연저항측정화면); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysRunning)
            {
                try
                {
                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeStart;

                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._05_1_충전중화면); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysStopping)
            {
                try
                {

                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._05_2_충전종료처리중); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysFinish)
            {
                try
                {
                    int nFaultCode = AVRIO.avrio.CarBMSFault;

                    DateTime dt = DateTime.Now;
                    string msg;

                    if (nFaultCode != 0)
                    {
                        AVRIO.avrio.FaultCode = AVRIO.avrio.CarBMSFault.ToString();
                        switch (nFaultCode)
                        {
                            case 1:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91040;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Battery Overvoltage flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 2:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91010;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Battery Undervoltage flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 3:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91010;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Battery Current Differential flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 4:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91008;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] High Battery Temperature flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 5:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91004;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Voltage Differential flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 6:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91080;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Vehicle Shift Position flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            case 7:
                                {
                                    AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.CarStop91002;
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] Other Vehicle Faults flag was Received" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                            default:
                                {
                                    msg = dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00") + "," + "0" + "," + "[EV_BMS] No define" + ",N";
                                    QuickChargeConfig.ChargeConfig.SetLog("Fault", AVRIO.avrio.SeqNumFault++, msg, dt);
                                }
                                break;
                        }

                    }

                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._06_커넥터분리); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysPayCheck)
            {
                try
                {
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._07_결재확인); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            if (args.CurrentSysStatus == AVRIO.SysStatus.SysWorkDone)
            {
                try
                {
                    // 충전중지화면 표시
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.SetPage(PageId._08_충전완료); });
                    return;
                }
                catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
            }

            /*
            if (args.CurrentSysStatus == AVRIO.SysStatus.SysFault)
            {
                this.Dispatcher.BeginInvoke((ThreadStart)delegate() { mainWindow.ShowFaultDialog(); });                
                return;
            }
            */

        }

        void avrio_SysDataChanged(string dataName, object value)
        {
            try
            {
                switch (dataName)
                {
                    // test
                    case "ChargeSOC":
                        {
                        }
                        break;
                    case "SoundPlayQS":
                        {
                        }
                        break;
                    case "ServerChargeCostDialog":
                        {
                            try
                            {
                                // bool b = (bool)value;

                                // this.Dispatcher.BeginInvoke((ThreadStart)delegate() { mainWindow.ShowServerChargeCostDialog(b); });
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case "ServerConfirmDialog":
                        {
                            try
                            {
                                // bool b = (bool)value;

                                // this.Dispatcher.BeginInvoke((ThreadStart)delegate() { mainWindow.ShowServerConfirmDialog(b); });                            
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case "FaultDialog":
                        {
                            try
                            {
                                bool b = (bool)value;

                                if (b)
                                {
                                    this.Dispatcher.BeginInvoke((ThreadStart)delegate () { mainWindow.ShowFaultDialog(); });
                                }
                            }
                            catch (Exception e)
                            {
                                AVRIO.Log.Eventlog = e.ToString();
                            }

                        }
                        break;
                }
            }
            catch (Exception err) { AVRIO.Log.Eventlog = err.ToString(); }
        }
    }
}

