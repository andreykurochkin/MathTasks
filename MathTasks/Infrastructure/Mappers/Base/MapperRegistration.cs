using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MathTasks.Infrastructure.Mappers.Base
{
    public static class MapperRegistration
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            var profiles = GetProfiles().ToList();
            return new MapperConfiguration(cfg =>
                GetProfiles().ToList()
                    .Select(profile => (Profile)Activator.CreateInstance(profile))
            );
        }

        private static IEnumerable<Type> GetProfiles()
        {
            return typeof(Startup).GetTypeInfo()
                .Assembly
                .GetTypes()
                .Where(t => typeof(IAutomapper).IsAssignableFrom(t))
                .Where(t => !t.GetTypeInfo().IsAbstract);
        }
    }
}
