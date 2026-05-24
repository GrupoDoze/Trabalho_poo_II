using System;

namespace SistemaCompanhiaAerea.Exceptions
{
    /// <summary>
    /// Excepção lançada quando o check-in é efectuado fora do prazo permitido.
    /// </summary>
    public class CheckInForaDoPrazoException : Exception
    {
        public DateTime DataVoo { get; private set; }
        public DateTime DataTentativa { get; private set; }

        public CheckInForaDoPrazoException(DateTime dataVoo, DateTime dataTentativa)
            : base(string.Format(
                "Check-in fora do prazo. O voo parte em {0:dd/MM/yyyy HH:mm}. " +
                "O check-in só é permitido entre 24 horas e 1 hora antes da partida.",
                dataVoo))
        {
            DataVoo = dataVoo;
            DataTentativa = dataTentativa;
        }

        public CheckInForaDoPrazoException(string message)
            : base(message)
        {
        }
    }
}
