using MathTasks.Models;
using Tynamix.ObjectFiller;

namespace MathTasks.Seed
{
    public static class EntityFactory
    {
        public static Tag CreateTag()
        {
            var filler = new Filler<Tag>();
            filler.Setup(TagFillerConfiguration.Setup());
            return filler.Create();
        }

        public static MathTask CreateMathTask()
        {
            var filler = new Filler<MathTask>();
            filler.Setup(MathTaskFillerConfiguration.Setup());
            return filler.Create();
        }
    }
}
