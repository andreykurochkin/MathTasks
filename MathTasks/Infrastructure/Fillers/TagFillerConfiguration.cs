using MathTasks.Models;
using Tynamix.ObjectFiller;

namespace MathTasks.Infrastructure.Fillers
{
    public static class TagFillerConfiguration
    {
        public static FillerSetup Setup() => new Filler<Tag>().Setup()
            .OnProperty(x => x.MathTasks).IgnoreIt()
            .OnProperty(x => x.Name).Use(new MnemonicString(1))
            .Result;
    }
}
