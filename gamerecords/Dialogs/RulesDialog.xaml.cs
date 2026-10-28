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
    /// Логика взаимодействия для RulesDialog.xaml
    /// </summary>
    public partial class RulesDialog : Window
    {
        GamesCategories Gamecat { get; set; }

        /// <summary>
        /// Конструктор окна RulesDialog
        /// </summary>
        /// <param name="gamecat">Категория игры</param>
        public RulesDialog(GamesCategories gamecat)
        {
            InitializeComponent();
            this.Gamecat = gamecat;

            TextGameRulesTB.Text = gamecat.Games.rules;
            TextCategoryRulesTB.Text = gamecat.rules;

            // если текста нет - заголовок скрывается
            if (gamecat.Games.rules == null) { HeaderGameRulesTB.Visibility = Visibility.Collapsed; }
            if (gamecat.rules == null) { HeaderCategoryRulesTB.Visibility = Visibility.Collapsed; }
        }
    }
}
