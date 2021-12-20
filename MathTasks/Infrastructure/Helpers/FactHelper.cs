using System.Collections.Generic;
using System.Linq;

namespace MathTasks.Infrastructure.Helpers;

public static class FactHelper
{
    public static (IEnumerable<string> toCreate, IEnumerable<string> toDelete) FindItemsToCreateAndToDelete(IEnumerable<string> old, IEnumerable<string> current) =>
        FindItemsToCreateAndToDelete(old.ToArray(), current.ToArray());
    public static (string[] toCreate, string[] toDelete) FindItemsToCreateAndToDelete(string[] old, string[] current)
    {
        var commonItems = current.Intersect(old);
        var toCreateInDatabase = current.Except(commonItems).ToArray();
        var toDeleteFromDatabase = old.Except(commonItems).ToArray();
        return (toCreateInDatabase, toDeleteFromDatabase);
    }
}
