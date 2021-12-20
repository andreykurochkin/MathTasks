using MathTasks.Models;
using MathTasks.ViewModels;
using MediatR;
namespace MathTasks.Controllers.AlterMathTasks.Commands;

public record UpdateMathTaskCommand(MathTaskEditViewModel mathTaskEditViewModel) : IRequest<MathTask>;