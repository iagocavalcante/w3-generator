namespace W3TraceCore.Models
{
    public class Erro
    {
        /// <summary>
        /// Código do erro
        /// </summary>
        public int codigo { get; set; }
        /// <summary>
        /// Mensagem do erro
        /// </summary>
        public string mensagem { get; set; }
        /// <summary>
        /// Propriedade que lançou o erro
        /// </summary>
        public string propriedade { get; set; }
    }
}
