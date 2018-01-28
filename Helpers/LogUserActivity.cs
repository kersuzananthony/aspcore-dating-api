using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingAPI.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAPI.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            var unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await repo.GetUserAsync(userId);

            user.LastActiveAt = DateTime.Now;

            await unitOfWork.CompleteAsync();
        }
    }
}