using Microsoft.AspNetCore.Authorization;
using SWBackend.Enum;

namespace SWBackend.Attributes;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
