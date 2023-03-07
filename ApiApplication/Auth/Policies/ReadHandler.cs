using ApiApplication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ApiApplication.Auth.Policies
{
    public class ReadHandler : AuthorizationHandler<Read>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly PoliciesOptions _policiesOptions;
        public ReadHandler(IHttpContextAccessor contextAccessor, IOptions<PoliciesOptions> policiesOptions)
        {
            _contextAccessor = contextAccessor;
            _policiesOptions = policiesOptions.Value;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Read requirement)
        {
            string apiHeader = _contextAccessor.HttpContext.Request.Headers["ApiKey"];
            if (_policiesOptions.ReadToken.Equals(apiHeader))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
