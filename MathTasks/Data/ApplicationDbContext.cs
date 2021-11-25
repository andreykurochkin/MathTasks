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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private MathTask CreateMathTask()
        {
            var mathTaskFiller = new Filler<MathTask>();
            mathTaskFiller.Setup()
                .OnProperty(x => x.Tags).IgnoreIt()
                .OnProperty(x => x.Theme).Use(new MnemonicString(5))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain));
            return mathTaskFiller.Create();
        }
    }
}
