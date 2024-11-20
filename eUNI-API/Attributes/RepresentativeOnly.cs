using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eUNI_API.Attributes;

public class RepresentativeOnly: AuthorizeAttribute, IAuthorizationFilter
{
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            
            var isRepresentativeClaim = user.FindFirst("IsRepresentative")?.Value;

            if (isRepresentativeClaim == null || !bool.TryParse(isRepresentativeClaim, out var isRepresentative) || !isRepresentative)
            {
                context.Result = new ForbidResult();
            }
        }
}