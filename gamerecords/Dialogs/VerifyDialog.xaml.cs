using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gamerecords.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для VerifyDialog.xaml
    /// </summary>
    public partial class VerifyDialog : Window
    {
        public VerifyDialog()
        {
            InitializeComponent();
        }
        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
