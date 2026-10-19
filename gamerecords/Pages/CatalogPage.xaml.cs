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
using gamerecords.Classes;
using gamerecords.Models;

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
            GamesLB.ItemsSource = Core.Context.Games.ToList();

            List<string> platforms = new List<string>() { "Все платформы" };
            foreach (Platforms platform in Core.Context.Platforms.ToList())
            {
                platforms.Add(Convert.ToString(platform.platform));
            }
            PlatformCB.ItemsSource = platforms;
            PlatformCB.SelectedIndex = 0;

            List<string> sort = new List<string>() 
            {
                "Названию",
                "Году релиза (возрастание)",
                "Году релиза (убывание)",
                "Количеству игроков (возрастание)",
                "Количеству игроков (убывание)",
                "Дате добавления (возрастание)",
                "Дате добавления (убывание)",
            };
            SortByCB.ItemsSource = sort;
            SortByCB.SelectedIndex = 5;
        }

        private void PlatformCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SortByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RandomGameBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
