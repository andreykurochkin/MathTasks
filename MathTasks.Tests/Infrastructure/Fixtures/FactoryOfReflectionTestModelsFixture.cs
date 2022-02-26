using MathTasks.Tests.Infrastructure.Helpers;
using MathTasks.Tests.Infrastructure.ViewModels;
using MathTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathTasks.Tests.Infrastructure.Fixtures;

public class FactoryOfReflectionTestModelsFixture
{
    public ReflectionTestsModel CreateModelFirstItemIsNotNull() => new ReflectionTestsModel 
    { 
        FirstItem = UserClaimsHelper.GetOne()
    };

    public ReflectionTestsModel CreateModelAllPropertiesAreNull() => new ReflectionTestsModel { };

    public ReflectionTestsModel CreateModelFirstItemAndSecondItemIsNotNull() => new ReflectionTestsModel
    {
        FirstItem = UserClaimsHelper.GetOne(),
        SecondItem = UserClaimsHelper.GetOne()
    };

    public ReflectionTestsModel CreateModelFirstCollectionAndSecondCollectionAreNotNull() => new ReflectionTestsModel
    {
        FirstCollection = UserClaimsHelper.GetManyWithoutIsAdmin().ToList(),
        SecondCollection = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
    };

    public ReflectionTestsModel CreateModelFirstCollectionIsNullAndSecondCollectionIsNotNull() => new ReflectionTestsModel
    {
        FirstCollection = null,
        SecondCollection = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
    };

    public ReflectionTestsModel CreateModelFirstCollectionIsEmptyListAndSecondCollectionIsNotNull() => new ReflectionTestsModel
    {
        FirstCollection = new List<UserClaim>(),
        SecondCollection = UserClaimsHelper.GetManyWithoutIsAdmin().ToList()
    };
}