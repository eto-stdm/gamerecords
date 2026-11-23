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
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
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
        Games game { get; set; }

        /// <summary>
        /// Конструктор страницы GamePage
        /// </summary>
        /// <param name="game">Выбранная игра</param>
        public GamePage(Games game)
        {
            InitializeComponent();
            this.game = game;

            // Изменение раздела
            ChangeBorder();

            // Заполнение изображения и названия игры 
            GameIB.ImageSource = new BitmapImage(new Uri($"../../{game.image}", UriKind.Relative));
            GameNameTB.Text = $"{game.name} ({game.created_year.Year})";

            // Заполнение строки с жанрами
            List<string> tags = new List<string>();
            foreach (GamesGenres g in Core.Context.GamesGenres.ToList())
            {
                if (g.game_id == game.id) { tags.Add(g.Genres.genre); }
            }    
            GameTagsTB.ItemsSource = tags;


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

            //List<Records> curGameRecords = Core.Context.Records.Where(x => x.GamesCategories.Games.id == game.id).ToList();

            List<Records> curGameRecords = Core.Context.Records
             .Where(x => x.GamesCategories.game_id == game.id && x.status_id == 2)
             .ToList();


            TimeSpan totalTime = new TimeSpan();
            totalTime += curGameRecords.Aggregate(TimeSpan.Zero, (sum, item) => sum + item.total_time);
            DateTime dt = DateTime.MinValue + totalTime;

            int uniquePlayersCount = curGameRecords
                .Select(x => x.player_id)
                .Distinct()
                .Count();


            // Расчёт чисел статистики (по всей игре)
            StatTotalRunsTB.Text += $"{curGameRecords.Count}";

            if (dt.Year - 1 > 0) StatTotalTimeTB.Text += $"{dt.Year - 1:D2}г ";
            if (dt.Month - 1 > 0) StatTotalTimeTB.Text += $"{dt.Month - 1:D2}мес ";
            if (dt.Day - 1 > 0) StatTotalTimeTB.Text += $"{dt.Day - 1:D2}д ";
            if (dt.Hour > 0) StatTotalTimeTB.Text += $"{dt.Hour:D2}ч ";
            if (dt.Minute > 0) StatTotalTimeTB.Text += $"{dt.Minute:D2}м ";
            if (dt.Second > 0) StatTotalTimeTB.Text += $"{dt.Second:D2}с ";
            if (dt == DateTime.MinValue) { StatTotalTimeTB.Text += "0с"; }

            StatTotalPlayersTB.Text += $"{uniquePlayersCount}";
            StatTotalGuidesTB.Text += $"{Core.Context.Guides.Where(x => x.game_id == game.id).Count()}";

            // Заполнение списка гайдов
            GuidesLB.ItemsSource = Core.Context.Guides.Where(x => x.Games.id == game.id).ToList();

            // Инициализация графика
            StatChart.ChartAreas.Add(new ChartArea("Main"));
            StatChart.Series.Add(new Series("Точки"));

            StatChart.Series["Точки"].ChartArea = "Main";
            StatChart.Series["Точки"].ChartType = SeriesChartType.Line;
        }

        /// <summary>
        /// Изменение раздела (по умолчанию - лидерборд)
        /// </summary>
        private void ChangeBorder(object sender = null, RoutedEventArgs e = null)
        {
            // По умолчанию отображаются лидерборды
            if (sender == null) { GameLeaderboardB.Visibility = Visibility.Visible; return; }

            var btn = (System.Windows.Controls.Button)sender;
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
            Core.ShowAppDialog(() => new RulesDialog(FindGameCategory()));

            //RulesDialog dialog = new RulesDialog(FindGameCategory());
            //dialog.Owner = System.Windows.Application.Current.MainWindow;
            //dialog.ShowDialog();
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
            return Core.Context.GamesCategories.FirstOrDefault(x => x.Categories.category == catname && x.game_id == game.id);
        }

        /// <summary>
        /// Вывод записей в лидерборде согласно категории
        /// </summary>
        private void ChangingCategory()
        {
            string catname = CategoryCB.SelectedItem.ToString();

            // Выбираем рекорды, из конкретной категории и с определённым статусом
            List<Records> curCatRecords = Core.Context.Records.Where(x => x.GamesCategories.Categories.category == catname && x.GamesCategories.Games.id == game.id && x.status_id == 2).ToList();

            // Делаем так, чтобы у конкретного пользователя была только одна запись в лидерборде (с самым мальеньким временем)
            List<Records> curOnePlayerRecords = curCatRecords
                .GroupBy(x => x.player_id)
                .Select(x => x.OrderBy(y => y.total_time).FirstOrDefault())
                .ToList();
            LeaderboardLB.ItemsSource = curOnePlayerRecords;
        }

        /// <summary>
        /// Обработка изменения категории
        /// </summary>
        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangingCategory();
        }

        /// <summary>
        /// Обработка двойного нажатия на запись в гайде
        /// </summary>
        private void GuidesLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectGuide = GuidesLB.SelectedItem as Guides;
            if (selectGuide == null) return;

            Core.ShowAppDialog(() => new GuidesDialog(selectGuide));
        }

        private void CreateGuideBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddGuidesPage(game));
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CatalogPage());
        }
    }
}
