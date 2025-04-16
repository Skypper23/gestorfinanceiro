using MySql.Data.MySqlClient;

namespace GestorFinanceiro.Database
{
    public class Db
    {
        private string connStr = "server=localhost;user=root;password=;database=gestorfinanceiro;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}