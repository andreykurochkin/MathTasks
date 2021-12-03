using MathTasks.Data;
using MathTasks.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TagService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<TagCloudViewModel>> GetCloudAsync()
        {
            var viewModels = (await _applicationDbContext.Tags
                .Include(t => t.MathTasks)
                .ToListAsync())
                .Select(entity => new TagCloudViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CssClass = string.Empty,
                    Total = entity.MathTasks == null ? 0 : entity.MathTasks.Count
                });

            var options = new ClusterOptions<TagCloudViewModel>((t) => t.Total);
            var cluster = new Cluster<TagCloudViewModel>(options, viewModels).ToList();

            cluster.ForEach(tuple => tuple.Item2.CssClass = $"tag{tuple.Item1}");
            
            return cluster.Select(tuple => tuple.Item2);
        }
    }

    public class ClusterOptions<T>
    {
        /// <summary>
        /// specifies property of T class which value is gonna be taken in order to build cluster
        /// </summary>
        public Func<T, int> OnMember { get; }

        /// <summary>
        /// specifies number of clusters gonna be processed
        /// </summary>
        public int UpperBoundOfClusters { get; }

        public ClusterOptions(Func<T, int> onMember, int upperBoundOfClusters = 10) => (OnMember, UpperBoundOfClusters) = (onMember, upperBoundOfClusters);
    }

    /// <summary>
    /// Generates list of tuples, where tuple.Item1 is number of cluster, tuple.Item2 is instance of T
    /// </summary>
    public class Cluster<T>
    {
        private readonly ClusterOptions<T> _options;
        private readonly IEnumerable<T> _items;

        public Cluster(ClusterOptions<T> options, IEnumerable<T> items)
        {
            _options = options;
            _items = items;
        }

        public List<Tuple<int, T>> ToList()
        {
            var result = new List<Tuple<int, T>>();
            CreateClusters().Select((ListOfT, @Index) => new { @Index, ListOfT })
                .ToList()
                .ForEach(anonym => result.AddRange(anonym.ListOfT.Select(instanceOfT => new Tuple<int, T>(anonym.Index, instanceOfT))));
            return result;
        }

        private List<List<T>> CreateClusters()
        {
            var clusters = new List<List<T>>();
            var orderedItems = _items.OrderBy(_options.OnMember).ToList();
            if (orderedItems.Any())
            {
                var min = orderedItems.Min(_options.OnMember);
                var max = orderedItems.Max(_options.OnMember) + min;
                var completeRange = max - min;
                var groupRange = completeRange / (double)_options.UpperBoundOfClusters;
                var cluster = new List<T>();
                var currentRange = min + groupRange;
                for (int i = 0; i < orderedItems.Count; i++)
                {
                    while (_options.OnMember(orderedItems.ToArray()[i]) > currentRange)
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
}
