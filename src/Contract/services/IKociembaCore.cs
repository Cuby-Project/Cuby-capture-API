namespace Contract.services
{
    public interface IKociembaCore
    {
        /// <summary>
        /// Checks if the cube is valid
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns>true if the cube is valid</returns>
        Task<bool> CheckValidity(string cube);

        /// <summary>
        /// Solves the cube
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns>the solve solution</returns>
        Task<string> Solve(string cube);
    }
}
