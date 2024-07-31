using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ASK CLR for Creating Object from "ResponseCacheSerevice" Explicity
            var responseCacheSerevice = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheSerevice>();

            var cahcheKey = GenerateCacheKeyFromResponse(context.HttpContext.Request);

            var response = await responseCacheSerevice.GetCacheResponseAsync(cahcheKey);

            if (!string.IsNullOrEmpty(response)) // Response is Not Cached
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = result;
                return;
            }

            var executedActionContext = await next.Invoke();  // Will Execute The Next Action Filter OR the Action Itself

            if (executedActionContext.Result is ObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await responseCacheSerevice.CacheResponseAsync(cahcheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromResponse(HttpRequest request)
        {
            // {{url}}/api/Products?pageIndex=2&pageSize=3&sort=priceDesc

            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);  // /api/products

            // pageIndex=2
            // pageSize=3
            // sort=name

            foreach (var (key, value) in request.Query.OrderBy(X => X.Key)) 
            {
                keyBuilder.Append($"|{key}--{value}");
                // /api/products|pageIndex-1
                // /api/products|pageIndex-1|pageSize-5
                // /api/products|pageIndex-1|pageSize-5|sort-name

            }

            return keyBuilder.ToString();
        }
    }
}
