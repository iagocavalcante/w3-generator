using <%= solutionName %>.Domain.Domain.Models;
using System;
using System.Threading.Tasks;

namespace <%= solutionName %>.Infra.Data.MSSQL.Repositories
{
    public class ConnExample
    {
        //protected readonly IConnectionFactory _conn;

        public ConnExample()
        {
            
        }

        public async Task<modelExample> ConsultarXyz(modelExample filtro)
        {
            //using (var conn = _conn.Connection)
            //{
            //    conn.Open();

            //    filtro.nome = await conn.QueryFirstOrDefaultAsync<string>
            //        (
            //    @"SELECT [dep_descr_dep] NomeAgencia
            //      FROM   tbl_contas_correntes a
            //        INNER JOIN[dbo].[tbl_dependencia] b ON[a].[ccr_cod_agencia] = [b].[dep_cod_agencia]
            //        AND[a].[ccr_cod_posto] = [b].[dep_cod_posto]
            //      WHERE[ccr_cod_agencia] = @agencia
            //        AND[ccr_cod_conta_corrente] = @conta"
            //    , new //Cria o filtro correspondente
            //    {
            //        //agencia = conta.Agencia,
            //        //conta = conta.Conta
            //        conta = filtro.id
            //    });
            //}

            throw new NotImplementedException();
        }
    }
}
