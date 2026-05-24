using System;
using SistemaCompanhiaAerea.Enums;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa um bilhete de avião. Compõe uma Reserva (relação de composição).
    /// </summary>
    public class Bilhete
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public string NumeroBilhete { get; private set; }
        public ClasseVoo Classe { get; private set; }
        public string NumeroAssento { get; private set; }
        public decimal Preco { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public bool Valido { get; private set; }

        public Bilhete(ClasseVoo classe, string numeroAssento, decimal preco)
        {
            if (string.IsNullOrWhiteSpace(numeroAssento))
                throw new ArgumentException("O número do assento não pode ser vazio.");
            if (preco < 0)
                throw new ArgumentException("O preço não pode ser negativo.");

            Id = _contadorId++;
            NumeroBilhete = GerarNumeroBilhete();
            Classe = classe;
            NumeroAssento = numeroAssento.ToUpper();
            Preco = preco;
            DataEmissao = DateTime.Now;
            Valido = true;
        }

        private string GerarNumeroBilhete()
        {
            return string.Format("BIL-{0:yyyyMMdd}-{1:D5}", DateTime.Now, _contadorId);
        }

        /// <summary>
        /// Emite o bilhete, mostrando os seus detalhes.
        /// </summary>
        public void EmitirBilhete(Reserva reserva)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║        BILHETE DE PASSAGEM               ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║ Nº Bilhete  : " + NumeroBilhete.PadRight(28) + "║");
            Console.WriteLine("║ Passageiro  : " + reserva.Passageiro.Nome.PadRight(28) + "║");
            Console.WriteLine("║ Passaporte  : " + reserva.Passageiro.NumeroPassaporte.PadRight(28) + "║");
            Console.WriteLine("║ Voo         : " + reserva.Voo.NumeroVoo.PadRight(28) + "║");
            Console.WriteLine("║ Origem      : " + reserva.Voo.Origem.PadRight(28) + "║");
            Console.WriteLine("║ Destino     : " + reserva.Voo.Destino.PadRight(28) + "║");
            Console.WriteLine("║ Partida     : " + reserva.Voo.DataHoraPartida.ToString("dd/MM/yyyy HH:mm").PadRight(28) + "║");
            Console.WriteLine("║ Assento     : " + NumeroAssento.PadRight(28) + "║");
            Console.WriteLine("║ Classe      : " + Classe.ToString().PadRight(28) + "║");
            Console.WriteLine("║ Preço       : " + Preco.ToString("C").PadRight(28) + "║");
            Console.WriteLine("║ Emissão     : " + DataEmissao.ToString("dd/MM/yyyy HH:mm").PadRight(28) + "║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
        }

        /// <summary>
        /// Invalida o bilhete (em caso de cancelamento).
        /// </summary>
        public void Invalidar()
        {
            Valido = false;
        }

        public void ExibirBilhete()
        {
            Console.WriteLine("  Bilhete Nº  : " + NumeroBilhete);
            Console.WriteLine("  Assento     : " + NumeroAssento);
            Console.WriteLine("  Classe      : " + Classe);
            Console.WriteLine("  Preço       : " + Preco.ToString("C"));
            Console.WriteLine("  Emissão     : " + DataEmissao.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("  Válido      : " + (Valido ? "Sim" : "Não"));
        }

        public override string ToString()
        {
            return string.Format("{0} | Assento: {1} | Classe: {2} | Preço: {3:C}",
                NumeroBilhete, NumeroAssento, Classe, Preco);
        }
    }
}
