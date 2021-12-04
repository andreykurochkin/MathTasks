using AutoMapper;
using MathTasks.Controllers.AlterMathTasks.Queries;
using MathTasks.Data;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.AlterMathTasks.Handlers;
public class GetTagCloudViewModelQueryHandler : IRequestHandler<GetTagCloudViewModelQuery, IEnumerable<TagCloudViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagCloudViewModelQueryHandler(ApplicationDbContext context, IMapper mapper) => (_context, _mapper) = (context, mapper);

    public async Task<IEnumerable<TagCloudViewModel>> Handle(GetTagCloudViewModelQuery request, CancellationToken cancellationToken)
    {
        var dbItems = await _context.Tags.Include(t => t.MathTasks).ToListAsync();
        var mappedItems = _mapper.Map<IEnumerable<TagCloudViewModel>>(dbItems);
        return mappedItems;
        //var viewModels = (await _applicationDbContext.Tags
        //        .Include(t => t.MathTasks)
        //        .ToListAsync())
        //        .Select(entity => new TagCloudViewModel
        //        {
        //            Id = entity.Id,
        //            Name = entity.Name,
        //            CssClass = string.Empty,
        //            Total = entity.MathTasks == null ? 0 : entity.MathTasks.Count
        //        });

        //throw new System.NotImplementedException();
    }
}
