using FluentAssertions;
using MathTasks.Authorization;
using MathTasks.Tests.Infrastructure.Fixtures;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MathTasks.Tests;

public class ReflectionGetValueFromPropertyOfIdentityUserEditViewModelTests : IClassFixture<IdentityUserEditViewModelWithSingleUserClaimPropertyFixture>
{
    private IdentityUserEditViewModel _sut;
    const string _propertyNameUnderTest = "IsAdmin";
    public ReflectionGetValueFromPropertyOfIdentityUserEditViewModelTests(IdentityUserEditViewModelWithSingleUserClaimPropertyFixture fixture)
    {
        _sut = fixture.Create();
    }

    [Fact]
    public void ReflectionGetProperty_ShouldBeNotNull_WhenDataIsValid()
    {
        var result = typeof(IdentityUserEditViewModel).GetProperty(_propertyNameUnderTest)?.GetValue(_sut);

        result.Should().NotBeNull();
    }

    [Fact]
    public void ReflectionGetProperty_ShouldHaveExpectedValue_WhenDataIsValid()
    {
        const string expectedValue = ClaimsStore.IsAdminClaimType;
        
        var result = typeof(IdentityUserEditViewModel).GetProperty(_propertyNameUnderTest)?.GetValue(_sut) as UserClaim;

        result.Should().NotBeNull();
        result!.ClaimType.Should().Be(expectedValue);
    }

    [Fact]
    public void ReflectionPropertyByType_ShouldBeFound_WhenDataIsValid()
    {
        var expectedType = typeof(UserClaim);

        var result = typeof(IdentityUserEditViewModel).GetProperties().Any(_ => _.PropertyType == expectedType);

        result.Should().BeTrue();
    }
}
