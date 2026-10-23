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
        Games Game { get; set; }
        public RulesDialog(Games game)
        {
            InitializeComponent();
            this.Game = game;

            test.Text = game.rules;
        }
    }
}
