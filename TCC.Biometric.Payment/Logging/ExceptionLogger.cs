
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

using System.Net;
using TCC.Biometric.Payment.DTOs;
using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Logging
{
    public static class ExceptionLogger
    {
        public static void ExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var errorContext = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorContext != null)
                    {
                        var response = new ResultDto<ErrorDto>();
                        response.error = new ErrorDto();
                        response.error.errorCode = context.Response.StatusCode.ToString();
                        response.error.errorMessage = HttpStatusCode.InternalServerError.ToString();
                        response.error.errorDetails = "Internal Server Error";
                        response.success = false;

                        //logger.Error(errorContext.Error, $"Internal Server Error: {errorContext.Endpoint}", errorContext.Error);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }
                });
            });
        }
    }
}
