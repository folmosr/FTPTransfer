namespace FTPTransfer
{
    public class SOB
    {

        #region Private Fields

        private string _codigoBarra;
        private string _codigoCliente;
        private string _codigoError;
        private string _codigoMoneda;
        private string _codigoPlaza;
        private string _codigoUsuario;
        private string _fechaVentas;
        private string _file;
        private string _montoSobre;
        private string _numRecibo;
        private string _numRouter;
        private string _numSobre;
        private string _raw;
        private string _rutACD;
        private string _servicioSobre;
        private string _tipoDeposito;
        private string _totalDocumentos;

        #endregion Private Fields

        #region Public Properties

        public string CodigoBarra { get { return _codigoBarra; } set { _codigoBarra = value; } }

        public string CodigoCliente { get { return _codigoCliente; } set { _codigoCliente = value; } }
        public string CodigoError { get { return _codigoError; } set { _codigoError = value; } }
        public string CodigoMoneda { get { return _codigoMoneda; } set { _codigoMoneda = value; } }
        public string CodigoPlaza { get { return _codigoPlaza; } set { _codigoPlaza = value; } }
        public string CodigoUsuario { get { return _codigoUsuario; } set { _codigoUsuario = value; } }
        public string FechaVentas { get { return _fechaVentas; } set { _fechaVentas = value; } }
        public string File { get { return _file; } set { _file = value; } }
        public string MontoSobre { get { return _montoSobre; } set { _montoSobre = value; } }
        public string NumRecibo { get { return _numRecibo; } set { _numRecibo = value; } }
        public string NumRouter { get { return _numRouter; } set { _numRouter = value; } }
        public string NumSobre { get { return _numSobre; } set { _numSobre = value; } }
        public string Raw { get { return _raw; } set { _raw = value; } }
        public string RutACD { get { return _rutACD; } set { _rutACD = value; } }
        public string ServicioSobre { get { return _servicioSobre; } set { _servicioSobre = value; } }
        public string TipoDeposito { get { return _tipoDeposito; } set { _tipoDeposito = value; } }
        public string TotalDocumentos { get { return _totalDocumentos; } set { _totalDocumentos = value; } }

        #endregion Public Properties

    }
}
