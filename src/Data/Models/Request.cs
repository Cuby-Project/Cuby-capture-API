using Cuby.Data.dto;

namespace Cuby.Data.Models
{
    /// <summary>
    /// an auto cube solve request
    /// </summary>
    public class Request
    {
        /// <summary>
        /// the id of the request
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Actual state of the request
        /// </summary>
        public RequestSteps State { get; set; }

        /// <summary>
        /// list of the steps already done
        /// </summary>
        public required List<RequestSteps> StepsDone { get; set; }
    }
}
