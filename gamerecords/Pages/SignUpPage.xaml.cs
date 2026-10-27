using gamerecords.Classes;
using gamerecords.Models;
using gamerecords.Dialogs;
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
            if (LoginTB.Text != "" && PasswordTB.Password != "")
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
                        password = Core.GetHash(PasswordTB.Password),
                        countryid = cid,
                        created_at = DateTime.Now,
                    };
                    Core.Context.Players.Add(newplayer);
                    Core.Context.SaveChanges();
                    Core.current_player = newplayer;
                    NavigationService.Navigate(new MainPage());
                }
                else { Core.ShowAppDialog(() => new DefaultOKDialog("Такой пользователь уже существует!")); }
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
    }
}