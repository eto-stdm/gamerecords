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
        public GamePage(Games game)
        {
            InitializeComponent();
            this.Game = game;
            Load();

            GameIB.ImageSource = new BitmapImage(new Uri($"../../{game.image}", UriKind.Relative));
            GameNameTB.Text = $"{game.name} ({game.created_year.Year})";
            
            List<GamesPlatforms> gamesPlatforms = Core.Context.GamesPlatforms.Where(x => x.game_id == game.id).ToList();
            string textPlatforms = "";
            foreach (GamesPlatforms gamepl in gamesPlatforms)
            {
                textPlatforms += $"{Core.Context.Platforms.First(x => x.id == gamepl.platform_id).platform}, ";
            }
            GameTagsTB.Text = "Теги: " + textPlatforms.Substring(0, textPlatforms.Length - 2);



            List<GamesCategories> gamesCategories = Core.Context.GamesCategories.Where(x => x.game_id == game.id).ToList();
            List<string> cat = new List<string>();
            foreach (GamesCategories gamec in gamesCategories)
            {
                cat.Add(Core.Context.Categories.FirstOrDefault(x => x.id == gamec.category_id).category);
            }

            CategoryCB.ItemsSource = cat;
            CategoryCB.SelectedIndex = 0;

            ChangingCategory();

            //GuidesLB.ItemsSource = Core.Context.Guides.Where(x => x.Games.id == Game.id).ToList();
        }
        private void Load()
        {
            GameLeaderboardB.Visibility = Visibility.Visible;
            //GameGuidesB.Visibility = Visibility.Collapsed;
            //GameStatsB.Visibility = Visibility.Collapsed;
        }

        private void GameLeaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void GameGuidesBtn_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardB.Visibility = Visibility.Collapsed;
            //GameGuidesB.Visibility = Visibility.Visible;
            //GameStatsB.Visibility = Visibility.Collapsed;
        }

        private void GameStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardB.Visibility = Visibility.Collapsed;
            //GameGuidesB.Visibility = Visibility.Collapsed;
            //GameStatsB.Visibility = Visibility.Visible;
        }

        private void LeaderboardLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = LeaderboardLB.SelectedItem as Records;

            if (selectRecord == null) return;

            Process.Start(selectRecord.video);
        }

        private void RulesBtn_Click(object sender, RoutedEventArgs e)
        {

            RulesDialog dialog = new RulesDialog(FindGameCategory());
            dialog.ShowDialog();
        }

        private void SubmitRunBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SubmitPage(FindGameCategory()));
        }

        private GamesCategories FindGameCategory()
        {
            string catname = CategoryCB.SelectedItem.ToString();
            return Core.Context.GamesCategories.FirstOrDefault(x => x.Categories.category == catname && x.game_id == Game.id);
        }

        private void ChangingCategory()
        {
            string catname = CategoryCB.SelectedItem.ToString();
            LeaderboardLB.ItemsSource = Core.Context.Records.Where(x => x.GamesCategories.Categories.category == catname && x.is_verified == true).ToList();
        }

        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangingCategory();
        }
    }
}
