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
    /// Логика взаимодействия для GuidesDialog.xaml
    /// </summary>
    public partial class GuidesDialog : Window
    {
        Guides guide { get; set; }
        /// <summary>
        /// Конструктор окна GuidesDialog
        /// </summary>
        public GuidesDialog(Guides guide)
        {
            InitializeComponent();
            this.guide = guide;

            HeaderGuideTB.Text += $"'{guide.title}' от {guide.Players.nickname}";
            TextGuideTB.Text = guide.description;
        }
    }
}
