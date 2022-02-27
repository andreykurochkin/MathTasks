using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace MathTasks.Tests;

public class IdentityUserEditViewModelTests
{
    public static IEnumerable<object[]> GetData()
    {
        yield return new object[]
        {
            new IdentityUserEditViewModel { },
            0
        };
        yield return new object[]
        { 
            new IdentityUserEditViewModel 
            {
                IsAdmin = UserClaimsHelper.GetOne(),
                MathTaskContentEditorClaims = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
            },
            5
        };
        yield return new object[]
        {
            new IdentityUserEditViewModel
            {
                IsAdmin = null,
                MathTaskContentEditorClaims = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
            },
            4
        };
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Enumerator_ShouldReturnExpectedAmountOfItems_WhenDataIsValid(IdentityUserEditViewModel sut, int expected)
    {
        var result = sut.Count();
         
        result.Should().Be(expected);
    }
}
