using ApiApplication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ApiApplication.Auth.Policies
{
    public class WriteHandler : AuthorizationHandler<Write>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly PoliciesOptions _policiesOptions;
        public WriteHandler(IHttpContextAccessor contextAccessor, IOptions<PoliciesOptions> policiesOptions)
        {
            _contextAccessor = contextAccessor;
            _policiesOptions = policiesOptions.Value;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Write requirement)
        {
            string apiHeader = _contextAccessor.HttpContext.Request.Headers["ApiKey"];
            if (_policiesOptions.WriteToken.Equals(apiHeader))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
