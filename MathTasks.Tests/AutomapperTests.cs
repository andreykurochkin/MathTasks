using MathTasks.Infrastructure.Mappers.Base;
using Xunit;
using FluentAssertions;
using AutoMapper;
using MathTasks.Models;
using MathTasks.ViewModels;

namespace MathTasks.Tests;

public class AutomapperTests
{
    internal class MapperRegistrationDongle
    {
        internal MapperConfiguration GetMapperConfiguration() => MapperRegistration.GetMapperConfiguration();
    }

    private readonly MapperRegistrationDongle _sut;
    private readonly IMapper _mapper;

    public AutomapperTests(IMapper mapper)
    {
        _sut = new MapperRegistrationDongle();
        _mapper = mapper;
    }

    [Fact]
    [Trait("Automapper", "Mapper configuration")]
    public void GetMapperConfiguration_ShouldBeNotNull_WhenDataIsValid()
    {
        var result = _sut.GetMapperConfiguration();

        result.Should().NotBeNull();
    }

    [Fact]
    [Trait("Automapper", "Mapper configuration")]
    public void GetMapperConfiguration_ShouldBeValid_WhenDataIsValid()
    {
        var result = _sut.GetMapperConfiguration();

        result.AssertConfigurationIsValid();
    }

    [Fact]
    [Trait("Automapper", "Mapper behavior")]
    public void Map_ShouldReturnNull_WhenInputIsNull()
    {
        MathTask entity = null!;

        var result = _mapper.Map<MathTaskViewModel>(entity);

        result.Should().BeNull();
    }
}
