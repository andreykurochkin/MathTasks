using MathTasks.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Tests.Infrastructure.Helpers;

public class DbContextHelper
{
    public ApplicationDbContext Context { get; }
    public DbContextHelper()
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase("unit-test-datatabase");
        var options = builder.Options;
        Context = new ApplicationDbContext(options);

        Context.Database.EnsureDeleted();

        Context.AddRange(MathTaskHelper.GetMany());
        Context.SaveChanges();
    }
}
