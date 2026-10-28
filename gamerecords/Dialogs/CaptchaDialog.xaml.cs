using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для CaptchaDialog.xaml
    /// </summary>
    public partial class CaptchaDialog : Window
    {
        bool canClose = false;

        /// <summary>
        /// Конструктор окна CaptchaDialog
        /// </summary>
        public CaptchaDialog()
        {
            InitializeComponent();
            CaptchaText();
        }

        /// <summary>
        /// Генерация капчи
        /// </summary>
        private void CaptchaText()
        {
            List<char> letters = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            List<char> numbers = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            string textcap = "";
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                textcap += letters[rnd.Next(0, letters.Count - 1)];
            }

            for (int i = 0; i < 2; i++)
            {
                textcap += numbers[rnd.Next(0, numbers.Count - 1)];
            }

            CaptchaTB.Text = textcap;
        }

        /// <summary>
        /// Обработка нажатия клавишы "Ввести"
        /// </summary>
        private async void CheckCaptchaB_Click(object sender, RoutedEventArgs e)
        {
            if (EnterCaptchaTB.Text == null || EnterCaptchaTB.Text != CaptchaTB.Text)
            {
                CaptchaTB.Foreground = new SolidColorBrush(Colors.Red);
                await Task.Delay(500);
                Thread.Sleep(500);
                EnterCaptchaTB.Clear();
                CaptchaText();
                CaptchaTB.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                CaptchaTB.Foreground = new SolidColorBrush(Colors.LightGreen);
                await Task.Delay(500);
                Thread.Sleep(500);
                canClose = true;
                this.Close();
            }
        }

        /// <summary>
        /// Обработка закрытия окна
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Не закрывать, если капча решена неправильно
            if (!canClose) { e.Cancel = true; }
        }
    }
}
