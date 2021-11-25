using MathTasks.Models;
using Tynamix.ObjectFiller;

namespace MathTasks.Seed
{
    public static class MathTaskFillerConfiguration
    {
        public static FillerSetup Setup() => new Filler<MathTask>().Setup()
            .OnProperty(x => x.Tags).IgnoreIt()
            .OnProperty(x => x.Theme).Use(new MnemonicString(5))
            .OnProperty(x => x.Content).Use(new Lipsum(LipsumFlavor.LoremIpsum, 10, 20))
            .OnProperty(x => x.CreatedBy).Use(new EmailAddresses(".com"))
            .OnProperty(x => x.UpdatedBy).Use(new EmailAddresses(".com"))
            .Result;
    }
}
