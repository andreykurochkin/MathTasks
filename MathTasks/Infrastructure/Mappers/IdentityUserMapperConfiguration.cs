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

            CreateMap<Tuple<IdentityUser, IList<Claim>>, IdentityUserEditViewModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Item1.Id))
                .ForMember(dest => dest.Email, options => options.MapFrom(source => source.Item1.Email))
                .ForMember(dest => dest.IsAdmin, options => options.MapFrom<IsAdminResolver>())
                .ForMember(dest => dest.MathTaskContentEditorClaims, options => options.MapFrom<MathTaskContentEditorClaimsResolver>());

            CreateMap<IdentityUserEditViewModel, IdentityUser>();
        }

        internal class MathTaskContentEditorClaimsResolver : IValueResolver<Tuple<IdentityUser, IList<Claim>>, IdentityUserEditViewModel, IList<UserClaim>?>
        {
            private const string CreateTitle = "Create";
            private const string ReadTitle = "Read";
            private const string UpdateTitle = "Update";
            private const string DeleteTitle = "Delete";

            public IList<UserClaim>? Resolve(Tuple<IdentityUser, IList<Claim>> source, IdentityUserEditViewModel destination, IList<UserClaim>? destMember, ResolutionContext context)
            {
                return new UserClaim[]
                {
                    new UserClaim
                    {
                        IsSelected = source.Item2.Any(_ => _.Type == ClaimsStore.CanCreateMathTask)
                            ? bool.Parse(source.Item2.First(_ => _.Type == ClaimsStore.CanCreateMathTask).Value)
                            : default(bool),
                        DisplayName = CreateTitle,
                        ClaimType = ClaimsStore.CanCreateMathTask,
                        ClaimValue = source.Item2.Any(_ => _.Type == ClaimsStore.CanCreateMathTask)
                            ? source.Item2.First(_ => _.Type == ClaimsStore.CanCreateMathTask).Value
                            : default(bool).ToString()
                    },
                    new UserClaim
                    {
                        IsSelected = source.Item2.Any(_ => _.Type == ClaimsStore.CanReadMathTask)
                            ? bool.Parse(source.Item2.First(_ => _.Type == ClaimsStore.CanReadMathTask).Value)
                            : default(bool),
                        DisplayName = ReadTitle,
                        ClaimType = ClaimsStore.CanReadMathTask,
                        ClaimValue = source.Item2.Any(_ => _.Type == ClaimsStore.CanReadMathTask)
                            ? source.Item2.First(_ => _.Type == ClaimsStore.CanReadMathTask).Value
                            : default(bool).ToString()
                    },
                    new UserClaim
                    {
                        IsSelected = source.Item2.Any(_ => _.Type == ClaimsStore.CanUpdateMathTask)
                            ? bool.Parse(source.Item2.First(_ => _.Type == ClaimsStore.CanUpdateMathTask).Value)
                            : default(bool),
                        DisplayName = UpdateTitle,
                        ClaimType = ClaimsStore.CanUpdateMathTask,
                        ClaimValue = source.Item2.Any(_ => _.Type == ClaimsStore.CanUpdateMathTask)
                            ? source.Item2.First(_ => _.Type == ClaimsStore.CanUpdateMathTask).Value
                            : default(bool).ToString()
                    },
                    new UserClaim
                    {
                        IsSelected = source.Item2.Any(_ => _.Type == ClaimsStore.CanDeleteMathTask)
                            ? bool.Parse(source.Item2.First(_ => _.Type == ClaimsStore.CanDeleteMathTask).Value)
                            : default(bool),
                        DisplayName = DeleteTitle,
                        ClaimType = ClaimsStore.CanDeleteMathTask,
                        ClaimValue = source.Item2.Any(_ => _.Type == ClaimsStore.CanDeleteMathTask)
                            ? source.Item2.First(_ => _.Type == ClaimsStore.CanDeleteMathTask).Value
                            : default(bool).ToString()
                    }
                };
            }
        }

        internal class IsAdminResolver : IValueResolver<Tuple<IdentityUser, IList<Claim>>, IdentityUserEditViewModel, UserClaim?>
        {
            public UserClaim? Resolve(Tuple<IdentityUser, IList<Claim>> source, IdentityUserEditViewModel destination, UserClaim? destMember, ResolutionContext context)
            {
                var claim = source.Item2.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
                var claimValue = claim is null ? default(bool).ToString() : claim.Value;
                var newClaim = new UserClaim
                {
                    ClaimType = ClaimsStore.IsAdminClaimType,
                    ClaimValue = claimValue,
                    IsSelected = bool.Parse(claimValue),
                    DisplayName = string.Empty
                };
                return newClaim;
            }
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
