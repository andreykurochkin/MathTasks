using AutoMapper;
using Calabonga.PredicatesBuilder;
using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Handlers
{
    public class GetMathTaskViewModelsQueryHandler : IRequestHandler<GetMathTaskViewModelsQuery, IEnumerable<MathTaskViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetMathTaskViewModelsQueryHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<MathTaskViewModel>> Handle(GetMathTaskViewModelsQuery request, CancellationToken cancellationToken)
        {
            var predicate = CreatePredicate(request);

            var dbItems = await _context.MathTasks.Include(mathTask => mathTask.Tags).Where(predicate).ToListAsync();
            var mappedItems = _mapper.Map<IEnumerable<MathTaskViewModel>>(dbItems);
            return mappedItems;
        }

        private Expression<Func<MathTask, bool>> CreatePredicate(GetMathTaskViewModelsQuery request)
        {
            var predicate = PredicateBuilder.True<MathTask>();
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                predicate = predicate.And(x => x.Content.Contains(request.Search));
            }
            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                predicate = predicate.And(x => x.Tags.Select(tag => tag.Name).Contains(request.Tag));
            }
            return predicate;
        }
    }
}
