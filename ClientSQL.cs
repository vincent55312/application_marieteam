using MySql.Data.MySqlClient;

namespace client_marieteam
{
    class ClientSQL
    {
        public ClientSQL()
        {
            Client = getClient();
            clientIsConnected();
        }
        public MySqlConnection Client { get; set; }
        public bool isConnected { get; set; }

        public void clientIsConnected()
        {
            try
            {
                Client.Open();
                isConnected = true;
            }
            catch
            {
                isConnected = false;
            }
        }
        public MySqlConnection getClient()
        {
            string dataSource = "127.0.0.1";
            string port = "3306";
            string username = "root";
            string password = " ";
            string dataBase = "marieteam";

            string connectionString = "datasource=" + dataSource + ";port=" + port + ";username=" + username + ";password=" + password + ";database=" + dataBase + ";";
            return new MySqlConnection(connectionString);
        }

        public MySqlCommand setQuery(string query, MySqlConnection client)
        {
            MySqlCommand commandDatabase = new MySqlCommand(query, client);
            commandDatabase.CommandTimeout = 60;
            return commandDatabase;
        }

        public MySqlDataReader Reader(string query)
        {
            MySqlConnection connection = getClient();
            connection.Open();
            MySqlDataReader reader = setQuery(query, connection).ExecuteReader();
            connection.Close();
            return reader;
        }
    }
}
