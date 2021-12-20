using System;

namespace MathTasks.Infrastructure.Helpers;

public class ClusterOptions<T>
{
    /// <summary>
    /// specifies property of T class which value is gonna be taken in order to build cluster
    /// </summary>
    public Func<T, int> OnMember { get; set; }

    /// <summary>
    /// specifies number of clusters gonna be processed
    /// </summary>
    public int UpperBoundOfClusters { get; set; }

    public ClusterOptions(int upperBoundOfClusters = 10) { }
}
