namespace Cuby.Services.interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// Initiate a solve request
        /// </summary>
        /// <returns>the request Id</returns>
        Task<string> InitiateRequest();
    }
}
