using MySql.Data.MySqlClient;
using System.Windows;

namespace client_marieteam
{
    class ClientSQL
    {
        public ClientSQL()
        {
            Client = getClient();
        }
        public MySqlConnection Client { get; set; }
        public MySqlConnection getClient()
        {
            // Login admins SQL
            string dataSource = "127.0.0.1";
            string port = "3306";
            string username = "root";
            string password = " ";
            string dataBase = "marieteam";

            string connectionString = "datasource=" + dataSource + ";port=" + port + ";username=" + username + ";password=" + password + ";database=" + dataBase + ";";
            return new MySqlConnection(connectionString);
        }
        public bool OpenConnection()
        {
            try
            {
                Client.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Login/password  du serveur sont erronés, veuillez contacter un administrateur", "Code erreur : " + ex.Number.ToString());
                        break;
                    case 1042:
                        MessageBox.Show("Serveur SQL non connecté", "Code erreur : " + ex.Number.ToString());
                        break;
                }
                return false;
            }
        }
        public bool CloseConnection()
        {
            try
            {
                Client.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Code erreur : " + ex.Number.ToString());
                return false;
            }
        }
    }
}
