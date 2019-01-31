﻿using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            try
            {
                FTP clientFtp = new FTP(logger);
                List<string> files = clientFtp.GetListFile();
                if (files.Count > 0)
                {
                    List<SOB> sobres = clientFtp.ProcessFiles(files);
                    if (sobres.Count() > 0)
                    {
                        CargarFTP cargar = new CargarFTP(logger);
                        cargar.CrearSOB(sobres);
                    }
                    clientFtp.MoveFiles(files);
                    clientFtp.DeleteFiles(files);
                }
            }
            catch (Exception ex)
            {
                logger.LogWriter.Write(new LogEntry() { Message = String.Format("Excepci{on General", ex.StackTrace), Categories = new List<string> { "General" }, Priority = 1, ProcessName = Logger.PROCESS_NAME });
            }
        }
    }
}
