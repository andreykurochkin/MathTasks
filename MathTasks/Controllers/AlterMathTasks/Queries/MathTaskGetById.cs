using AutoMapper;
using MathTasks.Data;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record MathTaskGetByIdQuery(Guid Id) : IRequest<MathTaskViewModel>;

    public class MathTaskGetByIdHandler : IRequestHandler<MathTaskGetByIdQuery, MathTaskViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MathTaskGetByIdHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MathTaskViewModel> Handle(MathTaskGetByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context!.MathTasks!.Include(_ => _.Tags).FirstOrDefaultAsync(x => x.Id == request.Id);
            var mappedItem = _mapper.Map<MathTaskViewModel>(entity);
            return mappedItem;
        }
    }
}
