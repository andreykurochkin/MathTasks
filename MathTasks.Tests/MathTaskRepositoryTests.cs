using Xunit;
using Microsoft.EntityFrameworkCore;
using MathTasks.Tests.Infrastructure.Fixtures;
using MathTasks.Persistent.Repositories;
using FluentAssertions;
using System.Linq;
using System;

namespace MathTasks.Tests;

public class MathTaskRepositoryTests : IClassFixture<MathTaskRepositoryFixture>
{
    private readonly MathTaskRepository? _sut;
    public MathTaskRepositoryTests(MathTaskRepositoryFixture fixture)
    {
        _sut = fixture.Create();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void MathTaskRepository_ShouldBeCreated()
    {
        _sut.Should().NotBeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async void GetAll_ShouldReturnTwoItems_WhenDataIsValid()
    {
        const int expected = 2;

        var result = (await _sut!.GetAll()).Count();

        result.Should().Be(expected);
    }

    [Fact, Trait("Category", "Unit")]
    public async void Get_ShouldReturnItem_WithThemeFranceAndTwoTags()
    {
        const string id = "93a448b8-3c02-4a70-b973-373cf4dc29bd";
        const string themeExpected = "Canada";
        const int tagsCountExpected = 2;

        var result = await _sut!.Get(Guid.Parse(id));

        result!.Theme.Should().Be(themeExpected);
        result!.Tags.Count.Should().Be(tagsCountExpected);
    }
}