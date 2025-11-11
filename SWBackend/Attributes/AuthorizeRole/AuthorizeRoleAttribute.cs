using Microsoft.AspNetCore.Authorization;

namespace SWBackend.Attributes.AuthorizeRole;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
