using MathTasks.Infrastructure.Providers.Base;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MathTasks.Providers;

public interface ICommandHandler<TCommand, TResult>
{
    TResult Handle(TCommand command);
}

public class AggregateUserClaimValue
{
    public virtual string ClaimType { get; set; } = string.Empty;
    public virtual IEnumerable<UserClaim> UserClaims { get; set; } = Enumerable.Empty<UserClaim>();
}

public class AggregateValueHandler : ICommandHandler<AggregateUserClaimValue, bool>
{
    private bool IsValid(AggregateUserClaimValue command)
    {
        foreach (var func in new Func<AggregateUserClaimValue, bool>[]
        {
            (_) => (_.UserClaims is null),
            (_) => (!_.UserClaims.Any())
        })
        {
            var result = func(command);
            if (result)
            {
                return default(bool);
            }
        }
        return true;
    }

    public bool Handle(AggregateUserClaimValue command)
    {
        if (!IsValid(command))
        {
            return default(bool);
        }
        var query = command.UserClaims.Where(_ => _.ClaimType == command.ClaimType)
            .Select(_ => _.IsSelected);
        var result = query.Aggregate((first, second) => first && second);
        return result;
    }
}

public class DefaultClaimProvider : IClaimProvider
{
    private readonly IEnumerable<UserClaim>? _userClaims;

    public DefaultClaimProvider(IEnumerable<UserClaim> userClaims)
    {
        _userClaims = userClaims;
    }

    //private readonly UserManager<IdentityUser> _userManager;
    //private readonly IHttpContextAccessor _httpContextAccessor;

    public Claim? Create(string type, IEnumerable<UserClaim> userClaims)
    {
        var value = GetAggregateUserClaimValue(type, userClaims);
        return value is null ? null : new Claim(type, value);
    }

    public Claim? Create(string type)
    {
        throw new NotImplementedException();
    }

    public Claim? Create(string type, string value)
    {
        throw new NotImplementedException();
    }

    //private void GetIEnumerable<UserClaim>

    private string GetAggregateUserClaimValue(string type, IEnumerable<UserClaim> userClaims)
    {
        //_userManager.FindByIdAsync(_httpContextAccessor.HttpContext.User.GetUserId())

        //_httpContextAccessor.HttpContext.User.
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
