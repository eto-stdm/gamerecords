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

        /// <summary>
        /// Конструктор страницы SignInPage
        /// </summary>
        public SignInPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Переход на основную страницу после корректного ввода пароля
        /// </summary>
        /// <param name="player">Текущий пользователь</param>
        private void RedirectAfterCorrect(Players player)
        {
            Core.current_player = player;
            NavigationService.Navigate(new MainPage());
        }

        /// <summary>
        /// Обработка кнопки "Войти"
        /// </summary>
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
                    Core.ShowAppDialog(() => new CaptchaDialog());
                    RedirectAfterCorrect(player);
                }
                else if (player.password != enteredHashedPassword && failedPassword < attemptAmount - 1)
                {
                    failedPassword++;
                    Core.ShowAppDialog(() => new DefaultOKDialog("Неверный пароль!"));
                }
                else
                {
                    failedPassword++;
                    Core.ShowAppDialog(() => new DefaultOKDialog("Неверный пароль!"));
                    Core.ShowAppDialog(() => new CaptchaDialog());
                }
            }
            else if (failedPassword < attemptAmount - 1)
            {
                failedPassword++;
                Core.ShowAppDialog(() => new DefaultOKDialog("Неверный пароль!"));
            }
            else
            {
                failedPassword++;
                Core.ShowAppDialog(() => new DefaultOKDialog("Неверный пароль!"));
                Core.ShowAppDialog(() => new CaptchaDialog());
            }
        }

        /// <summary>
        /// Обработка кнопки "Забыли пароль?"
        /// </summary>
        private void ForgotPasswordHL_Click(object sender, RoutedEventArgs e)
        {
            Core.ShowAppDialog(() => new ForgotPasswordDialog(LoginTB.Text));
        }

        /// <summary>
        /// Обработка кнопки "Создать аккаунт"
        /// </summary>
        private void SignUpHL_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUpPage());
        }
    }
}
