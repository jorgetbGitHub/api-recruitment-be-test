using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers
{
    public class ShowtimeExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new BadRequestObjectResult(context.Exception.Message);
        }
    }
}
