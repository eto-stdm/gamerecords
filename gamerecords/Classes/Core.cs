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
    /// <summary>
    /// Определение внутреннего класса Core
    /// </summary>
    internal class Core
    {
        public static gamerecordsEntities Context = new gamerecordsEntities();
        public static Players current_player;

        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Хэш пароля</returns>
        public static string GetHash(String password)
        {
            using var hash = SHA1.Create();
            return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
        }

        /// <summary>
        /// Создание и отображение диалоговых окон
        /// </summary>
        /// <typeparam name="T">Имя диалога</typeparam>
        /// <param name="windowFactory">Фабрика для создания окна</param>
        public static bool ShowAppDialog<T>(Func<T> windowFactory) where T : Window
        {
            // Вызываем переданную функцию для создания окна
            T dialog = windowFactory();

            dialog.Owner = Application.Current.MainWindow;
            return (bool)dialog.ShowDialog();
        }

        /// <summary>
        /// Применение темы приложения
        /// </summary>
        /// <param name="themePath"></param>
        public static void ApplyTheme(string themePath)
        {
            var uri = new Uri(themePath, UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
