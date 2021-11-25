using FluentAssertions;
using MathTasks.Models;
using MathTasks.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MathTasks.Tests
{
    internal class EntityFactoryDongle
    {
        internal Tag CreateTag() => EntityFactory.CreateTag();
        internal MathTask CreateMathTask() => EntityFactory.CreateMathTask();
    }
    public class EntityFactoryTests
    {
        private readonly EntityFactoryDongle _sut;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public EntityFactoryTests(ITestOutputHelper testOutputHelper)
        {
            _sut = new EntityFactoryDongle();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        internal void CreateTag_ShouldCreateInstance_WithProperlyInitializedFields()
        {
            var result = _sut.CreateTag();

            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().NotBeNullOrEmpty();
            result.MathTasks.Should().BeNull();
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(result, _serializerOptions));
        }

        [Fact]
        internal void CreateMathTask_ShouldCreateInstance_WithProperlyInitializedFields()
        {
            var result = _sut.CreateMathTask();

            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Theme.Should().NotBeNullOrEmpty();
            result.Content.Should().NotBeNullOrEmpty();
            result.CreatedAt.Should().BeOnOrAfter(default(DateTime));
            result.CreatedBy.Should().NotBeNullOrEmpty();
            result.UpdatedBy.Should().NotBeNullOrEmpty();
            result.UpdatedAt.Should().BeOnOrAfter(default(DateTime));
            result.Tags.Should().BeNull();

            _testOutputHelper.WriteLine(JsonSerializer.Serialize(result, _serializerOptions));
        }
    }
}
