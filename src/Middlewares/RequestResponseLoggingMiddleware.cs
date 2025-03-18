using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cuby.Middlewares
{
    /// <summary>
    /// A custom middleware for logging HTTP requests and responses.
    /// </summary>
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        /// <summary>
        /// Invoke the middleware to log request and response details.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request
            await LogRequestAsync(context);

            // Replace the response stream with a memory stream to capture the response for logging
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                // Invoke the next middleware in the pipeline
                await _next(context);

                // Log the outgoing response
                await LogResponseAsync(context);

                // Copy the response back to the original stream
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            var request = context.Request;
            // Enable buffering to allow the request body to be read multiple times
            request.EnableBuffering();

            var builder = new StringBuilder();
            builder.AppendLine("----- Incoming Request -----");
            builder.AppendLine($"Timestamp   : {DateTime.UtcNow:O}");
            builder.AppendLine($"Method      : {request.Method}");
            builder.AppendLine($"Path        : {request.Path}{request.QueryString}");

            builder.AppendLine("Headers     :");
            foreach (var header in request.Headers)
            {
                builder.AppendLine($"    {header.Key}: {header.Value}");
            }

            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                // Read and log the request body content
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    string bodyContent = await reader.ReadToEndAsync();
                    builder.AppendLine("Body        :");
                    builder.AppendLine(bodyContent);
                    request.Body.Position = 0;
                }
            }
            builder.AppendLine("-------------------------------");

            _logger.LogInformation(builder.ToString());
        }

        private async Task LogResponseAsync(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var builder = new StringBuilder();
            builder.AppendLine("----- Outgoing Response -----");
            builder.AppendLine($"Timestamp   : {DateTime.UtcNow:O}");
            builder.AppendLine($"Status Code : {context.Response.StatusCode}");

            using (var reader = new StreamReader(context.Response.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                string responseBody = await reader.ReadToEndAsync();
                builder.AppendLine("Body        :");
                builder.AppendLine(responseBody);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
            }
            builder.AppendLine("-------------------------------");

            _logger.LogInformation(builder.ToString());
        }
    }
}
