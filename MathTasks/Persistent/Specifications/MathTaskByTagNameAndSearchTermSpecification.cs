using Kurochkin.Persistence.UnitOfWork;
using LinqSpecs;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq.Expressions;

namespace MathTasks.Persistent.Specifications;

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