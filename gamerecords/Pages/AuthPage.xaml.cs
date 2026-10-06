using gamerecords.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LogTB_Click(object sender, RoutedEventArgs e)
        {
            Players player = Core.Context.Players.FirstOrDefault(p => p.nickname == NicknameTB.Text.ToLower());
            if (player != null)
            {
                Core.current_player = player;
                NavigationService.Navigate(new MainPage());
            }
            else { MessageBox.Show("Пользователь не найден!"); }
        }

        private void RegTB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}
