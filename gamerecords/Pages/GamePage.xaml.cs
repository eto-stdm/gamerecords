using gamerecords.Classes;
using gamerecords.Dialogs;
using gamerecords.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        Games Game { get; set; }

        /// <summary>
        /// Конструктор страницы GamePage
        /// </summary>
        /// <param name="game">Выбранная игра</param>
        public GamePage(Games game)
        {
            InitializeComponent();
            this.Game = game;

            // Изменение раздела
            ChangeBorder();

            // Заполнение изображения и названия игры 
            GameIB.ImageSource = new BitmapImage(new Uri($"../../{game.image}", UriKind.Relative));
            GameNameTB.Text = $"{game.name} ({game.created_year.Year})";

            // Заполнение строки с жанрами
            List<GamesPlatforms> gamesPlatforms = Core.Context.GamesPlatforms.Where(x => x.game_id == game.id).ToList();
            string textPlatforms = "";
            foreach (GamesPlatforms gamepl in gamesPlatforms)
            {
                textPlatforms += $"{Core.Context.Platforms.First(x => x.id == gamepl.platform_id).platform}, ";
            }
            GameTagsTB.Text = "Жанры: " + textPlatforms.Substring(0, textPlatforms.Length - 2);

            // Заполнение ComboBox с категориями
            List<GamesCategories> gamesCategories = Core.Context.GamesCategories.Where(x => x.game_id == game.id).ToList();
            List<string> cat = new List<string>();
            foreach (GamesCategories gamec in gamesCategories)
            {
                cat.Add(Core.Context.Categories.FirstOrDefault(x => x.id == gamec.category_id).category);
            }
            CategoryCB.ItemsSource = cat;
            CategoryCB.SelectedIndex = 0;

            // Заполнение лидерборда на основе категории
            ChangingCategory();

            // Заполнение списка гайдов
            //GuidesLB.ItemsSource = Core.Context.Guides.Where(x => x.Games.id == Game.id).ToList();
        }

        /// <summary>
        /// Изменение раздела (по умолчанию - лидерборд)
        /// </summary>
        private void ChangeBorder(object sender = null, RoutedEventArgs e = null)
        {
            // По умолчанию отображаются лидерборды
            if (sender == null) { GameLeaderboardB.Visibility = Visibility.Visible; return; }

            var btn = (Button)sender;
            GameLeaderboardB.Visibility = Visibility.Collapsed;
            GameGuidesB.Visibility = Visibility.Collapsed;
            GameStatsB.Visibility = Visibility.Collapsed;

            switch(btn.Name)
            {
                case "GameLeaderboardBtn":
                    GameLeaderboardB.Visibility = Visibility.Visible;
                    break;
                case "GameGuidesBtn":
                    GameGuidesB.Visibility = Visibility.Visible;
                    break;
                case "GameStatsBtn":
                    GameStatsB.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка двойного нажатия на запись в лидерборде
        /// </summary>
        private void LeaderboardLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = LeaderboardLB.SelectedItem as Records;
            if (selectRecord == null) return;

            // Открытие видео в браузере
            Process.Start(selectRecord.video);
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Правила"
        /// </summary>
        private void RulesBtn_Click(object sender, RoutedEventArgs e)
        {
            RulesDialog dialog = new RulesDialog(FindGameCategory());
            dialog.Owner = System.Windows.Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Опубликовать забег"
        /// </summary>
        private void SubmitRunBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SubmitPage(FindGameCategory()));
        }

        /// <summary>
        /// Находит категорию игры
        /// </summary>
        /// <returns>Категорию игры</returns>
        private GamesCategories FindGameCategory()
        {
            string catname = CategoryCB.SelectedItem.ToString();
            return Core.Context.GamesCategories.FirstOrDefault(x => x.Categories.category == catname && x.game_id == Game.id);
        }

        /// <summary>
        /// Вывод записей в лидерборде согласно категории
        /// </summary>
        private void ChangingCategory()
        {
            string catname = CategoryCB.SelectedItem.ToString();
            LeaderboardLB.ItemsSource = Core.Context.Records.Where(x => x.GamesCategories.Categories.category == catname && x.is_verified == true).ToList();
        }

        /// <summary>
        /// Обработка изменения категории
        /// </summary>
        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangingCategory();
        }
    }
}
