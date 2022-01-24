using AutoMapper;
using MathTasks.Infrastructure.Mappers.Base;
using MathTasks.Models;
using MathTasks.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MathTasks.Infrastructure.Mappers
{
    public class MathTaskMapperConfiguration : MapperConfigurationBase
    {
        public MathTaskMapperConfiguration()
        {
            CreateMap<MathTask, MathTaskViewModel>();
            CreateMap<MathTaskViewModel, MathTask>();

            CreateMap<MathTaskCreateViewModel, MathTask>()
                .ForMember(mathTask => mathTask.Id, option => option.Ignore())
                .ForMember(mathTask => mathTask.Tags, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedAt, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedAt, option => option.Ignore());

            CreateMap<MathTask, MathTaskUpdateViewModel>();

            // todo MathTaskUpdateViewModel seems to be obsolete
            CreateMap<MathTaskUpdateViewModel, MathTask>()
                .ForMember(mathTask => mathTask.Id, option => option.Ignore())
                .ForMember(mathTask => mathTask.Tags, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedAt, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedAt, option => option.Ignore());

            CreateMap<MathTask, MathTaskEditViewModel>()
                .ForMember(viewModel => viewModel.ReturnUrl, option => option.Ignore())
                .ForMember(viewModel => viewModel.TotalTags, option => option.MapFrom<TotalTagsCustomResolver>())
                .ForMember(viewModel => viewModel.Tags, option => option.MapFrom<TagsCustomResolver>());

            CreateMap<MathTaskEditViewModel, MathTask>()
                .ForMember(mathTask => mathTask.Id, option => option.Ignore())
                .ForMember(mathTask => mathTask.Tags, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedAt, option => option.Ignore())
                .ForMember(mathTask => mathTask.CreatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedBy, option => option.Ignore())
                .ForMember(mathTask => mathTask.UpdatedAt, option => option.Ignore());
        }
    }

    public class TotalTagsCustomResolver : IValueResolver<MathTask, MathTaskEditViewModel, int>
    {
        public int Resolve(MathTask source, MathTaskEditViewModel destination, int destMember, ResolutionContext context) => source.Tags is null ? 0 : source.Tags.Count;
    }

    public class TagsCustomResolver : IValueResolver<MathTask, MathTaskEditViewModel, IEnumerable<string>?>
    {
        public IEnumerable<string> Resolve(MathTask source, MathTaskEditViewModel destination, IEnumerable<string>? destMember, ResolutionContext context) => 
            source.Tags is null 
            ? Enumerable.Empty<string>() 
            : source.Tags.Select(x => x.Name);
    }
}
