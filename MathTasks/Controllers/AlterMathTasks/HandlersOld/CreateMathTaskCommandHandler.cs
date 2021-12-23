using AutoMapper;
using MathTasks.Controllers.AlterMathTasks.Commands;
using MathTasks.Data;
using MathTasks.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MathTasks.Infrastructure.Services;

namespace MathTasks.Controllers.AlterMathTasks.Handlers;

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