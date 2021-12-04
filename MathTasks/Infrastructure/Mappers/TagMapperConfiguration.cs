using AutoMapper;
using MathTasks.Infrastructure.Mappers.Base;
using MathTasks.Models;
using MathTasks.ViewModels;
using System.Collections.Generic;

namespace MathTasks.Infrastructure.Mappers
{
    public class TagMapperConfiguration : MapperConfigurationBase
    {
        public TagMapperConfiguration()
        {
            CreateMap<Tag, TagViewModel>();
            CreateMap<Tag, UpdateTagViewModel>();
            CreateMap<UpdateTagViewModel, Tag>()
                .ForMember(tag => tag.MathTasks, option => option.Ignore());
            CreateMap<Tag, TagCloudViewModel>()
                .ForMember(tagCloudViewModel => tagCloudViewModel.CssClass, option => option.Ignore())
                .ForMember(tagCloudViewModel => tagCloudViewModel.Total, option => option.MapFrom<CustomResolver>());
        }
    }

    public class CustomResolver : IValueResolver<Tag, TagCloudViewModel, int>
    {
        public int Resolve(Tag source, TagCloudViewModel destination, int destMember, ResolutionContext context) => source.MathTasks is null ? 0 : source.MathTasks.Count;
    }
}
