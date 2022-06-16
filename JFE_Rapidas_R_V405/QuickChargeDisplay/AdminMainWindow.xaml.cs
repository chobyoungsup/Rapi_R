using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace QuickChargeDisplay
{
    /// <summary>
    /// AdminMainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.WindowStyle = WindowStyle.SingleBorderWindow;
#else
            this.WindowStyle = WindowStyle.None;
#endif

            //WindowStyle = "None"
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "AdminMainWindow_Loaded ";

            //cbs 2020.06.11 
            AVRIO.avrio.RunMode = AVRIO.RunningMode.Admin;

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            try
            {
                MainFrame.Navigate(PageManager.GetPage(PageId._10_0_관리자메뉴));
            }
            catch { }

            try
            {
                Process[] prs = Process.GetProcesses();
                foreach (Process pr in prs)
                {
                    if (pr.ProcessName == "RapidasWatchDog")
                    {
                        // pr.Exited += new EventHandler(ProcessExited);                 
                        pr.Kill();

                    }
                }

                Thread.Sleep(1000);

                bool Result = false;
                prs = Process.GetProcesses();
                foreach (Process pr in prs)
                {
                    if (pr.ProcessName == "RapidasWatchDog")
                    {
                        Result = true;
                    }
                }


                if (!Result)
                {
                    AVRIO.avrio.EventMsg = "Killed WatchDog";
                }
                else
                {
                    AVRIO.avrio.EventMsg = "Not Kill WatchDog";
                }
            }
            catch (Exception err)
            {
                AVRIO.avrio.EventMsg = err.ToString();
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //WatchDog
            try
            {
                string Names = "RapidasWatchDog.exe";
                string fullPathss = @"C:\WatchDog";
                Process ps2 = new Process();

                ps2.StartInfo.FileName = Names;
                ps2.StartInfo.WorkingDirectory = fullPathss;
                ps2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                ps2.Start();
                
            }
            catch(Exception err)
            {
                AVRIO.avrio.EventMsg = err.ToString();
            }
        }
    }
}
