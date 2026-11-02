using gamerecords.Classes;
using gamerecords.Dialogs;
using gamerecords.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace gamerecords.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        /// <summary>
        /// Конструктор страницы SignUpPage
        /// </summary>
        public SignUpPage()
        {
            InitializeComponent();

            // Добавление стран в ListBox
            List<string> countries = new List<string>();
            foreach (Countries country in Core.Context.Countries.ToList())
            {
                countries.Add(Convert.ToString(country.country));
            }
            CountryTB.ItemsSource = countries;

            // По умолчанию страна не выбрана
            CountryTB.SelectedIndex = 18;
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Зарегистрироваться"
        /// </summary>
        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTB.Password;

            if (LoginTB.Text != "" && password != "")
            {
                if (password.Length < 6)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Пароль слишком короткий, должно быть минимум 6 символов!"));
                }
                else if (!password.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')))
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Используйте только английскую раскладку и цифры!"));
                }

                else if (!password.Any(char.IsDigit))
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Добавьте хотя бы одну цифру!"));
                }
                else
                {
                    Players play = Core.Context.Players.FirstOrDefault(x => x.nickname == LoginTB.Text);
                    if (play == null)
                    {
                        string s = CountryTB.Text;
                        int? cid = null;
                        if (s != " (нет)")
                        {
                            cid = Core.Context.Countries.FirstOrDefault(x => x.country == s).id;
                        }
                        Players newplayer = new Players
                        {
                            role_id = 2,
                            nickname = LoginTB.Text,
                            password = Core.GetHash(password),
                            countryid = cid,
                            created_at = DateTime.Now,
                        };
                        Core.Context.Players.Add(newplayer);
                        Core.Context.SaveChanges();
                        Core.current_player = newplayer;
                        NavigationService.Navigate(new MainPage());
                    }
                    else { Core.ShowAppDialog(() => new DefaultOKDialog("Пользователь с таким логином уже существует!")); }
                }
            }
            else { Core.ShowAppDialog(() => new DefaultOKDialog("Заполните все данные!")); }
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Войти"
        /// </summary>
        private void HyperLinkSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignInPage());
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Загрузить" [аватарку]
        /// </summary>
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

                    string fileName = System.IO.Path.GetFileName(sourceFilePath);
                    string destinationFilePath = System.IO.Path.Combine(targetFolder, fileName);
                    File.Copy(sourceFilePath, destinationFilePath, true);

                    Core.ShowAppDialog(() => new DefaultOKDialog("Картинка успешно сохранена!"));
                }
                catch (Exception)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog($"Ошибка при сохранении!"));
                }

            }
        }
    }
}