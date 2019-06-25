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

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
