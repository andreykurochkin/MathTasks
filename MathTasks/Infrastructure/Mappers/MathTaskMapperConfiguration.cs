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
            CreateMap<MathTaskCreateViewModel, MathTask>()
                .ForMember(mathTask => mathTask.Id, option => option.Ignore())
                .ForMember(mathTask => mathTask.Tags, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedAt, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedAt, option => option.Ignore());
            CreateMap<MathTask, MathTaskUpdateViewModel>();
            CreateMap<MathTaskUpdateViewModel, MathTask>()
                .ForMember(mathTask => mathTask.Id, option => option.Ignore())
                .ForMember(mathTask => mathTask.Tags, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedAt, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedAt, option => option.Ignore());
        }
    }
}
