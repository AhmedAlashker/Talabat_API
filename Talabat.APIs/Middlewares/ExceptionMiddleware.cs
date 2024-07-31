using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{


    // 1. By Conversion
    // 2. Factory Based
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> loggerFactory, IWebHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory;
            _env = env;
        }
        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    try
        //    {

        //        // Take an Action with the Requst

        //        await _next.Invoke(httpContext);  // Go to the Next Middleware

        //        // Take an Action with the Response
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);  // Develeopment
        //                                       // Log Exception in (Database | Files) // Production Env

        //        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        httpContext.Response.ContentType = "application/json";

        //        var response = _env.IsDevelopment() ?
        //            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
        //            : 
        //            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

        //        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        //        var json = JsonSerializer .Serialize(response, options);

        //        await httpContext.Response.WriteAsync(json);

        //    }


        //}

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Take an Action with the Requst

                await _next.Invoke(httpContext);  // Go to the Next Middleware

                // Take an Action with the Response
            }
             catch (Exception ex)
            {
                _logger.LogError(ex.Message);  // Develeopment
                                               // Log Exception in (Database | Files) // Production Env

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(json);

            }
        }
    }
}
