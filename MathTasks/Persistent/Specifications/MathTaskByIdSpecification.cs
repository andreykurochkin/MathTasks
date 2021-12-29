using LinqSpecs;
using MathTasks.Models;
using System;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

public class MathTaskByIdSpecification : Specification<MathTask>
{
    private readonly Guid _id;
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Id.Equals(_id);
    public MathTaskByIdSpecification(Guid id) => _id = id;
}
