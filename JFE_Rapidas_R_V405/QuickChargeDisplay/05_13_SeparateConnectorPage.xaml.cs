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
using System.IO;
using System.Media;
using System.Timers;
using System.Threading;

namespace QuickChargeDisplay
{
    /// <summary>
    /// SeparateConnectorPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SeparateConnectorPage : Page
    {
       // private SoundPlayer voicePlayer = new SoundPlayer();

        private System.Timers.Timer WatchDog;
        public SeparateConnectorPage()
        {
            InitializeComponent();

           
        }

        private System.Timers.Timer changePage;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "SeparateConnectorPage_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.nSelectCommand12 = 2;
          //  AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFinish;
            //AVRIO.avrio.StartButton = false;
           // AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;
            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor/05/충전중13.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }
            else
            {
                Stream stream = ResourceHelper.GetResourceStream(this.GetType(), "image/RAPIDAS-R_Monitor-English/13.png");
                Bitmap bitmap = new Bitmap(stream);
                BackImage.LoadSmile(bitmap);
                BackImage.StopAnimate();
                stream.Dispose();
            }


            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));
                //AVRIO.avrio.ChargeRemainTime
               // if (AVRIO.avrio.ChargeRemainTimemm == 0)
                    Txttimemm.Content = "0";
               // else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTimemm);

                Double tempSoc = Convert.ToDouble(AVRIO.avrio.OutAmpare);
                SetPower.Content = "0";// Convert.ToString(Math.Floor((tempSoc * 100) / 100));
            });

            BackImage.StartAnimate();
           // voicePlayer.Play();

            WatchDog = new System.Timers.Timer(250);
            WatchDog.Elapsed += new ElapsedEventHandler(timer_WatchDog);
            WatchDog.AutoReset = true;
            WatchDog.Enabled = true;

            changePage = new System.Timers.Timer(3 * 1000);
            changePage.AutoReset = false;
            changePage.Elapsed += new ElapsedEventHandler(changePage_Elapsed);
            changePage.Start();

            AVRIO.avrio.CsosCommand = AVRIO.CSOSCMD.ChargeFinish;
        }

        void changePage_Elapsed(object sender, ElapsedEventArgs e)
        {
            QCDV.ConfirmCharge.ChargeWatt = AVRIO.avrio.ChargeWatt;
            QCDV.ConfirmCharge.Payment = AVRIO.avrio.ChargePrice;
            AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysPayCheck;
        }
        void timer_WatchDog(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                Double tempSoc1 = Convert.ToDouble(AVRIO.avrio.ChargeSOC);
                chargingSoc.Content = Convert.ToString(Math.Floor((tempSoc1 * 100) / 100));
              //  if (AVRIO.avrio.ChargeRemainTime == 0)
                    Txttimemm.Content = "0";
               // else Txttimemm.Content = Convert.ToString(AVRIO.avrio.ChargeRemainTime);
                Double tempSoc = Convert.ToDouble(AVRIO.avrio.OutAmpare);
                SetPower.Content = "0";
            });


            if (AVRIO.avrio.StartButton)
            {

            }

            if (AVRIO.avrio.StopButton || AVRIO.avrio.StopButtonPlay)
            {

                    //다음 페이지 QCDV.ConfirmCharge.ChargeWatt = AVRIO.avrio.ChargeWatt;
                    WatchDog.Enabled = false;
                    QCDV.ConfirmCharge.Payment = AVRIO.avrio.ChargePrice;
                    AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysPayCheck;
                
            }
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            BackImage.StopAnimate();
            BackImage.Dispose();

            // voicePlayer.Stop();

            // AVRIO.avrio.StartButton = false;
            AVRIO.avrio.StopButton = false;
            AVRIO.avrio.StopButtonPlay = false;
            AVRIO.avrio.StartButtonPlay = false;

            if (changePage != null)
            {
                changePage.Enabled = false;
                changePage.Stop();
                changePage.Dispose();
                changePage = null;
            }


            if (WatchDog != null)
            {
                WatchDog.Enabled = false;
                WatchDog.Stop();
                WatchDog.Dispose();
                WatchDog = null;
            }
          
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //QCDV.ConfirmCharge.Payment = AVRIO.avrio.ChargePrice;
            //AVRIO.avrio.CurrentStatus = AVRIO.SysStatus.SysPayCheck;

        }
    }
}