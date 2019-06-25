/// <summary>
/// This file is part of application
/// that implements the Backprop Neural network
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace NeuralNetwork.Backpropagation
{
    using System;

    class Program
    {
        static void Main (string[] args)
        {
            string dataPath = @"E:\Projects\Github_Init\NeuralNetwork.Backpropagation\Data\banana\banana_2.csv";
            double gamma = 0.8;
            double alpha = 0.5;
            int numOfIterations = 4000;
            int hiddenLayerNeurons = 6;

            Run (dataPath, gamma, alpha, numOfIterations, hiddenLayerNeurons);

            Console.ReadKey ();
        }

        private static void Run (string path, double gamma, double alpha, int numOfIterations, int hiddenLayerNeurons)
        {
            DataReader dataReader = new DataReader (path);
            NeuralNetwork network = new NeuralNetwork (dataReader.InputVectorLength, gamma, alpha, numOfIterations, hiddenLayerNeurons);

            // Train neural network
            Console.WriteLine ("Training neural network...");
            network.TrainNeuralNetwork (dataReader.TrainData);

            // Test neural netowk
            Console.WriteLine ("Testing neural network...");
            network.TestNeuralNetwork (dataReader.TestData);
            var outputTable = network.PivotTable;

            Console.WriteLine ("Test samples:\t" + dataReader.TestData.GetLength(0));
            Console.WriteLine ("Correctly classified:\t" + network.CorrectlyClassified);
            Console.WriteLine ("Incorrectly classified:\t" + network.IncorrectlyClassified);

            Console.WriteLine ("Pivot table:");
            Console.WriteLine ("\ta0\ta1\ta2");
            Console.WriteLine ("a0\t{0}\t{1}\t{2}", outputTable[0, 0], outputTable[0, 1], outputTable[0, 2]);
            Console.WriteLine ("a1\t{0}\t{1}\t{2}", outputTable[1, 0], outputTable[1, 1], outputTable[1, 2]);
            Console.WriteLine ("a2\t{0}\t{1}\t{2}", outputTable[2, 0], outputTable[2, 1], outputTable[2, 2]);
        }
    }
}