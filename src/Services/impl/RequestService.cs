using Cuby.Data;
using Cuby.Data.dto;
using Cuby.Data.Models;
using Cuby.Services.interfaces;
using Microsoft.Extensions.Logging;

namespace Cuby.Services.impl
{
    /// <summary>
    /// Service to handle requests
    /// </summary>
    /// <param name="context"><see cref="RequestDbContext"/> db context</param>
    /// <param name="logger"><see cref="ILogger"/> logger</param>
    public class RequestService(RequestDbContext context, ILogger<RequestService> logger) : IRequestService
    {
        /// <inheritdoc/>
        public async Task<string> InitiateRequest()
        {
            logger.LogTrace("Initiating request");
            Request request = new Request()
            {
                Id = Guid.NewGuid().ToString(),
                State = RequestSteps.RecievedRequest,
                StepsDone = []
            };

            request.StepsDone.Add(RequestSteps.RecievedRequest);
            await context.Requests.AddAsync(request);
            await context.SaveChangesAsync();

            logger.LogTrace("Request added with ID: {RequestId}", request.Id);
            return request.Id;
        }

        /// <inheritdoc/>
        public async Task AddStepDone(string requestId, RequestSteps step)
        {
            logger.LogTrace("Adding step {Step} to request {RequestId}", step, requestId);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(requestId);

            Request request = await context.Requests.FindAsync(requestId) ?? throw new ArgumentException("Request not found");
            try
            {
                request.StepsDone.Add(step);
                await context.SaveChangesAsync();
                logger.LogTrace("Step {Step} added to request {RequestId}", step, requestId);
            } catch (Exception)
            {
                logger.LogError("Error adding step to request");
                throw new ArgumentException("Error adding step to request");
            }
        }
    }
}
