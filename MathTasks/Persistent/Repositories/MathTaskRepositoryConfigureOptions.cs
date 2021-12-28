using Kurochkin.Persistence.UnitOfWork;
using LinqSpecs;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepositoryConfigureOptions : EFCoreRepositoryConfigureOptions<MathTask, Guid>
{
    public MathTaskRepositoryConfigureOptions()
    {
        Include = (x) => x.Include(_ => _.Tags);
        DisableTracking = true;
        //Predicate = (x) => x.Id.Equals(SearchId);
        Predicate = new MathTaskByIdSpecification(SearchId).ToExpression();
    }
    
    // 1. specify member of this
    // 2. specify new value for this
    // 3. return this
    public Func<IQueryable<MathTask>, IOrderedQueryable<MathTask>>? OrderyBy { get; set; }
    public Func<IQueryable<MathTask>, IIncludableQueryable<MathTask, object>>? Include { get; set; }
    public bool DisableTracking { get; set; }
    public Expression<Func<MathTask, bool>>? Predicate { get; set; }
    public Guid SearchId { get; set; }
}

public class MathTaskByIdSpecification : Specification<MathTask>
{
    private readonly Guid _id;
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Id.Equals(_id);
    public MathTaskByIdSpecification(Guid id) => _id = id;

}

public class MathTaskIncludeTagsSpecification
{
    public Expression<Func<MathTask, object>> ToExpression() => _ => _.Tags;
}
