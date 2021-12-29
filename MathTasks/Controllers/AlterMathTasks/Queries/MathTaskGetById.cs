using AutoMapper;
using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Models;
using MathTasks.Persistent.Repositories;
using MathTasks.ViewModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Queries;

public record MathTaskGetByIdQuery(Guid Id) : IRequest<MathTaskViewModel>;

public class MathTaskGetByIdHandler : IRequestHandler<MathTaskGetByIdQuery, MathTaskViewModel>
{
    private readonly IMapper _mapper;
    private readonly IRepository<MathTask, Guid> _repository;

    public MathTaskGetByIdHandler(IMapper mapper, IRepository<MathTask, Guid> repository)
        => (_mapper, _repository) = (mapper, repository);

    public async Task<MathTaskViewModel> Handle(MathTaskGetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.Get(request.Id);
        var viewModel = _mapper.Map<MathTaskViewModel>(entity);
        return viewModel;
    }
}