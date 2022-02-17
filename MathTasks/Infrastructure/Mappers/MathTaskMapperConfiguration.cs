using AutoMapper;
using MathTasks.Authorization;
using MathTasks.Infrastructure.Mappers.Base;
using MathTasks.Models;
using MathTasks.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MathTasks.Infrastructure.Mappers
{

    public class IdentityUserMapperConfiguration : MapperConfigurationBase
    {

        public IdentityUserMapperConfiguration()
        {
            CreateMap<Tuple<IdentityUser, IList<Claim>>, IdentityUserViewModel>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Item1.Id))
                .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Item1.Email))
                .ForMember(destination => destination.RoleName, options => options.MapFrom<RoleNameResolver>());
        }
    }

    public class RoleNameResolver : IValueResolver<Tuple<IdentityUser, IList<Claim>>, IdentityUserViewModel, string>
    {
        // todo - maybe move to Enumeration
        private const string adminUser = "Administrator";
        private const string registeredUser = "Registered User";
        private const string notFoundUser = "not found";

        public string Resolve(Tuple<IdentityUser, IList<Claim>> source, IdentityUserViewModel destination, string destMember, ResolutionContext context)
        {
            var claim = source.Item2.FirstOrDefault(_=>_.Type == ClaimsStore.IsAdminClaimType);
            if (claim is null)
            {
                return notFoundUser;
            }
            return bool.Parse(claim.Value) ? adminUser : registeredUser;
        }
    }

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
