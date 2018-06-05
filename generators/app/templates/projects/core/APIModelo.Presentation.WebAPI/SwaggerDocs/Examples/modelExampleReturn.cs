using <%= solutionName %>.Presentation.WebAPI.Models;
using Swashbuckle.AspNetCore.Examples;

namespace <%= solutionName %>.Presentation.WebAPI.SwaggerDocs.Examples
{
    public class modelExampleReturn : IExamplesProvider
    {
        public object GetExamples()
        {
            return new modelExampleViewModel
            {
                nome = "nome",
                sobrenome = "sobrenome"
            };
        }
    }
}
