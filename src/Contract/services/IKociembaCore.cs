namespace Contract.services
{
    public interface IKociembaCore
    {
        /// <summary>
        /// Checks if the cube is valid
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns> 0: Cube is solvable<br>
        ///         -1: There is not exactly one facelet of each colour<br>
        ///         -2: Not all 12 edges exist exactly once<br>
        ///         -3: Flip error: One edge has to be flipped<br>
        ///         -4: Not all 8 corners exist exactly once<br>
        ///         -5: Twist error: One corner has to be twisted<br>
        ///         -6: Parity error: Two corners or two edges have to be exchanged </returns>
        bool CheckValidity(string cube);

        /// <summary>
        /// Solves the cube
        /// </summary>
        /// <param name="cube">the cube string representation</param>
        /// <returns>the solve solution</returns>
        string Solve(string cube);
    }
}
