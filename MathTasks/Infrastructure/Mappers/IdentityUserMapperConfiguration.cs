using AutoMapper;
using MathTasks.Authorization;
using MathTasks.Infrastructure.Mappers.Base;
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

            //CreateMap<Tuple<IdentityUser, IList<Claim>>, IdentityUserEditViewModel
        }
        internal class RoleNameResolver : IValueResolver<Tuple<IdentityUser, IList<Claim>>, IdentityUserViewModel, string>
        {
            // todo - maybe move to Enumeration
            private const string adminUser = "Administrator";
            private const string registeredUser = "Registered User";
            private const string notFoundUser = "not found";

            public string Resolve(Tuple<IdentityUser, IList<Claim>> source, IdentityUserViewModel destination, string destMember, ResolutionContext context)
            {
                var claim = source.Item2.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
                if (claim is null)
                {
                    return notFoundUser;
                }
                return bool.Parse(claim.Value) ? adminUser : registeredUser;
            }
        }
    }
}
