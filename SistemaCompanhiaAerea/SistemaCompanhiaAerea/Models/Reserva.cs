using System;
using SistemaCompanhiaAerea.Enums;
using SistemaCompanhiaAerea.Exceptions;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa uma reserva, associando um Passageiro a um Voo.
    /// Compõe um Bilhete (relação de composição: o Bilhete só existe dentro da Reserva).
    /// </summary>
    public class Reserva
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public string IdReserva { get; private set; }
        public Passageiro Passageiro { get; private set; }
        public Voo Voo { get; private set; }
        public DateTime DataReserva { get; private set; }
        public EstadoReserva Estado { get; private set; }

        // Composição: o Bilhete é criado e pertence à Reserva
        public Bilhete Bilhete { get; private set; }

        // Referência ao assento reservado na aeronave
        private Assento _assento;

        public Reserva(Passageiro passageiro, Voo voo, ClasseVoo classe)
        {
            if (passageiro == null) throw new ArgumentNullException("passageiro");
            if (voo == null) throw new ArgumentNullException("voo");

            // Verificar disponibilidade geral
            if (voo.CapacidadeDisponivel() == 0)
                throw new VooLotadoException(voo.NumeroVoo);

            // Procurar assento livre na classe pretendida
            _assento = voo.Aeronave.ProcurarAssentoLivre(classe);
            if (_assento == null)
                throw new VooLotadoException(voo.NumeroVoo,
                    string.Format("Não existem assentos disponíveis na classe '{0}' para o voo '{1}'.", classe, voo.NumeroVoo));

            // Ocupar o assento
            _assento.Ocupar();

            // Calcular preço com antecedência
            decimal preco = voo.CalcularPreco(classe, DateTime.Now);

            Id = _contadorId++;
            IdReserva = GerarIdReserva();
            Passageiro = passageiro;
            Voo = voo;
            DataReserva = DateTime.Now;
            Estado = EstadoReserva.Activa;

            // Composição: criar o bilhete dentro da reserva
            Bilhete = new Bilhete(classe, _assento.Numero, preco);
        }

        private string GerarIdReserva()
        {
            return string.Format("RES-{0:yyyyMMdd}-{1:D4}", DateTime.Now, _contadorId);
        }

        /// <summary>
        /// Efectua a reserva (alias para consistência com o diagrama UML).
        /// </summary>
        public void EfectuarReserva()
        {
            // A reserva já é efectuada no construtor; este método serve para confirmação
            if (Estado != EstadoReserva.Activa)
                throw new InvalidOperationException("Esta reserva não está num estado activo.");
            Console.WriteLine("Reserva " + IdReserva + " efectuada com sucesso.");
        }

        /// <summary>
        /// Cancela a reserva, libertando o assento e invalidando o bilhete.
        /// </summary>
        public void CancelarReserva()
        {
            if (Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException("Esta reserva já foi cancelada.");
            if (Estado == EstadoReserva.Embarcado)
                throw new InvalidOperationException("Não é possível cancelar uma reserva após o embarque.");

            Estado = EstadoReserva.Cancelada;
            _assento.Libertar();
            Bilhete.Invalidar();

            Console.WriteLine("Reserva " + IdReserva + " cancelada com sucesso. Assento " + _assento.Numero + " libertado.");
        }

        /// <summary>
        /// Realiza o check-in da reserva, verificando o prazo.
        /// O check-in só é permitido entre 24h e 1h antes da partida.
        /// </summary>
        public void RealizarCheckIn()
        {
            if (Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException("Não é possível fazer check-in de uma reserva cancelada.");
            if (Estado == EstadoReserva.CheckInRealizado)
                throw new InvalidOperationException("O check-in já foi realizado para esta reserva.");
            if (Estado == EstadoReserva.Embarcado)
                throw new InvalidOperationException("O passageiro já embarcou.");

            DateTime agora = DateTime.Now;
            TimeSpan tempoAtePartida = Voo.DataHoraPartida - agora;

            if (tempoAtePartida.TotalHours > 24)
                throw new CheckInForaDoPrazoException(Voo.DataHoraPartida, agora);

            if (tempoAtePartida.TotalHours < 1)
                throw new CheckInForaDoPrazoException(
                    string.Format("Check-in encerrado. O voo parte em menos de 1 hora ({0:dd/MM/yyyy HH:mm}).",
                        Voo.DataHoraPartida));

            Estado = EstadoReserva.CheckInRealizado;
            Console.WriteLine("Check-in realizado com sucesso para " + Passageiro.Nome + " | Assento: " + _assento.Numero);
        }

        /// <summary>
        /// Confirma o embarque do passageiro.
        /// </summary>
        public void ConfirmarEmbarque()
        {
            if (Estado != EstadoReserva.CheckInRealizado)
                throw new InvalidOperationException("O passageiro deve realizar o check-in antes de embarcar.");

            Estado = EstadoReserva.Embarcado;
            Console.WriteLine("Embarque confirmado para " + Passageiro.Nome + " | Voo: " + Voo.NumeroVoo);
        }

        /// <summary>
        /// Altera o estado da reserva directamente (para uso interno).
        /// </summary>
        public void AlterarEstado(EstadoReserva novoEstado)
        {
            Estado = novoEstado;
        }

        public void ExibirInformacoes()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("  RESERVA: " + IdReserva);
            Console.WriteLine("======================================");
            Console.WriteLine("  Passageiro : " + Passageiro.Nome + " (" + Passageiro.NumeroPassaporte + ")");
            Console.WriteLine("  Voo        : " + Voo.NumeroVoo + " | " + Voo.Origem + " -> " + Voo.Destino);
            Console.WriteLine("  Partida    : " + Voo.DataHoraPartida.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("  Data Res.  : " + DataReserva.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("  Estado     : " + Estado);
            Console.WriteLine("  --- Bilhete ---");
            Bilhete.ExibirBilhete();
            Console.WriteLine("======================================");
        }

        public override string ToString()
        {
            return string.Format("{0} | {1} | Voo: {2} | Assento: {3} | Estado: {4}",
                IdReserva, Passageiro.Nome, Voo.NumeroVoo, Bilhete.NumeroAssento, Estado);
        }
    }
}
