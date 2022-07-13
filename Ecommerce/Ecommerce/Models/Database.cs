
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Ecommerce
{
    public class Database
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        public static int computeMaxID(string table)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["E_commerce"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dr;
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                int max_id = 0;
                string query = "select max(id) from " + table + "";
                cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 180;
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    max_id = Int32.Parse(dr[0].ToString());
                }
                con.Close();
                //logger.Info("computeMaxID -" + query);
                return max_id;
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
               // logger.Error("computeMaxID  Error -" + e.Data + "...." + e.Message);
                return 0;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static string insertData(string sql)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["E_commerce"].ConnectionString);
            SqlCommand cmd;
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 180;
                cmd.ExecuteNonQuery();
                con.Close();
                //logger.Info("Insert data -" + sql);
                return "200";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                if (con.State == ConnectionState.Open)
                    con.Close();
                //logger.Error("Insert Data Error -" + e.Data + "...." + e.Message);
                return e.Data + "..." + e.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        

        public static List<string> readData(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["E_commerce"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dr;
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                List<string> data = new List<string>();
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 180;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(dr[0].ToString());
                }
                con.Close();
                //logger.Info("Read data -" + query);
                return data;
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                //logger.Error("Read Data Error -" + e.Data + "...." + e.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static DataSet getDataSet(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["E_commerce"].ConnectionString);
            SqlCommand cmd;
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 180;
                DataSet ds = new DataSet();
                Console.WriteLine(query);
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(ds);
                con.Close();
                //logger.Info("getDataset -" + query);
                return ds;
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                //logger.Error("getDataset -" + e.Data + "...." + e.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static string getValue(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["E_commerce"].ConnectionString);
            SqlCommand cmd;
            //FileStream fs1 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            //StreamWriter writer = new StreamWriter(fs1);
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                cmd = new SqlCommand(query, con);
                //writer.WriteLine("query: " + query);
                string value = cmd.ExecuteScalar().ToString();
                //writer.WriteLine("op: " + value);
                con.Close();
               // logger.Info("getValue -" + query);
                return value;
            }
            catch (Exception e)
            {
                //writer.WriteLine(e.Message + "\n" + e.StackTrace);
                if (con.State == ConnectionState.Open)
                    con.Close();
                //logger.Error("getValue -" + e.Data + "...." + e.Message);
                return null;
            }
            finally
            {
                //writer.Close();
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        
    }
}