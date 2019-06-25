/// <summary>
/// This file is part of application which implements
/// Iterative Weight Perturbation algorithm.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace IterativeWeightPerturbation.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Defines the initialization type
    /// </summary>
    public enum Initialization
    {
        Random,
        Heuristic
    }

    public static class Logic
    {
        private static readonly int xZero = -1;
        private static readonly Random random = new Random ();

        public static double[] BEST = new double[] { 0, 0 };
        public static int COUNT;

        public static double[] IWPalgorithm (List<InputPoint> ISET, int ATTS, int Max_Iterations, MainWindow mainWindow, Initialization initialization)
        {
            // Let H be an LTU with random wights between -1 and 1.
            double[] H = new double[2];

            if (initialization == Initialization.Heuristic)
            {
                InitializeWeights(ISET, out H[1], out H[0]);
            }
            else
            {
                H[0] = GetRandomNumber(random);
                H[1] = GetRandomNumber(random);
            }

            // Let BEST be the hypothesis H.
            BEST[0] = H[0];
            BEST[1] = H[1];
            // Let COUNT be Maximum-Iterations.
            COUNT = Max_Iterations;

            double highestScore = 0.5;
            // Repeat until COUNT is zero
            while (COUNT > 0)
            {
                if (highestScore >= 1 || highestScore <= 0)
                {
                    Pen myPen = new Pen (Color.Red);
                    mainWindow.DrawLineOnImage (-BEST[1], BEST[0], myPen, 2);
                    myPen.Dispose ();
                    return BEST;
                }

                H[0] = BEST[0];
                H[1] = BEST[1];

                // For each attribute K in ATTS
                for (int k = 0; k < ATTS; k++)
                {
                    double[] U = new double[ISET.Count];

                    // For each instance J in ISET
                    for (int j = 0; j < ISET.Count; j++)
                    {
                        // Compute U[k,j] using H and J
                        if (k == 0)
                        {
                            U[j] = (H[1] * ISET.ElementAt (j).X + 1 * ISET.ElementAt (j).Y) / (xZero);
                        }
                        else if (k == 1)
                        {
                            U[j] = (H[0] * xZero + 1 * ISET.ElementAt (j).Y) / ISET.ElementAt (j).X;
                        }
                    }

                    // Place the U[j] values in decreasing order.
                    for (int i = 0; i < U.Length; i++)
                    {
                        for (int j = i; j < U.Length; j++)
                        {
                            if (U[j] > U[i])
                            {
                                double temp = U[j];
                                U[j] = U[i];
                                U[i] = temp;
                            }
                        }
                    }

                    double[] score_results = new double[U.Length - 1];
                    double[] temporaryHypothesis = new double[U.Length - 1];

                    // For each adjacent pair of U values
                    for (int i = 0; i < U.Length - 1; i++)
                    {
                        // Let Wk_sharp be the average of the pair.
                        double Wk_sharp = -((U[i] + U[i + 1]) / 2);
                        // Let H_sharp be H with Wk replaced by Wk_sharp
                        H[k] = Wk_sharp;
                        // Compute Score(H_sharp, ISET)
                        score_results[i] = SCORE (H, ISET);
                        temporaryHypothesis[i] = Wk_sharp;
                    }

                    // Let H be the LTU with the highest score.
                    highestScore = score_results.Max ();
                    int indexOfHighestScore = score_results.ToList ().IndexOf (highestScore);
                    H[k] = temporaryHypothesis[indexOfHighestScore];
                    BEST[0] = H[0];
                    BEST[1] = H[1];

                    if (BEST[0] > 1000000000000) BEST[0] = 1000;
                    if (BEST[0] < -1000000000000) BEST[0] = -1000;
                    if (BEST[1] > 1000000000000) BEST[1] = 1000;
                    if (BEST[1] < -1000000000000) BEST[1] = -1000;

                    // Draw straight line on image.
                    Pen myPen = new Pen (Color.Black);
                    mainWindow.DrawLineOnImage (-BEST[1], BEST[0], myPen);
                    myPen.Dispose ();
                }

                // Decrement COUNT by one.
                COUNT--;
            }

            return BEST;
        }

        /// <summary>
        /// Score function
        /// </summary>
        /// <param name="H">The hypothesis.</param>
        /// <param name="ISET">List of points.</param>
        /// <returns>The score value.</returns>
        private static double SCORE (double[] H, List<InputPoint> ISET)
        {
            int numberOfCoveredPositivePoints = 0;
            int numberOfUncoveredNegativePoints = 0;

            for (int j = 0; j < ISET.Count; j++)
            {
                double V = H[1] * ISET.ElementAt (j).X + 1 * ISET.ElementAt (j).Y + H[0] * xZero;

                if (V > 0 && ISET.ElementAt (j).IsPositive == true)
                {
                    numberOfCoveredPositivePoints++;
                }
                else if (V < 0 && ISET.ElementAt (j).IsPositive == false)
                {
                    numberOfUncoveredNegativePoints++;
                }
            }

            return (double)(numberOfCoveredPositivePoints + numberOfUncoveredNegativePoints) / ISET.Count;
        }

        /// <summary>
        /// Initializes wights
        /// </summary>
        /// <param name="ISET"></param>
        /// <param name="k"></param>
        /// <param name="q"></param>
        private static void InitializeWeights (List<InputPoint> ISET, out double k, out double q)
        {
            var positivePoints = ISET.Where (x => x.IsPositive == true).ToList ();
            var negativePoints = ISET.Where (x => x.IsPositive == false).ToList ();

            if (positivePoints.Count == 0 || negativePoints.Count == 0)
            {
                k = GetRandomNumber (random);
                q = GetRandomNumber (random);
            }

            else
            {
                double maxDistance = ComputeDistance (positivePoints[0], negativePoints[0]);
                InputPoint startNegativePoint = negativePoints[0];
                InputPoint startPositivePoint = positivePoints[0];

                foreach (var negative in negativePoints)
                {
                    foreach (var positive in positivePoints)
                    {
                        double distance = ComputeDistance (positive, negative);

                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            startNegativePoint = negative;
                            startPositivePoint = positive;
                        }
                    }
                }

                k = (double)(((double)startPositivePoint.Y - (double)startNegativePoint.Y) / ((double)startPositivePoint.X - (double)startNegativePoint.X));
                q = (double)((double)startPositivePoint.Y - (double)(k * (double)startPositivePoint.X));
                k = -k;
            }
        }

        /// <summary>
        /// Converts Bitmap object to BitmapSource
        /// </summary>
        /// <param name="source">Bitmap object to be converted</param>
        /// <returns>Output object of type BitmapSource</returns>
        public static BitmapSource FromBitmapToBitmapSource (Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap (
                          source.GetHbitmap (),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions ());
        }

        /// <summary>
        /// Returns random number from range (-1,1)
        /// </summary>
        /// <returns>The random double.</returns>
        public static double GetRandomNumber (Random random)
        {
            double minimum = -1;
            double maximum = 1;
            return random.NextDouble () * (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Logs message to the file.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogMessage (string message)
        {
            string pathToLog = Directory.GetCurrentDirectory () + "\\IWP_Log.log";

            try
            {
                using (StreamWriter writer = new StreamWriter (pathToLog, true))
                {
                    writer.WriteLine (message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Logging error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Computes distance between 2 points.
        /// </summary>
        private static double ComputeDistance(InputPoint point1, InputPoint point2)
        {
            return Math.Sqrt (Math.Pow (Math.Abs (point1.X - point2.X), 2) + Math.Pow (Math.Abs (point1.Y - point2.Y), 2));
        }
    }
}
