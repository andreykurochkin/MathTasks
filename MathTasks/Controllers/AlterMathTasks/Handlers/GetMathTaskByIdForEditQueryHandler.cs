using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MathTasks.Controllers.AlterMathTasks.Handlers
{
    public class GetMathTaskByIdForEditQueryHandler : IRequestHandler<GetMathTaskByIdForEditQuery, MathTaskEditViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMathTaskByIdForEditQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MathTaskEditViewModel> Handle(GetMathTaskByIdForEditQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context!.MathTasks!.Include(_ => _.Tags).FirstOrDefaultAsync(cancellationToken);
            var result = (entity is null)
                ? default(MathTaskEditViewModel)
                : _mapper.Map<MathTaskEditViewModel>(entity);
            return result;
        }
    }
}
