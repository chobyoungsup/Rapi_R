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

using System.Drawing;
using System.Windows.Resources;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.IO;

//using System.Timers;

using System.Media;
using System.Threading;
using System.Timers;

namespace QuickChargeDisplay
{
    /// <summary>
    /// IdCardPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IsolationPage : Page
    {

        private System.Timers.Timer WatchDog;

        public IsolationPage()
        {
            InitializeComponent();
        }
               
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "IsolationPage_Loaded ";


            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;

            AVRIO.avrio.nSelectCommand12 = 3;
            Thread.Sleep(300);
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
          //  AVRIO.avrio.StopButton = false;
          //  AVRIO.avrio.StartButton = false;
            
            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeStart; 
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/10a10b.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/10a10b.gif");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            BackImage.StartAnimate();
            WatchDog = new System.Timers.Timer(250);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

        }

        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AVRIO.avrio.StartButton)
            {            
            }else
                if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
                {


                    AVRIO.avrio.EventMsg = "[SBC] TsVehicleFinish";
                    AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;

                }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

           // AVRIO.avrio.StopButton = false;
           // AVRIO.avrio.StartButton = false;

            BackImage.StopAnimate();
            BackImage.Dispose();

            
            try
            {
                if (WatchDog != null)
                {
                    WatchDog.Enabled = false;
                    WatchDog.Stop();
                    WatchDog.Dispose();
                    WatchDog = null;
                }
                    
            }
            catch { }
        }

        private void IsolationCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
           //  AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsVehicleFinish;
        }
    }
}
