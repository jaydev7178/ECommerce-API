using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class DBConnection
    {
        public static SqlConnection GlobalConnection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Configuration.GetConnectionString);
                //conn.Open();
                return conn;
            }
            catch
            {
                throw;
            }
        }
    }
}