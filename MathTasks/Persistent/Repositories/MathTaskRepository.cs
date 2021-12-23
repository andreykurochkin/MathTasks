using Kurochkin.Persistene.UnitOfWork;
using MathTasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.Persistent.Repositories;

public class MathTaskRepository : EFCoreRepository<MathTask, Guid>
{
    public EFCoreRepositoryConfigureOptions<MathTask, Guid> ConfigureOptions { get => Options; }
    public MathTaskRepository(DbContext context) : base(context, options =>
         {
             options.Include = (x) => x.Include(x => x.Tags);
         })
    { }
}
