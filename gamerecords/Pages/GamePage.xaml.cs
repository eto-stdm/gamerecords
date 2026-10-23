using gamerecords.Classes;
using gamerecords.Models;
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

            GameIB.ImageSource = new BitmapImage(new Uri($"../../{game.image}", UriKind.Relative));
            GameNameTB.Text = game.name;
            
            List<GamesPlatforms> gamesPlatforms = Core.Context.GamesPlatforms.Where(x => x.game_id == game.id).ToList();
            string textPlatforms = "";
            foreach (GamesPlatforms gamepl in gamesPlatforms)
            {
                textPlatforms += $"{Core.Context.Platforms.First(x => x.id == gamepl.platform_id).platform}, ";
            }
            GameTagsTB.Text = "Теги: " + textPlatforms.Substring(0, textPlatforms.Length - 2);
        }
    }
}
