using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Helpers;

public static class OutputHelper
{
    public static void PrintErrors(Action<string, object[]> WriteLine, IEnumerable<IdentityError> errors)
    {
        if (errors is null || !errors.Any())
        {
            return;
        }
        WriteLine($"found {errors.Count()} as follows:", null!);
        errors.Select((error, index) => new { index, error })
            .ToList()
            .ForEach(_ => WriteLine($"{_.index}) code: {_.error.Code}, description: {_.error.Description}", null!));
        //logger.LogDebug($"found {errors.Count()} as follows:");
        //errors.Select((error, index) => new { index, error })
        //    .ToList()
        //    .ForEach(_ => logger.LogDebug($"{_.index}) code: {_.error.Code}, description: {_.error.Description}"));
    }
}
