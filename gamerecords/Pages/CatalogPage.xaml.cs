using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using gamerecords.Classes;
using gamerecords.Models;

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private string Search { get; set; }
        private List<Games> CurrentGames = new List<Games>();

        /// <summary>
        /// Конструктор страницы CatalogPage (c аргументом null)
        /// </summary>
        public CatalogPage() : this(null) { }

        /// <summary>
        /// Конструктор страницы CatalogPage (с аргументом не null)
        /// </summary>
        /// <param name="Search">Поисковая строка</param>
        public CatalogPage(string Search)
        {
            InitializeComponent();
            this.Search = Search;

            List<string> platforms = new List<string>() { "Все платформы" };
            foreach (Platforms platform in Core.Context.Platforms.ToList())
            {
                platforms.Add(Convert.ToString(platform.platform));
            }
            PlatformCB.ItemsSource = platforms;
            PlatformCB.SelectedIndex = 0;

            List<string> sort = new List<string>()
            {
                "Названию (возрастание)",
                "Названию (убывание)",
                "Году релиза (возрастание)",
                "Году релиза (убывание)",
            };
            SortByCB.ItemsSource = sort;
            SortByCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Общая функция для сортировки игр в ListBox
        /// </summary>
        private void UpdateCatalog()
        {
            if (SortByCB.SelectedItem == null || PlatformCB.SelectedItem == null) { return; }

            List<Games> FCurrentGames = Core.Context.Games.ToList();

            // Поиск
            if (!string.IsNullOrWhiteSpace(Search)) 
            {
                FCurrentGames = FCurrentGames.Where(x => x.name.ToLower().Contains(Search.ToLower())).ToList();
            }

            // Фильтрация
            if (PlatformCB.SelectedIndex > 0)
            {
                string selPlatform = PlatformCB.SelectedItem.ToString();
                Platforms selectedPlatform = Core.Context.Platforms.FirstOrDefault(x => x.platform == selPlatform);

                if (selectedPlatform != null)
                {
                    List<int> gameIdPlat = Core.Context.GamesPlatforms
                                          .Where(x => x.platform_id == selectedPlatform.id)
                                          .Select(x => x.game_id)
                                          .ToList();
                    FCurrentGames = FCurrentGames.Where(x => gameIdPlat.Contains(x.id)).ToList();
                }
            }
            
            // Сортировка
            switch (SortByCB.SelectedItem.ToString())
            {
                case "Названию (возрастание)": CurrentGames = FCurrentGames.OrderBy(x => x.name).ToList(); break;
                case "Названию (убывание)": CurrentGames = FCurrentGames.OrderByDescending(x => x.name).ToList(); break;
                case "Году релиза (возрастание)": CurrentGames = FCurrentGames.OrderBy(x => x.created_year).ToList(); break;
                case "Году релиза (убывание)": CurrentGames = FCurrentGames.OrderByDescending(x => x.created_year).ToList(); break;
                default: CurrentGames = FCurrentGames; break;
            }

            GamesLB.ItemsSource = CurrentGames;
        }

        /// <summary>
        /// Обработка изменения платформы
        /// </summary>
        private void PlatformCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCatalog();
        }

        /// <summary>
        /// Обработка изменения "сортировки по..."
        /// </summary>
        private void SortByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCatalog();
        }

        /// <summary>
        /// Обработка нажатия по кнопке "Случайная игра"
        /// </summary>
        private void RandomGameBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Games> games = Core.Context.Games.ToList();

            Random rnd = new Random();
            int gameid = rnd.Next(games.Count);

            NavigationService.Navigate(new GamePage(games[gameid]));
        }

        /// <summary>
        /// Обработка двойного нажатия по игре
        /// </summary>
        private void GamesLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectGame = GamesLB.SelectedItem as Games;
            if (selectGame == null) return;

            NavigationService.Navigate(new GamePage(selectGame));
        }
    }
}
