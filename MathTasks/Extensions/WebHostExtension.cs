﻿using MathTasks.Data;
using MathTasks.Models;
using MathTasks.Seed;
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

        public StartupEntities(ApplicationDbContext? applicationDbContext)
        {
            _context = applicationDbContext;
            _tags = CreateEntities<Tag>(TagsCount, EntityFactory.CreateTag);
            _mathTasks = CreateEntities<MathTask>(MathTasksCount, EntityFactory.CreateMathTask);
        }

        public void Seed()
        {
            InitializeNavigationProperties();
            _context?.SaveChanges();
        }
        private void InitializeNavigationProperties()
        {

        }

        private List<T> CreateEntities<T>(int count, Func<T> func) => Enumerable.Range(1, count)
                    .Select(i => func())
                    .ToList();
    }
}