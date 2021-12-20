using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.Infrastructure.Helpers;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public TagService(IMediator mediatr, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) =>
            (_mediatr, _httpContextAccessor, _context) = (mediatr, httpContextAccessor, context);

        public async Task<IEnumerable<TagCloudViewModel>> GetCloudAsync()
        {
            var viewModels = await _mediatr.Send(new GetTagCloudViewModelQuery(), _httpContextAccessor!.HttpContext!.RequestAborted);
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

        public async Task UpdateTagsInDatabaseAsync(IEnumerable<string> tagNamesFromModel,
                                                    MathTask mathTask,
                                                    CancellationToken cancellationToken)
        {
            ThrowIfParametersAreNotValid(tagNamesFromModel, mathTask);
            MutateParameters(mathTask);
            var (toCreate, toDelete) = FactHelper.FindItemsToCreateAndToDelete(GetTagNamesLinkedWithSpecificMathTask(mathTask.Id), tagNamesFromModel);
            foreach (var tagName in toDelete)
            {
                var tag = await GetTagAsync(tagName);
                if (tag is null)
                {
                    continue;
                }
                mathTask.Tags.Remove(tag);
                if (!IsToDeleteFromTable(tagName))
                {
                    continue;
                }
                _context!.Tags!.Remove(tag);
            }
            foreach (var tagName in toCreate)
            {
                var tag = await GetTagAsync(tagName);
                if (tag is null)
                {
                    tag = new Tag() { Name = tagName.ToLower() };
                    await _context!.Tags!.AddAsync(tag);
                }
                mathTask.Tags.Add(tag);
            }
        }

        private bool IsToDeleteFromTable(string tagName) => CountTagsInDatabase(tagName) == 1;

        private int CountTagsInDatabase(string tagName) =>
            _context!.MathTasks!
                .Where(m => m.Tags.Select(t => t.Name).Contains(tagName))
                .Count();


        private Task<Tag?> GetTagAsync(string tagName) => 
            _context!.Tags!
                .FirstOrDefaultAsync(predicate: t => t.Name.ToLower() == tagName);

        private void ThrowIfParametersAreNotValid(IEnumerable<string> tagNamesFromModel,
                                                  MathTask mathTask)
        {
            if (tagNamesFromModel is null)
            {
                throw new ArgumentNullException(nameof(tagNamesFromModel));
            }
            if (mathTask is null)
            {
                throw new ArgumentException(nameof(mathTask));
            }
        }

        private Func<Tag, Guid, bool> TagIsLinkedToToSpecificMathTask = (tag, mathTaskGuid) =>
            tag!.MathTasks!
                .Select(m => m.Id).Contains(mathTaskGuid);

        private IEnumerable<string> GetTagNamesLinkedWithSpecificMathTask(Guid mathTaskId) =>
            _context!.Tags!
                .Where(t => t!.MathTasks!.Select(m => m.Id).Contains(mathTaskId))
                .Select(t => t.Name.ToLower());

        private void MutateParameters(MathTask mathTask) =>
            mathTask.Tags ??= new Collection<Tag>();

    }
}
