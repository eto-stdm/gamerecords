using gamerecords.Classes;
using gamerecords.Dialogs;
using gamerecords.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shell;

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

            List<Records> userRecords = Core.Context.Records.Where(x => x.player_id == Core.current_player.id).ToList();
            List<Guides> userGuides = Core.Context.Guides.Where(x => x.author_id == Core.current_player.id).ToList();

            // Вывод никнейма и его рекордов
            NickNameTB.Text += Core.current_player.nickname;
            CountryTB.Text += $"{Core.current_player.Countries.country} {Core.current_player.Countries.image}";
            ProfileImg.Source = new BitmapImage(new Uri(Core.current_player.image, UriKind.Relative));
            RecordsAmountTB.Text += userRecords.Count();
            RecordsCheckTB.Text += userRecords.Where(x => x.status_id == 1).Count();
            RecordsOKTB.Text += userRecords.Where(x => x.status_id == 2).Count();
            RecordsNoTB.Text += userRecords.Where(x => x.status_id == 3).Count();
            GuidesAmountTB.Text += userGuides.Count();

            RecordsLB.ItemsSource = userRecords;
            GuidesLB.ItemsSource = userGuides;

            SelectPageCB.ItemsSource = new List<string>
            {
                "Мои рекорды",
                "Мои гайды",
            };
            SelectPageCB.SelectedIndex = 0;
        }

        private void RecordsLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectRecord = RecordsLB.SelectedItem as Records;
            if (selectRecord == null) return;

            // Открытие видео в браузере
            Process.Start(selectRecord.video);
        }

        /// <summary>
        /// Обработка двойного нажатия на запись в гайде
        /// </summary>
        private void GuidesLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectGuide = GuidesLB.SelectedItem as Guides;
            if (selectGuide == null) return;

            Core.ShowAppDialog(() => new GuidesDialog(selectGuide));
        }

        /// <summary>
        /// Скрытие всех StackPanel
        /// </summary>
        private void HideAll()
        {
            RecordsLB.Visibility = Visibility.Collapsed;
            RecordsStatSP.Visibility = Visibility.Collapsed;
            GuidesLB.Visibility = Visibility.Collapsed;
            GuidesStatSP.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Изменение раздела
        /// </summary>
        private void PageChange()
        {
            string pageName = SelectPageCB.SelectedItem.ToString();
            HideAll();
            switch (pageName)
            {
                case "Мои рекорды":
                    RecordsLB.Visibility = Visibility.Visible;
                    RecordsStatSP.Visibility = Visibility.Visible;
                    break;
                case "Мои гайды":
                    GuidesLB.Visibility = Visibility.Visible;
                    GuidesStatSP.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка нажания на ListBox с названиями разделов
        /// </summary>
        private void SelectPageCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageChange();
        }

        private void AvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите изображение";
            openFileDialog.Filter = "Вся поддерживаемая графика|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";


            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string sourceFilePath = openFileDialog.FileName;

                    string targetFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Images/Avatars");
                    string destinationFilePath = System.IO.Path.Combine(targetFolder, $"avatar_{Core.current_player.nickname}.png");
                    File.Copy(sourceFilePath, destinationFilePath, true);

                    int index = destinationFilePath.IndexOf("../..");
                    string relativePath = $"/Images/Avatars/avatar_{Core.current_player.nickname}.png";

                    if (Core.current_player.image == "/Images/Avatars/avatar_default.png")
                    {
                        Core.current_player.image = relativePath;
                        Core.Context.SaveChanges();
                    }
                    
                    string absolutePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../..", relativePath.TrimStart('/')));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(absolutePath, UriKind.Absolute);
                        bitmap.EndInit();
                        bitmap.Freeze();
                        ProfileImg.Source = bitmap;
                    }

                    Core.ShowAppDialog(() => new DefaultOKDialog("Картинка успешно сохранена!"));
                }
                catch (Exception)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Ошибка при сохранении!"));
                }
            }
        }
    }
}
