using FluentAssertions;
using MathTasks.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MathTasks.Tests
{
    public class IEnumerableExtensionsTests
    {
        private List<int>? source;
        private readonly ITestOutputHelper _testOutputHelper;

        public IEnumerableExtensionsTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ToRandomList_ShouldNotBeNullOrEmpty_WhenDataIsValid()
        {
            source = Enumerable.Range(0, 10).ToList();

            var result = source.ToRandomList();

            result.Should().NotBeNullOrEmpty();
            _testOutputHelper.WriteLine("original items: ");
            _testOutputHelper.WriteLine(string.Join(",", source));
            _testOutputHelper.WriteLine("randomized items: ");
            _testOutputHelper.WriteLine(string.Join(",", result));
        }

        [Fact]
        public void ToRandomList_ShouldThrow_WhenSourceIsNull()
        {
            source = null;
            
            Action act = () => source!.ToRandomList();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ToRandomList_ShouldThrow_WhenSourceIsEmpty()
        {
            var source = Enumerable.Empty<int>();
            
            Action act = () => source.ToRandomList();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ToRandomList_ShouldReturnDifferentSeed_ForTheSameSource()
        {
            source = Enumerable.Range(0, 10).ToList();

            var result1 = source.ToRandomList();
            var result2 = source.ToRandomList();

            result1.Should().NotBeEquivalentTo(result2);
            _testOutputHelper.WriteLine("original items: ");
            _testOutputHelper.WriteLine(string.Join(",", source));
            _testOutputHelper.WriteLine("randomized items 1st: ");
            _testOutputHelper.WriteLine(string.Join(",", result1));
            _testOutputHelper.WriteLine("randomized items 2nd: ");
            _testOutputHelper.WriteLine(string.Join(",", result2));
        }
    }
}
