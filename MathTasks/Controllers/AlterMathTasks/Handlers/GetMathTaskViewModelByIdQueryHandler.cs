using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MathTasks.Controllers.AlterMathTasks.Handlers
{
    public class GetMathTaskViewModelByIdQueryHandler : IRequestHandler<GetMathTaskViewModelByIdQuery, MathTaskViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMathTaskViewModelByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MathTaskViewModel> Handle(GetMathTaskViewModelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context!.MathTasks!.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                return default;
            }
            var mappedItem = _mapper.Map<MathTaskViewModel>(entity);
            return mappedItem;
        }
    }
}
