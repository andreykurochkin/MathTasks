using FluentAssertions;
using MathTasks.Tests.Infrastructure.Fixtures;
using MathTasks.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MathTasks.Tests.ReflectionTests;

public class GetValueTests : IClassFixture<FactoryOfReflectionTestModelsFixture>
{
    private readonly FactoryOfReflectionTestModelsFixture _fixture;

    public GetValueTests(FactoryOfReflectionTestModelsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void GetValue_ShouldReturnNull_WhenFirstItemPropertyValueNull()
    {
        var data = _fixture.CreateModelAllPropertiesAreNull();
        var sut = data.GetType().GetProperty(nameof(data.FirstItem));

        var result = sut?.GetValue(data) as UserClaim;

        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_ShouldReturnNotNull_WhenFirstItemPropertyValueIsNotNull()
    {
        var data = _fixture.CreateModelFirstItemIsNotNull();
        var sut = data.GetType().GetProperty(nameof(data.FirstItem));

        var result = sut?.GetValue(data) as UserClaim;

        result.Should().NotBeNull();
    }


    [Fact]
    public void GetValue_ShouldReturnItemWithExpectedValues_WhenFirstItemPropertyValueIsNotNull()
    {
        var data = _fixture.CreateModelFirstItemIsNotNull();
        var expectedValue1 = data!.FirstItem!.ClaimValue;
        var expectedValue2 = data.FirstItem.ClaimType;
        var expectedValue3 = data.FirstItem.IsSelected;
        var sut = data.GetType().GetProperty(nameof(data.FirstItem));

        var result = sut?.GetValue(data) as UserClaim;

        result!.ClaimValue.Should().Be(expectedValue1);
        result!.ClaimType.Should().Be(expectedValue2);
        result!.IsSelected.Should().Be(expectedValue3);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstItemAndSecondItemPropertiesAreNotNull()
    {
        const int expected = 2;
        var data = _fixture.CreateModelFirstItemAndSecondItemIsNotNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(UserClaim))
            .Select(_ => _.GetValue(data) as UserClaim)
            .Where(_ => _ is not null);

        var result = sut?.Count();

        result.Should().Be(expected);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstItemAndSecondItemPropertiesAreNull()
    {
        const int expected = 0;
        var data = _fixture.CreateModelAllPropertiesAreNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(UserClaim))
            .Select(_ => _.GetValue(data))
            .Where(_ => _ is not null);

        var result = sut?.Count();

        result.Should().Be(expected);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstCollectionAndSecondCollectionPropertiesAreNotNull()
    {
        const int expected = 8;
        var data = _fixture.CreateModelFirstCollectionAndSecondCollectionAreNotNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(IList<UserClaim>))
            .SelectMany(_ => _.GetValue(data) as IList<UserClaim> ?? Enumerable.Empty<UserClaim>());

        var result = sut?.Count();

        result.Should().Be(expected);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstCollectionIsNullAndSecondCollectionPropertiesIsNotNull()
    {
        const int expected = 4;
        var data = _fixture.CreateModelFirstCollectionIsNullAndSecondCollectionIsNotNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(IList<UserClaim>))
            .SelectMany(_ => _.GetValue(data) as IList<UserClaim> ?? Enumerable.Empty<UserClaim>());

        var result = sut?.Count();

        result.Should().Be(expected);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstCollectionIsEmptyListAndSecondCollectionPropertiesIsNotNull()
    {
        const int expected = 4;
        var data = _fixture.CreateModelFirstCollectionIsEmptyListAndSecondCollectionIsNotNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(IList<UserClaim>))
            .SelectMany(_ => _.GetValue(data) as IList<UserClaim> ?? Enumerable.Empty<UserClaim>());

        var result = sut?.Count();

        result.Should().Be(expected);
    }

    [Fact]
    public void GetValue_ShouldReturnExpectedAmountOfItems_WhenFirstCollectionAndSecondCollectionPropertiesAreNull()
    {
        const int expected = 0;
        var data = _fixture.CreateModelAllPropertiesAreNull();
        var sut = data.GetType().GetProperties()
            .Where(_ => _.PropertyType == typeof(IList<UserClaim>))
            .SelectMany(_ => _.GetValue(data) as IList<UserClaim> ?? Enumerable.Empty<UserClaim>());

        var result = sut?.Count();

        result.Should().Be(expected);
    }
}
