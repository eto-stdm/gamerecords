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

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelPage.xaml
    /// </summary>
    public partial class AdminPanelPage : Page
    {
        /// <summary>
        /// Конструктор страницы AdminPanelPage
        /// </summary>
        public AdminPanelPage()
        {
            InitializeComponent();
            UpdateListBox();
        }

        /// <summary>
        /// Обновление ListBox
        /// </summary>
        private void UpdateListBox()
        {
            RecordsLB.ItemsSource = Core.Context.Records.Where(x => x.is_verified == false).ToList();
        }

        /// <summary>
        /// Обработка двойного нажатия на запись в ListBox
        /// </summary>
        private void RecordsLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = RecordsLB.SelectedItem as Records;
            if (selectRecord == null) return;

            
            try { Process.Start(selectRecord.video); }
            catch
            {
                Core.ShowAppDialog(() => new DefaultYesNoDialog("Видео нечитаемо, удалить рекорд?"));
                Core.Context.Records.Remove(selectRecord);
                Core.Context.SaveChanges();

                Core.ShowAppDialog(() => new DefaultOKDialog("Рекорд не прошёл проверку и был удалён из базы данных!"));
                UpdateListBox();
                return;
            }

            if (Core.ShowAppDialog(() => new DefaultYesNoDialog("Рекорд соответствует требованиям?")) == true)
            {
                selectRecord.is_verified = true;
                Core.Context.SaveChanges();
                Core.ShowAppDialog(() => new DefaultOKDialog("Рекорд был верифицирован!"));
            }
            else
            {
                Core.Context.Records.Remove(selectRecord);
                Core.Context.SaveChanges();
                Core.ShowAppDialog(() => new DefaultOKDialog("Рекорд не прошёл проверку и был удалён из базы данных!"));
            }
            UpdateListBox();
        }
    }
}
