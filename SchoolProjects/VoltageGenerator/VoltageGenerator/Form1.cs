/// <summary>
/// This file is part of school assignment.
/// Application can generate 4 types of signal: Rectangle, Saw, Triangle and Sinus
/// This signal is shown in a graph and also sent to the hardware component where it is shown on Oscilloscope.
/// User can change type of signal by pressing buttons in application or pressing buttons on hardware component.
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace VoltageGenerator
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    public enum Trajectory
    {
        Rectangle,
        Saw,
        Triangle,
        Sinus
    }

    public partial class Form1 : Form
    {
        private const int myBase = 0x220;
        private double time;
        private Trajectory selectedTrajectory;
        private double period;
        private double frequency;
        private double amplitude;

        // Value which is send into terminals in hardware component and shown on Oscilloscope
        private double outputTerminals;
        // Value which is shown in a graph
        private double outputGraph;       
        // Array of values which are shown in graph
        private double[] signalArray = new double[100];

        // Auxiliary Variable. Used to help calculate Rectangle and Triangle signal.
        private double count;
        // Auxiliary Variables. They are incremented in time.
        private double tempPeriod;
        private double timeSawAndTriangle;

        public Form1()
        {
            InitializeComponent();

            Out(myBase + 9, 0);
            Out(myBase + 1, 0);
            for (int i=0; i<this.signalArray.Length; i++)
            {
                this.signalArray[i] = 0;
            }

            this.time = 0;
            this.frequency = 1;
            this.amplitude = 2;
            this.outputTerminals = 0;
            this.outputGraph = 0;
            this.tempPeriod = 0;
            this.timeSawAndTriangle = 0;
        }

        private void BtnRectangle_Click (object sender, EventArgs e)
        {
            this.selectedTrajectory = Trajectory.Rectangle;
            Clear();
        }

        private void BtnSaw_Click (object sender, EventArgs e)
        {
            this.selectedTrajectory = Trajectory.Saw;
            Clear();
        }

        private void BtnTriangle_Click (object sender, EventArgs e)
        {
            this.selectedTrajectory = Trajectory.Triangle;
            Clear();
        }

        private void BtnSinus_Click (object sender, EventArgs e)
        {
            this.selectedTrajectory = Trajectory.Sinus;
            Clear();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Read values from Potentiometer1 - Frequency
            Out(myBase + 2, 0);
            Out(myBase + 0, 0);
            while (((In(myBase + 8) / 16) % 2) != 1) ;
            var potentiometerAmplitude = (In(myBase + 1) * 16) + (In(myBase + 0) / 16);
            this.frequency = ((500 * potentiometerAmplitude) / 409500);
            if (this.frequency < 0.001) this.frequency = 0.001;
            freqLabel.Text = "Frequency: " + this.frequency + "Hz";

            // Read values from Potentiometer2 - Amplitude
            Out(myBase + 2, 17);
            Out(myBase + 0, 0);
            while (((In(myBase + 8) / 16) % 2) != 1) ;
            var potentiometerFrequency = (In(myBase + 1) * 16) + (In(myBase + 0) / 16);
            this.amplitude = ((1000 * potentiometerFrequency) / 40950);
            ampLabel.Text = "Amplitude: " + this.amplitude + "V";

            this.period = 1 / this.frequency;
            
            // Incrementation of Time-depended variables
            this.time = this.time + 0.01;
            this.tempPeriod = this.tempPeriod + 0.01;
            this.timeSawAndTriangle = this.timeSawAndTriangle + 0.01;

            // Showing value into a graph
            this.signalArray[this.signalArray.Length - 1] = this.outputGraph;
            Array.Copy(this.signalArray, 1, this.signalArray, 0, this.signalArray.Length - 1);
            if (chart1.IsHandleCreated)
            {
                this.Invoke((MethodInvoker)delegate { UpdateCpuChart(); });
            }

            // Handling pressing buttons on hardware component
            if (In(myBase + 3) == 254)
            {
                this.selectedTrajectory = Trajectory.Sinus;
                Clear();
            }
            else if (In(myBase + 3) == 253)
            {
                this.selectedTrajectory = Trajectory.Triangle;
                Clear();
            }
            else if (In(myBase + 3) == 251)
            {
                this.selectedTrajectory = Trajectory.Saw;
                Clear();
            }
            else if (In(myBase + 3) == 247)
            {
                this.selectedTrajectory = Trajectory.Rectangle;
                Clear();
            }

                GenerateTrajectory();
        }

        /// <summary>
        /// Calculates the output value
        /// </summary>
        private void GenerateTrajectory ()
        {
            switch (this.selectedTrajectory)
            {
                case Trajectory.Rectangle:
                    TrajectoryRectangle ();
                    break;
                case Trajectory.Saw:
                    TrajectorySaw ();
                    break;
                case Trajectory.Triangle:
                    TrajectoryTriangle ();
                    break;
                case Trajectory.Sinus:
                    TrajectorySinus ();
                    break;
                default:
                    // At the beginning show constant: 5V
                    this.outputGraph = 5;
                    this.outputTerminals = 5 * 409.5;
                    if (this.outputTerminals > 4095) this.outputTerminals = 4095;
                    if (this.outputTerminals < 0) this.outputTerminals = 0;
                    break;
            }

            // Send the values to the terminals on hardware component
            var value1 = (int)this.outputTerminals / 256;
            var value2 = (int)this.outputTerminals % 256;
            Out (myBase + 4, value2);
            Out (myBase + 5, value1);
        }

        /// <summary>
        /// Calculation of Rectangle trajectory
        /// </summary>
        private void TrajectoryRectangle ()
        {
            double T2 = 1 / (this.frequency * 2);
            double newValue;

            if (this.tempPeriod >= T2)
            {
                this.count = this.count + 1;
                this.tempPeriod = 0;
            }
            
            // Switching of two constants
            double temp = this.count % 2;
            if (temp == 1)
            {
                newValue = 5 + this.amplitude;
            }
            else
            {
                newValue = 5 - this.amplitude;
            }

            this.outputGraph = newValue;
            this.outputTerminals = newValue * 409.5;

            if (this.outputTerminals > 4095) this.outputTerminals = 4095;
            if (this.outputTerminals < 0) this.outputTerminals = 0;
        }

        /// <summary>
        /// Calculation of Saw trajectory
        /// </summary>
        private void TrajectorySaw ()
        {
            // Increasing linear function
            double newValue = (2 * this.amplitude * this.frequency) * (this.timeSawAndTriangle) + (5 - this.amplitude);

            if (this.tempPeriod >= this.period)
            {
                this.timeSawAndTriangle = 0;
                this.tempPeriod = 0;
            }
            this.outputGraph = newValue;
            this.outputTerminals = newValue * 409.5;

            if (this.outputTerminals > 4095) this.outputTerminals = 4095;
            if (this.outputTerminals < 0) this.outputTerminals = 0;
        }

        /// <summary>
        /// Calculation of Triangle trajectory
        /// </summary>
        private void TrajectoryTriangle ()
        {
            double increase_decrease = 0;

            if (this.tempPeriod >= this.period)
            {
                this.count = this.count + 1;
                this.timeSawAndTriangle = 0;
                this.tempPeriod = 0;
            }

            // Switching of increasing and decreasing linear function
            increase_decrease = this.count % 2;
            double newValue;

            if (increase_decrease == 1)
            {
                newValue = (2 * this.amplitude * this.frequency) * (this.timeSawAndTriangle) + (5 - this.amplitude);
            }
            else
            {
                newValue = -(2 * this.amplitude * this.frequency) * (this.timeSawAndTriangle) + 5 + this.amplitude;
            }

            this.outputGraph = newValue;
            this.outputTerminals = newValue * 409.5;

            if (this.outputTerminals > 4095) this.outputTerminals = 4095;
            if (this.outputTerminals < 0) this.outputTerminals = 0;

        }

        /// <summary>
        /// Calculation of Sinus trajectory
        /// </summary>
        private void TrajectorySinus ()
        {
            double newValue = this.amplitude * Math.Sin (this.frequency * this.time) + 5;
            this.outputGraph = newValue;
            this.outputTerminals = newValue * 409.5;

            if (this.outputTerminals > 4095) this.outputTerminals = 4095;
            if (this.outputTerminals < 0) this.outputTerminals = 0;
        }

        /// <summary>
        /// Update Graph
        /// </summary>
        private void UpdateCpuChart ()
        {
            chart1.Series["Series1"].BorderWidth = 8;
            chart1.Series["Series1"].Points.Clear ();
            for (int i = 0; i < this.signalArray.Length - 1; ++i)
            {
                chart1.Series["Series1"].Points.AddY (this.signalArray[i]);
            }
        }

        /// <summary>
        /// Clear several variables
        /// </summary>
        private void Clear ()
        {
            this.tempPeriod = 0;
            this.count = 0;
            this.timeSawAndTriangle = 0;
        }

        #region DLL imports

        [DllImport ("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Out (int add, int val);

        [DllImport ("inpout32.dll", EntryPoint = "Inp32")]
        public static extern byte In (int addr);

        #endregion
    }
}
