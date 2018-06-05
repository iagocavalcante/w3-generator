using <%= solutionName %>.Domain.Domain.Models;
using <%= solutionName %>.Presentation.WebAPI.Models;
using AutoMapper;

namespace <%= solutionName %>.Presentation.WebAPI.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<modelExample, modelExampleViewModel> ();
        }
    }
}
