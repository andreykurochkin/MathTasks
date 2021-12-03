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
                    Weight = entity.MathTasks == null ? 0 : entity.MathTasks.Count
                });
            return viewModels;
        }
    }

    public class ClusterOptions<T>
    {
        //private readonly int _startIndex = 0;
        //private readonly int _endIndex = 0;

        public Func<T, int> OnMember { get; }
        public int UpperBoundOfClusters { get; }

        public ClusterOptions(Func<T, int> onMember, int upperBoundOfClusters)
        {
            //_endIndex = upperBoundOfClusters - 1;
            OnMember = onMember;
            UpperBoundOfClusters = upperBoundOfClusters;
        }
    }

    /// <summary>
    /// Generates cluster
    /// for each instance of TagCloudViewModel creates a number
    /// numbers belong to the range from 0 to count - 1
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
            var orderedItems = _items.OrderBy(_options.OnMember).ToList();

            var clusters = new List<List<T>>();
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

            var result = new List<Tuple<int, T>>();

            clusters.Select((ListOfT, @Index) => new { @Index, ListOfT })
                .ToList()
                .ForEach(anonym => result.AddRange(anonym.ListOfT.Select(instanceOfT => new Tuple<int, T>(anonym.Index, instanceOfT))));

            return result;
        }
    }
}
