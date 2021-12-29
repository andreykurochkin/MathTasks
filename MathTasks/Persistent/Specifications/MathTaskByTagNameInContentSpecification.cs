using LinqSpecs;
using MathTasks.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

public class MathTaskByTagNameInContentSpecification : Specification<MathTask>
{
    private readonly string _tagName;

    public MathTaskByTagNameInContentSpecification(string tagName) => _tagName = tagName;
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Tags.Select(_ => _.Name).Any(_ => _.Contains(_tagName));
}
