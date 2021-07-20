using IranJob.Services.Exeptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IranJob.Services.Api;

namespace IranJob.Services.Middlewares
{
    public class CustomExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        public CustomExeptionHandlerMiddleware(RequestDelegate next , IHostingEnvironment env)
        {
            _env = env;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            List<string> messages = new List<string>();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiResultStatus = ApiResultStatusCode.ServerError;
            try
            {
                await _next(context);
            }
            catch (ApiUnAutorizationExcpetion ex)
            {
                if (_env.IsDevelopment())
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
                if (_env.IsDevelopment())
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
