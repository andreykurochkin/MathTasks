using MathTasks.Controllers.AlterMathTasks.Commands;
using MathTasks.Data;
using MathTasks.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using MathTasks.Infrastructure.Services;

namespace MathTasks.Controllers.AlterMathTasks.Handlers;

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
            return null;
        }
        _mapper.Map(request.mathTaskEditViewModel, entity);
        _context.Update(entity);
        await _tagService.UpdateTagsInDatabaseAsync(request.mathTaskEditViewModel.Tags!, entity, cancellationToken);
        await _context.SaveChangesAsync();
        return entity;
    }
}
