using MathTasks.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Seed;

public class IdentityEntities
{
    public const string AdministratorRoleName = "Administrator";
    public const string RegisteredUserRoleName = "RegisteredUser";
    public const string AdministratorUserEmail = "dev@gmail.com";
    public const string RegisteredUserEmail = "registeredUser@gmail.com";
    private readonly ApplicationDbContext? _applicationDbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserStore<IdentityUser> _userStore;
    private readonly RoleStore<IdentityRole> _roleStore;
    public IdentityEntities(ApplicationDbContext? applicationDbContext, UserManager<IdentityUser> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _roleStore = new RoleStore<IdentityRole>(_applicationDbContext);
        _userStore = new UserStore<IdentityUser>(_applicationDbContext);
    }

    public IEnumerable<string> RoleNames
    {
        get
        {
            yield return AdministratorRoleName;
            yield return RegisteredUserRoleName;
        }
    }

    public IEnumerable<string> UserEmails
    {
        get
        {
            yield return AdministratorUserEmail;
            yield return RegisteredUserEmail;
        }
    }

    private async Task<bool> IsSeeded()
    {
        if (await HasUserWithSpecifiedEmail(AdministratorUserEmail))
        {
            return true;
        }

        if (await HasUserWithSpecifiedEmail(RegisteredUserEmail))
        {
            return true;
        }

        return false;
    }

    public async Task Seed()
    {
        // 1. Validate
        if (await IsSeeded())
        {
            return;
        }

        // 2. Validate roles and Create roles
        foreach (var roleName in RoleNames)
        {
            if (!(await IsValidRoleName(_applicationDbContext!, roleName)))
            {
                continue;
            }
            var identityResult = await _roleStore.CreateAsync(CreateIdentityRole(roleName));
            if (!identityResult.Succeeded)
            {
                throw new Exception(string.Join(", ", identityResult.Errors.Select(e => $"{e.Code}: {e.Description}")));
            }
        }

        // 3. Validate user name Create users
        foreach (var email in UserEmails)
        {
            if (!(await HasUserWithSpecifiedEmail(email)))
            {
                continue;
            }
            var identityResult = await _userStore.CreateAsync(CreateIdentityUser(email));
            if (!identityResult.Succeeded)
            {
                throw new Exception(string.Join(", ", identityResult.Errors.Select(e => $"{e.Code}: {e.Description}")));
            }
        }

        // 4. Add roles to user
        var administrator = await _userStore.FindByEmailAsync(AdministratorUserEmail.ToUpper());
        await _userManager.AddToRolesAsync(administrator, RoleNames);

        // 5. Add roles to user
        var registeredUser = await _userStore.FindByEmailAsync(AdministratorUserEmail.ToUpper());
        await _userManager.AddToRoleAsync(registeredUser, AdministratorRoleName);

        await _applicationDbContext!.SaveChangesAsync();
    }

    private static async Task<bool> IsValidRoleName(IdentityDbContext context, string roleName) => !(await context.Roles.AnyAsync(role => role.Name == roleName));

    private static IdentityRole CreateIdentityRole(string roleName) => new(roleName) { NormalizedName = roleName.ToUpper() };

    private async Task<bool> HasUserWithSpecifiedEmail(string email) => !await _applicationDbContext!.Users.AnyAsync(u => u.Email == email);

    private static IdentityUser CreateIdentityUser(string email)
    {
        var user = new IdentityUser()
        {
            Email = email,
            EmailConfirmed = true,
            UserName = email,
            NormalizedEmail = email.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString("D"),
            NormalizedUserName = email.ToUpper(),
        };
        user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, "123qwe!@#");
        return user;
    }

}
