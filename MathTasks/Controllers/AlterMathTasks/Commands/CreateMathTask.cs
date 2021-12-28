using AutoMapper;
using MathTasks.Data;
using MathTasks.Infrastructure.Services;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Commands
{
    public record CreateMathTaskCommand(MathTaskCreateViewModel viewModel) : IRequest<MathTask>;

    public class CreateMathTaskCommandHandler : IRequestHandler<CreateMathTaskCommand, MathTask>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ITagService _tagService;

        public CreateMathTaskCommandHandler(IMapper mapper, ApplicationDbContext context, ITagService tagService)
        {
            _mapper = mapper;
            _context = context;
            _tagService = tagService;
        }
        public async Task<MathTask> Handle(CreateMathTaskCommand request, CancellationToken cancellationToken)
        {
            var mathTask = _mapper.Map<MathTask>(request.viewModel);
            await _context!.MathTasks!.AddAsync(mathTask, cancellationToken);
            await _tagService.UpdateTagsInDatabaseAsync(request.viewModel.Tags, mathTask, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return mathTask;
        }
    }
}

