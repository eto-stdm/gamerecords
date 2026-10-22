using gamerecords.Dialogs;
using gamerecords.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace gamerecords.Classes
{
    internal class Core
    {
        public static gamerecordsEntities Context = new gamerecordsEntities();
        public static Players current_player;

        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        public static void ShowCaptcha()
        {
            CaptchaDialog dialog = new CaptchaDialog();
            dialog.ShowDialog();
        }
    }
}
