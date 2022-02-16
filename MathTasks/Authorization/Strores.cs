using System.Collections.Generic;
using System.Security.Claims;

namespace MathTasks.Authorization;

public static class Stores
{
    public static class ClaimsStore
    {
        public static IEnumerable<Claim> AllClaims()
        {
            yield return new Claim("IsAdmin", "False");
        }
    }
}
