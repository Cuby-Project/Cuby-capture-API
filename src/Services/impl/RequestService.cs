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
            logger.LogInformation("RequestService.InitiateRequest() Initiating request");
            Request request = new Request()
            {
                Id = Guid.NewGuid().ToString(),
                State = RequestSteps.RecievedRequest,
                StepsDone = []
            };

            request.StepsDone.Add(RequestSteps.RecievedRequest);
            await context.Requests.AddAsync(request);
            await context.SaveChangesAsync();

            logger.LogInformation("RequestService.InitiateRequest() Request added with ID: {RequestId}", request.Id);
            return request.Id;
        }

        /// <inheritdoc/>
        public async Task AddStepDone(string requestId, RequestSteps step)
        {
            logger.LogInformation("RequestService.AddStepDone() Adding step {Step} to request {RequestId}", step, requestId);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(requestId);

            Request request = await context.Requests.FindAsync(requestId) ?? throw new ArgumentException("Request not found");
            if (request.StepsDone.Contains(step))
            {
                logger.LogError("RequestService.AddStepDone() Step {Step} already done in request {RequestId}", step, requestId);
                throw new ArgumentException("Step already done");
            }

            request.StepsDone.Add(step);
            await context.SaveChangesAsync();
            logger.LogInformation("RequestService.AddStepDone() Step {Step} added to request {RequestId}", step, requestId);
        }

    }
}
