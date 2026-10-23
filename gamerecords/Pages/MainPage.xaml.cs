using gamerecords.Classes;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            if (Core.current_player == null) { }
            else { 
                if (Core.current_player.theme != null)
                {
                    switch (Core.current_player.theme)
                    {
                        case "Стандартная": Core.ApplyTheme("Styles/DictionaryDefault.xaml"); break;
                        case "По заданию": Core.ApplyTheme("Styles/DictionaryTask.xaml"); break;
                        default: break;
                    }
                }

                if (Core.current_player.role_id == 1) { AdminBtn.Visibility = Visibility.Visible; }
                else { AdminBtn.Visibility = Visibility.Collapsed; }
            }
        }

        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new AdminPanelPage());
        }

        private void GamesBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new CatalogPage(""));
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new CatalogPage(SearchTB.Text));
        }

        private void ProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new ProfilePage());
        }
    }
}
