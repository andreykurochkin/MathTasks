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
using MathTasks.Extensions;

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

        [Fact]
        public void Randomizer_ShouldReturnRandomSubsetOfItems_WhenDataIsValid()
        {
            //var localTags = Enumerable.Range(0, 10).Select(i=>CreateTag()).ToList();

            //var collectionizer = new Collectionizer<Tag, TagsRandomizer>(new TagsRandomizer { Tags = localTags }, (uint)1, (uint)localTags.Count);

            //_testOutputHelper.WriteLine(string.Join(",", localTags.Select(tag=>tag.Name)));
            //_testOutputHelper.WriteLine(string.Join(",", collectionizer.GetValue().Select(tag => tag.Name).ToList()));


            int elementsCount = 10;
            var list = Enumerable.Range(0, elementsCount).ToList();
            _testOutputHelper.WriteLine(string.Join(",", list.Select(i => i.ToString())));
            _testOutputHelper.WriteLine("\n");
            for (int i = 0; i < elementsCount; i++)
            {
                var result = GetRandomElements<int>(list, Random.Shared.Next(0, elementsCount-1));
                _testOutputHelper.WriteLine(string.Join(",", result.Select(i => i.ToString())));
            }
        }

        public List<T> GetRandomElements<T>(IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        private Tag CreateTag()
        {
            var tagFiller = new Filler<Tag>();
            tagFiller.Setup()
                .OnProperty(x => x.MathTasks).IgnoreIt()
                .OnProperty(x => x.Name).Use(new MnemonicString(1));
            return tagFiller.Create();
        }
    }
}
