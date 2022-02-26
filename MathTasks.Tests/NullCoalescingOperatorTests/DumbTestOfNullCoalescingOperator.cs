using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using MathTasks.ViewModels;
using MathTasks.Authorization;
using System;
using MathTasks.Tests.Infrastructure.Fixtures;
using System.Linq;

namespace MathTasks.Tests.NullCoalescingOperatorTests;

public class DumbTestOfNullCoalescingOperator : IClassFixture<UserClaimsFixture>
{
    private IEnumerable<UserClaim> _sut = null!;

    public DumbTestOfNullCoalescingOperator(UserClaimsFixture fixture)
    {
        _sut = fixture.Create();
    }

    [Fact]
    public void NullCoalescingOperator_ShouldReturnValueOfPropertyOfInstance_WhenUserClaimFound()
    {
        var result = _sut.FirstOrDefault(_ => _.ClaimType == ClaimsStore.IsAdminClaimType)?.IsSelected.ToString() ?? string.Empty;
        result.Should().Be(false.ToString());
    }

    [Fact]
    public void NullCoalescingOperator_ShouldReturnEmptyString_WhenNoUserClaimFound()
    {
        var result = _sut.FirstOrDefault(_ => _.ClaimType == ClaimsStore.CanDeleteMathTask)?.IsSelected.ToString() ?? string.Empty;
        result.Should().Be(string.Empty);
    }
}
