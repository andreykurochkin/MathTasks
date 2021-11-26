using MathTasks.Extensions;
using MathTasks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using Tynamix.ObjectFiller;

namespace MathTasks.Data
{
    public class ApplicationDbContext : DbContextBase
    {
        private readonly string _topLevelDomain = ".com";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MathTask> MathTasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
