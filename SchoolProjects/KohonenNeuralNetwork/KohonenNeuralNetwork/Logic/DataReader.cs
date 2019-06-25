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

    public class DataReader
    {
        Random rng = new Random ();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReader" class. />
        /// </summary>
        /// <param name="filePath">Input bitmap image</param>
        public DataReader (Bitmap inputImage)
        {
            this.InputPoints = new List<Point> ();

            for (int x = 0; x < inputImage.Width; x++)
            {
                for (int y = 0; y < inputImage.Height; y++)
                {
                    Color pixelColor = inputImage.GetPixel (x, y);
                    // Basically if point is black.
                    if (pixelColor.R < 21 && pixelColor.G < 21 && pixelColor.B < 21)
                    {
                        this.InputPoints.Add (new Point (x, y));
                    }
                }
            }

            this.ImageWidth = inputImage.Width;
            this.ImageHeight = inputImage.Height;

            this.Shuffle (this.InputPoints);
        }

        /// <summary>
        /// List of input points.
        /// It's list of black images.
        /// </summary>
        public List<Point> InputPoints { get; private set; }

        /// <summary>
        /// Width of input image.
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// Height of input image.
        /// </summary>
        public int ImageHeight { get; private set; }

        /// <summary>
        /// Shuffles the list of objects.
        /// </summary>
        /// <typeparam name="T">System.Drawing.Point</typeparam>
        /// <param name="list">List of points.</param>
        private void Shuffle<T> (IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next (n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
