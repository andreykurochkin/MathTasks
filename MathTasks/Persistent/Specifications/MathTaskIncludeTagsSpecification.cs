using MathTasks.Models;
using System;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

public class MathTaskIncludeTagsSpecification
{
    public Expression<Func<MathTask, object>> ToExpression() => _ => _.Tags;
}
