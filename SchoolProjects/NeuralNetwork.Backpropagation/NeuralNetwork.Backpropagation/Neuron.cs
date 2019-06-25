/// <summary>
/// This file is part of application
/// that implements the Backprop Neural network
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace NeuralNetwork.Backpropagation
{
    using System;

    public class Neuron
    {
        private readonly double alpha;

        /// <summary>
        /// Initializes new instance of <see cref="Neuron"/> class
        /// </summary>
        /// <param name="al">The alpha value.</param>
        public Neuron (double al)
        {
            this.alpha = al;
        }

        /// <summary>
        /// Neuron output value
        /// </summary>
        public double Output { get; private set; }

        /// <summary>
        /// Neuron error
        /// </summary>
        public double Delta { get; private set; }

        /// <summary>
        /// Computes the neuron output value.
        /// </summary>
        /// <param name="input">The neuron input</param>
        public void Compute (double input)
        {
            // this.Output = 1 / (1 + Math.Pow(Math.E, -0.75 * input));
            this.Output = 1 / (1 + Math.Exp (-alpha * input));
        }

        /// <summary>
        /// Computes the final error of neuron.
        /// </summary>
        /// <param name="setpoint"></param>
        /// <param name="input"></param>
        public void ComputeDeltaFinal (double setpoint, double input)
        {
            // double temp = (0.75*Math.Pow(Math.E, -0.75 * input)) / (Math.Pow((1 + Math.Pow(Math.E, -0.75 * input)),2));
            double temp = (0.75 * Math.Exp (-alpha * input)) / (Math.Pow ((1 + Math.Exp (-alpha * input)), 2));
            this.Delta = (setpoint - this.Output) * temp;
        }

        /// <summary>
        /// Computes the error of neuron.
        /// </summary>
        /// <param name="outputs"></param>
        /// <param name="input"></param>
        public void ComputeDelta (double outputs, double input)
        {
            //double temp = (0.75*Math.Pow(Math.E, -0.75 * input)) / (Math.Pow((1 + Math.Pow(Math.E, -0.75 * input)),2));
            double temp = (0.75 * Math.Exp (-alpha * input)) / (Math.Pow ((1 + Math.Exp (-alpha * input)), 2));
            this.Delta = outputs * temp;
        }

        /// <summary>
        /// Resets the neuron output value and error.
        /// </summary>
        public void ResetNeuron ()
        {
            this.Delta = 0;
            this.Output = 0;
        }
    }
}
