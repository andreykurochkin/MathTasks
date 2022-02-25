using MathTasks.Models;
using System;
using System.Collections.Generic;

namespace MathTasks.Tests.Infrastructure.Helpers;

public static class MathTaskHelper
{
    public static MathTask GetOne(string id = "93a448b8-3c02-4a70-b973-373cf4dc29bd")
    {
        return new MathTask
        {
            Id = Guid.Parse(id),
            Theme = "Canada",
            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            Tags = new List<Tag>(TagHelper.GetMany())
        };
    }

    public static IEnumerable<MathTask> GetMany()
    {
        yield return GetOne();
        yield return new MathTask()
        {
            Id = Guid.Parse("9d166ed6-d944-47a4-b8ab-c25ae60a4747"),
            Content = "Ut enim ad minim veniam",
            Theme = "France",
            Tags = new List<Tag>() { TagHelper.GetOne() }
        };
    }
}