using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FTPTransfer
{
    public class FTP
    {
        #region Private Fields

        private Logger logger;

        #endregion Private Fields

        #region Public Constructors

        public FTP(Logger _logger)
        {
            this.logger = _logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void DeleteFiles(List<string> files)
        {
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Inicio de limpieza de archivos en Host"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            foreach (string fileName in files)
            {
                FtpWebRequest request = FtpWebRequest.Create(new Uri(String.Format("{0}{1}", ConfigurationManager.AppSettings["FTPHost"], fileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"], ConfigurationManager.AppSettings["FTPPassword"]);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Archivo {0}, Delete status: {1}", fileName, response.StatusDescription), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                }
            }
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Fin de limpieza de archivos en Host"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
        }

        public List<string> GetListFile()
        {

            List<string> files = new List<string>();
            Regex regex = new Regex(@"^SOB\d{8}.\d{5,}");
            FtpWebRequest request = FtpWebRequest.Create(new Uri(ConfigurationManager.AppSettings["FTPHost"])) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"], ConfigurationManager.AppSettings["FTPPassword"]);

            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Buscando archivos a procesar"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });

            using (var response = (FtpWebResponse)request.GetResponse())
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Contando al FTP HOST:{0}", ConfigurationManager.AppSettings["FTPHost"]), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Archivo encontrado:{0}", line), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                        files.Add(line);
                    }
                    line = streamReader.ReadLine();
                }
            }
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Fin de búsqueda de archivos", ConfigurationManager.AppSettings["FTPHost"]), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            return files;
        }

        public void MoveFiles(List<string> files)
        {
            List<SOB> sobres = new List<SOB>();
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Moviendo archivos procesados"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            foreach (string fileName in files)
            {
                string LocalDestinationPath = string.Format("{0}{1}", ConfigurationManager.AppSettings["Destino"], fileName);
                FtpWebRequest request = FtpWebRequest.Create(new Uri(String.Format("{0}{1}", ConfigurationManager.AppSettings["FTPHost"], fileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"], ConfigurationManager.AppSettings["FTPPassword"]);
                request.KeepAlive = true;
                request.UsePassive = false;
                request.UseBinary = false;
                using (var response = (FtpWebResponse)request.GetResponse())
                using (Stream streamReader = response.GetResponseStream())
                using (FileStream writer = new FileStream(LocalDestinationPath, FileMode.Create))
                {
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Conectando al FTP Host {0}", ConfigurationManager.AppSettings["FTPHost"]), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                    long length = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[2048];

                    readCount = streamReader.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        writer.Write(buffer, 0, readCount);
                        readCount = streamReader.Read(buffer, 0, bufferSize);
                    }
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Archivo movido a {0}", LocalDestinationPath), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                }

            }
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Fin Moviendo Archivos"), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
        }
        public List<SOB> ProcessFiles(List<string> files)
        {
            List<SOB> sobres = new List<SOB>();
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Inicia procesamiento de archivos", ConfigurationManager.AppSettings["FTPHost"]), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            foreach (string fileName in files)
            {
                this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Analizando archivo:{0}", fileName), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                FtpWebRequest request = FtpWebRequest.Create(new Uri(String.Format("{0}{1}", ConfigurationManager.AppSettings["FTPHost"], fileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"], ConfigurationManager.AppSettings["FTPPassword"]);
                request.KeepAlive = true;
                request.UsePassive = false;
                request.UseBinary = false;
                using (var response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Contando al FTP HOST:{0}", ConfigurationManager.AppSettings["FTPHost"]), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Línea a procesar:{0}", line), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
                        sobres.Add(new Parser(line, fileName).GetSOB());
                    }
                }

            }
            this.logger.LogWriter.Write(new LogEntry() { Message = String.Format("Fin de procesamiento de archivos, total de lineas a insertar en BD {0}", sobres.Count.ToString()), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            return sobres;
        }

        #endregion Public Methods

    }
}
