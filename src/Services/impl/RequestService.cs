using Cuby.Data;
using Cuby.Data.dto;
using Cuby.Data.Models;
using Cuby.Services.interfaces;

namespace Cuby.Services.impl
{
    /// <summary>
    /// Service to handle requests
    /// </summary>
    /// <param name="context"><see cref="RequestDbContext"/> db context</param>
    public class RequestService(RequestDbContext context) : IRequestService
    {
        /// <inheritdoc/>
        public async Task<string> InitiateRequest()
        {
            Request request = new Request()
            {
                Id = Guid.NewGuid().ToString(),
                State = RequestSteps.RecievedRequest,
                StepsDone = []
            };

            request.StepsDone.Add(RequestSteps.RecievedRequest);
            await context.Requests.AddAsync(request);
            await context.SaveChangesAsync();

            return request.Id;
        }
    }
}
