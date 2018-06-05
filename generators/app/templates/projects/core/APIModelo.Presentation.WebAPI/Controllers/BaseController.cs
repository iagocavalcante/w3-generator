using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using W3TraceCore;
using W3TraceCore.Models;
using Newtonsoft.Json;

namespace <%= solutionName %>.Presentation.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        protected IMapper _mapper;
        protected Trace trace;

        public BaseController(IMapper mapper, Trace t)
        {
            _mapper = mapper;
            trace = t;
        }

        /// <summary>
        /// Retorna status code 400 com a lista de erros de validação
        /// </summary>
        /// <param name="validation"></param>
        /// <returns></returns>
        protected BadRequestObjectResult BadRequest(ValidationResult validation)
        {
            return BadRequest(validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));
        }


        public class InternalServerError : IActionResult
        {
            string error = "";

            public InternalServerError(string e)
            {
                error = e;
            }

            public async Task ExecuteResultAsync(ActionContext context)
            {
                var objectResult = new ObjectResult(error)
                {
                    StatusCode = 500
                };

                await objectResult.ExecuteResultAsync(context);
            }
        }

        //internal BaseSPA GetSPA()
        //{
        //    return HttpContext.Request.GetBaseSPA();
        //}

        //internal IActionResult ExecutarMetodo<T, T2>(Func<Task<T>> metodo)
        //{
        //    try
        //    {
        //        var retorno = metodo();
        //        return Ok(Mapper.Map<T2>(retorno.Result));
        //    }
        //    catch (Exception ex)
        //    {
        //        var details = "";
        //        if (ex.GetType() == typeof(SqlException))
        //        {
        //            details = GetSqlDetails(ex);
        //            trace.GravarTrace($" MESSAGE: {ex.Message}\nDETAILS:\n {details}\n" +
        //                $"INNEREXCEPTION:\n{Trace.GetInnerExceptions(ex)}\nSTACKTRACE:\n{ex.StackTrace}", 1);
        //        }

        //        if (ex.InnerException.GetType() == typeof(SPAException))
        //        {
        //            var spaEx = (SPAException)ex.InnerException;
        //            if (spaEx.IsNegocio)
        //            {
        //                if (ex.InnerException != null && spaEx.InnerException.GetType() == typeof(SqlException))
        //                {
        //                    details = GetSqlDetails(spaEx.InnerException);
        //                }
        //            }

        //            trace.GravarTrace($" MESSAGE: {ex.Message}\nDETAILS:\n {details}\n" +
        //                $"INNEREXCEPTION:\n{Trace.GetInnerExceptions(ex)}\nSTACKTRACE:\n{ex.StackTrace}", 1);

        //            if (spaEx.IsNegocio)
        //                return BadRequest(spaEx.Message);
        //        }

        //        if (ex.GetType() == typeof(BusinessException))
        //        {
        //            BusinessException bex = (BusinessException)ex;
        //            var dados = bex.Dados;
        //            var descricao = bex.Descricao;
        //            string msg = JsonConvert.SerializeObject(dados);
        //            trace.GravarTrace($" MESSAGE: {msg}\nINNEREXCEPTION:\n\t{descricao}\nSTACKTRACE:\n{ex.StackTrace}", 1);

        //            return BadRequest(dados);
        //        }
        //        throw;
        //    }
        //}

        private static string GetSqlDetails(Exception ex)
        {
            SqlException sex = (SqlException)ex;
            string details = $"\tLineNumber: {sex.LineNumber}\n\t" +
                        $"Message: {sex.Message}\n\t" +
                        $"Number: {sex.Number}\n\t" +
                        $"Procedure: {sex.Procedure}\n\t" +
                        $"Server: {sex.Server}";
            return details;
        }

    }
}