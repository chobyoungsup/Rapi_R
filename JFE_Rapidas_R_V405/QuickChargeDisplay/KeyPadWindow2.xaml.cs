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
    public partial class KeyPadWindow2 : Window
    {
        private static DependencyProperty InputValueProperty = DependencyProperty.Register("InputValue", typeof(string), typeof(KeyPadWindow2));
       
        bool useDot = false;
        bool useMinus = false;
        public string InputValue
        {
            get { return (string)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }

        public KeyPadWindow2(string Value)
        {
            InitializeComponent();
           

            InputValue = Value;
            inputText.SelectedText = Value;
        }

        private void Close_windows()
        {
            this.Dispatcher.Invoke((ThreadStart)delegate ()
            {
                this.Close();
            });
        }
        private void key_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "1";
        }

        private void key_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "2";
        }

        private void key_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "3";
        }

        private void key_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "4";
        }

        private void key_5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "5";
        }

        private void key_6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "6";
        }

        private void key_7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "7";
        }

        private void key_8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "8";
        }

        private void key_9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "9";
        }

        private void key_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue += "0";
        }

        private void key_00_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!useDot)
            {
                if (InputValue.Length >= 1)
                {
                    useDot = true;
                    InputValue += ".";
                }
            }            
        }

        private void key_Dot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InputValue = "";
            this.DialogResult = true;
            this.Close();
        }

        private void key_Fix_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!useMinus)
            {
                useMinus = true;
                InputValue = InputValue.Insert(0, "-");
            }          
            else
            {
                useMinus = false;
                InputValue = InputValue.Replace("-", null);
            }
        }

        private void key_Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InputValue.Length > 0)
            {
                string FindDot = InputValue.Substring(InputValue.Length - 1);
                InputValue = InputValue.Remove(InputValue.Length - 1, 1);
                if (FindDot.Contains("."))
                {
                    useDot = false;
                }
                //if (FindDot.Contains("-"))
                //{
                //    useMinus = false;
                //}
            }
        }

        private void key_Confirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AVRIO.avrio.EventMsg = "KeyPadWindow2_Loaded ";

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
        }
        private void closeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inputText.SelectedText = "";
            this.Close();
        }

        private void inputText_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
