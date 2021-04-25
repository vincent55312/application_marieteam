using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;

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

            if (!File.Exists(Licence.path))
            {
                licencecanvas.Visibility = Visibility.Visible;
            }
            else{ // auto connexion
                dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(Licence.path));
                string key = jsonFile["LICENCE"];
                if (new Licence(key).isValid) licencecanvas.Visibility = Visibility.Hidden;
            }

            maincanvas.Visibility = Visibility.Hidden;
        }
        private void button_licence_Click(object sender, RoutedEventArgs e)
        {
            string inputLicence = tb_licence.Text;
            Licence licence = new Licence(inputLicence);
            if(licence.isValid)
            {
                string stringConfig = "{'LICENCE':'"+ inputLicence +"'}";
                dynamic json = JsonConvert.DeserializeObject(stringConfig);
                string jsonSerializedObj = JsonConvert.SerializeObject(json);
                File.WriteAllText(Licence.path, jsonSerializedObj);
                licencecanvas.Visibility = Visibility.Hidden;
            }
            else
            {   
                if(licence.APIisWorking) MessageBox.Show("Licence non existante");
            }
        }
        private void clickConnect(object sender, RoutedEventArgs e)
        {
            if (new ClientSQL().OpenConnection())
            {
                startcanvas.Visibility = Visibility.Hidden;
                maincanvas.Visibility = Visibility.Visible;
            }
        }
        private void clickGeneratePDF(object sender, RoutedEventArgs e)
        {
            if (TextboxSortie.Text.Length != 0)
            {
                string textEditor = maintextbox.Text;
                string output = TextboxSortie.Text;

                PDF myPdf = new PDF(output, textEditor);
                myPdf.Generate();
            }
            else
            {
                MessageBox.Show("Veuillez saisir un repertoire de sortie");
            }
        }
        private void clickResetEditor(object sender, RoutedEventArgs e)
        {
            maintextbox.Text = "";
        }

        private void clickOutput(object sender, RoutedEventArgs e)
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
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                TextboxSortie.Text = path + "\\MarieTeam.pdf";
            }
        }

        private void clickGetEditor(object sender, RoutedEventArgs e)
        {
            ClientSQL client = new ClientSQL();
            if (client.OpenConnection())
            {
                var sql = "SELECT * FROM bateau";
                var cmd = new MySqlCommand(sql, client.Client);
                MySqlDataReader rdr = cmd.ExecuteReader();
                Passerelle Bateaux = new Passerelle();
                while (rdr.Read())
                {
                    Bateaux.bateauVoyageurs.Add(new BateauVoyageur(rdr.GetInt32(0), rdr.GetString(1), rdr.GetFloat(2), rdr.GetFloat(3), rdr.GetString(4), rdr.GetFloat(5), Passerelle.chargerLesEquipements(rdr.GetString(6))));
                }
                client.CloseConnection();
                foreach (var item in Bateaux.bateauVoyageurs) maintextbox.Text += item.ToString() + "#NEWPAGE\n";
            }
        }


    }
}
