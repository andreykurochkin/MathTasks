using MathTasks.Infrastructure.Mappers.Base;
using MathTasks.Models;
using MathTasks.ViewModels;

namespace MathTasks.Infrastructure.Mappers
{
    public class TagMapperConfiguration:MapperConfigurationBase
    {
        public TagMapperConfiguration()
        {
            CreateMap<Tag, TagViewModel>();
            CreateMap<Tag, UpdateTagViewModel>();
            CreateMap<UpdateTagViewModel,Tag>();
        }
    }
}
