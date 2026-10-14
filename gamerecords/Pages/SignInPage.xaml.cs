using gamerecords.Classes;
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
        int failedPassword;
        public SignInPage()
        {
            InitializeComponent();
        }
        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            Players player = Core.Context.Players.FirstOrDefault(p => p.nickname == LoginTB.Text.ToLower());
            if (player != null)
            {
                if (player.password == PasswordTB.Password)
                {
                    Core.current_player = player;
                    NavigationService.Navigate(new MainPage());
                }
                else if (failedPassword < 3) { failedPassword++; /*Показать диалог неверный пароль*/ }
                else { /*Показать диалог с капчей*/ }
            }
            else if (failedPassword < 3) { failedPassword++; /*Показать диалог неверный пароль*/ }
            else { /*Показать диалог с капчей*/ }
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUpPage());
        }

    }
}
