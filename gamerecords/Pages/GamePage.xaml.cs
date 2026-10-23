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


            //List<Records> records = new List<Records>()
            //{

            //}; // добавление заголовка
            //records.AddRange(Core.Context.Records.ToList());
            //LeaderboardLB.ItemsSource = records;
            LeaderboardLB.ItemsSource = Core.Context.Records.ToList();
        }
        private void Load()
        {
            GameLeaderboardB.Visibility = Visibility.Visible;
            GameGuidesB.Visibility = Visibility.Collapsed;
            GameStatsB.Visibility = Visibility.Collapsed;
        }

        private void GameLeaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void GameGuidesBtn_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardB.Visibility = Visibility.Collapsed;
            GameGuidesB.Visibility = Visibility.Visible;
            GameStatsB.Visibility = Visibility.Collapsed;
        }

        private void GameStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            GameLeaderboardB.Visibility = Visibility.Collapsed;
            GameGuidesB.Visibility = Visibility.Collapsed;
            GameStatsB.Visibility = Visibility.Visible;
        }

        private void LeaderboardLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = LeaderboardLB.SelectedItem as Records;

            if (selectRecord == null) return;

            Process.Start(selectRecord.video);
        }

        private void RulesBtn_Click(object sender, RoutedEventArgs e)
        {
            RulesDialog dialog = new RulesDialog(Game);
            dialog.ShowDialog();
        }

        private void SubmitRunBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SubmitPage(Game));
        }
    }
}
