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
    using System.IO;

    public class MainWindowModel
    {
        private Bitmap finalImage;
        private List<(int, int)> centers;

        public event EventHandler<NewImageEventArgs> LearningDone;
        public event EventHandler OutputSaved;

        /// <summary>
        /// Starts the whole process of learning algorithm.
        /// </summary>
        public void StartAlgorithm(Bitmap inputImage,
            double gamma, double radius,
            int gridWidth, int gridHeight, int numOfIterations)
        {
            // Parse inputs
            var dataReader = new DataReader (inputImage);
            // Select one input point randomly
            int randomPoint = new Random ().Next (0, dataReader.InputPoints.Count - 1);
            // Create neural network
            using (NeuralNetwork network = new NeuralNetwork (gridWidth, gridHeight, dataReader.InputPoints[randomPoint].X, dataReader.InputPoints[randomPoint].Y))
            {
                // Start learning algorithm
                network.LearnNeuralNetwork (dataReader.InputPoints, numOfIterations, gamma, radius);
                // Create final image
                this.finalImage = network.Paint (dataReader.InputPoints, dataReader.ImageWidth, dataReader.ImageHeight);
                this.centers = network.GetClusterCenters ();

                this.LearningDone?.Invoke (this, new NewImageEventArgs(finalImage));
            }
        }

        /// <summary>
        /// Saves the output
        /// </summary>
        /// <param name="outputDirectory">Directory where output will be saved.</param>
        /// <param name="fileName">The name of saved file.</param>
        /// <param name="saveImage">TRUE if final image will be saved.</param>
        /// <param name="saveTextFile">TRUE if centers of clusters will be saved.</param>
        public void SaveOutput(string outputDirectory, string fileName, bool saveImage, bool saveTextFile)
        {
            if (saveImage && this.finalImage != null)
            {
                this.finalImage.Save (Path.Combine(outputDirectory, fileName + ".bmp"));
            }

            if (saveTextFile && this.centers != null)
            {
                Logger.WriteResultToFile (this.centers, outputDirectory, fileName + ".txt");
            }

            this.OutputSaved?.Invoke (this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Class for sending image through Event
    /// </summary>
    public class NewImageEventArgs : EventArgs
    {
        public NewImageEventArgs (Bitmap image)
        {
            this.Image = image;
        }

        public Bitmap Image { get; }
    }
}
