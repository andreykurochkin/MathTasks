using AutoMapper;
using MathTasks.Authorization;
using MathTasks.ViewModels;
using MediatR;
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

    public UpdateIdentityUserCommandHandler(UserManager<IdentityUser> userManager, IMapper mapper, ILogger<UpdateIdentityUserCommandHandler> logger)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
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
        //await SequentualUpdateClaimsInStore(new EnumeratorIdentityUserEditViewModel(request.ViewModel), user, await _userManager.GetClaimsAsync(user));
        // todo process here
        await UpdateClaims(
            user: user,
            claimProvider: new DefaultClaimProvider(userClaims),
            func: UpdateClaimLogged);
        return user;

    }

    private async Task SequentualUpdateClaimsInStore(IEnumerable<UserClaim> userClaims, IdentityUser user, IEnumerable<Claim> claims)
    {
        var logId = Guid.NewGuid().ToString();
        _logger.LogDebug($"{logId}: start {nameof(UpdateClaimsInStore)}");
        var tasks = new List<IdentityResult>();
        var indexedUserClaims = userClaims.Select((item, index) => new { index, item });
        foreach (var indexedUserClaim in indexedUserClaims)
        {
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) start");
            var claimsToUpdate = claims.Where(_ => _.Type == indexedUserClaim.item.ClaimType).ToList();
            var newClaimValue = GetAggregateUserClaimValue(indexedUserClaim.item.ClaimType!, userClaims);
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) aggregate value {newClaimValue}");
            _logger.LogDebug($"{logId} claim type {indexedUserClaim.item.ClaimType}");
            var newClaim = new Claim(indexedUserClaim.item.ClaimType!, newClaimValue);
            foreach (var claim in claimsToUpdate)
            {
                tasks.Add(await _userManager.ReplaceClaimAsync(user, claim, newClaim));
            }
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) stop");
        }
        tasks.ForEach(_ => _logger.LogDebug($"{logId}: taskResult: {_.Succeeded}"));
    }

    private async Task AlterUpdateClaims(IdentityUser user, IClaimProvider claimProvider)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in claims)
        {
            var newClaim = claimProvider.Create(claim.Type);
            if (newClaim is not null)
            {
                await UpdateClaim(user!, claim!, newClaim);
            }
        }
    }

    private async Task UpdateClaims(IdentityUser user, IClaimProvider claimProvider, Func<IdentityUser, Claim, Claim, Task<IdentityResult>>  func)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in claims)
        {
            var newClaim = claimProvider.Create(claim.Type);
            if (newClaim is not null)
            {
                await func(user!, claim!, newClaim);
            }
        }
    }

    internal interface IClaimProvider
    {
        Claim? Create(string type);
    }

    internal class DefaultClaimProvider : IClaimProvider
    {
        private readonly IEnumerable<UserClaim>? _userClaims;

        public DefaultClaimProvider(IEnumerable<UserClaim> userClaims) => _userClaims = userClaims;

        public Claim? Create(string type)
        {
            var value = GetAggregateUserClaimValue(type);
            return (value is null) ? null : new Claim(type, value);
        }

        private string GetAggregateUserClaimValue(string type)
        {
            if (_userClaims is null)
            {
                return default(bool).ToString();
            }
            if (!_userClaims.Any())
            {
                return default(bool).ToString();
            }
            var filter = _userClaims.Where(_ => _.ClaimType == type);
            var filterValuesAsBool = filter.Select(_ => _.IsSelected);
            var result = filterValuesAsBool.Aggregate((first, second) => first && second);
            return result.ToString();
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
            PrintErrors(_logger, result.Errors);
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
        errors.Select((error, index) => new {index, error})
            .ToList()
            .ForEach(_ => logger.LogDebug($"{_.index}) code: {_.error.Code}, description: {_.error.Description}"));
    }

    /// <summary>
    /// that one raises error because of parallel access to dbContext
    /// </summary>
    /// <param name="userClaims"></param>
    /// <param name="user"></param>
    /// <param name="claims"></param>
    /// <returns></returns>
    private async Task UpdateClaimsInStore(IEnumerable<UserClaim> userClaims, IdentityUser user, IEnumerable<Claim> claims)
    {
        var logId = Guid.NewGuid().ToString();
        _logger.LogDebug($"{logId}: start {nameof(UpdateClaimsInStore)}");
        var tasks = new List<Task<IdentityResult>>();
        var indexedUserClaims = userClaims.Select((item, index) => new { index, item });
        foreach (var indexedUserClaim in indexedUserClaims)
        {
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) start");
            var claimsToUpdate = claims.Where(_ => _.Type == indexedUserClaim.item.ClaimType).ToList();
            var newClaimValue = GetAggregateUserClaimValue(indexedUserClaim.item.ClaimType!, userClaims);
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) aggregate value {newClaimValue}");
            _logger.LogDebug($"{logId} claim type {indexedUserClaim.item.ClaimType}");
            var newClaim = new Claim(indexedUserClaim.item.ClaimType!, newClaimValue);
            foreach (var claim in claimsToUpdate)
            {
                tasks.Add(_userManager.ReplaceClaimAsync(user, claim, newClaim));
            }
            _logger.LogDebug($"{logId}: {indexedUserClaim.index}) stop");
        }
        Task t = Task.WhenAll(tasks);

        try
        {
            await t;
            _logger.LogDebug($"{logId}: task results:");
            foreach (var task in tasks)
            {
                _logger.LogDebug($"{logId}: task results: {task.Status}");
            }
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
        return await _userManager.AddClaimAsync(user, new Claim(ClaimsStore.IsAdminClaimType, request.ViewModel.IsAdmin?.IsSelected.ToString() ?? default(bool).ToString()));
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
            var value = _request.ViewModel.IsAdmin?.ToString() ?? default(bool).ToString();
            return new Claim(ClaimsStore.IsAdminClaimType, value.ToString());
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
