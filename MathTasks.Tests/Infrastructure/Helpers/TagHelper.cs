using MathTasks.Models;
using System;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.Helpers;

public static class TagHelper
{
    public static Tag GetOne(string id = "a2059a26-b8c4-4448-9017-51ff466efec2") => new Tag
    {
        Id = Guid.Parse(id),
        Name = "french"
    };

    public static IEnumerable<Tag> GetMany()
    {
        yield return GetOne();
        yield return new Tag
        {
            Id = Guid.Parse("ea566c3a-cf4e-40a9-98d5-8ecaee1da653"),
            Name = "english"
        };
    }
}
