using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPTransfer
{
    public class CargarFTP
    {
        #region Private Fields

        private DataBase _BaseDatos;
        private Logger logger;

        #endregion Private Fields

        #region Public Constructors

        public CargarFTP(Logger _logger)
        {
            _BaseDatos = new DataBase();
            this.logger = _logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CrearSOB(List<SOB> sobres)
        {
            SqlTransaction objTrans = null;

            using (SqlConnection objConn = new SqlConnection(_BaseDatos.Stringcon()))
            {

                objConn.Open();
                int c = 0;
                objTrans = objConn.BeginTransaction();
                try
                {
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Iniciando inserción a BD"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                    foreach (SOB sobre in sobres)
                    {

                        if (_BaseDatos.GetScalar(String.Format("SELECT COALESCE(COUNT(1),0) FROM CargaFTP WHERE CargaFTP.Num_Contenedor = '{0}'", sobre.NumSobre), objConn, objTrans) == 0)
                        {
                            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("N° Sobre a insertar", sobre.NumSobre), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                            int nDocumentos = Convert.ToInt32(sobre.TotalDocumentos);
                            string insertQUERY = String.Format(@"INSERT INTO  CargaFTP
                        (   
                            Num_Contenedor, 
                            Cantidad_Documentos, 
                            Id_tipo_Contenido, 
                            Id_Tipo_Moneda, 
                            Fecha_Ventas, 
                            Monto_Sobre, 
                            Linea,
                            FileName,
                            Codigo_Barra,
                            Etiquetas,
                            Valores
                        )
                        VALUES 
                            ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})
                        
                ", String.Format("'{0}'", sobre.NumSobre),
                        nDocumentos.ToString(),
                        (nDocumentos == 0) ? Constantes.CONTENIDO_EFECTIVO.ToString() : Constantes.CONTENIDO_CHEQUE.ToString(),
                        Constantes.MONEDA.ToString(),
                        String.Format("'{0}/{1}/{2}'",
                            sobre.FechaVentas.Substring(sobre.FechaVentas.Length - 2, 2),
                            sobre.FechaVentas.Substring(sobre.FechaVentas.Length - 4, 2),
                            sobre.FechaVentas.Substring(0, 4)
                        ),
                        sobre.MontoSobre.TrimStart('0'),
                        String.Format("'{0}'", sobre.Raw),
                        String.Format("'{0}'", sobre.File),
                        String.Format("'{0}'", sobre.CodigoBarra),
                        "'Fecha de Ventas|Rut Cajero ACD|'",
                        String.Format("'{0}|{1}|'", sobre.FechaVentas, sobre.RutACD)
                        );
                            _BaseDatos.Insert(insertQUERY, objConn, objTrans);
                            c++;
                        }
                        else
                        {
                            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Sobres existente {0}", sobre.NumSobre), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                        }
                    }
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Sobres incertados {0}", c.ToString()), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                    objTrans.Commit();
                }
                catch (Exception ex)
                {
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Error al insertar {0}, ROLLBACK!!", ex.Message), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                    objTrans.Rollback();
                    throw;
                }
                finally
                {
                    objConn.Close();
                    objTrans.Dispose();
                }
            }
        }

        #endregion Public Methods
    }
}
