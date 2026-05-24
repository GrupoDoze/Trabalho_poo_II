using System;

namespace SistemaCompanhiaAerea.Exceptions
{
    /// <summary>
    /// Excepção lançada quando se tenta fazer uma reserva num voo sem lugares disponíveis.
    /// </summary>
    public class VooLotadoException : Exception
    {
        public string NumeroVoo { get; private set; }

        public VooLotadoException(string numeroVoo)
            : base(string.Format("O voo '{0}' está lotado. Não existem lugares disponíveis.", numeroVoo))
        {
            NumeroVoo = numeroVoo;
        }

        public VooLotadoException(string numeroVoo, string message)
            : base(message)
        {
            NumeroVoo = numeroVoo;
        }
    }
}
