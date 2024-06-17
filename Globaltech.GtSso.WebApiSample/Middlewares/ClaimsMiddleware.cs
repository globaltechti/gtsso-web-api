using System.Security.Claims;
using Newtonsoft.Json;

namespace Globaltech.GtSso.WebApiSample.Middlewares
{
    public class ClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserInfo _userInfo;

        public ClaimsMiddleware(RequestDelegate next, UserInfo userInfo)
        {
            _next = next;
            _userInfo = userInfo;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // Extract all claims
                var claims = context.User.Claims ?? Array.Empty<Claim>();
                foreach (var claim in claims )
                {
                    _userInfo.Claims.Add(claim.Value);
                }

                // Extract all scopes
                var scopeClaim = context?.User.Claims.FirstOrDefault(c => c.Type == "scope");
                var scopes = scopeClaim?.Value.Split(' ') ?? Array.Empty<string>();
                foreach (var scope in scopes)
                {
                    _userInfo.Scopes.Add(scope);
                }
            }

            await _next(context);
        }
    }
}
