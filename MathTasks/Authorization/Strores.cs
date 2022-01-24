using System.Collections.Generic;
using System.Security.Claims;

namespace MathTasks.Authorization;

public static class Stores
{
    public static class ClaimsStore
    {
        public static class Names
        {
            public const string CreateRole = "Create Role";
        }

        public static readonly Claim CreateRole = new Claim(Names.CreateRole, false.ToString());

        public static IEnumerable<Claim> GetClaims()
        {
            yield return CreateRole;
        }
    }
}
