using MathTasks.Persistent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Kurochkin.Persistene.UnitOfWork;
using MathTasks.Models;

namespace MathTasks.Tests;

public class MathTaskRepositoryTests
{
    private readonly MathTaskRepository _sut;
    private const string dataBaseName = "testDataBase";
    
    public MathTaskRepositoryTests()
    {

        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseInMemoryDatabase(dataBaseName);
        using var _dbContext = new DbContext(optionsBuilder.Options);
        _sut = new MathTaskRepository(_dbContext);
    }

    [Fact]
    public void MembersOfConfigureOptions_ShouldBeInitialized_AfterConstructingInstanceOfRepository()
    {
        var result = _sut.ConfigureOptions.IncludeTags;

        result.Should().NotBeNull();
    }
}

public class EFCoreRepositoryTests
{
    private const string dataBaseName = "testDataBase";
    private readonly EFCoreRepository<MathTask, Guid> _sut;

    public EFCoreRepositoryTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(dataBaseName);
        using DbContext context = new DbContext(optionsBuilder.Options);
        Action<EFCoreRepositoryConfigureOptions<MathTask, Guid>> configureOptions = (_) => _.IncludeTags = (mathTasks) => mathTasks.Include(x => x.Tags);
        _sut = new EFCoreRepository<MathTask, Guid>(context, configureOptions);
    }
    [Fact]
    public void MembersOfConfiguration()
    {
        _sut.Options.IncludeTags.Should().NotBeNull();
    }
}
