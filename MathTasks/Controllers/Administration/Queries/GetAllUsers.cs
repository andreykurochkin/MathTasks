using AutoMapper;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.Administration.Queries;

public record GetIdentityUserViewModelsQuery(IEnumerable<IdentityUser> IdentityUsers) : IRequest<IEnumerable<IdentityUserViewModel>>;

public class GeIdentityUsersViewModelQueryHandler : IRequestHandler<GetIdentityUserViewModelsQuery, IEnumerable<IdentityUserViewModel>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public GeIdentityUsersViewModelQueryHandler(IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<IEnumerable<IdentityUserViewModel>> Handle(GetIdentityUserViewModelsQuery request, CancellationToken cancellationToken)
    {
        var dbTuples = await GetIdentityUsersWithClaims(request);
        var mappedItems = _mapper.Map<IEnumerable<IdentityUserViewModel>>(dbTuples);
        return mappedItems;
    }

    private async Task<List<Tuple<IdentityUser, IList<Claim>>>> GetIdentityUsersWithClaims(GetIdentityUserViewModelsQuery request)
    {
        var dbTuples = new List<Tuple<IdentityUser, IList<Claim>>>();
        foreach (var user in request.IdentityUsers)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var tuple = new Tuple<IdentityUser, IList<Claim>>(user, claims);
            dbTuples.Add(tuple);
        }
        return dbTuples;
    }
}