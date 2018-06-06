using <%= solutionName %>.Domain.Domain.Models;
using <%= solutionName %>.Domain.Domain.Validations;
using <%= solutionName %>.Presentation.WebAPI.Models;
using <%= solutionName %>.Presentation.WebAPI.SwaggerDocs.Examples;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;
using W3TraceCore;

namespace <%= solutionName %>.Presentation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        private readonly modelExampleValidations _modelValidations;

        public ValuesController(IMapper mapper, Trace trace, modelExampleValidations modelValidations) : base(mapper, trace)
        {
            _modelValidations = modelValidations;
        }

        /// <summary>
        /// Examplo GET do model example
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(200, typeof(modelExampleViewModel))]
        [SwaggerResponse(400)]
        [SwaggerResponse(500)]
        [SwaggerRequestExample(typeof(modelExampleViewModel), typeof(FiltromodelExampleEx))]
        [SwaggerResponseExample(200, typeof(modelExampleReturn))]
        [HttpGet]
        public string Get([FromRoute] int id)
        {
            return "value";
        }

        /// <summary>
        /// Exemplo POST
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [SwaggerResponse(200, typeof(modelExampleViewModel))]
        [SwaggerResponse(400)]
        [SwaggerResponse(500)]
        [SwaggerRequestExample(typeof(modelExampleViewModel), typeof(FiltromodelExampleEx))]
        [SwaggerResponseExample(200, typeof(modelExampleReturn))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]modelExampleViewModel filtro)
        {
            //VIEWMODEL TO MODEL
            modelExample dadosModel = _mapper.Map<modelExample>(filtro);

            var validations = await _modelValidations.ValidateAsync(dadosModel);

            if (!validations.IsValid)
                BadRequest(validations);

            //MODEL TO VIEWMODEL
            var retornoViewModel = _mapper.Map<modelExampleViewModel>(dadosModel);

            return Ok(retornoViewModel);

            //EXEMPLO P/ SPACORE
            //var filtrosaldo = _mapper.Map(viewModel, new SaldoPoupanca(GetSPA()));
            //var result = await _FiltroSaldoPoupValidations.ValidateAsync(filtrosaldo == null ? new SaldoPoupanca(GetSPA()) { } : filtrosaldo);
            //if (!result.IsValid)
            //    return BadRequest(result);
            //return await Task.Run(() =>
            //{
            //    return ExecutarMetodo<SaldoPoupanca, SaldoPoupancaViewModel>(() => _saldoPoupanca.Consultar(filtrosaldo));
            //});
        }

    }
}
