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
        string Search { get; set; }
        readonly List<Games> CurrentGames = new List<Games>();
        public CatalogPage(string Search)
        {
            InitializeComponent();
            this.Search = Search;

            if (Search != "")
            {
                CurrentGames = Core.Context.Games.Where(x => x.name.ToLower().Contains(Search.ToLower())).ToList();
            }
            else
            {
                CurrentGames = Core.Context.Games.ToList();
            }
            GamesLB.ItemsSource = CurrentGames.OrderBy(x => x.name);

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
                "Количеству игроков (возрастание)",
                "Количеству игроков (убывание)",
            };
            SortByCB.ItemsSource = sort;
            SortByCB.SelectedIndex = 0;
            
        }

        //public CatalogPage() : this(null) { }

        private void Sort()
        {

        }

        private void PlatformCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlatformCB.SelectedIndex == 0) { GamesLB.ItemsSource = Core.Context.Games.ToList(); }
            else { GamesLB.ItemsSource = Core.Context.Games.Where().ToList(); }
            Sort();
        }

        private void CountGamesPlayers(List<Games> games)
        {
            List<>
            games


            //docs = docs.OrderBy(d => docsIds.IndexOf(d.Id)).ToList();

        }

        private void SortByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SortByCB.SelectedItem.ToString())
            {
                case "Названию (возрастание)": GamesLB.ItemsSource = CurrentGames.OrderBy(x => x.name); break;
                case "Названию (убывание)": GamesLB.ItemsSource = CurrentGames.OrderByDescending(x => x.name); break;
                case "Году релиза (возрастание)": GamesLB.ItemsSource = CurrentGames.OrderBy(x => x.created_year); break;
                case "Году релиза (убывание)": GamesLB.ItemsSource = CurrentGames.OrderByDescending(x => x.created_year); break;
                case "Количеству игроков (возрастание)": break;
                case "Количеству игроков (убывание)": break;
                default: break;
            }
        }

        private void RandomGameBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Games> games = Core.Context.Games.ToList();

            Random rnd = new Random();
            int gameid = rnd.Next(games.Count);

            NavigationService.Navigate(new GamePage(games[gameid]));
        }
    }
}
