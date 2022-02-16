using MathTasks.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MathTasks.Authorization.Stores;

namespace MathTasks.Seed;

public class IdentityEntities
{
    public const string AdministratorUserEmail = "admin@gmail.com";
    public const string RegisteredUserEmail = "registeredUser@gmail.com";
    public const string DefaultPassword = "123qwe!@#";
    private readonly ApplicationDbContext? _applicationDbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserStore<IdentityUser> _userStore;
    public IdentityEntities(ApplicationDbContext? applicationDbContext, UserManager<IdentityUser> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _userStore = new UserStore<IdentityUser>(_applicationDbContext);
    }

    public IEnumerable<string> GetUserEmails()
    {
        yield return AdministratorUserEmail;
        yield return RegisteredUserEmail;
    }

    public async Task Seed()
    {
        foreach (var email in GetUserEmails())
        {
            var userFound = await _userStore.FindByEmailAsync(email.ToUpper()) is not null;
            if (userFound)
            {
                continue;
            }
            var user = IdentityUserFactory.CreateIdentityUser(email, DefaultPassword);
            var identityResult = await _userStore.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new Exception(string.Join(", ", identityResult.Errors.Select(e => $"{e.Code}: {e.Description}")));
            }
            var claims = (email == AdministratorUserEmail) 
                ? ClaimsFactory.CreateAdminClaims() 
                : ClaimsFactory.CreateRegisteredUserClaims();
            await _userManager.AddClaimsAsync(user, claims);
        }
    }

    internal static class ClaimsFactory
    {
        internal static IEnumerable<Claim> CreateAdminClaims() => new Claim[] 
        { 
            new Claim("IsAdmin", "True") 
        };

        internal static IEnumerable<Claim> CreateRegisteredUserClaims() => new Claim[] 
        { 
            new Claim("IsAdmin", "False") 
        };
    }

    internal static class IdentityUserFactory
    {
        internal static IdentityUser CreateIdentityUser(string email, string password)
        {
            var user = NewUser(email);
            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, password);
            return user;
        }

        private static IdentityUser NewUser(string email) => new IdentityUser()
        {
            Email = email,
            EmailConfirmed = true,
            UserName = email,
            NormalizedEmail = email.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            NormalizedUserName = email.ToUpper(),
        };
    }
}
