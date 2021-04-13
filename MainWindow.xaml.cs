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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client_marieteam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            maincanvas.Visibility = Visibility.Hidden;

        }

        private void Button_connection_Click(object sender, RoutedEventArgs e)
        {
            ClientSQL client = new ClientSQL();
            if (client.isConnected)
            {
                startcanvas.Visibility = Visibility.Hidden;
                maincanvas.Visibility = Visibility.Visible;
            }
            else
            {
                LabelConnection.Content = "Connection à la base de donnée serveur a échouée";
            }
        }

        private void sortie_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = TextboxSortie.Text;
            dialog.Title = "Select a Directory"; 
            dialog.Filter = "Directory|*.this.directory"; 
            dialog.FileName = "select";
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                TextboxSortie.Text = path;
            }
        }

        private void GenererPDF(object sender, RoutedEventArgs e)
        {

        }

        private void ResetEditeur(object sender, RoutedEventArgs e)
        {

        }

        private void CollectEditeur(object sender, RoutedEventArgs e)
        {
             
        }
    }
}
