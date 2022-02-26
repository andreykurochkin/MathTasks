using FluentAssertions;
using MathTasks.Tests.Infrastructure.ViewModels;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MathTasks.Tests.ReflectionTests;

public class GetPropertiesTests
{
    [Theory]
    [InlineData("System.String", 0)]
    [InlineData("MathTasks.ViewModels.UserClaim,MathTasks", 2)]
    [InlineData("System.Collections.Generic.IList`1[[MathTasks.ViewModels.UserClaim, MathTasks]]", 2)]
    public void GetProperties_ShouldReturnExpectedAmountOfProperties_WhenDataIsValid(string typeName, int expected)
    {
        var sut = typeof(ReflectionTestsModel).GetProperties();

        var result = sut.Count(_ => _.PropertyType == Type.GetType(typeName));

        result.Should().Be(expected);
    }
}