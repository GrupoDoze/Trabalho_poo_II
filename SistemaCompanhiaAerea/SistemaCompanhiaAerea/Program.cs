using System;
using System.Collections.Generic;
using SistemaCompanhiaAerea.Enums;
using SistemaCompanhiaAerea.Exceptions;
using SistemaCompanhiaAerea.Models;
using SistemaCompanhiaAerea.Services;

namespace SistemaCompanhiaAerea
{
    /// <summary>
    /// Ponto de entrada do programa. Demonstra todas as funcionalidades do sistema.
    /// </summary>
    class Program
    {
        static GestorCompanhia gestor = new GestorCompanhia("AirPortugal Regional");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Sistema de Gestão de Companhia Aérea";

            bool continuar = true;

            // Inicializar dados de demonstração
            InicializarDadosDemonstracao();

            while (continuar)
            {
                MostrarMenuPrincipal();
                string opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        MenuAeronaves();
                        break;
                    case "2":
                        MenuVoos();
                        break;
                    case "3":
                        MenuPassageiros();
                        break;
                    case "4":
                        MenuReservas();
                        break;
                    case "5":
                        MenuCheckIn();
                        break;
                    case "6":
                        gestor.EmitirRelatorio();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("Obrigado por utilizar o Sistema de Gestão de Companhia Aérea.");
                        break;
                    default:
                        Console.WriteLine("Opcao invalida. Tente novamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        // =====================================================================
        // MENUS
        // =====================================================================

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║    SISTEMA DE GESTÃO DE COMPANHIA AÉREA          ║");
            Console.WriteLine("║    " + gestor.NomeCompanhia.PadRight(46) + "║");
            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Gestão de Aeronaves                          ║");
            Console.WriteLine("║  2. Gestão de Voos                               ║");
            Console.WriteLine("║  3. Gestão de Passageiros                        ║");
            Console.WriteLine("║  4. Gestão de Reservas e Bilhetes                ║");
            Console.WriteLine("║  5. Check-In e Embarque                          ║");
            Console.WriteLine("║  6. Relatório Geral                              ║");
            Console.WriteLine("║  0. Sair                                         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.Write("Escolha uma opcao: ");
        }

        static void MenuAeronaves()
        {
            Console.Clear();
            Console.WriteLine("===== GESTÃO DE AERONAVES =====");
            Console.WriteLine("1. Cadastrar nova aeronave");
            Console.WriteLine("2. Listar aeronaves");
            Console.WriteLine("3. Ver detalhes de aeronave");
            Console.Write("Opcao: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    CadastrarAeronave();
                    break;
                case "2":
                    ListarAeronaves();
                    break;
                case "3":
                    VerDetalhesAeronave();
                    break;
            }
        }

        static void MenuVoos()
        {
            Console.Clear();
            Console.WriteLine("===== GESTÃO DE VOOS =====");
            Console.WriteLine("1. Criar novo voo");
            Console.WriteLine("2. Listar todos os voos");
            Console.WriteLine("3. Pesquisar voos por rota");
            Console.WriteLine("4. Ver detalhes de voo");
            Console.WriteLine("5. Listar passageiros de um voo");
            Console.Write("Opcao: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    CriarVoo();
                    break;
                case "2":
                    gestor.ListarTodosVoos();
                    break;
                case "3":
                    PesquisarVoos();
                    break;
                case "4":
                    VerDetalhesVoo();
                    break;
                case "5":
                    ListarPassageirosVoo();
                    break;
            }
        }

        static void MenuPassageiros()
        {
            Console.Clear();
            Console.WriteLine("===== GESTÃO DE PASSAGEIROS =====");
            Console.WriteLine("1. Cadastrar novo passageiro");
            Console.WriteLine("2. Listar todos os passageiros");
            Console.WriteLine("3. Pesquisar passageiro");
            Console.Write("Opcao: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    CadastrarPassageiro();
                    break;
                case "2":
                    gestor.ListarTodosPassageiros();
                    break;
                case "3":
                    PesquisarPassageiro();
                    break;
            }
        }

        static void MenuReservas()
        {
            Console.Clear();
            Console.WriteLine("===== GESTÃO DE RESERVAS =====");
            Console.WriteLine("1. Efectuar reserva");
            Console.WriteLine("2. Cancelar reserva");
            Console.WriteLine("3. Ver bilhete de reserva");
            Console.WriteLine("4. Listar todas as reservas");
            Console.Write("Opcao: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    EfectuarReserva();
                    break;
                case "2":
                    CancelarReserva();
                    break;
                case "3":
                    VerBilhete();
                    break;
                case "4":
                    ListarReservas();
                    break;
            }
        }

        static void MenuCheckIn()
        {
            Console.Clear();
            Console.WriteLine("===== CHECK-IN E EMBARQUE =====");
            Console.WriteLine("1. Realizar check-in");
            Console.WriteLine("2. Confirmar embarque");
            Console.Write("Opcao: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    RealizarCheckIn();
                    break;
                case "2":
                    ConfirmarEmbarque();
                    break;
            }
        }

        // =====================================================================
        // ACÇÕES
        // =====================================================================

        static void CadastrarAeronave()
        {
            Console.WriteLine("\n--- CADASTRAR AERONAVE ---");
            Console.Write("Matrícula: ");
            string matricula = Console.ReadLine();
            Console.Write("Tipo (ex: Boeing 737): ");
            string tipo = Console.ReadLine();
            Console.Write("Lugares Primeira Classe: ");
            int primeira = LerInteiro();
            Console.Write("Lugares Executiva: ");
            int executiva = LerInteiro();
            Console.Write("Lugares Económica: ");
            int economica = LerInteiro();

            try
            {
                var aeronave = new Aeronave(matricula, tipo, economica, executiva, primeira);
                gestor.CadastrarAeronave(aeronave);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void ListarAeronaves()
        {
            Console.WriteLine("\n===== FROTA DE AERONAVES =====");
            if (gestor.Aeronaves.Count == 0)
            {
                Console.WriteLine("Nenhuma aeronave cadastrada.");
                return;
            }
            foreach (var a in gestor.Aeronaves)
                Console.WriteLine(a.ToString());
        }

        static void VerDetalhesAeronave()
        {
            Console.Write("Matrícula da aeronave: ");
            string matricula = Console.ReadLine();
            var aeronave = gestor.ProcurarAeronave(matricula);
            if (aeronave == null)
                Console.WriteLine("Aeronave não encontrada.");
            else
                aeronave.ExibirInformacoes();
        }

        static void CriarVoo()
        {
            Console.WriteLine("\n--- CRIAR VOO ---");
            Console.Write("Número do voo: ");
            string numero = Console.ReadLine();
            Console.Write("Origem (código IATA, ex: LIS): ");
            string origem = Console.ReadLine();
            Console.Write("Destino (código IATA, ex: OPO): ");
            string destino = Console.ReadLine();
            Console.Write("Data/hora partida (dd/MM/yyyy HH:mm): ");
            DateTime partida;
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out partida))
            {
                Console.WriteLine("Data inválida.");
                return;
            }
            Console.Write("Data/hora chegada (dd/MM/yyyy HH:mm): ");
            DateTime chegada;
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out chegada))
            {
                Console.WriteLine("Data inválida.");
                return;
            }
            Console.Write("Matrícula da aeronave: ");
            string matricula = Console.ReadLine();
            var aeronave = gestor.ProcurarAeronave(matricula);
            if (aeronave == null)
            {
                Console.WriteLine("Aeronave não encontrada.");
                return;
            }

            Console.Write("Preço Económica (EUR): ");
            decimal precoE = LerDecimal();
            Console.Write("Preço Executiva (EUR): ");
            decimal precoEx = LerDecimal();
            Console.Write("Preço Primeira Classe (EUR): ");
            decimal precoP = LerDecimal();

            try
            {
                var voo = new Voo(numero, origem, destino, partida, chegada, aeronave, precoE, precoEx, precoP);
                gestor.CriarVoo(voo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void PesquisarVoos()
        {
            Console.Write("Origem: ");
            string origem = Console.ReadLine();
            Console.Write("Destino: ");
            string destino = Console.ReadLine();

            var voos = gestor.PesquisarVoos(origem, destino);
            if (voos.Count == 0)
            {
                Console.WriteLine("Nenhum voo encontrado para a rota indicada.");
                return;
            }

            Console.WriteLine("\n===== VOOS ENCONTRADOS =====");
            foreach (var v in voos)
                Console.WriteLine(v.ToString());
        }

        static void VerDetalhesVoo()
        {
            Console.Write("Número do voo: ");
            string numero = Console.ReadLine();
            var voo = gestor.ProcurarVoo(numero);
            if (voo == null)
                Console.WriteLine("Voo não encontrado.");
            else
                voo.ExibirInformacoes();
        }

        static void ListarPassageirosVoo()
        {
            Console.Write("Número do voo: ");
            string numero = Console.ReadLine();
            try
            {
                var passageiros = gestor.ListarPassageiros(numero);
                Console.WriteLine("\n===== PASSAGEIROS DO VOO " + numero.ToUpper() + " =====");
                if (passageiros.Count == 0)
                    Console.WriteLine("Nenhum passageiro com reserva activa.");
                else
                    foreach (var p in passageiros)
                        Console.WriteLine(p.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void CadastrarPassageiro()
        {
            Console.WriteLine("\n--- CADASTRAR PASSAGEIRO ---");
            Console.Write("Nome completo: ");
            string nome = Console.ReadLine();
            Console.Write("Número de passaporte: ");
            string passaporte = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();
            Console.Write("Data de nascimento (dd/MM/yyyy): ");
            DateTime nascimento;
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out nascimento))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            try
            {
                var passageiro = new Passageiro(nome, passaporte, email, telefone, nascimento);
                gestor.CadastrarPassageiro(passageiro);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void PesquisarPassageiro()
        {
            Console.Write("Nome ou número de passaporte: ");
            string termo = Console.ReadLine();
            var passageiro = gestor.ProcurarPassageiros(termo);
            if (passageiro == null)
                Console.WriteLine("Passageiro não encontrado.");
            else
                passageiro.ExibirDados();
        }

        static void EfectuarReserva()
        {
            Console.WriteLine("\n--- EFECTUAR RESERVA ---");
            Console.Write("ID do passageiro: ");
            int idPass = LerInteiro();
            var passageiro = gestor.ProcurarPassageiroPorId(idPass);
            if (passageiro == null)
            {
                Console.WriteLine("Passageiro não encontrado.");
                return;
            }

            Console.Write("Número do voo: ");
            string numVoo = Console.ReadLine();
            var voo = gestor.ProcurarVoo(numVoo);
            if (voo == null)
            {
                Console.WriteLine("Voo não encontrado.");
                return;
            }

            Console.WriteLine("Classes disponíveis:");
            Console.WriteLine("  1. Económica  - " + voo.Aeronave.LugaresDisponiveisPorClasse(ClasseVoo.Economica) + " lugares disponíveis");
            Console.WriteLine("  2. Executiva  - " + voo.Aeronave.LugaresDisponiveisPorClasse(ClasseVoo.Executiva) + " lugares disponíveis");
            Console.WriteLine("  3. 1.ª Classe - " + voo.Aeronave.LugaresDisponiveisPorClasse(ClasseVoo.PrimeiraClasse) + " lugares disponíveis");
            Console.Write("Escolha a classe (1-3): ");

            ClasseVoo classe;
            string escolhaClasse = Console.ReadLine();
            switch (escolhaClasse)
            {
                case "1": classe = ClasseVoo.Economica; break;
                case "2": classe = ClasseVoo.Executiva; break;
                case "3": classe = ClasseVoo.PrimeiraClasse; break;
                default:
                    Console.WriteLine("Classe inválida.");
                    return;
            }

            try
            {
                Reserva reserva = gestor.EfectuarReserva(passageiro, voo, classe);
                Console.WriteLine("\nReserva efectuada com sucesso!");
                reserva.Bilhete.EmitirBilhete(reserva);
            }
            catch (VooLotadoException ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
            catch (AssentoOcupadoException ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void CancelarReserva()
        {
            Console.Write("ID da reserva (ex: RES-20260101-0001): ");
            string id = Console.ReadLine();
            try
            {
                gestor.CancelarReserva(id);
                Console.WriteLine("Reserva cancelada com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void VerBilhete()
        {
            Console.Write("ID da reserva: ");
            string id = Console.ReadLine();
            var reserva = gestor.ProcurarReserva(id);
            if (reserva == null)
                Console.WriteLine("Reserva não encontrada.");
            else
                reserva.Bilhete.EmitirBilhete(reserva);
        }

        static void ListarReservas()
        {
            var reservas = gestor.ListarReservas();
            if (reservas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva registada.");
                return;
            }

            Console.WriteLine("\n===== LISTA DE RESERVAS =====");
            foreach (var r in reservas)
                Console.WriteLine(r.ToString());
        }

        static void RealizarCheckIn()
        {
            Console.Write("ID da reserva: ");
            string id = Console.ReadLine();
            try
            {
                gestor.RealizarCheckIn(id);
            }
            catch (CheckInForaDoPrazoException ex)
            {
                Console.WriteLine("ERRO Check-In: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        static void ConfirmarEmbarque()
        {
            Console.Write("ID da reserva: ");
            string id = Console.ReadLine();
            try
            {
                gestor.ConfirmarEmbarque(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: " + ex.Message);
            }
        }

        // =====================================================================
        // DADOS DE DEMONSTRAÇÃO
        // =====================================================================

        static void InicializarDadosDemonstracao()
        {
            Console.WriteLine("A inicializar dados de demonstracao...\n");

            try
            {
                // --- Aeronaves ---
                var aeronave1 = new Aeronave("CS-TTA", "Airbus A320", 150, 20, 8);
                var aeronave2 = new Aeronave("CS-TTB", "Boeing 737-800", 160, 16, 0);
                var aeronave3 = new Aeronave("CS-TTC", "Embraer E190", 88, 10, 0);

                gestor.CadastrarAeronave(aeronave1);
                gestor.CadastrarAeronave(aeronave2);
                gestor.CadastrarAeronave(aeronave3);

                // --- Voos ---
                DateTime agora = DateTime.Now;

                var voo1 = new Voo("AP101", "LIS", "OPO",
                    agora.AddDays(30), agora.AddDays(30).AddHours(1),
                    aeronave1, 89m, 189m, 350m);

                var voo2 = new Voo("AP202", "LIS", "FAO",
                    agora.AddDays(45), agora.AddDays(45).AddHours(1).AddMinutes(15),
                    aeronave2, 79m, 159m, 299m);

                var voo3 = new Voo("AP303", "OPO", "LIS",
                    agora.AddDays(5), agora.AddDays(5).AddHours(1),
                    aeronave3, 99m, 199m, 0m);

                var voo4 = new Voo("AP404", "LIS", "PDL",
                    agora.AddHours(3), agora.AddHours(5),
                    aeronave1, 120m, 220m, 450m);

                gestor.CriarVoo(voo1);
                gestor.CriarVoo(voo2);
                gestor.CriarVoo(voo3);
                gestor.CriarVoo(voo4);

                // --- Passageiros ---
                var p1 = new Passageiro("Maria Silva", "PT123456", "maria@email.com", "912345678", new DateTime(1985, 3, 15));
                var p2 = new Passageiro("Joao Santos", "PT789012", "joao@email.com", "963456789", new DateTime(1990, 7, 22));
                var p3 = new Passageiro("Ana Ferreira", "PT345678", "ana@email.com", "934567890", new DateTime(1978, 11, 5));
                var p4 = new Passageiro("Carlos Mendes", "PT901234", "carlos@email.com", "916789012", new DateTime(2000, 1, 30));
                var p5 = new Passageiro("Sofia Costa", "PT567890", "sofia@email.com", "967890123", new DateTime(1995, 6, 18));

                gestor.CadastrarPassageiro(p1);
                gestor.CadastrarPassageiro(p2);
                gestor.CadastrarPassageiro(p3);
                gestor.CadastrarPassageiro(p4);
                gestor.CadastrarPassageiro(p5);

                // --- Reservas ---
                Reserva r1 = gestor.EfectuarReserva(p1, voo1, ClasseVoo.Executiva);
                Reserva r2 = gestor.EfectuarReserva(p2, voo1, ClasseVoo.Economica);
                Reserva r3 = gestor.EfectuarReserva(p3, voo2, ClasseVoo.Economica);
                Reserva r4 = gestor.EfectuarReserva(p4, voo3, ClasseVoo.Economica);
                Reserva r5 = gestor.EfectuarReserva(p5, voo4, ClasseVoo.PrimeiraClasse);

                // Simular check-in para o voo AP404 (parte em 3h - dentro do prazo de check-in)
                Console.WriteLine("\n[Demo] A simular check-in para o voo AP404...");
                try
                {
                    r5.RealizarCheckIn();
                }
                catch (CheckInForaDoPrazoException)
                {
                    // Em demo, o voo pode estar fora do prazo; forçar o estado
                    r5.AlterarEstado(EstadoReserva.CheckInRealizado);
                    Console.WriteLine("[Demo] Estado de check-in definido manualmente para demonstracao.");
                }

                Console.WriteLine("\nDados de demonstracao carregados com sucesso!");
                Console.WriteLine("Exemplos de IDs de reserva:");
                Console.WriteLine("  " + r1.IdReserva + " (Maria Silva - VOO AP101 - Executiva)");
                Console.WriteLine("  " + r2.IdReserva + " (Joao Santos - VOO AP101 - Economica)");
                Console.WriteLine("  " + r5.IdReserva + " (Sofia Costa - VOO AP404 - 1.a Classe - CheckIn OK)");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inicializar dados: " + ex.Message);
            }

            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        // =====================================================================
        // UTILITÁRIOS
        // =====================================================================

        static int LerInteiro()
        {
            int valor;
            while (!int.TryParse(Console.ReadLine(), out valor))
                Console.Write("Valor inválido. Introduza um número inteiro: ");
            return valor;
        }

        static decimal LerDecimal()
        {
            decimal valor;
            while (!decimal.TryParse(Console.ReadLine(), out valor) || valor < 0)
                Console.Write("Valor inválido. Introduza um número positivo: ");
            return valor;
        }
    }
}
