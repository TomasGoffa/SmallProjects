/// <summary>
/// This file is part of application
/// that implements the Backprop Neural network
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace NeuralNetwork.Backpropagation
{
    using System;
    using System.Threading;

    public class HiddenLayer
    {
        public Synapsis[,] inputHiddenLayerSynapsis;
        public Synapsis[,] outputHiddenLayerSynapsis;

        public Neuron[] hiddenLayerNeurons;
        private readonly Neuron[] inputNeurons;
        private readonly Neuron[] outputNeurons;

        /// <summary>
        /// Initializes new instance of <see cref="HiddenLayer"/> class
        /// </summary>
        /// <param name="numberOfNeurons">Number of neurons in hidden layer</param>
        /// <param name="inputLayer">Input layer of neurons</param>
        /// <param name="outputLayer">Output layer of neurons</param>
        /// <param name="alpha">The alhpa</param>
        public HiddenLayer (int numberOfNeurons, Neuron[] inputLayer, Neuron[] outputLayer, double alpha)
        {
            this.outputNeurons = outputLayer;
            this.inputNeurons = inputLayer;

            this.inputHiddenLayerSynapsis = new Synapsis[numberOfNeurons, inputLayer.Length + 1];
            this.outputHiddenLayerSynapsis = new Synapsis[3, numberOfNeurons];
            this.hiddenLayerNeurons = new Neuron[numberOfNeurons];

            for (int i = 0; i < numberOfNeurons; i++)
            {
                for (int j = 0; j < outputLayer.Length; j++)
                {
                    this.outputHiddenLayerSynapsis[j, i] = new Synapsis (GetRandomNumber ());
                    // Sleep to get a random number
                    Thread.Sleep (10);
                }

                hiddenLayerNeurons[i] = new Neuron (alpha);

                for (int j = 0; j < inputLayer.Length + 1; j++)
                {
                    this.inputHiddenLayerSynapsis[i, j] = new Synapsis (GetRandomNumber ());
                    // Sleep to get a random number
                    Thread.Sleep (10);
                }
            }
        }

        /// <summary>
        /// Computes the output of hidden layer
        /// </summary>
        public void ComputeHiddenLayerOutput ()
        {
            for (int i = 0; i < this.inputHiddenLayerSynapsis.GetLength(0); i++)
            {

                double inTemp = 0;
                for (int j = 0; j < this.inputHiddenLayerSynapsis.GetLength (1); j++)
                {
                    if (j == this.inputHiddenLayerSynapsis.GetLength (1) - 1)
                    {
                        // Bias
                        inTemp = inTemp + (-1 * this.inputHiddenLayerSynapsis[i, j].Weight);
                    }
                    else
                    {
                        inTemp = inTemp + this.inputNeurons[j].Output * this.inputHiddenLayerSynapsis[i, j].Weight;
                    }
                }

                hiddenLayerNeurons[i].Compute (inTemp);
            }
        }

        /// <summary>
        /// Computes the delta (error) of hidden layer
        /// </summary>
        public void ComputeHiddenLayerDelta ()
        {
            for (int i = 0; i < this.hiddenLayerNeurons.Length; i++)
            {
                double inTemp = 0;
                double inDel = 0;

                // Compute input to the neuron
                for (int j = 0; j < this.inputHiddenLayerSynapsis.GetLength(1); j++)
                {
                    if (j == this.inputHiddenLayerSynapsis.GetLength (1) - 1)
                    {
                        // Bias
                        inTemp = inTemp + (-1 * this.inputHiddenLayerSynapsis[i, j].Weight);
                    }
                    else
                    {
                        inTemp = inTemp + this.inputNeurons[j].Output * this.inputHiddenLayerSynapsis[i, j].Weight;
                    }
                }

                for (int j = 0; j < this.outputNeurons.Length; j++)
                {
                    inDel = inDel + this.outputNeurons[j].Delta * this.outputHiddenLayerSynapsis[j, i].Weight;
                }

                this.hiddenLayerNeurons[i].ComputeDelta (inDel, inTemp);
            }
        }

        /// <summary>
        /// Adapts the weights
        /// </summary>
        public void AdaptWieghts ()
        {
            for (int i = 0; i < this.inputHiddenLayerSynapsis.GetLength(0); i++)
            {
                for (int j = 0; j < this.inputHiddenLayerSynapsis.GetLength (1); j++)
                {
                    if (j == this.inputHiddenLayerSynapsis.GetLength (1) - 1)
                    {
                        // Bias
                        this.inputHiddenLayerSynapsis[i, j].ModifyWeight (this.hiddenLayerNeurons[i].Delta, -1);
                    }
                    else
                    {
                        this.inputHiddenLayerSynapsis[i, j].ModifyWeight (this.hiddenLayerNeurons[i].Delta, this.inputNeurons[j].Output);
                    }
                }
            }

            for (int i = 0; i < this.outputHiddenLayerSynapsis.GetLength(1); i++)
            {
                for (int j = 0; j < this.outputHiddenLayerSynapsis.GetLength (0); j++)
                {
                    this.outputHiddenLayerSynapsis[j, i].ModifyWeight (this.outputNeurons[j].Delta, this.hiddenLayerNeurons[i].Output);
                }
            }
        }

        /// <summary>
        /// Gets the input for the output neuron
        /// </summary>
        public double GetInputForLastNeurons (int numOfOutputNeuron)
        {
            double outTemp = 0;

            for (int i = 0; i < this.hiddenLayerNeurons.Length; i++)
            {
                outTemp = outTemp + this.hiddenLayerNeurons[i].Output * this.outputHiddenLayerSynapsis[numOfOutputNeuron, i].Weight;
            }
            return outTemp;
        }

        /// <summary>
        /// Gets the output from input neuron and computes delta
        /// </summary>
        public double GetDeltaForFirstLayer (int numOfInputNeuron)
        {
            double delta = 0;

            for (int i = 0; i < this.hiddenLayerNeurons.Length; i++)
            {
                delta = delta + this.hiddenLayerNeurons[i].Delta * this.inputHiddenLayerSynapsis[i, numOfInputNeuron].Weight;
            }

            return delta;
        }

        /// <summary>
        /// Generates the random number from <-0.6, 0.6>
        /// </summary>
        public static double GetRandomNumber ()
        {
            Random random = new Random ();
            return (-0.6 + (random.NextDouble () * (1.2)));
        }

        /// <summary>
        /// Resets the values of neurons in hidden layer.
        /// </summary>
        public void ResetHiddenLayer ()
        {
            for (int i = 0; i < this.hiddenLayerNeurons.Length; i++)
            {
                this.hiddenLayerNeurons[i].ResetNeuron ();
            }
        }
    }
}
