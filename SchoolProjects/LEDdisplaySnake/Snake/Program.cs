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