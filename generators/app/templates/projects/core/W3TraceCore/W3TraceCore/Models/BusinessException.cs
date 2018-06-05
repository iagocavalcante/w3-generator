using System;
using System.Data.SqlClient;

namespace W3TraceCore.Models
{
    public class BusinessException : Exception
    {
        public dynamic Dados { get; set; }
        public string Descricao { get; set; }

        public BusinessException(dynamic dados, string message = "", string descricao = "") : base(message)
        {
            Dados = dados;
            Descricao = descricao;
        }

        public BusinessException(dynamic dados, string message, Exception exception) : base(message, exception)
        {
            Dados = dados;
        }
    }
}
