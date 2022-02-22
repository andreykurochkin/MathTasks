using AutoMapper;
using MathTasks.Authorization;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.Administration.Commands;

public record UpdateIdentityUserCommand(IdentityUserEditViewModel ViewModel) : IRequest<IdentityUser>;

public class UpdateIdentityUserCommandHandler : IRequestHandler<UpdateIdentityUserCommand, IdentityUser?>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateIdentityUserCommandHandler(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<IdentityUser?> Handle(UpdateIdentityUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ViewModel.Id);
        if (user is null)
        {
            return await Task.FromResult<IdentityUser?>(null);
        }
        _mapper.Map(request.ViewModel, user);
        await UpdateClaims(request, user);
         return user;
    }

    private async Task UpdateClaims(UpdateIdentityUserCommand request, IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var claim = claims.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
        if (claim is not null)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }
        await _userManager.AddClaimAsync(user, new Claim(ClaimsStore.IsAdminClaimType, request.ViewModel.IsAdmin.ToString()));
    }
}
