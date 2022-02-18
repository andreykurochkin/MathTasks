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

public record CreateEditIdentityUserViewModelCommand(IdentityUser IdentityUser) : IRequest<EditIdentityUserViewModel?>;

public class CreateEditIdentityUserViewModelCommandHandler : IRequestHandler<CreateEditIdentityUserViewModelCommand, EditIdentityUserViewModel?>
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateEditIdentityUserViewModelCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<EditIdentityUserViewModel?> Handle(CreateEditIdentityUserViewModelCommand request, CancellationToken cancellationдToken)
    {
        if (request.IdentityUser is null)
        {
            return await Task.FromResult<EditIdentityUserViewModel?>(null);
        }
        var model = CreateModel(request.IdentityUser, await _userManager.GetClaimsAsync(request.IdentityUser));
        return model;
    }

    private EditIdentityUserViewModel CreateModel(IdentityUser identityUser, IList<Claim> claims)
    {
        var model = new EditIdentityUserViewModel
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
