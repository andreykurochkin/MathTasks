using MathTasks.Data;
using MathTasks.Models;
using MathTasks.Seed;
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
            var context = scope.ServiceProvider
                .GetService(typeof(ApplicationDbContext));
            new StartupEntities(context as ApplicationDbContext).Seed();
            return host;
        }
    }

    // CreateEntity - Factory
    // 

    public interface ISeedItem
    {
        public void Seed();
    }

    public interface ISeedValidation
    {
        public bool Validate();
    }

    public class StartupEntities
    {
        private const int MathTasksCount = 50;
        private const int TagsCount = 10;
        private ApplicationDbContext? _context;
        private List<MathTask> _mathTasks;
        private List<Tag> _tags;

        public StartupEntities(ApplicationDbContext? applicationDbContext)
        {
            _context = applicationDbContext;
            _tags = CreateEntities<Tag>(TagsCount, EntityFactory.CreateTag);
            _mathTasks = CreateEntities<MathTask>(MathTasksCount, EntityFactory.CreateMathTask);
            InitializeNavigationProperties();
        }
        private bool IsSeeded()
        {
            if (_context?.Tags.Count() != 0)
            {
                return true;
            }
            if (_context.MathTasks.Count() != 0)
            {
                return true;
            }
            return false;
        }
        public void Seed()
        {
            if (IsSeeded())
            {
                return;
            }
            _context?.Tags.AddRange(_tags);
            _context?.MathTasks.AddRange(_mathTasks);
            _context?.SaveChanges();
        }
        private void InitializeNavigationProperties() => _mathTasks.ForEach(t => t.Tags = _tags.ToRandomList());

        private List<T> CreateEntities<T>(int count, Func<T> func) => Enumerable.Range(1, count)
                    .Select(i => func())
                    .ToList();
    }
}
