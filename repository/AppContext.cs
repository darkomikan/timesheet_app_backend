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

        public AppContext() 
        {
            Connection = new MySqlConnection("server=localhost;userid=root;password=praksa;database=timesheet_db");
            Connection.Open();
        }

        ~AppContext()
        {
            Connection.Close();
        }
    }
}
