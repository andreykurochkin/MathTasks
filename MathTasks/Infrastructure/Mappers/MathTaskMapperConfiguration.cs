using MathTasks.Infrastructure.Mappers.Base;
using MathTasks.Models;
using MathTasks.ViewModels;

namespace MathTasks.Infrastructure.Mappers
{
    public class MathTaskMapperConfiguration : MapperConfigurationBase
    {
        public MathTaskMapperConfiguration()
        {
            CreateMap<MathTask, MathTaskViewModel>();
            CreateMap<MathTaskCreateViewModel, MathTask>();
            CreateMap<MathTask, MathTaskUpdateViewModel>();
            CreateMap<MathTaskUpdateViewModel, MathTask>();
        }
    }
}
