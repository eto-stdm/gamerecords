using gamerecords.Classes;
using gamerecords.Dialogs;
using gamerecords.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        int failedPassword = 0;

        public SignInPage()
        {
            InitializeComponent();
        }

        private void ShowCaptcha()
        {
            CaptchaDialog dialog = new CaptchaDialog();
            bool? result = dialog.ShowDialog();
        }

        private void RedirectAfterCorrect(Players player)
        {
            Core.current_player = player;
            NavigationService.Navigate(new CatalogPage());
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            int attemptAmount = 3;
            Players player = Core.Context.Players.FirstOrDefault(p => p.nickname == LoginTB.Text.ToLower());
            if (player != null)
            {
                string enteredHashedPassword = Core.GetHash(PasswordTB.Password);
                if (player.password == enteredHashedPassword && failedPassword < attemptAmount) { RedirectAfterCorrect(player);  }
                else if (player.password == enteredHashedPassword && failedPassword >= attemptAmount) 
                {
                    ShowCaptcha();
                    RedirectAfterCorrect(player);
                }
                else if (player.password != enteredHashedPassword && failedPassword < attemptAmount - 1)
                {
                    failedPassword++;
                    MessageBox.Show("Неверный пароль!"); /*диалог неверный пароль*/
                }
                else
                {
                    failedPassword++;
                    MessageBox.Show("Неверный пароль!"); /*диалог неверный пароль*/
                    ShowCaptcha();
                }
            }
            else if (failedPassword < attemptAmount - 1)
            {
                failedPassword++;
                MessageBox.Show("Неверный пароль!"); /*диалог неверный пароль*/
            }
            else
            {
                failedPassword++;
                MessageBox.Show("Неверный пароль!"); /*диалог неверный пароль*/
                ShowCaptcha();
            }
        }
    }
}
