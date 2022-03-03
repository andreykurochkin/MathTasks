using AutoMapper;
using MathTasks.Authorization;
using MathTasks.Infrastructure.Helpers;
using MathTasks.Infrastructure.Providers.Base;
using MathTasks.Providers;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    private readonly ILogger<UpdateIdentityUserCommandHandler> _logger;
    private readonly ICommandHandler<AggregateUserClaimValue, bool> _aggregateUserClaimValueHandler;

    public UpdateIdentityUserCommandHandler(UserManager<IdentityUser> userManager, IMapper mapper, ILogger<UpdateIdentityUserCommandHandler> logger, ICommandHandler<AggregateUserClaimValue, bool> aggregateUserClaimValueHandler)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _aggregateUserClaimValueHandler = aggregateUserClaimValueHandler;
    }
    public async Task<IdentityUser?> Handle(UpdateIdentityUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ViewModel.Id);
        if (user is null)
        {
            return await Task.FromResult<IdentityUser?>(null);
        }

        _mapper.Map(request.ViewModel, user);
        IEnumerable<UserClaim> userClaims = new EnumeratorIdentityUserEditViewModel(request.ViewModel).ToList();
        await UpdateClaims(
            user: user,
            userClaims: userClaims.ToList(),
            func: UpdateClaimLogged);
        return user;

    }

    private async Task UpdateClaims(IdentityUser user, IEnumerable<UserClaim> userClaims, Func<IdentityUser, Claim, Claim, Task<IdentityResult>> func)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in claims)
        {
            var aggregateClaimValue = new AggregateUserClaimValue
            {
                ClaimType = claim.Type,
                UserClaims = userClaims
            };
            var claimValue = _aggregateUserClaimValueHandler.Handle(aggregateClaimValue);
            var newClaim = new Claim(claim.Type, claimValue.ToString());
            if (newClaim is not null)
            {
                await func(user!, claim!, newClaim);
            }
        }
    }

    private Task<IdentityResult> UpdateClaim(IdentityUser user, Claim claim, Claim newClaim)
    {
        return _userManager.ReplaceClaimAsync(user, claim, newClaim);
    }

    private async Task<IdentityResult> UpdateClaimLogged(IdentityUser user, Claim claim, Claim newClaim)
    {
        _logger.LogDebug($"start claim update");
        _logger.LogDebug($"update data: old claim type:{claim.Type}; value: {claim.Value}");
        _logger.LogDebug($"update data: new claim type:{newClaim.Type}; value: {newClaim.Value}");
        var result = await UpdateClaim(user, claim, newClaim);
        if (result.Succeeded)
        {
            _logger.LogDebug($"end claim update");
        }
        else
        {
            OutputHelper.PrintErrors(_logger.LogDebug, result.Errors);
        }
        return result;
    }

    private void PrintErrors<T>(ILogger<T> logger, IEnumerable<IdentityError> errors)
    {
        if (errors == null)
        {
            return;
        }
        if (!errors.Any())
        {
            return;
        }
        logger.LogDebug($"found {errors.Count()} as follows:");
        errors.Select((error, index) => new { index, error })
            .ToList()
            .ForEach(_ => logger.LogDebug($"{_.index}) code: {_.error.Code}, description: {_.error.Description}"));
    }
}
