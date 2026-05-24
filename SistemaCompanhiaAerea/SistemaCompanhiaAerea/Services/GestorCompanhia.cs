using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCompanhiaAerea.Enums;
using SistemaCompanhiaAerea.Exceptions;
using SistemaCompanhiaAerea.Models;

namespace SistemaCompanhiaAerea.Services
{
    /// <summary>
    /// Gestor central da companhia aérea. Coordena aeronaves, voos, passageiros e reservas.
    /// </summary>
    public class GestorCompanhia
    {
        public string NomeCompanhia { get; private set; }
        public List<Aeronave> Aeronaves { get; private set; }
        public List<Voo> Voos { get; private set; }
        public List<Passageiro> Passageiros { get; private set; }
        public List<Reserva> Reservas { get; private set; }

        public GestorCompanhia(string nomeCompanhia)
        {
            if (string.IsNullOrWhiteSpace(nomeCompanhia))
                throw new ArgumentException("O nome da companhia não pode ser vazio.");

            NomeCompanhia = nomeCompanhia;
            Aeronaves = new List<Aeronave>();
            Voos = new List<Voo>();
            Passageiros = new List<Passageiro>();
            Reservas = new List<Reserva>();
        }

        // =====================================================================
        // AERONAVES
        // =====================================================================

        /// <summary>
        /// Cadastra uma nova aeronave na frota da companhia.
        /// </summary>
        public void CadastrarAeronave(Aeronave aeronave)
        {
            if (aeronave == null) throw new ArgumentNullException("aeronave");

            if (Aeronaves.Any(a => a.Matricula.Equals(aeronave.Matricula, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    string.Format("Já existe uma aeronave com a matrícula '{0}'.", aeronave.Matricula));

            Aeronaves.Add(aeronave);
            Console.WriteLine("Aeronave " + aeronave.Matricula + " (" + aeronave.Tipo + ") cadastrada com sucesso.");
        }

        /// <summary>
        /// Adiciona uma aeronave (alias para CadastrarAeronave).
        /// </summary>
        public void AdicionarAeronave(Aeronave aeronave)
        {
            CadastrarAeronave(aeronave);
        }

        /// <summary>
        /// Procura uma aeronave pela matrícula.
        /// </summary>
        public Aeronave ProcurarAeronave(string matricula)
        {
            return Aeronaves.FirstOrDefault(a =>
                a.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase));
        }

        // =====================================================================
        // PASSAGEIROS
        // =====================================================================

        /// <summary>
        /// Cadastra um novo passageiro no sistema.
        /// </summary>
        public void CadastrarPassageiro(Passageiro passageiro)
        {
            if (passageiro == null) throw new ArgumentNullException("passageiro");

            if (Passageiros.Any(p => p.NumeroPassaporte.Equals(passageiro.NumeroPassaporte, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    string.Format("Já existe um passageiro com o passaporte '{0}'.", passageiro.NumeroPassaporte));

            Passageiros.Add(passageiro);
            Console.WriteLine("Passageiro " + passageiro.Nome + " cadastrado com sucesso (ID: " + passageiro.Id + ").");
        }

        /// <summary>
        /// Adiciona um passageiro (alias para CadastrarPassageiro).
        /// </summary>
        public void AdicionarPassageiros(Passageiro passageiro)
        {
            CadastrarPassageiro(passageiro);
        }

        /// <summary>
        /// Procura um passageiro por nome ou passaporte.
        /// </summary>
        public Passageiro ProcurarPassageiros(string termoPesquisa)
        {
            return Passageiros.FirstOrDefault(p =>
                p.Nome.IndexOf(termoPesquisa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                p.NumeroPassaporte.Equals(termoPesquisa, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Procura um passageiro pelo ID.
        /// </summary>
        public Passageiro ProcurarPassageiroPorId(int id)
        {
            return Passageiros.FirstOrDefault(p => p.Id == id);
        }

        // =====================================================================
        // VOOS
        // =====================================================================

        /// <summary>
        /// Cria e regista um novo voo.
        /// </summary>
        public void CriarVoo(Voo voo)
        {
            if (voo == null) throw new ArgumentNullException("voo");

            if (Voos.Any(v => v.NumeroVoo.Equals(voo.NumeroVoo, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    string.Format("Já existe um voo com o número '{0}'.", voo.NumeroVoo));

            voo.CriarVoo();
            Voos.Add(voo);
            Console.WriteLine("Voo " + voo.NumeroVoo + " (" + voo.Origem + " -> " + voo.Destino + ") criado com sucesso.");
        }

        /// <summary>
        /// Adiciona um voo (alias para CriarVoo).
        /// </summary>
        public void AdicionarVoo(Voo voo)
        {
            CriarVoo(voo);
        }

        /// <summary>
        /// Procura um voo pelo número.
        /// </summary>
        public Voo ProcurarVoo(string numeroVoo)
        {
            return Voos.FirstOrDefault(v =>
                v.NumeroVoo.Equals(numeroVoo, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Lista todos os voos disponíveis (com lugares disponíveis e data futura).
        /// </summary>
        public List<Voo> ListarVoos()
        {
            return Voos.Where(v => v.Activo).OrderBy(v => v.DataHoraPartida).ToList();
        }

        /// <summary>
        /// Pesquisa voos por origem e destino.
        /// </summary>
        public List<Voo> PesquisarVoos(string origem, string destino, DateTime? data = null)
        {
            var resultado = Voos.Where(v =>
                v.Activo &&
                v.Origem.Equals(origem.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase) &&
                v.Destino.Equals(destino.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase));

            if (data.HasValue)
                resultado = resultado.Where(v => v.DataHoraPartida.Date == data.Value.Date);

            return resultado.OrderBy(v => v.DataHoraPartida).ToList();
        }

        // =====================================================================
        // RESERVAS
        // =====================================================================

        /// <summary>
        /// Efectua uma reserva para um passageiro num voo, numa determinada classe.
        /// </summary>
        public Reserva EfectuarReserva(Passageiro passageiro, Voo voo, ClasseVoo classe)
        {
            if (passageiro == null) throw new ArgumentNullException("passageiro");
            if (voo == null) throw new ArgumentNullException("voo");

            // Verificar se o passageiro já tem reserva activa neste voo
            bool jaTemReserva = Reservas.Any(r =>
                r.Passageiro.Id == passageiro.Id &&
                r.Voo.NumeroVoo == voo.NumeroVoo &&
                (r.Estado == EstadoReserva.Activa || r.Estado == EstadoReserva.CheckInRealizado));

            if (jaTemReserva)
                throw new InvalidOperationException(
                    string.Format("O passageiro '{0}' já tem uma reserva activa no voo '{1}'.",
                        passageiro.Nome, voo.NumeroVoo));

            // Criar a reserva (o construtor verifica disponibilidade e ocupa o assento)
            Reserva reserva = new Reserva(passageiro, voo, classe);

            // Adicionar ao voo e à lista global
            voo.AdicionarReserva(reserva);
            Reservas.Add(reserva);

            Console.WriteLine("Reserva efectuada: " + reserva.IdReserva +
                " | " + passageiro.Nome + " | Voo " + voo.NumeroVoo +
                " | Assento " + reserva.Bilhete.NumeroAssento +
                " | Preço: " + reserva.Bilhete.Preco.ToString("C"));

            return reserva;
        }

        /// <summary>
        /// Cancela uma reserva existente.
        /// </summary>
        public void CancelarReserva(string idReserva)
        {
            Reserva reserva = ProcurarReserva(idReserva);
            if (reserva == null)
                throw new InvalidOperationException("Reserva '" + idReserva + "' não encontrada.");

            reserva.CancelarReserva();
            reserva.Voo.RemoverReserva(reserva);
        }

        /// <summary>
        /// Procura uma reserva pelo ID.
        /// </summary>
        public Reserva ProcurarReserva(string idReserva)
        {
            return Reservas.FirstOrDefault(r =>
                r.IdReserva.Equals(idReserva, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Remove uma reserva da lista global.
        /// </summary>
        public void RemoverReservas(Reserva reserva)
        {
            if (reserva != null)
                Reservas.Remove(reserva);
        }

        /// <summary>
        /// Lista todas as reservas de um passageiro.
        /// </summary>
        public List<Reserva> ListarReservas(Passageiro passageiro = null)
        {
            if (passageiro == null)
                return Reservas.ToList();
            return Reservas.Where(r => r.Passageiro.Id == passageiro.Id).ToList();
        }

        /// <summary>
        /// Lista todos os passageiros de um voo.
        /// </summary>
        public List<Passageiro> ListarPassageiros(string numeroVoo)
        {
            Voo voo = ProcurarVoo(numeroVoo);
            if (voo == null)
                throw new InvalidOperationException("Voo '" + numeroVoo + "' não encontrado.");
            return voo.ListarPassageiros();
        }

        // =====================================================================
        // CHECK-IN E EMBARQUE
        // =====================================================================

        /// <summary>
        /// Realiza o check-in de uma reserva.
        /// </summary>
        public void RealizarCheckIn(string idReserva)
        {
            Reserva reserva = ProcurarReserva(idReserva);
            if (reserva == null)
                throw new InvalidOperationException("Reserva '" + idReserva + "' não encontrada.");

            reserva.RealizarCheckIn();
        }

        /// <summary>
        /// Confirma o embarque de uma reserva.
        /// </summary>
        public void ConfirmarEmbarque(string idReserva)
        {
            Reserva reserva = ProcurarReserva(idReserva);
            if (reserva == null)
                throw new InvalidOperationException("Reserva '" + idReserva + "' não encontrada.");

            reserva.ConfirmarEmbarque();
        }

        // =====================================================================
        // RELATÓRIOS
        // =====================================================================

        /// <summary>
        /// Emite um relatório geral da companhia.
        /// </summary>
        public void EmitirRelatorio()
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║         RELATÓRIO DA COMPANHIA AÉREA             ║");
            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║ Companhia  : " + NomeCompanhia.PadRight(36) + "║");
            Console.WriteLine("║ Data       : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm").PadRight(36) + "║");
            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║ Aeronaves  : " + Aeronaves.Count.ToString().PadRight(36) + "║");
            Console.WriteLine("║ Voos       : " + Voos.Count.ToString().PadRight(36) + "║");
            Console.WriteLine("║ Passageiros: " + Passageiros.Count.ToString().PadRight(36) + "║");
            Console.WriteLine("║ Reservas   : " + Reservas.Count.ToString().PadRight(36) + "║");

            int reservasActivas = Reservas.Count(r => r.Estado == EstadoReserva.Activa);
            int reservasCanceladas = Reservas.Count(r => r.Estado == EstadoReserva.Cancelada);
            int checkInsRealizados = Reservas.Count(r => r.Estado == EstadoReserva.CheckInRealizado);
            int embarcados = Reservas.Count(r => r.Estado == EstadoReserva.Embarcado);

            Console.WriteLine("║   - Activas       : " + reservasActivas.ToString().PadRight(29) + "║");
            Console.WriteLine("║   - Canceladas    : " + reservasCanceladas.ToString().PadRight(29) + "║");
            Console.WriteLine("║   - Check-In OK   : " + checkInsRealizados.ToString().PadRight(29) + "║");
            Console.WriteLine("║   - Embarcados    : " + embarcados.ToString().PadRight(29) + "║");

            decimal receitaTotal = Reservas
                .Where(r => r.Estado != EstadoReserva.Cancelada)
                .Sum(r => r.Bilhete.Preco);

            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║ Receita Total: " + receitaTotal.ToString("C").PadRight(34) + "║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Lista todos os voos registados.
        /// </summary>
        public void ListarTodosVoos()
        {
            if (Voos.Count == 0)
            {
                Console.WriteLine("Não existem voos registados.");
                return;
            }

            Console.WriteLine("\n===== LISTA DE VOOS =====");
            foreach (var voo in Voos.OrderBy(v => v.DataHoraPartida))
            {
                Console.WriteLine(voo.ToString());
            }
            Console.WriteLine("=========================");
        }

        /// <summary>
        /// Lista todos os passageiros registados.
        /// </summary>
        public void ListarTodosPassageiros()
        {
            if (Passageiros.Count == 0)
            {
                Console.WriteLine("Não existem passageiros registados.");
                return;
            }

            Console.WriteLine("\n===== LISTA DE PASSAGEIROS =====");
            foreach (var p in Passageiros.OrderBy(p => p.Nome))
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine("================================");
        }
    }
}
