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
        public AdminPanelPage()
        {
            InitializeComponent();

            UpdateListBox();
        }

        private void UpdateListBox()
        {
            RecordsLB.ItemsSource = Core.Context.Records.Where(x => x.is_verified == false).ToList();
        }

        private void RecordsLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = RecordsLB.SelectedItem as Records;

            if (selectRecord == null) return;

            try
            {
                Process.Start(selectRecord.video);
            }
            catch
            {
                MessageBox.Show("Видео нечитаемо, удалить рекорд?");
                Core.Context.Records.Remove(selectRecord);
                Core.Context.SaveChanges();
                MessageBox.Show("Рекорд не прошёл проверку и был удалён из базы данных!");
                UpdateListBox();
                return;
            }


            VerifyDialog dialog = new VerifyDialog();

            if (dialog.ShowDialog() == true)
            {
                selectRecord.is_verified = true;
                Core.Context.SaveChanges();
                MessageBox.Show("Рекорд был верифицирован!");

            }
            else
            {
                Core.Context.Records.Remove(selectRecord);
                Core.Context.SaveChanges();
                MessageBox.Show("Рекорд не прошёл проверку и был удалён из базы данных!");
            }
            UpdateListBox();
        }
    }
}
