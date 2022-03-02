using System.Security.Claims;

namespace MathTasks.Infrastructure.Providers.Base;

public interface IClaimProvider
{
    Claim? Create(string type, string value);
}
