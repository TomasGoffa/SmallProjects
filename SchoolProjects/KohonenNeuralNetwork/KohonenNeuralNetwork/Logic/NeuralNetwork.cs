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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class NeuralNetwork : IDisposable
    {
        public Node[,] kohonenGrid;     // Kohonen Grid
        public double[,] distances;     // Array of calculated distances of nodes from current input point
        private static readonly Pen redPen = new Pen (Color.Red, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Node" class. />
        /// Initialization of Kohonen Grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="firstX"></param>
        /// <param name="firstY"></param>
        public NeuralNetwork (int x, int y, int firstX, int firstY)
        {
            this.kohonenGrid = new Node[x, y];
            this.distances = new double[x, y];

            // Initialization of Kohonen Grid
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    this.kohonenGrid[i, j] = new Node (i, j, firstX, firstY);
                }
            }
        }

        ~NeuralNetwork()
        {
            this.Dispose ();
        }

        /// <summary>
        /// Learning algorithm
        /// </summary>
        /// <param name="inputPoints">List of input points.</param>
        /// <param name="numberOfIterations">Number of iterations.</param>
        /// <param name="gamma">Learning parameter</param>
        /// <param name="radius">Radius of neighborhood</param>
        public void LearnNeuralNetwork (List<Point> inputPoints, int numberOfIterations, double gamma, double radius)
        {
            // Compute lambda - Time constant
            double lambda = numberOfIterations / Math.Log (radius);
            int iteration = 0;

            while (iteration < numberOfIterations)
            {
                iteration = iteration + 1;
                radius = radius * Math.Exp (-iteration / lambda);
                gamma = gamma * Math.Exp (-iteration / lambda);

                for (int i = 0; i < inputPoints.Count; i++)
                {
                    int inputPointX = inputPoints.ElementAt (i).X;
                    int inputPointY = inputPoints.ElementAt (i).Y;

                    // Calculation of distances
                    for (int j = 0; j < this.distances.GetLength (0); j++)
                    {
                        for (int k = 0; k < this.distances.GetLength (1); k++)
                        {
                            this.distances[j, k] = this.kohonenGrid[j, k].ComputeEuclidDistance (inputPointX, inputPointY);
                        }
                    }

                    // Find the winner node
                    // Winner node is the node with the LOWEST distance
                    double winner = this.distances[0, 0];
                    Node winnerNode = this.kohonenGrid[0, 0];

                    for (int j = 0; j < this.distances.GetLength (0); j++)
                    {
                        for (int k = 0; k < this.distances.GetLength (1); k++)
                        {
                            if (this.distances[j, k] < winner)
                            {
                                winner = this.distances[j, k];
                                winnerNode = this.kohonenGrid[j, k];
                            }
                        }
                    }

                    // Searching for the neighboring nodes and modifying their weights
                    // This is the main magic of the Kohonen neural network

                    int radiusDistance = (int)Math.Ceiling (radius);

                    // Search nodes around the winner node.
                    for (int j = winnerNode.PositionX - radiusDistance; j < winnerNode.PositionX + radiusDistance; j++)
                    {
                        for (int k = winnerNode.PositionY - radiusDistance; k < winnerNode.PositionY + radiusDistance; k++)
                        {
                            if (j >= 0 && k >= 0 && j < this.kohonenGrid.GetLength (0) && k < this.kohonenGrid.GetLength (1))
                            {
                                double tempDistX = Math.Abs (j - winnerNode.PositionX);
                                double tempDistY = Math.Abs (k - winnerNode.PositionY);
                                
                                // Equation of circle
                                if (tempDistX * tempDistX + tempDistY * tempDistY <= radius * radius)
                                {
                                    // Distance of the current node from the winner node == length of vector == Pythagorean theorem
                                    double dist = Math.Sqrt ((Math.Pow (tempDistX, 2) + Math.Pow (tempDistY, 2)));
                                    double neighborhood = Math.Exp (-(dist * dist) / (2 * radius * radius));
                                    // Modify weights
                                    this.kohonenGrid[j, k].ModifyValues (neighborhood, gamma, inputPointX, inputPointY);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the List of cluster centers.
        /// </summary>
        public List<(int, int)> GetClusterCenters ()
        {
            var centers = new List<(int, int)> ();

            for (int i = 0; i < this.kohonenGrid.GetLength (0); i++)
            {
                for (int j = 0; j < this.kohonenGrid.GetLength (1); j++)
                {
                    centers.Add (((int)Math.Round (this.kohonenGrid[i, j].WeightX), (int)Math.Round (this.kohonenGrid[i, j].WeightY)));
                }
            }

            return centers;
        }

        /// <summary>
        /// Creates the final image.
        /// Weights of nodes are coordintes of clusters center
        /// </summary>
        /// <param name="inputPoints">List of input points</param>
        /// <param name="imageWidth">Weight of original image</param>
        /// <param name="imageHeight">Height of original image</param>
        /// <returns></returns>
        public Bitmap Paint (List<Point> inputPoints, int imageWidth, int imageHeight)
        {
            Bitmap finalImage = new Bitmap (imageWidth, imageHeight);

            // Create image with white points
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageHeight; j++)
                {
                    finalImage.SetPixel (i, j, Color.White);
                }
            }

            // Draw input points (black points) on the final image
            for (int i = 0; i < inputPoints.Count; i++)
            {
                finalImage.SetPixel (inputPoints.ElementAt (i).X, inputPoints.ElementAt (i).Y, Color.Black);
            }

            for (int i = 0; i < this.kohonenGrid.GetLength (0); i++)
            {
                for (int j = 0; j < this.kohonenGrid.GetLength (1); j++)
                {
                    // Draw centers of clusters (draw rectangle 3x3)
                    int tempX = (int)Math.Round (this.kohonenGrid[i, j].WeightX);
                    int tempY = (int)Math.Round (this.kohonenGrid[i, j].WeightY);
                    for (int k = tempX - 1; k <= tempX + 1; k++)
                    {
                        for (int m = tempY - 1; m <= tempY + 1; m++)
                        {
                            if (k >= 0 && m >= 0 && k < imageWidth && m < imageHeight)
                            {
                                finalImage.SetPixel (k, m, Color.Red);
                            }
                        }
                    }

                    // Weights of the node adjacent to the currently examined node
                    int neighborWeightX = 0;
                    int neighborWeightY = 0;

                    // Draw connections between centers of clusters.
                    using (var graphics = Graphics.FromImage (finalImage))
                    {
                        // RIGHT
                        if (i + 1 > 0 && i + 1 < this.kohonenGrid.GetLength (0))
                        {
                            neighborWeightX = (int)Math.Round (this.kohonenGrid[i + 1, j].WeightX);
                            neighborWeightY = (int)Math.Round (this.kohonenGrid[i + 1, j].WeightY);
                            graphics.DrawLine (redPen, tempX, tempY, neighborWeightX, neighborWeightY);
                        }
                        // LEFT
                        if (i - 1 > 0 && i - 1 < this.kohonenGrid.GetLength (0))
                        {
                            neighborWeightX = (int)Math.Round (this.kohonenGrid[i - 1, j].WeightX);
                            neighborWeightY = (int)Math.Round (this.kohonenGrid[i - 1, j].WeightY);
                            graphics.DrawLine (redPen, tempX, tempY, neighborWeightX, neighborWeightY);
                        }
                        // UP
                        if (j + 1 > 0 && j + 1 < this.kohonenGrid.GetLength (1))
                        {
                            neighborWeightX = (int)Math.Round (this.kohonenGrid[i, j + 1].WeightX);
                            neighborWeightY = (int)Math.Round (this.kohonenGrid[i, j + 1].WeightY);
                            graphics.DrawLine (redPen, tempX, tempY, neighborWeightX, neighborWeightY);
                        }
                        // DOWN
                        if (j - 1 > 0 && j - 1 < this.kohonenGrid.GetLength (1))
                        {
                            neighborWeightX = (int)Math.Round (this.kohonenGrid[i, j - 1].WeightX);
                            neighborWeightY = (int)Math.Round (this.kohonenGrid[i, j - 1].WeightY);
                            graphics.DrawLine (redPen, tempX, tempY, neighborWeightX, neighborWeightY);
                        }
                    }
                }
            }
            return finalImage;
        }

        public void Dispose ()
        {
            this.kohonenGrid = null;
            this.distances = null;
        }
    }
}
