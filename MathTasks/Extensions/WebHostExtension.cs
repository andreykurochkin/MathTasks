using MathTasks.Data;
using MathTasks.Models;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tynamix.ObjectFiller;

namespace MathTasks.Extensions
{
    public static class WebHostExtension
    {
        public static IHost SeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetService(typeof(ApplicationDbContext));
            new StartupEntities(context as ApplicationDbContext).Seed();
            return host;
        }
    }

    public class StartupEntities
    {
        private const int MathTasksCount = 50;
        private const int TagsCount = 10;
        private ApplicationDbContext? _context;
        private List<MathTask> _mathTasks;
        private List<Tag> _tags;
        private string _topLevelDomain = ".com";

        public StartupEntities(ApplicationDbContext? applicationDbContext)
        {
            _context = applicationDbContext;
            _mathTasks = CreateEntities<MathTask>(MathTasksCount, CreateMathTask);
            _tags = CreateTags(TagsCount);
        }

        public void Seed()
        {
            //InitializeNavigationProperties(CreateMathTasks(50), CreateTags(10));
            _context?.SaveChanges();
        }
        //private void InitializeNavigationProperties(IEnumerable<MathTask> mathTasks, IEnumerable<Tag> tags)
        //{
        //    foreach (var mathTask in mathTasks)
        //    {
        //        foreach (var item in collection)
        //        {
        //            new RandomListItem
        //        }
        //    }
        //}
        private List<T> CreateEntities<T>(int count, Func<T> func) => Enumerable.Range(1, count)
                .Select(i => func())
                .ToList();

        //private List<MathTask> CreateMathTasks(int count) =>
        //    Enumerable.Range(0, count - 1).Select(i => CreateMathTask()).ToList();
        //private List<Tag> CreateTags(int count) =>
        //    Enumerable.Range(0, count - 1).Select(i => CreateTag()).ToList();

        private MathTask CreateMathTask()
        {
            var mathTaskFiller = new Filler<MathTask>();
            mathTaskFiller.Setup()
                .OnProperty(x => x.Tags).IgnoreIt()
                .OnProperty(x => x.Theme).Use(new MnemonicString(5))
                .OnProperty(x => x.Content).Use(new Lipsum(LipsumFlavor.LoremIpsum, 10, 20))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain));
            return mathTaskFiller.Create();
        }

        private Tag CreateTag()
        {
            var tagFiller = new Filler<Tag>();
            tagFiller.Setup()
                .OnProperty(x => x.MathTasks).IgnoreIt()
                .OnProperty(x => x.Name).Use(new MnemonicString(1));
            return tagFiller.Create();
        }
    }
}
