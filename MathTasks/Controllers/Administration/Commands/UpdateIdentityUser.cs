using AutoMapper;
using MathTasks.Authorization;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

        //_mapper.Map(request.ViewModel, user);
        //// UpdateClaimsInStore(IEnumerable<UserClaim> userClaims, IdentityUser user, IEnumerable<Claim> claims) // call expected here
        //var t1 = await UpdateClaims(request, user);
        ////var t2 = await UpdateUserContentClaims(request, user);
        //return user; // should return some object with user and errors, each tagged to specific claim types

        //await UpdateClaimsInStore(request.ViewModel.ToList(), user, await _userManager.GetClaimsAsync(user));
        return user;
    }

    private async Task UpdateClaimsInStore(IEnumerable<UserClaim> userClaims, IdentityUser user, IEnumerable<Claim> claims)
    {
        var tasks = new List<Task<IdentityResult>>();
        foreach (var userClaim in userClaims)
        {
            var claimsToUpdate = claims.Where(_ => _.Type == userClaim.ClaimType);
            var newClaimValue = GetAggregateUserClaimValue(userClaim.ClaimType!, userClaims);
            var newClaim = new Claim(userClaim.ClaimType!, newClaimValue);
            foreach (var claim in claimsToUpdate)
            {
                tasks.Add(_userManager.ReplaceClaimAsync(user, claim, newClaim));
            }
        }
        Task t = Task.WhenAll(tasks);

        try
        {
            await t;
        }
        catch (Exception)
        { }
    }

    private string GetAggregateUserClaimValue(string type, IEnumerable<UserClaim> userClaims)
    {
        if (userClaims is null)
        {
            return default(bool).ToString();
        }
        if (!userClaims.Any())
        {
            return default(bool).ToString();
        }
        var filter = userClaims.Where(_ => _.ClaimType == type);
        var filterValuesAsBool = filter.Select(_ => _.IsSelected);
        var result = filterValuesAsBool.Aggregate((first, second) => first && second);
        return result.ToString();
    }













    private async Task<IdentityResult> UpdateClaims(UpdateIdentityUserCommand request, IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var claim = claims.FirstOrDefault(_ => _.Type == ClaimsStore.IsAdminClaimType);
        if (claim is not null)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }
        return await _userManager.AddClaimAsync(user, new Claim(ClaimsStore.IsAdminClaimType, request.ViewModel.IsAdmin.IsSelected.ToString()));
    }

    private async Task<IdentityResult> UpdateUserContentClaims(UpdateIdentityUserCommand request, IdentityUser user)
    {
        var claimTypesToDelete = request.ViewModel.MathTaskContentEditorClaims?.Select(_ => _.ClaimType).ToArray();
        var claimsToDelete = (await _userManager.GetClaimsAsync(user)).Where(_ => claimTypesToDelete?.Contains(_.Type) ?? false);
        var result = await _userManager.RemoveClaimsAsync(user, claimsToDelete);
        if (!result.Succeeded)
        {
            throw new Exception($"Cannot delete existing claims from the user id = {user.Id}");
        }
        result = await _userManager.AddClaimsAsync(user, request.ViewModel.MathTaskContentEditorClaims?.Select(_ => new Claim(_.ClaimType!, _.IsSelected.ToString())));
        if (!result.Succeeded)
        {
            throw new Exception($"Cannot create claims for user id = {user.Id}");
        }
        return result;
    }

    // update claim value like a transaction
    // 1. claim exists
    // 1.1. delete claims by type
    // 1.2. if task failed return object with error message
    // 1.3. create new claim by type and value
    // 1.4. if task failed return object with error message
    // 1.5. return object with successful message
    // 2. claim does not exist
    // 2.1. run 1.3. create new claim by type and value
    // 2.2. run 1.4.
    private async Task UpdateClaim(IdentityUser identityUser, string claimType, string claimName)
    {
        var claimsToUpdate = (await _userManager.GetClaimsAsync(identityUser))
            .Where(_ => ClaimsStore.GetAllClaimTypes().Contains(_.Type));
        var tasks = new List<Task<IdentityResult>>();
        foreach (var claim in claimsToUpdate)
        {
            var newClaim = new Claim("", ""); //CreateClaim(UpdateIdentityUserCommand request, claim.Type);
            tasks.Add(_userManager.ReplaceClaimAsync(identityUser, claim, newClaim));
        }
    }

    // todo uncomment
    //private Claim CreateClaim(UpdateIdentityUserCommand request, string ClaimType)
    //{
    //    if (ClaimType == ClaimsStore.IsAdminClaimType)
    //    {
    //        return new ClaimsFactory(request).CreateIsAdminClaim();
    //    }
    //    if (ClaimType == )
    //    {

    //    }
    //    var claimValue = request.ViewModel.
    //}

    internal class ClaimsFactory
    {
        private readonly UpdateIdentityUserCommand _request;

        internal ClaimsFactory(UpdateIdentityUserCommand request)
        {
            _request = request;
        }

        internal Claim CreateIsAdminClaim()
        {
            var value = _request.ViewModel.IsAdmin.ToString();
            return new Claim(ClaimsStore.IsAdminClaimType, value);
        }

        private string GetClaimValueByType(IList<UserClaim> claims, string claimType) => GetUserClaimByType(claims, claimType)?.IsSelected.ToString() ?? string.Empty;

        private UserClaim? GetUserClaimByType(IList<UserClaim> claims, string claimType) => claims.FirstOrDefault(_ => _.ClaimType == claimType);

        internal Claim CreateCanCreateMathTaskClaim()
        {
            var claimType = ClaimsStore.CanCreateMathTask;
            var value = _request.ViewModel.MathTaskContentEditorClaims?.First(_ => _.ClaimType == claimType).IsSelected.ToString();
            return new Claim(claimType, value!);
        }

        // todo uncomment
        //internal Claim CreateCanCreateMathTaskClaim()
        //{
        //    var claimType = ClaimsStore.CanCreateMathTask;
        //    var value = _request.ViewModel.MathTaskContentEditorClaims?.First(_ => _.ClaimType == claimType).IsSelected.ToString();
        //    return new Claim(claimType, value);
        //}
    }

    private Task AlterVersionUpdateClaim(IdentityUser identityUser, IEnumerable<Claim> claimsToUpdate, Claim newClaim)
    {
        var tasks = new List<Task<IdentityResult>>();
        foreach (var claim in claimsToUpdate)
        {
            tasks.Add(_userManager.ReplaceClaimAsync(identityUser, claim, newClaim));
        }
        return Task.WhenAll(tasks);
    }


}
