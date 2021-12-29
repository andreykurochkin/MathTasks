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

public class MathTaskBySearchTermInContentSpecification : Specification<MathTask>
{
    private readonly string _searchTerm;

    public MathTaskBySearchTermInContentSpecification(string searchTerm) => _searchTerm = searchTerm;

    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Content.Contains(_searchTerm);
}

public class MathTaskByTagNameInContentSpecification : Specification<MathTask>
{
    private readonly string _tagName;

    public MathTaskByTagNameInContentSpecification(string tagName) => _tagName = tagName;
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => _.Tags.Select(_ => _.Name).Any(_ => _.Contains(_tagName));
}

public class MathTaskDefaultSpecification : Specification<MathTask>
{
    public override Expression<Func<MathTask, bool>> ToExpression() => _ => true;
}

public class MathTaskByTagNameAndSearchTermSpecification : Specification<MathTask>
{
    private readonly string _tagName;
    private readonly string _searchTerm;
    public MathTaskByTagNameAndSearchTermSpecification(string tagName, string searchTerm) => (_tagName, _searchTerm) = (tagName, searchTerm);

    public override Expression<Func<MathTask, bool>> ToExpression() => (string.IsNullOrWhiteSpace(_tagName), string.IsNullOrWhiteSpace(_searchTerm)) switch
    {
        (true, true) => new MathTaskDefaultSpecification(),
        (true, _) => new MathTaskBySearchTermInContentSpecification(_searchTerm).ToExpression(),
        (_, true) => new MathTaskByTagNameInContentSpecification(_tagName).ToExpression(),
        (_, _) => (new MathTaskByTagNameInContentSpecification(_tagName) && new MathTaskBySearchTermInContentSpecification(_searchTerm)).ToExpression()
    };
}