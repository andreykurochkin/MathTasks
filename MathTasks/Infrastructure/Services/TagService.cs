using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private readonly ApplicationDbContext _applicationDbContext;

        //public TagService(ApplicationDbContext applicationDbContext)
        //{
        //    _applicationDbContext = applicationDbContext;
        //}
        public TagService(IMediator mediatr, IHttpContextAccessor httpContextAccessor) => (_mediatr, _httpContextAccessor) = (mediatr, httpContextAccessor);
        public async Task<IEnumerable<TagCloudViewModel>> GetCloudAsync()
        {
            var viewModels = await _mediatr.Send(new GetTagCloudViewModelQuery(), _httpContextAccessor.HttpContext.RequestAborted);
            var cluster = new Cluster<TagCloudViewModel>(options =>
                {
                    options.OnMember = (t) => t.Total;
                    options.UpperBoundOfClusters = 10;
                }
            );

            var clusterResult = cluster.ToList(viewModels);
            clusterResult.ForEach(tuple => tuple.Item2.CssClass = $"tag{tuple.Item1}");

            return clusterResult.Select(tuple => tuple.Item2);
        }
        //public async Task<IEnumerable<TagCloudViewModel>> GetCloudAsync()
        //{
        //    var viewModels = (await _applicationDbContext.Tags
        //        .Include(t => t.MathTasks)
        //        .ToListAsync())
        //        .Select(entity => new TagCloudViewModel
        //        {
        //            Id = entity.Id,
        //            Name = entity.Name,
        //            CssClass = string.Empty,
        //            Total = entity.MathTasks == null ? 0 : entity.MathTasks.Count
        //        });

        //    var cluster = new Cluster<TagCloudViewModel>(options => 
        //        {
        //            options.OnMember = (t) => t.Total;
        //            options.UpperBoundOfClusters = 10;
        //        }
        //    );

        //    var clusterResult = cluster.ToList(viewModels);
        //    clusterResult.ForEach(tuple => tuple.Item2.CssClass = $"tag{tuple.Item1}");

        //    return clusterResult.Select(tuple => tuple.Item2);
        //}
    }

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

        //public ClusterOptions(Func<T, int> onMember, int upperBoundOfClusters = 10) => (OnMember, UpperBoundOfClusters) = (onMember, upperBoundOfClusters);

        public ClusterOptions(int upperBoundOfClusters = 10) { }
    }

    /// <summary>
    /// Generates list of tuples, where tuple.Item1 is number of cluster, tuple.Item2 is instance of T
    /// </summary>
    public class Cluster<T>
    {
        private readonly ClusterOptions<T> _options = new();

        public Cluster(Action<ClusterOptions<T>> configureOptions) { 
            configureOptions(_options); 
        }

        //public Cluster(ClusterOptions<T> options) => _options = options;

        public List<Tuple<int, T>> ToList(IEnumerable<T> items)
        {
            var result = new List<Tuple<int, T>>();
            CreateClusters(items).Select((ListOfT, @Index) => new { @Index, ListOfT })
                .ToList()
                .ForEach(anonym => result.AddRange(anonym.ListOfT.Select(instanceOfT => new Tuple<int, T>(anonym.Index, instanceOfT))));
            return result;
        }

        private List<List<T>> CreateClusters(IEnumerable<T> items)
        {
            var clusters = new List<List<T>>();
            var orderedItems = items.OrderBy(_options.OnMember).ToList();
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
