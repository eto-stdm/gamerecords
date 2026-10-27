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
    /// Логика взаимодействия для DefaultOKDialog.xaml
    /// </summary>
    public partial class DefaultOKDialog : Window
    {
        string Message { get; set; }
        /// <summary>
        /// Конструктор окна DefaultOKDialog
        /// </summary>
        /// <param name="message">Сообщение</param>
        public DefaultOKDialog(string message)
        {
            InitializeComponent();
            Message = message;

            TextTB.Text = Message;
        }

        /// <summary>
        /// Обработка нажатия на кнопку "ОК"
        /// </summary>
        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
