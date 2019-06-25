/// <summary>
/// This file is part of application
/// that implements the Backprop Neural network
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace NeuralNetwork.Backpropagation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    public class DataReader
    {
        private readonly double[,] inputDataArray;

        public int NumberOfTrainData { get; private set; }
        public int InputVectorLength { get; private set; }
        public double[,] TrainData { get; private set; }
        public double[,] TestData { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="DataReader"/> class
        /// </summary>
        /// <param name="path">Path to file with input data.</param>
        public DataReader (string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new ArgumentException ("File does not exist.");
            }

            List<string> listA = new List<string> ();

            using (StreamReader reader = new StreamReader (path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine ();
                    var values = line.Split (';');

                    listA.Add (values[0]);
                }
            }

            NumberOfTrainData = listA.Count - 1;
            var inputData = new string[NumberOfTrainData];
            listA.CopyTo (1, inputData, 0, NumberOfTrainData);

            string row = inputData[0];
            int tempSize = row.Length;
            string[] splitRow = new string[tempSize];
            splitRow = row.Split (',');
            InputVectorLength = splitRow.Length;

            // Create 2D array in which all input data will be stored
            this.inputDataArray = new double[NumberOfTrainData, InputVectorLength];
            int trainSize = (NumberOfTrainData * 3) / 4;
            int testSize = NumberOfTrainData - trainSize;
            TrainData = new double[trainSize, InputVectorLength];
            TestData = new double[testSize, InputVectorLength];

            for (int i = 0; i < NumberOfTrainData; i++)
            {
                row = inputData[i];
                splitRow = row.Split (',');
                for (int j = 0; j < InputVectorLength; j++)
                {
                    this.inputDataArray[i, j] = double.Parse (splitRow[j], CultureInfo.InvariantCulture);
                }

            }

            // Shuffle the data
            this.inputDataArray = Shuffle (this.inputDataArray);

            for (int i = 0; i < trainSize; i++)
            {
                for (int j = 0; j < InputVectorLength; j++)
                {
                    TrainData[i, j] = inputDataArray[i, j];
                }
            }
            for (int i = 0; i < testSize; i++)
            {
                for (int j = 0; j < InputVectorLength; j++)
                {
                    TestData[i, j] = inputDataArray[i + trainSize, j];
                }
            }

        }

        /// <summary>
        /// Shuffles the list of objects.
        /// </summary>
        private static void Shuffle<T> (IList<T> list)
        {
            Random rng = new Random ();
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

        /// <summary>
        /// Shuffles the list of objects.
        /// </summary>
        /// <returns>Shuffled 2D array.</returns>
        private static T[,] Shuffle<T> (T[,] array)
        {
            Random rng = new Random ();
            int n = array.GetLength (0);

            while (n > 1)
            {
                n--;
                int k = rng.Next (n + 1);
                T[] value = GetRow(array, k);

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    array[k, i] = array[n, i];
                    array[n, i] = value[i];
                }
            }

            return array;
        }

        /// <summary>
        /// Gets the row of 2D array.
        /// </summary>
        /// <param name="array">2D array.</param>
        /// <param name="row">index of row.</param>
        /// <returns>Selected row.</returns>
        private static T[] GetRow<T> (T[,] array, int row)
        {
            if (!typeof (T).IsPrimitive)
                throw new InvalidOperationException ("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException ("array");

            int cols = array.GetUpperBound (1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof (T) == typeof (bool))
                size = 1;
            else if (typeof (T) == typeof (char))
                size = 2;
            else
                size = Marshal.SizeOf<T> ();

            Buffer.BlockCopy (array, row * cols * size, result, 0, cols * size);

            return result;
        }
    }
}
