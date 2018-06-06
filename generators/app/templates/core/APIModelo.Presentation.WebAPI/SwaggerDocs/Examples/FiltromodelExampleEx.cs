using <%= solutionName %>.Domain.Domain.Models;
using Swashbuckle.AspNetCore.Examples;

namespace <%= solutionName %>.Presentation.WebAPI.SwaggerDocs.Examples
{
    public class FiltromodelExampleEx : IExamplesProvider
    {
        public object GetExamples()
        {
            return new modelExample
            {
                id = 1,
                nome = "Estevao",
                sobrenome = "Braga"
            };
        }
    }
}
