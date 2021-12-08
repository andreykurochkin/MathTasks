using MathTasks.Data;
using MathTasks.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;

namespace MathTasks.Extensions
{
    public static class WebHostExtension
    {
        public static async Task<IHost> SeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService(typeof(ApplicationDbContext));
            new StartupEntities(context as ApplicationDbContext).Seed();

            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            await new IdentityEntities(context as ApplicationDbContext, userManager!).Seed();
            return host;
        }
    }
}
