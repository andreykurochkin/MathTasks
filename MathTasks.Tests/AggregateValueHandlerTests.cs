using FluentAssertions;
using MathTasks.Providers;
using MathTasks.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MathTasks.Tests;

public class AggregateValueHandlerTests
{
    private readonly AggregateValueHandler _sut;
    public AggregateValueHandlerTests()
    {
        _sut = new AggregateValueHandler();
    }
    public static IEnumerable<object[]> InvalidValues()
    {
        yield return new object[] { null! };
        yield return new object[] { Enumerable.Empty<UserClaim>()! };
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public void Handle_ShouldReturnFalse_WhenDataIsNotValid(IEnumerable<UserClaim> userClaims)
    {
        var mock = new Mock<AggregateUserClaimValue>();
        mock.Setup(_ => _.UserClaims).Returns(userClaims);
        var sut = new AggregateValueHandler();

        var result = sut.Handle(mock.Object);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    [InlineData(true, true, true)]
    [InlineData(false, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, true, false, true)]
    public void Handle_ShouldReturnExpectedValue_WhenDataIsValid(bool expected, params bool[] values)
    {
        const string foo = "foo";
        var mock = new Mock<AggregateUserClaimValue>();
        mock.Setup(x => x.ClaimType).Returns(foo);
        mock.Setup(x => x.UserClaims).Returns(values.Select(_ => new UserClaim { ClaimType = foo, IsSelected = _}));

        var result = _sut.Handle(mock.Object);

        result.Should().Be(expected);
    }
}
