using gamerecords.Classes;
using gamerecords.Dialogs;
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

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddGuidesPage.xaml
    /// </summary>
    public partial class AddGuidesPage : Page
    {
        Games Game { get; set; }
        public AddGuidesPage(Games game)
        {
            InitializeComponent();
            this.Game = game;
            UserTB.Text = Core.current_player.nickname;
        }

        private void CreateGuideBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(HeaderTB.Text) || string.IsNullOrEmpty(TextTB.Text))
            {
                Core.ShowAppDialog(() => new DefaultOKDialog("Заполните необходимые поля!"));
                return;
            }

            if (Core.Context.Guides.Any(x => x.title == HeaderTB.Text && x.game_id == Game.id))
            {
                Core.ShowAppDialog(() => new DefaultOKDialog("Гайд с таким заголовком уже существует!"));
                return;
            }

            Guides newGuide = new Guides
            {
                game_id = Game.id,
                title = HeaderTB.Text,
                description = TextTB.Text,
                author_id = Core.current_player.id,
                created_date = DateTime.Now,
            };
            Core.Context.Guides.Add(newGuide);
            Core.Context.SaveChanges();
            Core.ShowAppDialog(() => new DefaultOKDialog("Гайд успешно опубликован!"));
            NavigationService.Navigate(new GamePage(Game));
            return;
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
