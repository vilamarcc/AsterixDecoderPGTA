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

namespace AsterixDisplay
{
    /// <summary>
    /// Lógica de interacción para versionCAT21.xaml
    /// </summary>
    public partial class versionCAT21 : Window
    {
        int version;
        public versionCAT21()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public int GetVersion()
        {
            return this.version;
        }

        private void v24_Click(object sender, RoutedEventArgs e)
        {
            this.version = 24;
        }

        private void v23_Click(object sender, RoutedEventArgs e)
        {
            this.version = 23;
        }
    }
}
