using gamerecords.Classes;
using gamerecords.Dialogs;
using gamerecords.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        string fullPath = "";
        string avatarPath = "";
        public SignUpPage()
        {
            InitializeComponent();

            // Добавление стран в ListBox
            List<string> countries = new List<string>();
            foreach (Countries country in Core.Context.Countries.ToList())
            {
                countries.Add(Convert.ToString(country.country));
            }
            countries.Sort();
            CountryTB.ItemsSource = countries;

            // По умолчанию страна не выбрана
            CountryTB.SelectedIndex = 0;

            PasswordTB.ToolTip = "Разрешённые специальные символы: !@#$%^&*()_+-=[]{};,.?";
        }

        /// <summary>
        /// Обработка нажатия на кнопку "Зарегистрироваться"
        /// </summary>
        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTB.Password;

            if (LoginTB.Text != "" && password != "")
            {
                string special = "!@#$%^&*()_+-=[]{};,.?";

                if (LoginTB.Text.Length < 6)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Логин слишком короткий, должно быть минимум 6 символов!")); return;
                }
                else if (LoginTB.Text.Length > 30)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Логин слишком длинный, должно быть не больше 30 символов!")); return;
                }
                else if (password.Length < 6)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Пароль слишком короткий, должно быть минимум 6 символов!")); return;
                }
                else if (LoginTB.Text.Length > 50)
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Пароль слишком длинный, должно быть не больше 50 символов!")); return;
                }
                else if (!LoginTB.Text.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')))
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Для логина используйте только английскую раскладку и цифры!")); return;
                }
                else if (!password.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || special.Contains(c)))
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Для пароля используйте только английскую раскладку, цифры и специальные символы!")); return;
                }
                else if (!password.Any(char.IsDigit) || !password.Any(char.IsDigit))
                {
                    Core.ShowAppDialog(() => new DefaultOKDialog("Добавьте хотя бы одну цифру!")); return;
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
                        if (avatarPath == "")
                        {
                            avatarPath = "/Images/Avatars/avatar_default.png";
                        }
                        else
                        {
                            int ind = fullPath.LastIndexOf("\\");
                            string tempPath = fullPath.Substring(0, ind);
                            avatarPath = $"/Images/Avatars/avatar_{LoginTB.Text}.png";
                            File.Move(fullPath, $"{tempPath}/avatar_{LoginTB.Text}.png");
                        }
                        Players newplayer = new Players
                        {
                            role_id = 2,
                            nickname = LoginTB.Text.ToLower(),
                            password = Core.GetHash(password),
                            countryid = cid,
                            image = avatarPath,
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
            if (avatarPath != "")
            {
                File.Delete(fullPath);
            }
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
                    if (avatarPath != "")
                    {
                        File.Delete(fullPath);
                    }

                    string sourceFilePath = openFileDialog.FileName;

                    string targetFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Images/Avatars");

                    string fileName = System.IO.Path.GetFileName(sourceFilePath); // получение имени файла
                    string destinationFilePath = System.IO.Path.Combine(targetFolder, fileName);
                    File.Copy(sourceFilePath, destinationFilePath, true);

                    fullPath = destinationFilePath;
                    int index = destinationFilePath.IndexOf("../.."); 
                    avatarPath = destinationFilePath.Substring(index + 5).Replace("\\", "/");

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