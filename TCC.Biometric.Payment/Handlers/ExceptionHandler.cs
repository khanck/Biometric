using Microsoft.AspNetCore.Diagnostics;
using Orchestrator.Core.Exceptions;
using Orchestrator.Core.Interfaces;

namespace TCC.DigitalID.Services.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        private ILogger Logger { get; }

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            Logger = logger;
        }

        public IErrorResponse Handle(Exception exception)
        {
            LogError(exception);

            if (exception is ExternalApiException externalApiException && externalApiException.Response is not null)
            {
                return externalApiException.Response;
            }

            return new ErrorResponse(
                GetResponseCode(exception),
                GetMessage(exception),
                GetStatus(exception),
                GetErrors(exception),
                GetValidationErrors(exception),
                GetAdditionalData(exception)
            );
        }

        private string? GetAdditionalData(Exception exception)
        {
            return exception switch
            {
                AppException appException => appException.AdditionalData,
                _ => null
            };
        }

        private void LogError(Exception exception)
        {
            switch (exception)
            {
                case Exception ex when ex is not AppException:
                    Logger.LogError(ex, "Received unhandled exception");
                    break;

                case ExternalApiValidationException externalApiValidationException:
                    Logger.LogError(
                        "{ExternalServiceName} validation failed with response code {ResponseCode}.\r\nErrors = {@Errors}\r\nValidation Errors = {@ValidationErrors}",
                        externalApiValidationException.ExternalServiceName,
                        externalApiValidationException.ResponseCode,
                        externalApiValidationException.Errors,
                        externalApiValidationException.ValidationErrors
                    );
                    break;

                case ExternalApiException externalApiException:
                    Logger.LogError(
                        "{ExternalServiceName} errored with response code {ResponseCode}.\r\nErrors = {@Errors}",
                        externalApiException.ExternalServiceName,
                        externalApiException.ResponseCode,
                        externalApiException.Errors
                    );
                    break;

                case ValidationException validationException:
                    Logger.LogError(
                        "{Message}.\r\nErrors = {@Errors}\r\nValidation Errors = {@ValidationErrors}",
                        validationException.Message,
                        validationException.Errors,
                        validationException.ValidationErrors
                    );
                    break;

                case AppException appException:
                    Logger.LogError(
                        "{Message}.\r\nErrors = {@Errors}",
                        appException.Message,
                        appException.Errors
                    );
                    break;
            }

            if (exception.InnerException is AppException)
            {
                LogError(exception.InnerException);
            }
        }

        private string GetMessage(Exception exception)
        {
            return exception switch
            {
                AppException appException => appException.Message,
                _ => "Server error"
            };
        }

        private string GetResponseCode(Exception exception)
        {
            return exception switch
            {
                AppException externalApiException => externalApiException.AppResponseCode,
                _ => "500"
            };
        }

        private int GetStatus(Exception exception)
        {
            return exception switch
            {
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                ExternalApiException externalApiException => externalApiException.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private IReadOnlyCollection<string> GetErrors(Exception exception)
        {
            return exception switch
            {
                AppException appException => appException.Errors,
                _ => new List<string>()
            };
        }

        private IReadOnlyDictionary<string, string[]> GetValidationErrors(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => validationException.ValidationErrors,
                MalformedRequestException malformedRequestException => malformedRequestException.ValidationErrors,
                ExternalApiValidationException externalApiValidationException => externalApiValidationException.ValidationErrors,
                _ => new Dictionary<string, string[]>()
            };
        }
    }
