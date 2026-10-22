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
        string login {  set; get; }
        public ForgotPasswordDialog(string login)
        {
            InitializeComponent();
            this.login = login;
            LoginTB.Text = login;

            if (login != "") { LoginTB.IsEnabled = false;  }
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            Core.ShowCaptcha();
            Players player = Core.Context.Players.FirstOrDefault(p => p.nickname == LoginTB.Text.ToLower());
            
            if (player != null && PasswordTB.Password != "")
            {
                string newpass = Core.GetHash(PasswordTB.Password);
                if (newpass != player.password)
                {
                    player.password = newpass;
                    Core.Context.SaveChanges();
                }
                MessageBox.Show("Пароль изменён");
            }
            else { MessageBox.Show("Заполните все поля!"); }
        }
    }
}
