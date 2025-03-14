using DotNet8WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNet8WebAPI.Helper
    

    
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User?)context.HttpContext.Items["user"];
            if (user == null) { 
            context.Result=new JsonResult(new {message="Unauauthorized"}) {StatusCode=StatusCodes.Status401Unauthorized};
            }
        }
    }
}
