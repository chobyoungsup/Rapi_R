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
    /// PasswordFaultDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PasswordFaultDialog : Window
    {
        public PasswordFaultDialog()
        {
            InitializeComponent();

            if (AVRIO.avrio.nMouseCuser)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = Cursors.None;

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
