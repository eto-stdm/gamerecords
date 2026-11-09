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

            TimeTB.Text = "00:00:00";
            DateTimeTB.Text = "00:00:00";
            DateTimeDP.SelectedDate = DateTime.Now;
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
            // Проверка на заполненность времени забега и видео
            if (string.IsNullOrEmpty(TimeTB.Text) || string.IsNullOrEmpty(VideoTB.Text))
            {
                Core.ShowAppDialog(() => new DefaultOKDialog("Заполните все необходимые поля!"));
                return;
            }

            // Парсинг времени забега
            if (!TimeSpan.TryParse(TimeTB.Text, out TimeSpan time))
            {
                Core.ShowAppDialog(() => new DefaultOKDialog("Время забега заполнено неверно!"));
                return;
            }

            DateTime dateTime = new DateTime();

            // Если выбрано текущее время
            if (JustFinishedRB.IsChecked == true) { dateTime = DateTime.Now; }
            else
            {
                // Проверка на заполненность кастомной даты и времени
                if (string.IsNullOrEmpty(DateTimeTB.Text) || DateTimeDP.SelectedDate == null)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Заполните дату и время забега!"));
                    return;
                }

                if (TimeSpan.TryParse(DateTimeTB.Text, out TimeSpan datetimetime) == false)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Время в дате забега заполнено неверно"));
                    return;
                }
                dateTime = Convert.ToDateTime(DateTimeDP.SelectedDate);
                dateTime = dateTime.Add(datetimetime);
            }

            //try
            //{
                string categorystr = CategoryCB.SelectedItem.ToString();
                string platformstr = PlatformCB.SelectedItem.ToString();

                int catid = Core.Context.GamesCategories.FirstOrDefault(x => x.Categories.category == categorystr && x.Games.id == GameCat.game_id).id;
                int platid = Core.Context.GamesPlatforms.FirstOrDefault(x => x.Platforms.platform == platformstr && x.Games.id == GameCat.game_id).id;

                bool isexist = Core.Context.Records.Any(x => x.category_id == catid && x.player_id == Core.current_player.id && x.total_time == time && x.platform_id == platid);

                if (isexist)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Подобный рекорд уже существует!"));
                    return;
                }

                Records newrecord = new Records()
                {
                    category_id = catid,
                    player_id = Core.current_player.id,
                    total_time = time,
                    platform_id = platid,
                    video = VideoTB.Text,
                    date = dateTime,
                };

                Core.Context.Records.Add(newrecord);
                Core.Context.SaveChanges();
                Core.ShowAppDialog(() => new DefaultOKDialog("Рекорд успешно опубликован! Вы сможете увидеть его в таблице лидеров после проверки модераторами."));
                NavigationService.Navigate(new CatalogPage());
                return;
            //}
            //catch { Core.ShowAppDialog(() => new DefaultOKDialog("Неверно заполненные поля!")); } // не помню зачем
        }
    }
}