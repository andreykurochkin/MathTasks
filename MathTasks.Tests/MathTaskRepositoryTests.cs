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
using MathTasks.Models;
using Kurochkin.Persistence.UnitOfWork;

namespace MathTasks.Tests;

public class MathTaskRepositoryTests
{
    //private readonly MathTaskRepository _sut;
    private const string dataBaseName = "testDataBase";
    
    public MathTaskRepositoryTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseInMemoryDatabase(dataBaseName);
        using var _dbContext = new DbContext(optionsBuilder.Options);
        //_sut = new MathTaskRepository(_dbContext);
    }

    [Fact]
    public void MembersOfConfigureOptions_ShouldBeInitialized_AfterConstructingInstanceOfRepository()
    {
        //var result = _sut.ConfigureOptions.Include;

        //result.Should().NotBeNull();
    }
}