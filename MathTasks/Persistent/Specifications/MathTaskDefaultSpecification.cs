using LinqSpecs;
using MathTasks.Models;
using System;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

public class MathTaskDefaultSpecification : Specification<MathTask>
{
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => true;
}
