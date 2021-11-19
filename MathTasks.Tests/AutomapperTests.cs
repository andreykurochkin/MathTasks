using MathTasks.Infrastructure.Mappers.Base;
using Xunit;
using FluentAssertions;
using AutoMapper;

namespace MathTasks.Tests
{
    public class AutomapperTests
    {
        internal class MapperRegistrationDongle
        {
            internal MapperConfiguration GetMapperConfiguration() => MapperRegistration.GetMapperConfiguration();
        }

        private readonly MapperRegistrationDongle _sut;

        public AutomapperTests()
        {
            _sut = new MapperRegistrationDongle();
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
    }
}
