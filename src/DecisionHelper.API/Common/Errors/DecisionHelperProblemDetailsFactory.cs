using System.Diagnostics;
using DecisionHelper.API.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace DecisionHelper.API.Common.Errors
{
    /// <summary>
    /// Based on Microsoft's DefaultProblemDeatilsFactory
    /// https://github.com/aspnet/AspNetCore/blob/2e4274cb67c049055e321c18cc9e64562da52dcf/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs
    /// </summary>
    public class DecisionHelperProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;

        public DecisionHelperProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null,
            string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 500;
            var problemDetails = new ProblemDetails()
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };
            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
            ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null,
            string? detail = null, string? instance = null)
        {
            if (modelStateDictionary == null)
                throw new ArgumentNullException(nameof(modelStateDictionary));
            statusCode ??= 400;

            var validationProblemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };
            validationProblemDetails.Title ??= title;
            ApplyProblemDetailsDefaults(httpContext, (ProblemDetails)validationProblemDetails, statusCode.Value);
            return validationProblemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;
            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
            if (errors is not null)
            {
                problemDetails.Extensions.Add("errorCodes", errors.Select(x => x.Code));
            }
        }
    }
}
