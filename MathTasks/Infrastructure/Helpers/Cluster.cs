using System;
using System.Collections.Generic;
using System.Linq;

namespace MathTasks.Infrastructure.Helpers;

/// <summary>
/// Generates list of tuples, where tuple.Item1 is number of cluster, tuple.Item2 is instance of T
/// </summary>
public class Cluster<T>
{
    private readonly ClusterOptions<T> _options = new();

    public Cluster(Action<ClusterOptions<T>> configureOptions) => configureOptions(_options);

    public List<Tuple<int, T>> ToList(IEnumerable<T> items) =>
        CreateClusters(items)
            .SelectMany((ListOfT, @IndexOfCluster) => ListOfT.Select(t => new Tuple<int, T>(IndexOfCluster, t)))
            .ToList();

    private List<List<T>> CreateClusters(IEnumerable<T> items)
    {
        var clusters = new List<List<T>>();
        var orderedItems = items.OrderBy(_options.OnMember!).ToList();
        if (orderedItems.Any())
        {
            var min = orderedItems.Min(_options.OnMember!);
            var max = orderedItems.Max(_options.OnMember!) + min;
            var completeRange = max - min;
            var groupRange = completeRange / (double)_options.UpperBoundOfClusters;
            var cluster = new List<T>();
            var currentRange = min + groupRange;
            for (int i = 0; i < orderedItems.Count; i++)
            {
                while (_options.OnMember!(orderedItems.ToArray()[i]) > currentRange)
                {
                    clusters.Add(cluster);
                    cluster = new List<T>();
                    currentRange += groupRange;
                }
                cluster.Add(orderedItems.ToArray()[i]);
            }
            clusters.Add(cluster);
        }
        return clusters;
    }
}
