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
using System.Media;

namespace QuickChargeDisplay
{
    /// <summary>
    /// CompleteChargePage.xaml에 대한 상호 작용 논리
    /// </summary>
        
    public partial class Emergency1Page : Page
    {
        private SoundPlayer voicePlayer = new SoundPlayer();
        private System.Timers.Timer timer;

        public Emergency1Page()
        {
            InitializeComponent();

            //  voicePlayer.SoundLocation = "Voice/11-01.wav";
            //  voicePlayer.Load();

           
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "Emergency1Page_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;


            timer = new System.Timers.Timer(1000 * 60); // 5초간 대기 (테스트)
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Start();

          //  voicePlayer.Play();
        }    
        
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
          //  AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysStandby;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
           // voicePlayer.Stop();

            if (timer != null)
                timer.Close();
            timer = null;
        }

        private void batteryStopBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AVRIO.avrio.EventMsg = "BatteryRunning cancel : TsStandby";
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsStandby;
        }
    }
}
