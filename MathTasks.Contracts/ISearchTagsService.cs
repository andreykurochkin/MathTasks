namespace MathTasks.Contracts;

public interface ISearchTagsService
{
    List<string> SearchTags(string term);
}
