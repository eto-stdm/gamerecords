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
        /// <summary>
        /// Конструктор страницы MainPage
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Выбор темы на основании того, вошёл пользователь или нет
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

                // Показ кнопки с админкой у администраторов
                if (Core.current_player.role_id == 1) { AdminBtn.Visibility = Visibility.Visible; }
                else { AdminBtn.Visibility = Visibility.Collapsed; }
            }
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Админ"
        /// </summary>
        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new AdminPanelPage());
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Игры"
        /// </summary>
        private void GamesBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new CatalogPage(""));
        }

        /// <summary>
        /// Обработка нажатия на кнопку "🔍"
        /// </summary>
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new CatalogPage(SearchTB.Text));
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Профиль"
        /// </summary>
        private void ProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPageFrame.NavigationService.Navigate(new ProfilePage());
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Выход"
        /// </summary>
        private void AccountExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // Обнуление текущего пользователя и переход на страницу входа
            Core.current_player = null;
            NavigationService.Navigate(new SignInPage());
        }
    }
}
