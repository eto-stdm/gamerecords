using gamerecords.Classes;


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
using System.Windows.Shapes;

namespace gamerecords.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ForgotPasswordDialog.xaml
    /// </summary>
    public partial class ForgotPasswordDialog : Window
    {
        string Login {  set; get; }

        /// <summary>
        /// Конструктор окна ForgotPasswordDialog
        /// </summary>
        /// <param name="login">Логин</param>
        public ForgotPasswordDialog(string login)
        {
            InitializeComponent();
            this.Login = login;
            LoginTB.Text = login;

            // Автоматический ввод логина, если он введён на предыдущей странице
            if (login != "") { LoginTB.IsEnabled = false;  }
        }
        
        /// <summary>
        /// Отбработка кнопки "Поменять пароль"
        /// </summary>
        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            Core.ShowAppDialog(() => new CaptchaDialog());
            Players player = Core.Context.Players.FirstOrDefault(p => p.nickname == LoginTB.Text.ToLower());
            
            if (player != null && PasswordTB.Password != "")
            {
                string newpass = Core.GetHash(PasswordTB.Password);
                if (newpass != player.password)
                {
                    player.password = newpass;
                    Core.Context.SaveChanges();
                }
                Core.ShowAppDialog(() => new DefaultOKDialog("Пароль изменён"));
                this.Close();
            }
            else
            {
                Core.ShowAppDialog(() => new DefaultOKDialog("Заполните все необходимые поля!"));
            }
        }
    }
}
