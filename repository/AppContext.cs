using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository
{
    public class AppContext
    {
        public MySqlConnection Connection { get; }

        public AppContext(string connectionString) 
        {
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        ~AppContext()
        {
            Connection.Close();
        }
    }
}
