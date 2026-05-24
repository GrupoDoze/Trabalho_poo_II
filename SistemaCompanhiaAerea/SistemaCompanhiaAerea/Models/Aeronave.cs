using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaCompanhiaAerea.Enums;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa uma aeronave da companhia aérea, com tipo, capacidade e lista de assentos por classe.
    /// </summary>
    public class Aeronave
    {
        public string Matricula { get; private set; }
        public string Tipo { get; private set; }
        public int CapacidadeTotal { get; private set; }
        public List<Assento> Assentos { get; private set; }

        public Aeronave(string matricula, string tipo, int lugaresEconomica, int lugaresExecutiva, int lugaresPrimeiraClasse)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                throw new ArgumentException("A matrícula da aeronave não pode ser vazia.");
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("O tipo da aeronave não pode ser vazio.");
            if (lugaresEconomica < 0 || lugaresExecutiva < 0 || lugaresPrimeiraClasse < 0)
                throw new ArgumentException("O número de lugares não pode ser negativo.");

            Matricula = matricula;
            Tipo = tipo;
            Assentos = new List<Assento>();

            // Gerar assentos da Primeira Classe (P)
            for (int i = 1; i <= lugaresPrimeiraClasse; i++)
            {
                Assentos.Add(new Assento("P" + i, ClasseVoo.PrimeiraClasse));
            }

            // Gerar assentos Executivos (E)
            for (int i = 1; i <= lugaresExecutiva; i++)
            {
                Assentos.Add(new Assento("E" + i, ClasseVoo.Executiva));
            }

            // Gerar assentos Económicos (A, B, C...)
            for (int i = 1; i <= lugaresEconomica; i++)
            {
                Assentos.Add(new Assento("Y" + i, ClasseVoo.Economica));
            }

            CapacidadeTotal = Assentos.Count;
        }

        /// <summary>
        /// Devolve o número de assentos disponíveis para uma determinada classe.
        /// </summary>
        public int LugaresDisponiveisPorClasse(ClasseVoo classe)
        {
            return Assentos.Count(a => a.Classe == classe && !a.Ocupado);
        }

        /// <summary>
        /// Devolve o número total de assentos disponíveis.
        /// </summary>
        public int TotalLugaresDisponiveis()
        {
            return Assentos.Count(a => !a.Ocupado);
        }

        /// <summary>
        /// Calcula a percentagem de ocupação da aeronave.
        /// </summary>
        public double CalcularOcupacao()
        {
            if (CapacidadeTotal == 0) return 0;
            int ocupados = Assentos.Count(a => a.Ocupado);
            return (double)ocupados / CapacidadeTotal * 100.0;
        }

        /// <summary>
        /// Procura um assento livre numa dada classe.
        /// </summary>
        public Assento ProcurarAssentoLivre(ClasseVoo classe)
        {
            return Assentos.FirstOrDefault(a => a.Classe == classe && !a.Ocupado);
        }

        /// <summary>
        /// Procura um assento pelo número.
        /// </summary>
        public Assento ProcurarAssentoPorNumero(string numero)
        {
            return Assentos.FirstOrDefault(a => a.Numero.Equals(numero, StringComparison.OrdinalIgnoreCase));
        }

        public void ExibirInformacoes()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("  AERONAVE: " + Matricula);
            Console.WriteLine("======================================");
            Console.WriteLine("  Tipo           : " + Tipo);
            Console.WriteLine("  Capacidade     : " + CapacidadeTotal + " lugares");
            Console.WriteLine("  1.ª Classe     : " + Assentos.Count(a => a.Classe == ClasseVoo.PrimeiraClasse) + " lugares");
            Console.WriteLine("  Executiva      : " + Assentos.Count(a => a.Classe == ClasseVoo.Executiva) + " lugares");
            Console.WriteLine("  Económica      : " + Assentos.Count(a => a.Classe == ClasseVoo.Economica) + " lugares");
            Console.WriteLine(string.Format("  Ocupação actual: {0:0.0}%", CalcularOcupacao()));
            Console.WriteLine("======================================");
        }

        public override string ToString()
        {
            return string.Format("Aeronave {0} ({1}) | Capacidade: {2} | Disponíveis: {3}",
                Matricula, Tipo, CapacidadeTotal, TotalLugaresDisponiveis());
        }
    }
}
