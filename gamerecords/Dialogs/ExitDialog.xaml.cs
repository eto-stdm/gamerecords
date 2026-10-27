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
    /// Логика взаимодействия для ExitDialog.xaml
    /// </summary>
    public partial class ExitDialog : Window
    {
        /// <summary>
        /// Конструктор окна ExitDialog
        /// </summary>
        public ExitDialog()
        {
            InitializeComponent();
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
