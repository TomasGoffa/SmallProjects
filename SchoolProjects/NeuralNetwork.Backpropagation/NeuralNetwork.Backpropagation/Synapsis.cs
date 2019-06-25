/// <summary>
/// This file is part of application
/// that implements the Backprop Neural network
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace NeuralNetwork.Backpropagation
{
    public class Synapsis
    {   
        /// <summary>
        /// Synapsis weight
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="Synapsis"/> class
        /// </summary>
        /// <param name="al">The init value of weight</param>
        public Synapsis (double value)
        {
            this.Weight = value;
        }

        /// <summary>
        /// Change the weight
        /// </summary>
        /// <param name="getDeltaJ">Error of neuron behind the synapsis.</param>
        /// <param name="outputI">Output of neuron in front the synapsis.</param>
        public void ModifyWeight (double getDeltaJ, double outputI)
        {
            double deltaValue = NeuralNetwork.Gamma * getDeltaJ * outputI;
            this.Weight = this.Weight + deltaValue;
        }
    }
}
