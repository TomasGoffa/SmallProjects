/// <summary>
/// This file is part of school assignment.
/// Functionality:  Snake created from LED lights on school hardware component
/// 
/// Author:         Tomas Goffa
/// </summary>

namespace Snake
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    public enum Trajectory
    {
        Rectangle,
        S
    }

    public partial class Form1 : Form
    {
        // Rectangle trajectory
        private readonly int[] trajectoryRec  = new int[12] {1,1,1,1,2,4,8,8,8,8,16,32};
        // S-trajectory
        private readonly int[] trajectoryS = new int[14] {16,32,1,2,4,8,16,32,1,32,16,8,4,2};

        private const int maxNumberOfSegmentsRec = 11;
        private const int maxNumberOfSegmentsS = 13;

        // Indexes of which value is shown on which diplay
        private int[] displayRec = new int[12] { 1, 2, 3, 4, 4, 4, 4, 3, 2, 1, 1, 1 };
        private int[] displayS = new int[14] {1,1,1,1,1,2,3,3,3,4,4,4,4,4};

        // Speed of snake: <-10; 10>
        private int speed = 1;
        private int milliseconds = 100;
        private int numberOfSegments = 1;

        private Trajectory selectedTrajectory;

        // Values which are shown on specific segments of LED display
        private int[] segment = new int[4];
        private int[] d0sec = new int[4];
        

        public Form1()
        {
            InitializeComponent();
            this.selectedTrajectory = Trajectory.Rectangle;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Buttons on hardware component
            // If they are pressed, they send decimal code
            switch (In(0x379))
            {
                case 119: // Decrement the number of snake's segments
                    this.BtnSegmentsDec_Click (this, EventArgs.Empty);
                    break;
                
                case 111: // Increment the number of snake's segments
                    this.BtnSegmentsInc_Click (this, EventArgs.Empty);
                    break;
                
                case 95: // Changing the shape of trajectory
                    this.BtnChangeTrajectory_Click (this, EventArgs.Empty);
                    break;
                
                case 255: // Decrement snake's speed
                    this.BtnSpeedDec_Click (this, EventArgs.Empty);
                    break;
                
                case 63: // Increment snake's speed
                    this.BtnSpeedInc_Click (this, EventArgs.Empty);
                    break;
            }

            this.ShowOutput ();
        }

        // Buttons in application have the same functionality as buttons on hardware component

        private void BtnSegmentsDec_Click (object sender, EventArgs e)
        {
            numberOfSegments = numberOfSegments - 1;
            if (numberOfSegments <= 1) numberOfSegments = 1;
        }

        private void BtnSegmentsInc_Click (object sender, EventArgs e)
        {
            numberOfSegments = numberOfSegments + 1;
            
            // Rectangle
            if (this.selectedTrajectory == Trajectory.Rectangle)
            {
                if (numberOfSegments >= maxNumberOfSegmentsRec) numberOfSegments = maxNumberOfSegmentsRec;
            }
            
            // S-trajectory
            else
            {
                if (numberOfSegments >= maxNumberOfSegmentsS) numberOfSegments = maxNumberOfSegmentsS;
            }
        }

        private void BtnChangeTrajectory_Click (object sender, EventArgs e)
        {
            this.selectedTrajectory = this.selectedTrajectory == Trajectory.Rectangle ? Trajectory.S : Trajectory.Rectangle;
        }

        private void BtnSpeedDec_Click (object sender, EventArgs e)
        {
            speed = speed - 1;
            if (speed <= -10) speed = -10;
        }

        private void BtnSpeedInc_Click (object sender, EventArgs e)
        {
            speed = speed + 1;
            if (speed >= 10) speed = 10;
        }

        /// <summary>
        /// Show output on LED display
        /// </summary>
        private void Display()
        {
            LblTrajectory.Text = this.selectedTrajectory == Trajectory.Rectangle ? "Rectangle trajectory" : "S trajcetory";
            LblSpeed.Text = this.speed == 0 ? "Speed: 0" : "Speed: " + speed;
            LblSegments.Text = ("Number of segments: " + numberOfSegments);

            Out(0x378, 0);
            Out(0x37A, 10);
            Out(0x378, segment[0]);
            for (int i = 0; i < 100000; i++) ;
            Out(0x378, 0);
            Out(0x37A, 9);
            Out(0x378, segment[1]);
            for (int i = 0; i < 100000; i++) ;
            Out(0x378, 0);
            Out(0x37A, 15);
            Out(0x378, segment[2]);
            for (int i = 0; i < 100000; i++) ;
            Out(0x378, 0);
            Out(0x37A, 3);
            Out(0x378, segment[3]);
            for (int i = 0; i < 100000; i++) ;
        }

        /// <summary>
        /// I don't remember
        /// </summary>
        private void ShowOutput ()
        {
            int k, g, t;

            g = this.selectedTrajectory == Trajectory.Rectangle ? 12 : 14;

            // Direction - Right
            if (speed > 0)
            {
                for (int j = 0; j < g; j++)
                {
                    for (int r = 0; r < milliseconds / speed; r++)
                    {
                        t = j - numberOfSegments;
                        for (k = j; k > t; k--)
                        {
                            int y = k < 0 ? (k + g) : k;

                            if (this.selectedTrajectory == Trajectory.Rectangle)
                            {
                                segment[displayRec[y] - 1] += trajectoryRec[y];
                            }
                            else
                            {
                                segment[displayS[y] - 1] += trajectoryS[y];
                            }
                        }

                        this.Display ();

                        // Store the last shown values in case of speed == 0
                        for (int h = 0; h < 4; h++)
                        {
                            d0sec[h] = segment[h];
                            segment[h] = 0;
                        }
                    }
                }
            }

            // Direction - Left
            else if (speed < 0)
            {
                for (int j = g - 1; j >= 0; j--)
                {
                    for (int r = 0; r > milliseconds / speed; r--)
                    {
                        t = j + numberOfSegments;
                        for (k = j; k < t; k++)
                        {
                            int y = k >= g ? (k - g) : k;

                            if (selectedTrajectory == Trajectory.Rectangle)
                            {
                                segment[displayRec[y] - 1] += trajectoryRec[y];
                            }
                            else
                            {
                                segment[displayS[y] - 1] += trajectoryS[y];
                            }
                        }

                        this.Display ();

                        // Store the last shown values in case of speed == 0
                        for (int h = 0; h < 4; h++)
                        {
                            d0sec[h] = segment[h];
                            segment[h] = 0;
                        }
                    }
                }
            }

            // Speed == 0
            else if (speed == 0)
            {
                for (int h = 0; h < 4; h++) segment[h] = d0sec[h];
                for (int a = 0; a < 100; a++) this.Display ();
                for (int h = 0; h < 4; h++) segment[h] = 0;
            }
        }

        #region DLL imports

        [DllImport ("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Out (int add, int val);
        [DllImport ("inpout32.dll", EntryPoint = "Inp32")]
        public static extern int In (int add);

        #endregion
    }
}