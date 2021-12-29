using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Data;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepository : EFCoreRepository<MathTask, Guid>
{
    public MathTaskRepository(ApplicationDbContext context) : base(context) { }

    public override Task<MathTask?> Get(Guid id) => GetFirstOrDefaultAsync<MathTask>(
        predicate: new MathTaskByIdSpecification(id).ToExpression(),
        include: new MathTaskIncludeTagsSpecification().ToExpression(),
        disableTracking: true
        );

    public override Task<IEnumerable<MathTask>> GetAll(string tagName, string searchTerm) => ToListAsync(
        predicate: new MathTaskByTagNameAndSearchTermSpecification(tagName, searchTerm).ToExpression(),
        include: new MathTaskIncludeTagsSpecification().ToExpression(),
        disableTracking: true
        );
}