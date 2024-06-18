namespace Contract.services
{
    public interface IKociembaCore
    {
        /// <summary>
        /// Checks if the cube is valid
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns></returns>
        bool CheckValidity(string cube);

        /// <summary>
        /// Solves the cube
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns>the solve solution</returns>
        string Solve(string cube);
    }
}
