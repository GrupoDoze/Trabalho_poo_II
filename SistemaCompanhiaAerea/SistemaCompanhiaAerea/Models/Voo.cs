using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCompanhiaAerea.Enums;
using SistemaCompanhiaAerea.Exceptions;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa um voo, associando uma aeronave a uma rota e horário.
    /// </summary>
    public class Voo
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public string NumeroVoo { get; private set; }
        public string Origem { get; private set; }
        public string Destino { get; private set; }
        public DateTime DataHoraPartida { get; private set; }
        public DateTime DataHoraChegada { get; private set; }
        public Aeronave Aeronave { get; private set; }
        public List<Reserva> Reservas { get; private set; }
        public bool Activo { get; private set; }

        // Preços base por classe
        private Dictionary<ClasseVoo, decimal> _precoBase;

        public Voo(string numeroVoo, string origem, string destino,
                   DateTime dataHoraPartida, DateTime dataHoraChegada,
                   Aeronave aeronave,
                   decimal precoEconomica, decimal precoExecutiva, decimal precoPrimeiraClasse)
        {
            if (string.IsNullOrWhiteSpace(numeroVoo))
                throw new ArgumentException("O número do voo não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(origem))
                throw new ArgumentException("A origem não pode ser vazia.");
            if (string.IsNullOrWhiteSpace(destino))
                throw new ArgumentException("O destino não pode ser vazio.");
            if (dataHoraChegada <= dataHoraPartida)
                throw new ArgumentException("A data de chegada deve ser posterior à data de partida.");
            if (aeronave == null)
                throw new ArgumentNullException("aeronave", "A aeronave não pode ser nula.");

            Id = _contadorId++;
            NumeroVoo = numeroVoo.Trim().ToUpper();
            Origem = origem.Trim().ToUpper();
            Destino = destino.Trim().ToUpper();
            DataHoraPartida = dataHoraPartida;
            DataHoraChegada = dataHoraChegada;
            Aeronave = aeronave;
            Reservas = new List<Reserva>();
            Activo = true;

            _precoBase = new Dictionary<ClasseVoo, decimal>
            {
                { ClasseVoo.Economica, precoEconomica },
                { ClasseVoo.Executiva, precoExecutiva },
                { ClasseVoo.PrimeiraClasse, precoPrimeiraClasse }
            };
        }

        /// <summary>
        /// Calcula o preço do bilhete com base na classe e antecedência da compra.
        /// </summary>
        public decimal CalcularPreco(ClasseVoo classe, DateTime dataCompra)
        {
            decimal precoBase = _precoBase[classe];
            TimeSpan antecedencia = DataHoraPartida - dataCompra;
            double dias = antecedencia.TotalDays;

            decimal multiplicador;
            if (dias >= 60)
                multiplicador = 0.70m;       // 30% desconto para compras com mais de 60 dias
            else if (dias >= 30)
                multiplicador = 0.85m;       // 15% desconto para 30-59 dias
            else if (dias >= 14)
                multiplicador = 1.00m;       // preço normal para 14-29 dias
            else if (dias >= 7)
                multiplicador = 1.15m;       // 15% extra para 7-13 dias
            else
                multiplicador = 1.30m;       // 30% extra para menos de 7 dias

            return Math.Round(precoBase * multiplicador, 2);
        }

        /// <summary>
        /// Devolve a capacidade disponível total no voo.
        /// </summary>
        public int CapacidadeDisponivel()
        {
            return Aeronave.TotalLugaresDisponiveis();
        }

        /// <summary>
        /// Adiciona uma reserva ao voo, verificando disponibilidade.
        /// </summary>
        public void AdicionarReserva(Reserva reserva)
        {
            if (reserva == null)
                throw new ArgumentNullException("reserva");

            if (CapacidadeDisponivel() == 0)
                throw new VooLotadoException(NumeroVoo);

            Reservas.Add(reserva);
        }

        /// <summary>
        /// Remove uma reserva do voo (em caso de cancelamento).
        /// </summary>
        public void RemoverReserva(Reserva reserva)
        {
            if (reserva != null)
                Reservas.Remove(reserva);
        }

        /// <summary>
        /// Devolve a duração do voo.
        /// </summary>
        public TimeSpan Duracao()
        {
            return DataHoraChegada - DataHoraPartida;
        }

        /// <summary>
        /// Lista todos os passageiros do voo com reservas activas.
        /// </summary>
        public List<Passageiro> ListarPassageiros()
        {
            return Reservas
                .Where(r => r.Estado == EstadoReserva.Activa || r.Estado == EstadoReserva.CheckInRealizado || r.Estado == EstadoReserva.Embarcado)
                .Select(r => r.Passageiro)
                .ToList();
        }

        public void CriarVoo()
        {
            Activo = true;
        }

        public void ExibirInformacoes()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("  VOO: " + NumeroVoo);
            Console.WriteLine("======================================");
            Console.WriteLine("  Rota        : " + Origem + " -> " + Destino);
            Console.WriteLine("  Partida     : " + DataHoraPartida.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("  Chegada     : " + DataHoraChegada.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("  Duração     : " + string.Format("{0}h {1}min", (int)Duracao().TotalHours, Duracao().Minutes));
            Console.WriteLine("  Aeronave    : " + Aeronave.Matricula + " (" + Aeronave.Tipo + ")");
            Console.WriteLine("  Capacidade  : " + Aeronave.CapacidadeTotal + " | Disponíveis: " + CapacidadeDisponivel());
            Console.WriteLine("  Reservas    : " + Reservas.Count);
            Console.WriteLine("  Preço Econ. : " + _precoBase[ClasseVoo.Economica].ToString("C"));
            Console.WriteLine("  Preço Exec. : " + _precoBase[ClasseVoo.Executiva].ToString("C"));
            Console.WriteLine("  Preço 1.ª   : " + _precoBase[ClasseVoo.PrimeiraClasse].ToString("C"));
            Console.WriteLine("======================================");
        }

        public override string ToString()
        {
            return string.Format("Voo {0} | {1} -> {2} | {3} | Disponíveis: {4}",
                NumeroVoo, Origem, Destino,
                DataHoraPartida.ToString("dd/MM/yyyy HH:mm"),
                CapacidadeDisponivel());
        }
    }
}
