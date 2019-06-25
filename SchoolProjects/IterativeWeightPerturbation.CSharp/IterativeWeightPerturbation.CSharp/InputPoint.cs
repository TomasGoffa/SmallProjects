/// <summary>
/// This file is part of application which implements
/// Iterative Weight Perturbation algorithm.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace IterativeWeightPerturbation.CSharp
{
    /// <summary>
    /// Class which represents points on image.
    /// </summary>
    public class InputPoint
    {
        /// <summary>
        /// "X" coordinate of point.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// "Y" coordinate of point.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// True if point is positive.
        /// False, otherwise.
        /// </summary>
        public bool IsPositive { get; }

        /// <summary>
        /// Creates new instance of InputPoint class.
        /// </summary>
        /// <param name="x">The "x" coordinate.</param>
        /// <param name="y">The "y" coordinate.</param>
        /// <param name="positive">
        /// Boolean which represents
        /// if point is positive or negative.
        /// </param>
        public InputPoint (int x, int y, bool positive)
        {
            this.X = x;
            this.Y = y;
            this.IsPositive = positive;
        }
    }
}
