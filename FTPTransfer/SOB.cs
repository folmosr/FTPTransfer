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

        public string CodigoBarra { get => _codigoBarra; set => _codigoBarra = value; }

        public string CodigoCliente { get => _codigoCliente; set => _codigoCliente = value; }

        public string CodigoError { get => _codigoError; set => _codigoError = value; }

        public string CodigoMoneda { get => _codigoMoneda; set => _codigoMoneda = value; }

        public string CodigoPlaza { get => _codigoPlaza; set => _codigoPlaza = value; }

        public string CodigoUsuario { get => _codigoUsuario; set => _codigoUsuario = value; }

        public string FechaVentas { get => _fechaVentas; set => _fechaVentas = value; }

        public string File { get => _file; set => _file = value; }

        public string MontoSobre { get => _montoSobre; set => _montoSobre = value; }

        public string NumRecibo { get => _numRecibo; set => _numRecibo = value; }

        public string NumRouter { get => _numRouter; set => _numRouter = value; }

        public string NumSobre { get => _numSobre; set => _numSobre = value; }

        public string Raw { get => _raw; set => _raw = value; }

        public string RutACD { get => _rutACD; set => _rutACD = value; }
        public string ServicioSobre { get => _servicioSobre; set => _servicioSobre = value; }

        public string TipoDeposito { get => _tipoDeposito; set => _tipoDeposito = value; }

        public string TotalDocumentos { get => _totalDocumentos; set => _totalDocumentos = value; }

        #endregion Public Properties
    }
}
