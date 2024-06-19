using Contract.services;

namespace Impl
{
    /// <summary>
    /// Core implementation of the Kociemba algorithm
    /// </summary>
    public class KociembaCore : IKociembaCore
    {
        // <inheritdoc />
        public int CheckValidity(string cube)
        {
            return Kociemba.Tools.verify(cube);
        }

        // <inheritdoc />
        public string Solve(string cube)
        {
            throw new NotImplementedException();
        }
    }
}
