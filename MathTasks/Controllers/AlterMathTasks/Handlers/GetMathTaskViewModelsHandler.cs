using AutoMapper;
using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Handlers
{
    public class GetMathTaskViewModelsHandler : IRequestHandler<GetMathTaskViewModelsQuery, IEnumerable<MathTaskViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public GetMathTaskViewModelsHandler(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<MathTaskViewModel>> Handle(GetMathTaskViewModelsQuery request, CancellationToken cancellationToken)
        {
            var dbItems = await _context.MathTasks.ToListAsync();
            var mappedItems = _mapper.Map<IEnumerable<MathTaskViewModel>>(dbItems);
            return mappedItems;
        }
    }
}
