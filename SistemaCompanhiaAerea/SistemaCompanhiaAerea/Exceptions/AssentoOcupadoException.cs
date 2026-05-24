using System;

namespace SistemaCompanhiaAerea.Exceptions
{
    /// <summary>
    /// Excepção lançada quando se tenta reservar um assento já ocupado.
    /// </summary>
    public class AssentoOcupadoException : Exception
    {
        public string NumeroAssento { get; private set; }

        public AssentoOcupadoException(string numeroAssento)
            : base(string.Format("O assento '{0}' já se encontra ocupado.", numeroAssento))
        {
            NumeroAssento = numeroAssento;
        }

        public AssentoOcupadoException(string numeroAssento, string message)
            : base(message)
        {
            NumeroAssento = numeroAssento;
        }
    }
}
