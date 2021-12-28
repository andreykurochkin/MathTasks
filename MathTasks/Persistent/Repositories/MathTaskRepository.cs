using Kurochkin.Persistence.UnitOfWork;
using MathTasks.Data;
using MathTasks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepository : EFCoreRepository<MathTask, Guid>
{
    public MathTaskRepository(ApplicationDbContext context, MathTaskRepositoryConfigureOptions options) : base(context, options) { }
}