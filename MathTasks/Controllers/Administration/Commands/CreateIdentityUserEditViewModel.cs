using AutoMapper;
using MathTasks.Authorization;
using MathTasks.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Controllers.Administration.Commands;

public record CreateIdentityUserEditViewModelCommand(IdentityUser IdentityUser) : IRequest<IdentityUserEditViewModel?>;

public class CreateIdentityUserEditViewModelCommandHandler : IRequestHandler<CreateIdentityUserEditViewModelCommand, IdentityUserEditViewModel?>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public CreateIdentityUserEditViewModelCommandHandler(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<IdentityUserEditViewModel?> Handle(CreateIdentityUserEditViewModelCommand request, CancellationToken cancellationдToken)
    {
        if (request.IdentityUser is null)
        {
            return await Task.FromResult<IdentityUserEditViewModel?>(null);
        }
        var source = new Tuple<IdentityUser, IList<Claim>>(request.IdentityUser, await _userManager.GetClaimsAsync(request.IdentityUser));
        var model = _mapper.Map<IdentityUserEditViewModel>(source);
        return model;
    }
}