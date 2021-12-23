using MathTasks.Data;
using MathTasks.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;

namespace MathTasks.Controllers.AlterMathTasks.Queries
{
    public record GetMathTaskByIdForEditQuery(Guid Id, string ReturnUrl = null!) : IRequest<MathTaskEditViewModel>;

    public class MathTaskGetByIdForEditHandler : IRequestHandler<GetMathTaskByIdForEditQuery, MathTaskEditViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MathTaskGetByIdForEditHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MathTaskEditViewModel> Handle(GetMathTaskByIdForEditQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context!.MathTasks!.Include(_ => _.Tags).FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var mappedItem = _mapper.Map<MathTaskEditViewModel>(entity);
            return mappedItem;
        }
    }
}
