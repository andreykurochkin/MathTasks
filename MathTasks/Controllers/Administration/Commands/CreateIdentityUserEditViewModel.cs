using MathTasks.Authorization;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.Administration.Commands;

public record CreateIdentityUserEditViewModelCommand(IdentityUser IdentityUser) : IRequest<IdentityUserEditViewModel?>;

public class CreateIdentityUserEditViewModelCommandHandler : IRequestHandler<CreateIdentityUserEditViewModelCommand, IdentityUserEditViewModel?>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateIdentityUserEditViewModelCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IdentityUserEditViewModel?> Handle(CreateIdentityUserEditViewModelCommand request, CancellationToken cancellationдToken)
    {
        if (request.IdentityUser is null)
        {
            return await Task.FromResult<IdentityUserEditViewModel?>(null);
        }
        var model = CreateModel(request.IdentityUser, await _userManager.GetClaimsAsync(request.IdentityUser));
        return model;
    }

    private IdentityUserEditViewModel CreateModel(IdentityUser identityUser, IList<Claim> claims)
    {
        var model = new IdentityUserEditViewModel
        {
            Id = identityUser.Id,
            Email = identityUser.Email,
            IsAdmin = GetIsAdminClaimValue(claims)
        };
        return model;
    }

    private bool GetIsAdminClaimValue(IList<Claim> claims)
    {
        var claim = claims.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
        return claim is null ? default(bool) : bool.Parse(claim.Value);
    }
}
