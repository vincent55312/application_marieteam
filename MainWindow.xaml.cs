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
using MySql.Data.MySqlClient;

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
            if (client.OpenConnection())
            {
                startcanvas.Visibility = Visibility.Hidden;
                maincanvas.Visibility = Visibility.Visible;
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
        private void CollectEditeur(object sender, RoutedEventArgs e)
        {
            ClientSQL client = new ClientSQL();
            if (client.OpenConnection())
            {
                var sql = "SELECT * FROM bateau";

                using var cmd = new MySqlCommand(sql, client.Client);

                using MySqlDataReader rdr = cmd.ExecuteReader();
                JeuEnregistrement Bateaux = new JeuEnregistrement();

                while (rdr.Read())
                {
                    Bateaux.bateauVoyageurs.Add(new BateauVoyageur(rdr.GetInt32(0), rdr.GetString(1), rdr.GetFloat(2), rdr.GetFloat(3), rdr.GetString(4), rdr.GetFloat(5)));
                }
                client.CloseConnection();

                foreach (var item in Bateaux.bateauVoyageurs)
                {
                    maintextbox.Text += item.ToString();
                }
            }
        }
        private void GenererPDF(object sender, RoutedEventArgs e)
        {

        }

        private void ResetEditeur(object sender, RoutedEventArgs e)
        {
            maintextbox.Text = "";
        }


    }
}
