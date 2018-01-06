using Microsoft.AspNetCore.Http;

namespace DatingAPI.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddApplicationErrorHeader(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}