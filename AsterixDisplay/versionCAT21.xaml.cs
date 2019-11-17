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
        public versionCAT21(string safefilename)
        {
            InitializeComponent();
            filenamebox.Text = "The File: " + safefilename;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e) // botón OK
        {
            this.Close();
        }

        public int GetVersion() //devuelve la versión que el usuario ha escogido
        {
            return this.version;
        }

        private void v24_Click(object sender, RoutedEventArgs e) // click en v2.4 --> assigna la versión 2.4 como la elegida
        {
            this.version = 24;
        }

        private void v23_Click(object sender, RoutedEventArgs e) // click en v023 --> assigna la versión 23 como la elegida
        {
            this.version = 23;
        }
    }
}
