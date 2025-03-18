using Cuby.Data.dto;

namespace Cuby.Services.interfaces
{
    /// <summary>
    /// Service to handle solves
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Initiate a solve request
        /// </summary>
        /// <returns>the request ID</returns>
        Task<string> InitiateRequest();

        /// <summary>
        /// Add a step done to a request
        /// </summary>
        /// <param name="requestId">the request Id</param>
        /// <param name="step">the step to add</param>
        /// <exception cref="ArgumentException">if the request does not exist</exception>
        /// <exception cref="ArgumentException">if the step is already done</exception>
        Task AddStepDone(string requestId, RequestSteps step);
    }
}
