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
    /// Логика взаимодействия для SubmitPage.xaml
    /// </summary>
    public partial class SubmitPage : Page
    {
        GamesCategories GameCat { get; set; }

        /// <summary>
        /// Конструктор страницы SubmitPage
        /// </summary>
        /// <param name="gamecat">Категория игры</param>
        public SubmitPage(GamesCategories gamecat)
        {
            InitializeComponent();
            this.GameCat = gamecat;

            // Инициализация CategoryCB
            List<GamesCategories> gamesCategories = Core.Context.GamesCategories.Where(x => x.game_id == GameCat.game_id).ToList();
            List<string> cat = new List<string>();
            foreach (GamesCategories gamec in gamesCategories)
            {
                cat.Add(Core.Context.Categories.FirstOrDefault(x => x.id == gamec.category_id).category);
            }
            CategoryCB.ItemsSource = cat;
            int indexOfCat = cat.IndexOf(GameCat.Categories.category);
            CategoryCB.SelectedIndex = indexOfCat;

            // Инициализация PlatformCB
            List<GamesPlatforms> gamesPlatforms = Core.Context.GamesPlatforms.Where(x => x.game_id == GameCat.game_id).ToList();
            List<string> plat = new List<string>();
            foreach (GamesPlatforms gamec in gamesPlatforms)
            {
                plat.Add(Core.Context.Platforms.FirstOrDefault(x => x.id == gamec.platform_id).platform);
            }
            PlatformCB.ItemsSource = plat;
            PlatformCB.SelectedIndex = 0;

            // Значение по умолчанию - Только что [закончил играть]
            JustFinishedRB.IsChecked = true;
        }

        /// <summary>
        /// Обработка отметки "Только что [закончил играть]"
        /// </summary>
        private void JustFinishedRB_Checked(object sender, RoutedEventArgs e)
        {
            DateTimeSP.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Обработка отметки "Кастомное [время окончания игры]"
        /// </summary>
        private void CustomRB_Checked(object sender, RoutedEventArgs e)
        {
            DateTimeSP.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Опубликовать [забег]"
        /// </summary>
        private void SubmitRunBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TimeTB.Text != "" && VideoTB.Text != "")
            {

                if (TimeSpan.TryParse(TimeTB.Text, out TimeSpan time) == false) { Core.ShowAppDialog(() => new DefaultOKDialog("Неверно заполненные поля!")); }
                
                DateTime dateTime = new DateTime();
                if (Convert.ToBoolean(JustFinishedRB.IsChecked)) { dateTime = DateTime.Now; }
                else
                {
                    if (TimeTB.Text != "" && DateTimeDP != null)
                    {
                        if (TimeSpan.TryParse(TimeTB.Text, out TimeSpan datetimetime) == false) { Core.ShowAppDialog(() => new DefaultOKDialog("Неверно заполненные поля!")); }
                        dateTime = Convert.ToDateTime(DateTimeDP.SelectedDate);
                        dateTime = dateTime.Add(time);
                    }
                    else { Core.ShowAppDialog(() => new DefaultOKDialog("Заполните все необходимые поля!")); }
                }
                string categorystr = CategoryCB.SelectedItem.ToString();
                string platformstr = PlatformCB.SelectedItem.ToString();

                Records newrecord = new Records()
                {
                    category_id = Core.Context.GamesCategories.FirstOrDefault(x => x.Categories.category == categorystr && x.Games.id == GameCat.game_id).id,
                    player_id = Core.current_player.id,
                    total_time = time,
                    platform_id = Core.Context.GamesPlatforms.FirstOrDefault(x => x.Platforms.platform == platformstr && x.Games.id == GameCat.game_id).id,
                    video = VideoTB.Text,
                    date = dateTime,
                };

                List<Records> tempRec = Core.Context.Records.ToList();
                try
                {
                    bool isexist = Core.Context.Records.Any(x =>
                                        x.category_id == newrecord.category_id &&
                                        x.player_id == newrecord.player_id &&
                                        x.total_time == newrecord.total_time &&
                                        x.platform_id == newrecord.platform_id);
                    if (isexist)
                    {
                        Core.ShowAppDialog(() => new DefaultOKDialog("Подобный рекорд уже существует!"));
                    }
                    else
                    {
                        Core.Context.Records.Add(newrecord);
                        Core.Context.SaveChanges();
                        Core.ShowAppDialog(() => new DefaultOKDialog("Рекорд успешно опубликован! Вы сможете увидеть его в таблице лидеров после проверки модераторами."));
                        NavigationService.Navigate(new CatalogPage());
                    }
                }
                catch { Core.ShowAppDialog(() => new DefaultOKDialog("Неверно заполненные поля!")); }
            }
            else { Core.ShowAppDialog(() => new DefaultOKDialog("Заполните все необходимые поля!"));  }
        }
    }
}
