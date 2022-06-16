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


namespace QuickChargeDisplay
{
    /// <summary>
    /// Fault.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChargeWarnningDialog : Window
    {
        public ChargeWarnningDialog()
        {
            InitializeComponent();

         
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "ChargeWarnningDialog_Loaded ";


            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            AVRIO.avrio.bWarnningWindowClosed = false;

            if (AVRIO.avrio.Rapidaslanguage == 0)
            {
                //if(
                   // AVRIO.avrio.IsControlBDComm == false ||
                  //  AVRIO.avrio.IsBMSCanComm == false || 
                  //  AVRIO.bmsio.byBMSStatus != 3 
                //    )
                //{
                    strTitle.Text = "システム確認中";
                    strInfo2.Text = " しばらくお待ちください.";
                //}
                //else
                //{
                //    strTitle.Text = "Please System Version";
                //    strInfo2.Text = "Do not Sbc Version Dsp";

                //}
            }
            else
            {
                strTitle.Text = "Checking System";
                strInfo2.Text = "it will take a few minutes..";
  
            }
        }

        private void closeWarring_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //cbs
            AVRIO.avrio.WarnningType = AVRIO.WARNNINGTYPE.None;

            //WaitFualt1.Start();

            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            AVRIO.avrio.FaultDlgType = AVRIO.FAULT_DLG_TYPE.None;
            AVRIO.avrio.FaultCode = "";

            AVRIO.bmsio.SendData_304.Control_Status.byBMSFaultFlagResetControl = 1;


            AVRIO.bmsio.SendData_304.Control_Status.byBMSWakeSleepModeControl = 0;

            AVRIO.bmsio.SendData_304.Control_Status.byBMSMainContactor = 1;
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;


            // AVRIO.avrio.Fualtstop = false;
            // AVRIO.avrio.FualtstopFalg = false;
            AVRIO.avrio.nonefaultcheck = true;
            AVRIO.avrio.nSelectCommand56 = 1;
            AVRIO.avrio.nSelectCommand78 = 1;
            AVRIO.avrio.nSelectCommand910 = 1;
            AVRIO.avrio.TsCommand = AVRIO.TsCommand.TsResetFault;
            AVRIO.avrio.Fualtstop = true;

            //cbs
            //this.Close();   

            AVRIO.avrio.WarnningType = AVRIO.WARNNINGTYPE.None;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //cbs
            AVRIO.avrio.bWarnningWindowClosed = true;
        }
    }
}
