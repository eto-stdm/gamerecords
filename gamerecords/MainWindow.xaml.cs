using gamerecords.Dialogs;
using gamerecords.Classes;
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

namespace gamerecords
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Добавление тем в список
            ChooseThemeCB.ItemsSource = new List<string>{ "Стандартная", "По заданию" };
            ChooseThemeCB.SelectedIndex = 0;
        }
        /// <summary>
        /// Логика при загрузке окна MainWindow
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание таймера, тикающего каждую секунду
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += (o, t) => {
                DateTimeNow.Text = "🗓: " + DateTime.Now.ToString().Replace(" ", " 🕓: ");
            };
            timer.Start();
        }
        /// <summary>
        /// Обработка изменения темы
        /// </summary>
        private void ChooseThemeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(ChooseThemeCB.SelectedItem.ToString())
            {
                case "Стандартная": Core.ApplyTheme("Styles/DictionaryDefault.xaml"); break;
                case "По заданию": Core.ApplyTheme("Styles/DictionaryTask.xaml"); break;
                default: break;
            }
            if (Core.current_player != null)
            {
                Core.current_player.theme = ChooseThemeCB.SelectedItem.ToString();
                Core.Context.SaveChanges();
            }
        }
        /// <summary>
        /// Обработка кнопки закрытия приложения
        /// </summary>
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Core.ShowAppDialog(() => new DefaultYesNoDialog("Вы действительно хотите выйти?")) == true) { this.Close(); }
        }
    }
}