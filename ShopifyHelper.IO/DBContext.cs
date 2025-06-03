using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyHelper.IO
{
    public class DBContext
    {
        private SqlConnection pconnection = new SqlConnection();
        public DBContext(string dbname, string connectionstring)
        {
            Connection.ConnectionString = connectionstring;

            Connection.Open();

            Connection.ChangeDatabase(dbname);
        }

        public SqlConnection Connection
        {
            get { return pconnection; }
            set { pconnection = value; }
        }

        public DataTable ExecuteDatatable(string sqlCommand)
        {
            //if (pconnection.State != System.Data.ConnectionState.Open)
            //    openConnection();

            //if (General.cm == null)
            //    General.cm = General.DbConnection.CreateCommand();

            SqlCommand cmd = pconnection.CreateCommand();
            DataTable dt = new DataTable();
            using (cmd)
            {
                cmd.CommandText = sqlCommand;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);


                sda.Fill(dt);
            }
            return dt;

        }

        public DataSet ExecuteDataSet(string sqlCommand)
        {
            //if (pconnection.State != System.Data.ConnectionState.Open)
            //    openConnection();

            //if (General.cm == null)
            //    General.cm = General.DbConnection.CreateCommand();

            SqlCommand cmd = pconnection.CreateCommand();
            DataSet ds = new DataSet();
            using (cmd)
            {

                cmd.CommandText = sqlCommand;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(ds);

            }
            return ds;

        }

        public string ConnectionString
        {
            get { return Connection.ConnectionString; }
            set { Connection.ConnectionString = value; }
        }

        public void InitializeConnectino()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

    }
}
