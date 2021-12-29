using LinqSpecs;
using MathTasks.Models;
using System;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

public class MathTaskBySearchTermInContentSpecification : Specification<MathTask>
{
    private readonly string _searchTerm;

    public MathTaskBySearchTermInContentSpecification(string searchTerm) => _searchTerm = searchTerm;

    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Content.Contains(_searchTerm);
}
