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
    using System.Linq;
    using System.Threading;

    public class NeuralNetwork
    {
        public static double Gamma { get; private set; }

        public int CorrectlyClassified { get; private set; }

        public int IncorrectlyClassified { get; private set; }
        
        private double setpoint;
        private double[] setpointField;     
        private int numberOfOuputNeurons;

        private readonly double alpha;
        private readonly int numberOfIterations;
        private readonly int numOfHiddenLayerNeurons;
        private readonly Synapsis[,] allSynapsis;
        private readonly Neuron[] inputLayerNeurons;

        private HiddenLayer hiddenLayer;
        private Neuron[] outputLayerNeurons;

        //Synapsis that connects bias to neurons
        private Synapsis[] biasSynapsis;

        /// <summary>
        /// Output table
        /// </summary>
        public int[,] PivotTable { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="NeuralNetwork"/> class
        /// </summary>
        /// <param name="inputVectorLength">Length of input data vector.</param>
        /// <param name="gamma">The Gamma = Learning parameter</param>
        /// <param name="alpha">The alpha</param>
        /// <param name="numberOfIterations">Number of iterations</param>
        /// <param name="numOfHiddenLayerNeurons">Number of neurons in Hidden layer</param>
        public NeuralNetwork (int inputVectorLength, double gamma, double alpha, int numberOfIterations, int numOfHiddenLayerNeurons)
        {
            Gamma = gamma;
            this.alpha = alpha;
            this.numberOfIterations = numberOfIterations;
            this.numOfHiddenLayerNeurons = numOfHiddenLayerNeurons;
            this.allSynapsis = new Synapsis[inputVectorLength - 1, inputVectorLength];
            inputLayerNeurons = new Neuron[inputVectorLength - 1];
            int i, j;

            for (i = 0; i < inputVectorLength - 1; i++)
            {
                for (j = 0; j < inputVectorLength; j++)
                {
                    this.allSynapsis[i, j] = new Synapsis (HiddenLayer.GetRandomNumber ());
                    Thread.Sleep (10);
                }
                this.inputLayerNeurons[i] = new Neuron (this.alpha);
            }
        }

        /// <summary>
        /// Trains the Neural Network
        /// </summary>
        /// <param name="inputData">2D array of input data</param>
        public void TrainNeuralNetwork (double[,] inputData)
        {
            int bad = 10000;
            int good = 0;
            int currentIteration = 0;
            int numberOfInputs = inputData.GetLength (0);
            int inputVectorLength = inputData.GetLength (1);

            // Classes of data
            List<double> classes = new List<double> ();

            for (int i = 0; i < numberOfInputs; i++)
            {
                double setter = inputData[i, inputVectorLength - 1];
                bool obsahuje = false;
                for (int j = 0; j < classes.Count; j++)
                {
                    if (setter == classes.ElementAt (j)) obsahuje = true;
                }

                if (obsahuje == false) classes.Add (setter);
            }

            setpointField = new double[classes.Count];
            for (int i = 0; i < classes.Count; i++)
            {
                setpointField[i] = classes.ElementAt (i);
            }

            Array.Sort (setpointField);

            // Create output neurons
            // 1 neuron if output classes are 2
            // 3 neurons if output classes are 3 
            if (classes.Count == 2) this.numberOfOuputNeurons = 1;
            else this.numberOfOuputNeurons = 3;



            this.outputLayerNeurons = new Neuron[numberOfOuputNeurons];
            this.biasSynapsis = new Synapsis[numberOfOuputNeurons];
            for (int i = 0; i < numberOfOuputNeurons; i++)
            {
                this.outputLayerNeurons[i] = new Neuron (this.alpha);
                this.biasSynapsis[i] = new Synapsis (HiddenLayer.GetRandomNumber ());
                Thread.Sleep (10);
            }

            // Hidden layer
            this.hiddenLayer = new HiddenLayer (this.numOfHiddenLayerNeurons, this.inputLayerNeurons, this.outputLayerNeurons, this.alpha);


            // inputs to the neurons
            double[] inOutNeur = new double[numberOfOuputNeurons];
            double[] inNeur = new double[inputVectorLength - 1];
            
            // inputs to the neurons to help compute an error
            double[] inputDelta = new double[inputVectorLength - 1];
            // inputs to the network
            double[] inputs = new double[inputVectorLength - 1];

            int x = 0;

            while (currentIteration < this.numberOfIterations)
            {
                bad = 0;
                good = 0;
                currentIteration = currentIteration + 1;
                if (x == numberOfInputs - 1)
                {
                    x = 0;
                }

                for (x = 0; x < numberOfInputs; x++)
                {
                    // Expected value of the output
                    this.setpoint = inputData[x, inputVectorLength - 1];

                    // Process the inputs
                    for (int j = 0; j < inputVectorLength - 1; j++)
                    {
                        inputs[j] = inputData[x, j];
                    }


                    // Calculation of outputs from neurons
                    for (int j = 0; j < inputVectorLength - 1; j++)
                    {
                        inNeur[j] = 0;
                        for (int k = 0; k <= inputVectorLength - 1; k++)
                        {
                            if (k == inputVectorLength - 1) inNeur[j] = inNeur[j] + (-1 * allSynapsis[j, k].Weight);
                            else inNeur[j] = inNeur[j] + inputs[k] * allSynapsis[j, k].Weight;
                        }

                        inputLayerNeurons[j].Compute (inNeur[j]);
                    }
                    this.hiddenLayer.ComputeHiddenLayerOutput ();

                    for (int j = 0; j < numberOfOuputNeurons; j++)
                    {
                        inOutNeur[j] = 0;
                        inOutNeur[j] = this.hiddenLayer.GetInputForLastNeurons (j) + (-1 * this.biasSynapsis[j].Weight);
                        this.outputLayerNeurons[j].Compute (inOutNeur[j]);
                    }

                    // Get number of incorrectly clasified neurons
                    double max = 0;
                    int memo;
                    if (numberOfOuputNeurons == 1)
                    {
                        if (setpointField[0] == 0)
                        {
                            if (this.outputLayerNeurons[0].Output < 0.5) memo = 0;
                            else memo = 1;

                            if (memo == setpoint) good = good + 1;
                            else bad = bad + 1;
                        }

                        else
                        {
                            if (this.outputLayerNeurons[0].Output < 1.5) memo = 1;
                            else memo = 2;

                            if (memo == setpoint) good = good + 1;
                            else bad = bad + 1;
                        }
                    }

                    else
                    {
                        max = Math.Max (this.outputLayerNeurons[0].Output, Math.Max (this.outputLayerNeurons[1].Output, this.outputLayerNeurons[2].Output));

                        if (max == this.outputLayerNeurons[0].Output)
                        {
                            if (setpoint != setpointField[0]) bad = bad + 1;
                            else good = good + 1;
                        }
                        else if (max == this.outputLayerNeurons[1].Output)
                        {
                            if (setpoint != setpointField[1]) bad = bad + 1;
                            else good = good + 1;
                        }
                        else if (max == this.outputLayerNeurons[2].Output)
                        {
                            if (setpoint != setpointField[2]) bad = bad + 1;
                            else good = good + 1;
                        }
                        max = 0;
                    }
                    
                    // Calculation of errors
                    if (numberOfOuputNeurons == 1)
                    {
                        this.outputLayerNeurons[0].ComputeDeltaFinal (setpoint, inOutNeur[0]);
                    }

                    else
                    {
                        if (setpoint == setpointField[0])
                        {
                            this.outputLayerNeurons[0].ComputeDeltaFinal (1, inOutNeur[0]);
                            this.outputLayerNeurons[1].ComputeDeltaFinal (0, inOutNeur[1]);
                            this.outputLayerNeurons[2].ComputeDeltaFinal (0, inOutNeur[2]);
                        }
                        else if (setpoint == setpointField[1])
                        {
                            this.outputLayerNeurons[0].ComputeDeltaFinal (0, inOutNeur[0]);
                            this.outputLayerNeurons[1].ComputeDeltaFinal (1, inOutNeur[1]);
                            this.outputLayerNeurons[2].ComputeDeltaFinal (0, inOutNeur[2]);
                        }
                        else if (setpoint == setpointField[2])
                        {
                            this.outputLayerNeurons[0].ComputeDeltaFinal (0, inOutNeur[0]);
                            this.outputLayerNeurons[1].ComputeDeltaFinal (0, inOutNeur[1]);
                            this.outputLayerNeurons[2].ComputeDeltaFinal (1, inOutNeur[2]);
                        }
                    }

                    this.hiddenLayer.ComputeHiddenLayerDelta ();

                    for (int j = 0; j < inputVectorLength - 1; j++)
                    {
                        inputDelta[j] = this.hiddenLayer.GetDeltaForFirstLayer (j);
                        inputLayerNeurons[j].ComputeDelta (inputDelta[j], inNeur[j]);
                        inNeur[j] = 0;
                        inputDelta[j] = 0;
                    }

                    // Adaptation of weights
                    for (int j = 0; j < inputVectorLength - 1; j++)
                    {
                        for (int k = 0; k < inputVectorLength; k++)
                        {
                            if (k == inputVectorLength - 1)
                            {
                                allSynapsis[j, k].ModifyWeight (inputLayerNeurons[j].Delta, -1);
                            }
                            else
                            {
                                allSynapsis[j, k].ModifyWeight (inputLayerNeurons[j].Delta, inputs[k]);
                            }
                        }
                    }

                    this.hiddenLayer.AdaptWieghts ();

                    for (int j = 0; j < numberOfOuputNeurons; j++)
                    {
                        this.biasSynapsis[j].ModifyWeight (this.outputLayerNeurons[j].Delta, -1);
                        inOutNeur[j] = 0;
                        this.outputLayerNeurons[j].ResetNeuron ();
                    }

                    this.hiddenLayer.ResetHiddenLayer ();

                    for (int j = 0; j < inputVectorLength - 1; j++)
                    {
                        inputLayerNeurons[j].ResetNeuron ();
                    }
                }

                Console.WriteLine ("Training. Current interation: " + currentIteration);
                Console.WriteLine ("Good:\t" + good);
                Console.WriteLine ("Bad:\t" + bad);
            }
        }

        /// <summary>
        /// Test the Neural Network
        /// </summary>
        /// <param name="testData">2D array of test data</param>
        public void TestNeuralNetwork (double[,] testData)
        {
            int size = testData.GetLength (0);
            int inputVectorLength = testData.GetLength (1);

            PivotTable = new int[3, 3];
            int c, m;
            for (c = 0; c < 3; c++)
            {
                for (m = 0; m < 3; m++) PivotTable[c, m] = 0;
            }

            int result = 0;
            int bad = 0;
            int good = 0;

            // inputs to the neurons
            double[] inOutNeur = new double[numberOfOuputNeurons];
            double[] inNeur = new double[inputVectorLength - 1];

            // inputs to the network
            double[] inputs = new double[inputVectorLength - 1];
            for (int i = 0; i < size; i++)
            {
                // Expected value on the output
                double setpoint = testData[i, inputVectorLength - 1];

                // Processing of inputs
                for (int j = 0; j < inputVectorLength - 1; j++)
                {
                    inputs[j] = testData[i, j];
                }


                // Output calculation of neurons
                for (int j = 0; j < inputVectorLength - 1; j++)
                {
                    inNeur[j] = 0;
                    for (int k = 0; k <= inputVectorLength - 1; k++)
                    {
                        if (k == inputVectorLength - 1)
                        {
                            // Bias
                            inNeur[j] = inNeur[j] + (-1 * allSynapsis[j, k].Weight);
                        }
                        else
                        {
                            inNeur[j] = inNeur[j] + inputs[k] * allSynapsis[j, k].Weight;
                        }
                    }

                    inputLayerNeurons[j].Compute (inNeur[j]);
                }

                this.hiddenLayer.ComputeHiddenLayerOutput ();

                for (int j = 0; j < numberOfOuputNeurons; j++)
                {
                    inOutNeur[j] = this.hiddenLayer.GetInputForLastNeurons (j) + (-1 * this.biasSynapsis[j].Weight);
                    this.outputLayerNeurons[j].Compute (inOutNeur[j]);
                }

                // Get number of incorrectly clasified inputs
                double max = 0;
                int memo;

                if (numberOfOuputNeurons == 1)
                {
                    if (setpointField[0] == 0)
                    {
                        if (this.outputLayerNeurons[0].Output < 0.5) memo = 0;
                        else memo = 1;

                        if (memo == setpoint) good = good + 1;
                        else bad = bad + 1;
                    }

                    else
                    {
                        if (this.outputLayerNeurons[0].Output < 1.5) memo = 1;
                        else memo = 2;

                        if (memo == setpoint) good = good + 1;
                        else bad = bad + 1;
                    }

                    result = memo;
                }

                else
                {
                    max = Math.Max (this.outputLayerNeurons[0].Output, Math.Max (this.outputLayerNeurons[1].Output, this.outputLayerNeurons[2].Output));
                    if (max == this.outputLayerNeurons[0].Output)
                    {
                        result = 0;
                        if (setpoint != setpointField[0]) bad = bad + 1;
                        else good = good + 1;

                    }
                    else if (max == this.outputLayerNeurons[1].Output)
                    {
                        result = 1;
                        if (setpoint != setpointField[1]) bad = bad + 1;
                        else good = good + 1;
                    }
                    else if (max == this.outputLayerNeurons[2].Output)
                    {
                        result = 2;
                        if (setpoint != setpointField[2]) bad = bad + 1;
                        else good = good + 1;
                    }
                }

                int classIndex = Convert.ToInt32 (setpoint);
                if (setpointField[0] != 0 && classIndex != 0)
                {
                    classIndex = classIndex - 1;
                }

                PivotTable[classIndex, result] += 1;
            }

            this.CorrectlyClassified = good;
            this.IncorrectlyClassified = bad;
        }
    }
}
