using MathTasks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using MediatR;
using MathTasks.Infrastructure.Services;
using MathTasks.Contracts;
using MathTasks.Models;
using MathTasks.Persistent.Repositories;
using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Authorization.Security;
using Microsoft.AspNetCore.Authorization;
using MathTasks.Providers;
using MathTasks.Infrastructure.Providers.Base;

namespace MathTasks;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });


        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));
        services.AddDatabaseDeveloperPageExceptionFilter();

        //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.AddRequirements(new ManageIsAdminClaimRequirement()));
        });

        //MapperRegistration.GetMapperConfiguration();
        services.AddAutoMapper(typeof(Startup).Assembly);
        services.AddMediatR(typeof(Startup));

        services.AddControllersWithViews().AddRazorRuntimeCompilation();

        services.AddRazorPages();

        // dependency injection
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<ISearchTagsService, SearchTagsService>();

        services.AddScoped<DbContext, ApplicationDbContext>();
        services.AddTransient<IRepository<MathTask, Guid>, EFCoreRepository<MathTask, Guid>>();

        services.AddScoped(typeof(IRepository<MathTask, Guid>), typeof(MathTaskRepository));

        services.AddSingleton<IAuthorizationHandler, IsAdminClaimIsFreeForAllHandler>();

        //services.AddTransient<IClaimProvider, DefaultClaimProvider>();
        services.AddScoped(typeof(ICommandHandler<AggregateUserClaimValue, bool>), typeof(AggregateValueHandler));

        services.AddServerSideBlazor();

    }

    private void MathTaskRepository()
    {
        throw new NotImplementedException();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "alter",
                pattern: "{controller=AlterMathTasks}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
            endpoints.MapBlazorHub();
        });
    }
}
