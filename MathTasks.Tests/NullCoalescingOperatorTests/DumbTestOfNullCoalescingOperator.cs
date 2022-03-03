using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using MathTasks.ViewModels;
using MathTasks.Authorization;
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
        var result = _sut.FirstOrDefault(_ => _.ClaimType == ClaimsStore.IsAdminClaimType)?.IsSelected ?? false;
        result.Should().BeFalse();
    }

    [Fact]
    public void NullCoalescingOperator_ShouldReturnEmptyString_WhenNoUserClaimFound()
    {
        var result = _sut.FirstOrDefault(_ => _.ClaimType == ClaimsStore.CanDeleteMathTask)?.IsSelected ?? false;
        result.Should().Be(false);
    }
}
