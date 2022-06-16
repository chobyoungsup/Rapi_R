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
using System.Timers;
using System.Threading;
using System.Runtime.InteropServices;

namespace QuickChargeDisplay
{
    /// <summary>
    /// KeyPadWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyPadWindow : Window
    {
        private static DependencyProperty InputValueProperty = DependencyProperty.Register("InputValue", typeof(string), typeof(KeyPadWindow));
        private System.Timers.Timer timer;

        public string InputValue
        {
            get { return (string)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }

        public KeyPadWindow()
        {
            InitializeComponent();
            InputValue = "";
            inputText.SelectedText = "";

           
        }

        private void Watchtimer_Elapsed(object sender, ElapsedEventArgs e)
        {// 1 분간 방치시 첫 화면 이동 
            if (AVRIO.avrio.WwindowsClose1)
            {
                Close_windows();



            }
        }

        private void Close_windows()
        {

            this.Dispatcher.Invoke((ThreadStart)delegate()
            {
                this.Close();
            });

        }
        private void key_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "1";
        }

        private void key_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "2";
        }

        private void key_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "3";
        }

        private void key_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "4";
        }

        private void key_5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "5";
        }

        private void key_6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "6";
        }

        private void key_7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "7";
        }

        private void key_8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "8";
        }

        private void key_9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "9";
        }

        private void key_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "0";
        }

        private void key_00_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length >= 4)
            {

            }
            else
            InputValue += "00";
        }

        private void key_Dot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue = "";
            this.DialogResult = true;
            this.Close();
        }

        private void key_Fix_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue = "";
        }

        private void key_Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length > 0)
                InputValue = InputValue.Remove(InputValue.Length - 1, 1);
        }

        private void key_Confirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (timer != null)
                timer.Close();
            timer = null;
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "KeyPadWindow_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

            timer = new System.Timers.Timer(200); // 60초간 대기 (테스트)
            timer.Elapsed += new ElapsedEventHandler(Watchtimer_Elapsed);
            timer.AutoReset = true;
            timer.Start();
               InputValue = "";
                inputText.SelectedText = "";
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //  timer.Enabled = false;
            if (timer != null)
                timer.Close();
            timer = null;

        }
        private void closeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (timer != null)
                timer.Close();
            timer = null;
            inputText.SelectedText = "";
            this.Close();
        }

        private void inputText_TextChanged(object sender, TextChangedEventArgs e)
        {
          //  switch (QCDV.ChargeMoney.ChargeMode)
            //{
            //    case ChargeMode.Money:
            //        {
                        
            //        }
            //        break;
            //    case ChargeMode.Soc:
            //        {
            //            int temp = (int)Convert.ToDouble(InputValue);
            //            if (temp > 80)
            //            {
            //                InputValue = "80";
            //            }
            //        }
            //        break;
            //    case ChargeMode.Watt:
            //        break;
            //}
        }
    }
}
