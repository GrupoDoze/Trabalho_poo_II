using SistemaCompanhiaAerea.Enums;

namespace SistemaCompanhiaAerea.Models
{
    /// <summary>
    /// Representa um assento numa aeronave, com classe e estado de ocupação.
    /// </summary>
    public class Assento
    {
        public string Numero { get; private set; }
        public ClasseVoo Classe { get; private set; }
        public bool Ocupado { get; private set; }

        public Assento(string numero, ClasseVoo classe)
        {
            Numero = numero;
            Classe = classe;
            Ocupado = false;
        }

        /// <summary>
        /// Marca o assento como ocupado.
        /// </summary>
        public void Ocupar()
        {
            Ocupado = true;
        }

        /// <summary>
        /// Liberta o assento, ficando disponível novamente.
        /// </summary>
        public void Libertar()
        {
            Ocupado = false;
        }

        public override string ToString()
        {
            string estado = Ocupado ? "Ocupado" : "Livre";
            return string.Format("Assento {0} | Classe: {1} | Estado: {2}", Numero, Classe, estado);
        }
    }
}
