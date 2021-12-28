using AutoMapper;
using MathTasks.Data;
using MathTasks.Infrastructure.Services;
using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;

namespace MathTasks.Controllers.AlterMathTasks.Commands;

public record UpdateMathTaskCommand(MathTaskEditViewModel mathTaskEditViewModel) : IRequest<MathTask>;

public class UpdateMathTaskHandler : IRequestHandler<UpdateMathTaskCommand, MathTask>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITagService _tagService;
    public UpdateMathTaskHandler(ApplicationDbContext context, IMapper mapper, ITagService tagService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
        _tagService = tagService;
    }
    public async Task<MathTask> Handle(UpdateMathTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context!.MathTasks!
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id.Equals(request.mathTaskEditViewModel.Id));
        if (entity is null)
        {
            return null!;
        }
        _mapper.Map(request.mathTaskEditViewModel, entity);
        _context.Update(entity);
        await _tagService.UpdateTagsInDatabaseAsync(request.mathTaskEditViewModel.Tags!, entity, cancellationToken);
        await _context.SaveChangesAsync();
        return entity;
    }
}