using gamerecords.Dialogs;
using gamerecords.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace gamerecords.Classes
{
    internal class Core
    {
        public static gamerecordsEntities Context = new gamerecordsEntities();
        public static Players current_player;

        public static string GetHash(String password)
        {
            using var hash = SHA1.Create();
            return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
        }

        public static void ShowCaptcha()
        {
            CaptchaDialog dialog = new CaptchaDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        public static void ApplyTheme(string themePath)
        {
            var uri = new Uri(themePath, UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
