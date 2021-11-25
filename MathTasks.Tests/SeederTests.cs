using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FluentAssertions.Xml;
using MathTasks.Models;
using System.Text.Json;
using Xunit.Sdk;
using Xunit.Abstractions;
using Tynamix.ObjectFiller;

namespace MathTasks.Tests
{
    public class SeederTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        private readonly string _topLevelDomain = ".com";

        public SeederTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void FillerInstance_ShouldBeNotNull()
        {
            var mathTaskFiller = new Filler<MathTask>();
            mathTaskFiller.Setup()
                .OnProperty(x => x.Tags).IgnoreIt()
                .OnProperty(x => x.Theme).Use(new MnemonicString(5))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain))
                .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(_topLevelDomain));

            var mathTask = mathTaskFiller.Create();
            
            var tagFiller = new Filler<Tag>();
            tagFiller.Setup()
                .OnProperty(x => x.MathTasks).IgnoreIt()
                .OnProperty(x => x.Name).Use(new MnemonicString(1));

            var tag = tagFiller.Create();

            var mathTaskToJson = JsonSerializer.Serialize(mathTask, _serializerOptions);
            mathTaskToJson.Should().NotBeNull();
            _testOutputHelper.WriteLine(mathTaskToJson);

            var tagToJson = JsonSerializer.Serialize(tag, _serializerOptions);
            tagToJson.Should().NotBeNull();
            _testOutputHelper.WriteLine(tagToJson);
        }
    }
}
