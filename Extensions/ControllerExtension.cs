using System.Security.Claims;
using System.Threading.Tasks;
using DatingAPI.Core;
using DatingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAPI.Extensions
{
    public static class ControllerExtension
    {
        public static async Task<User> GetCurrentUser(this Controller controller)
        {
            var currentUserId = int.Parse(controller.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var datingRepository = controller.HttpContext.RequestServices.GetService<IDatingRepository>();
            
            return await datingRepository.GetUserAsync(currentUserId);
        }

        public static int GetCurrentUserId(this Controller controller)
        {
            return int.Parse(controller.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}