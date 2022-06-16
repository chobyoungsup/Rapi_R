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
using System.IO;
using System.Xml.Linq;
using System.Timers;
using System.Threading;
using Microsoft.Win32;

namespace QuickChargeDisplay
{
    /// <summary>
    /// PasswordPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PasswordPage : Page
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer Watchtimer;
        private int check = 0;
        public PasswordPage()
        {
            InitializeComponent();          
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.WwindowsClose = true;
            AVRIO.avrio.AdminManuClose = false;
            Watchtimer.Enabled = false;
            timer.Enabled = false;           

            if (Watchtimer != null)
                Watchtimer.Close();
            Watchtimer = null;

            if (timer != null)
                timer.Close();
            timer = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "PasswordPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.WwindowsClose = false;
            Watchtimer = new System.Timers.Timer(500); // 60초간 대기 (테스트)
            Watchtimer.Elapsed += new ElapsedEventHandler(Watchtimer_Elapsed);
            Watchtimer.AutoReset = true;
            Watchtimer.Start();

            timer = new System.Timers.Timer(1000 * 60); // 60초간 대기 (테스트)
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();
            passwordBox.Password = "";
            AVRIO.avrio.RunMode = AVRIO.RunningMode.Admin; // RunMode 변경
            AVRIO.avrio.AdminPage = AVRIO.AdminPage.Password;
            check = 0;
            cbAdminSelect.SelectedIndex = 0;
        }

        private void Watchtimer_Elapsed(object sender, ElapsedEventArgs e)
        {// 1 분간 방치시 첫 화면 이동 
            if (AVRIO.avrio.AdminManuClose)
            {
                AVRIO.avrio.AdminManuClose = false;
                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
            }
        }

        private void passwordBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPadWindow window = new PasswordPadWindow();
            window.Owner = ControlHelper.FindAncestor<Window>(this);
            window.InputValue = this.passwordBox.Password;
            if (window.ShowDialog() == true)
            {
                passwordBox.Password = window.InputValue;
            }
        }

        private void cbAdminSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAdminSelect.SelectedIndex == 0)
            {
                AVRIO.avrio.RapidasLoginType = 1;
            }
            else if (cbAdminSelect.SelectedIndex == 1)
            {
                AVRIO.avrio.RapidasLoginType = 2;
            }
            else
            {
                AVRIO.avrio.RapidasLoginType = 1;
            }
        }

        private void adminPwConfirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
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

            // Admin Password 확인
            AVRIO.avrio.AdminPassWord = Convert.ToString(reg.GetValue("ADMINPASS", "1234"));
            AVRIO.avrio.ManualPassWord = Convert.ToString(reg.GetValue("MANUPASS", "8611"));

            if (cbAdminSelect.SelectedIndex == 0)
            {
                if ((AVRIO.avrio.AdminPassWord == passwordBox.Password)&& (passwordBox.Password.Length == 4))
                {
                    // 패스워드가 맞을 경우 관리자 초기모드로 이동
                    AVRIO.avrio.RapidasLoginType = 1;
                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.AdminPage;
                    check = 1;
                    AdminMainWindow adminWindow = new AdminMainWindow();
                    adminWindow.ShowDialog();
                }
                else
                {
                    PasswordFaultDialog dlg = new PasswordFaultDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();
                }
            }
            else if (cbAdminSelect.SelectedIndex == 1)
            {
                if ((AVRIO.avrio.ManualPassWord == passwordBox.Password) && (passwordBox.Password.Length == 4))
                {
                    // 패스워드가 맞을 경우 관리자 초기모드로 이동
                    AVRIO.avrio.RapidasLoginType = 2;
                    AVRIO.avrio.AdminPage = AVRIO.AdminPage.AdminPage;
                    check = 1;
                    AdminMainWindow adminWindow = new AdminMainWindow();
                    adminWindow.ShowDialog();
                }
                else
                {
                    PasswordFaultDialog dlg = new PasswordFaultDialog();
                    dlg.Owner = ControlHelper.FindAncestor<Window>(this);
                    dlg.ShowDialog();

                }
            }
            else
            {
               // adminPwConfirm.Visibility = Visibility.Visible;
            }
            passwordBox.Password = "";
        }

        private void logOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;

            //cbs 2020.06.11
            AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
        }


        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {// 1 분간 방치시 첫 화면 이동 


            if (check == 0)
            {
                AVRIO.avrio.RunMode = AVRIO.RunningMode.Normal;
                AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
                AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;

                //cbs 2020.06.11
                AVRIO.avrio.AdminPage = AVRIO.AdminPage.Ready;
            }
        }
        
    }
}
