namespace FTPTransfer
{
    public class Parser
    {

        #region Private Fields

        private string _fileName;
        private string _linea;

        #endregion Private Fields

        #region Public Constructors

        public Parser(string linea, string fileName)
        {
            _linea = linea;
            _fileName = fileName;
        }

        #endregion Public Constructors

        #region Public Methods

        public SOB GetSOB()
        {
            return new SOB()
            {
                CodigoPlaza = _linea.Substring(0, 5),
                CodigoCliente = _linea.Substring(5, 9),
                CodigoUsuario = _linea.Substring(14, 9),
                ServicioSobre = _linea.Substring(23, 3),
                NumSobre = _linea.Substring(25, 10), //.TrimStart('0')
                FechaVentas = _linea.Substring(36, 8),
                RutACD = _linea.Substring(44, 9),
                NumRouter = _linea.Substring(53, 10),
                TotalDocumentos = _linea.Substring(63, 8),
                TipoDeposito = _linea.Substring(71, 5),
                CodigoMoneda = _linea.Substring(76, 5),
                MontoSobre = _linea.Substring(81, 12),
                CodigoError = _linea.Substring(93, 5),
                NumRecibo = _linea.Substring(105, 8),
                CodigoBarra = _linea.Substring(113, 35),
                Raw = _linea,
                File = _fileName
            };
        }

        #endregion Public Methods

    }
}
