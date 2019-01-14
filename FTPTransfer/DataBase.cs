using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPTransfer
{
    public class DataBase
    {

        #region Private Fields

        private SqlConnection conn;

        #endregion Private Fields

        #region Public Methods

        public void CerrarConexion()
        {
            try
            {
                this.conn.Close();
                this.conn.Dispose();
            }
            catch { }
        }

        public int GetScalar(string sqlQuery, SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                int result = 0;
                using (SqlCommand command = new SqlCommand(sqlQuery, conn, trans))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Insert(string sqlQuery, SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, conn, trans))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Stringcon()
        {
            try
            {
                string ambiente = ConfigurationManager.AppSettings["environment"];
                string conexionBaseDatos = ConfigurationManager.ConnectionStrings["baseDatosSql_" + ambiente].ConnectionString;
                return (conexionBaseDatos);
            }
            catch { return (null); }
        }

        #endregion Public Methods
    }
}
