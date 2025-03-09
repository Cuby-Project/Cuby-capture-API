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
    }
}
