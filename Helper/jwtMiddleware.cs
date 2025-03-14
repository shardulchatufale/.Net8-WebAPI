using DotNet8WebAPI.Model;
using DotNet8WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DotNet8WebAPI.Helper
{
    public class jwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSetting _appSetting;

        public jwtMiddleware(RequestDelegate next, IOptions<AppSetting> appsetting)
        {
            _next = next;
            _appSetting = appsetting.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Split(" ").Last() : null;

            if (token != null)
            {
                await attachUserToContext(context, userService, token);
            }

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "sub").Value);
                context.Items["User"] = await userService.GetByID(UserId);
            }
            catch
            {
                // Log error if needed
            }
        }
    }
}