using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace W3TraceCore
{
    /// <summary>
    /// Classe de retorno
    /// </summary>
    public class Retorno
    {
        /// <summary>
        /// Indicador de sucesso. 
        /// </summary>
        [Required]
        public bool sucesso { get; set; }
        /// <summary>
        /// Dados dinâmicos
        /// </summary>
        [Required]
        public dynamic dados { get; set; }

        public Retorno()
        {
            sucesso = true;
            dados = new ExpandoObject();
        }

        public Retorno(dynamic dados)
        {
            this.sucesso = true;
            this.dados = dados;
        }

        public Retorno(bool sucesso, dynamic dados)
        {
            this.sucesso = sucesso;
            this.dados = dados;
        }
    }
}
