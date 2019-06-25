/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System;

    /// <summary>
    /// Node of network is just another name for Neuron.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node" class. />
        /// </summary>
        /// <param name="pX">Position X.</param>
        /// <param name="pY">Position Y.</param>
        /// <param name="x">Weight X.</param>
        /// <param name="y">Weight Y.</param>
        public Node (int pX, int pY, double x, double y)
        {
            this.PositionX = pX;
            this.PositionY = pY;
            this.WeightX = x;
            this.WeightY = y;
        }

        public int PositionX { get; private set; }

        public int PositionY { get; private set; }

        public double WeightX { get; private set; }

        public double WeightY { get; private set; }

        /// <summary>
        /// Computes Euclid distance from currently
        /// investigated input (black) point
        /// </summary>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
        /// <returns>Euclic distance</returns>
        public double ComputeEuclidDistance (int x, int y)
        {
            return Math.Sqrt (Math.Pow ((x - this.WeightX), 2) + Math.Pow ((y - this.WeightY), 2));
        }

        /// <summary>
        /// Modifies values of current node.
        /// </summary>
        public void ModifyValues (double neighborhood, double gamma, int inputX, int inputY)
        {
            this.WeightX = this.WeightX + neighborhood * gamma * (inputX - this.WeightX);
            this.WeightY = this.WeightY + neighborhood * gamma * (inputY - this.WeightY);
        }
    }
}
