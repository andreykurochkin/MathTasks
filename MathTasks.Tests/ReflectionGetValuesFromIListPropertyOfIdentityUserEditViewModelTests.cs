using FluentAssertions;
using MathTasks.Tests.Infrastructure.Fixtures;
using MathTasks.Tests.Infrastructure.ViewModels;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MathTasks.Tests;

public class ReflectionGetValuesFromIListPropertiesOfModelWithTwoUserClaimCollectionsTests : IClassFixture<ModelWithTwoUserClaimCollectionsFixture>
{
    private ModelWithTwoUserClaimCollections _sut;
    private System.Type? _expectedType = typeof(IList<UserClaim>);

    public ReflectionGetValuesFromIListPropertiesOfModelWithTwoUserClaimCollectionsTests(ModelWithTwoUserClaimCollectionsFixture fixture)
    {
        _sut = fixture.Create();
    }

    [Fact]
    public void ReflectionGetValuesFromIListOfClaimValueProperties_ShouldReturnExpectedCount_WhenDataIsValid()
    {
        int expected = _sut.FirstCollection!.Count + _sut.SecondCollection!.Count;

        var collections = typeof(ModelWithTwoUserClaimCollections).GetProperties().Where(_ => _.PropertyType == _expectedType);
        var flattenUserClaims = collections.SelectMany(_ => _.GetValue(_sut) as IList<UserClaim>);

        flattenUserClaims.Count().Should().Be(expected);
    }

    [Fact]
    public void ReflectionGetValuesFromIListOfClaimValueProperties_ShouldReturnExpectedCount_WhenPropertiesAreEmpty()
    {
        const int expected = 0;
        var sut = new ModelWithTwoUserClaimCollections
        {
            FirstCollection = new List<UserClaim>(),
            SecondCollection = new List<UserClaim>(),
        };

        var collections = typeof(ModelWithTwoUserClaimCollections).GetProperties().Where(_ => _.PropertyType == _expectedType);
        var flattenUserClaims = collections.SelectMany(_ => _.GetValue(sut) as IList<UserClaim>);

        flattenUserClaims.Count().Should().Be(expected);
    }

    [Fact]
    public void ReflectionGetValuesFromIListOfClaimValueProperties_ShouldFail_WhenPropertiesAreNull()
    {
        const int expected = 0;
        var sut = new ModelWithTwoUserClaimCollections
        {
            FirstCollection = null,
            SecondCollection = null,
        };

        var collections = typeof(ModelWithTwoUserClaimCollections).GetProperties().Where(_ => _.PropertyType == _expectedType);
        var flattenUserClaims = collections.SelectMany(_ => _.GetValue(sut) as IList<UserClaim>).ToList();

        flattenUserClaims.Count().Should().Be(expected);
    }
}


public class ReflectionGetValuesFromIListPropertyOfIdentityUserEditViewModelTests : IClassFixture<IdentityUserEditViewModelWithMultipleUserClaimPropertiesFixture>
{
    private IdentityUserEditViewModel _sut;

    public ReflectionGetValuesFromIListPropertyOfIdentityUserEditViewModelTests(IdentityUserEditViewModelWithMultipleUserClaimPropertiesFixture fixture)
    {
        _sut = fixture.Create();
    }

    [Fact]
    public void ReflectionGetValue_ShouldIterateOverEnumerableProperty_WhenDataIsValid()
    {
        var expectedType = typeof(IList<UserClaim>);
        
        var collections = typeof(IdentityUserEditViewModel).GetProperties().Where(_ => _.PropertyType == expectedType);
        var result = collections.SelectMany(_ => _.GetValue(_sut) as IList<UserClaim>);

        result.Count().Should().Be(_sut?.MathTaskContentEditorClaims?.Count);
    }
}
