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
    /// Логика взаимодействия для DefaultYesNoDialog.xaml
    /// </summary>
    public partial class DefaultYesNoDialog : Window
    {
        string Message { get; set; }
        /// <summary>
        /// Конструктор окна DefaultYesNoDialog
        /// </summary>
        /// <param name="message">Сообщение</param>
        public DefaultYesNoDialog(string message)
        {
            InitializeComponent();
            Message = message;

            TextTB.Text = Message;
            Message = message;
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Да"
        /// </summary>
        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Нет"
        /// </summary>
        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
