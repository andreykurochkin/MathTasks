using AutoMapper;
using Calabonga.PredicatesBuilder;
using Kurochkin.Persistence.UnitOfWork;
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

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record GetMathTaskViewModelsQuery(string Tag, string Search) : IRequest<IEnumerable<MathTaskViewModel>>;

    public class GetMathTaskViewModelsQueryHandler : IRequestHandler<GetMathTaskViewModelsQuery, IEnumerable<MathTaskViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<MathTask, Guid> _repository;

        public GetMathTaskViewModelsQueryHandler(IMapper mapper, IRepository<MathTask, Guid> repository) => (_mapper, _repository) = (mapper, repository);

        public async Task<IEnumerable<MathTaskViewModel>> Handle(GetMathTaskViewModelsQuery request, CancellationToken cancellationToken)
        {
            var dbItems = await _repository.GetAll(request.Tag, request.Search);
            var mappedItems = _mapper.Map<IEnumerable<MathTaskViewModel>>(dbItems);
            return mappedItems;
        }
    }
}
