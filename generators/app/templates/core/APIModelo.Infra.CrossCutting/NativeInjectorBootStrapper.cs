using <%= solutionName %>.Domain.Domain.Validations;
using Microsoft.Extensions.DependencyInjection;
using W3TraceCore;

namespace <%= solutionName %>.Infra.CrossCutting
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Repositories 

            #endregion

            #region Services 
            //services.AddScoped<IConnectionFactory, ConnectionFactory>(x => new ConnectionFactory(new CryptSPA().decryptDES(configuration.GetConnectionString("sqlclust05"), "w3@sb1r0")));
            #endregion

            #region Validations
            services.AddScoped<modelExampleValidations>();
            #endregion

        }
    }
}
