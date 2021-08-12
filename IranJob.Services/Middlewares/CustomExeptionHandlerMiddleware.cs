using IranJob.Services.Api;
using IranJob.Services.Exeptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace IranJob.Services.Middlewares
{
    public class CustomExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        public CustomExeptionHandlerMiddleware(RequestDelegate next, IHostingEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _next = next;
            _configuration = configuration;
        }
        public async Task Invoke(HttpContext context)
        {
            List<string> messages = new List<string>();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiResultStatus = ApiResultStatusCode.ServerError;
            var isDevlopement = _configuration["ShowExceptions"] == "true";
            try
            {
                await _next(context);
            }
            catch (ApiUnAutorizationExcpetion ex)
            {
                apiResultStatus = ApiResultStatusCode.UnAuthorized;
                if (isDevlopement)
                {
                    var error = new Dictionary<string, string>
                    {
                        ["Exeption"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    messages.Add(JsonConvert.SerializeObject(error));
                }
                else
                {
                    messages.Add("خطایی در احراز هویت پیش امد.");
                }
                await WriteToResponseAsync();
            }
            catch (Exception ex)
            {
                if (isDevlopement)
                {
                    var error = new Dictionary<string, string>
                    {
                        ["Exeption"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    messages.Add(JsonConvert.SerializeObject(error));
                }
                else
                {
                    messages.Add("خطایی سمت سرور رخ داده است.");
                }
                await WriteToResponseAsync();
            }

            async Task WriteToResponseAsync()
            {

                var result = new ApiResult(false, apiResultStatus, messages);
                var jsonResult = JsonConvert.SerializeObject(result);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResult);
            }
        }
    }
    public static class CustomExeptionHandlerMiddlewareExtentions
    {
        public static IApplicationBuilder UseApiExeption(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExeptionHandlerMiddleware>();
        }
    }
}
