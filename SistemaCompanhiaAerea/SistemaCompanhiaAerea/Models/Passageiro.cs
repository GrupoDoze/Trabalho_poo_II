using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa um passageiro da companhia aérea.
    /// </summary>
    public class Passageiro
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string NumeroPassaporte { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public DateTime DataNascimento { get; private set; }

        public Passageiro(string nome, string numeroPassaporte, string email, string telefone, DateTime dataNascimento)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do passageiro não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(numeroPassaporte))
                throw new ArgumentException("O número de passaporte não pode ser vazio.");

            Id = _contadorId++;
            Nome = nome.Trim();
            NumeroPassaporte = numeroPassaporte.Trim().ToUpper();
            Email = email != null ? email.Trim() : string.Empty;
            Telefone = telefone != null ? telefone.Trim() : string.Empty;
            DataNascimento = dataNascimento;
        }

        /// <summary>
        /// Calcula a idade actual do passageiro.
        /// </summary>
        public int Idade()
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;
            return idade;
        }

        public void ActualizarDados(string email, string telefone)
        {
            if (!string.IsNullOrWhiteSpace(email))
                Email = email.Trim();
            if (!string.IsNullOrWhiteSpace(telefone))
                Telefone = telefone.Trim();
        }

        public void ExibirDados()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("  PASSAGEIRO #" + Id);
            Console.WriteLine("======================================");
            Console.WriteLine("  Nome       : " + Nome);
            Console.WriteLine("  Passaporte : " + NumeroPassaporte);
            Console.WriteLine("  Email      : " + (string.IsNullOrEmpty(Email) ? "N/A" : Email));
            Console.WriteLine("  Telefone   : " + (string.IsNullOrEmpty(Telefone) ? "N/A" : Telefone));
            Console.WriteLine("  Nascimento : " + DataNascimento.ToString("dd/MM/yyyy") + " (" + Idade() + " anos)");
            Console.WriteLine("======================================");
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} | Passaporte: {2}", Id, Nome, NumeroPassaporte);
        }
    }
}
