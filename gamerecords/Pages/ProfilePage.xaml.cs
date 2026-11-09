using gamerecords.Classes;
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

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        /// <summary>
        /// Конструктор страницы ProfilePage
        /// </summary>
        public ProfilePage()
        {
            InitializeComponent();

            // Вывод никнейма и его рекордов
            NickNameTB.Text += Core.current_player.nickname;
            RecordsLB.ItemsSource = Core.Context.Records.Where(x => x.player_id == Core.current_player.id).ToList();
        }

        private void RecordsLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = RecordsLB.SelectedItem as Records;
            if (selectRecord == null) return;

            // Открытие видео в браузере
            Process.Start(selectRecord.video);
        }
    }
}
