using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Data;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepository : EFCoreRepository<MathTask, Guid>
{
    public MathTaskRepository(ApplicationDbContext context, MathTaskRepositoryConfigureOptions options) : base(context, options) { }
    //public override Task<MathTask?> Get(Guid id) => GetFirstOrDefaultAsync<MathTask>(
    //    predicate: new MathTaskByIdSpecification(id).ToExpression(),
    //    include: x => x.Include(_ => _.Tags),
    //    disableTracking: true);
    public override Task<MathTask?> Get(Guid id) => AlterGetFirstOrDefaultAsync<MathTask>(
        predicate: new MathTaskByIdSpecification(id).ToExpression(),
        include: new MathTaskIncludeTagsSpecification().ToExpression()
        );
}