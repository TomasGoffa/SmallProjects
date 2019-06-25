/// <summary>
/// This file is part of application which implements
/// Iterative Weight Perturbation algorithm.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace IterativeWeightPerturbation.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Bitmap image.
        /// </summary>
        private Bitmap imageToShow;

        /// <summary>
        /// List of points.
        /// </summary>
        private List<InputPoint> inputPoints = new List<InputPoint> ();

        /// <summary>
        /// Maximum iterations of algorithm.
        /// </summary>
        private int maxIterations;

        /// <summary>
        /// Type of algorithm initialization
        /// </summary>
        private Initialization initialization;

        /// <summary>
        /// Creates new instance of a MainWindow
        /// </summary>
        public MainWindow ()
        {
            try
            {
                InitializeComponent ();

                // If log file exists from previous running of application, delete it.
                string pathToLogFile = Directory.GetCurrentDirectory () + "\\IWP_Log.log";
                if (File.Exists (pathToLogFile))
                {
                    File.Delete (pathToLogFile);
                    File.Create (pathToLogFile);
                }

                CreateEmptyImage ();
                myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);

                PositiveClassRadioBtn.IsEnabled = false;
                NegativeClassRadioBtn.IsEnabled = false;
                MaxIterationsTextBox.Text = "400";
                ShowFinalLineBtn.IsEnabled = false;
                this.initialization = Initialization.Random;
                this.InitRandom.IsChecked = true;
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: MainWindow():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.ToString ());
            }
        }

        #region Events
        /// <summary>
        /// Draws point on image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyImage_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DrawPointsCheckBox.IsChecked == true)
                {
                    System.Windows.Point clickPoint = e.GetPosition (myImage);

                    if (PositiveClassRadioBtn.IsChecked == true)
                    {
                        this.DrawPointToImage ((int)clickPoint.X, (int)clickPoint.Y, System.Drawing.Color.Blue);
                        this.inputPoints.Add (new InputPoint ((int)clickPoint.X, (int)clickPoint.Y, true));
                        myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);
                    }
                    else if (NegativeClassRadioBtn.IsChecked == true)
                    {
                        this.DrawPointToImage ((int)clickPoint.X, (int)clickPoint.Y, System.Drawing.Color.Purple);
                        this.inputPoints.Add (new InputPoint ((int)clickPoint.X, (int)clickPoint.Y, false));
                        myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);
                    }
                }
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: myImage_MouseLeftButtonDown():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Selects Random Initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectRandomInit(object sender, RoutedEventArgs e)
        {
            this.initialization = Initialization.Random;
        }

        /// <summary>
        /// Selects Heuristic initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectHeuristicInit(object sender, RoutedEventArgs e)
        {
            this.initialization = Initialization.Heuristic;
        }

        /// <summary>
        /// Enables, disables the radio buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawPointsCheckBox_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                if (DrawPointsCheckBox.IsChecked == true)
                {
                    PositiveClassRadioBtn.IsEnabled = true;
                    NegativeClassRadioBtn.IsEnabled = true;
                }
                else
                {
                    PositiveClassRadioBtn.IsEnabled = false;
                    NegativeClassRadioBtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: DrawPointsCheckBox_Click():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Starts the algorithm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Click (object sender, RoutedEventArgs e)
        {
            bool iterationsParsed = Int32.TryParse (MaxIterationsTextBox.Text, out this.maxIterations);

            if (iterationsParsed)
            {
                if (this.inputPoints.Count > 0)
                {
                    this.StartComputing ();
                }
                else
                {
                    MessageBox.Show ("Load or create data set before you start algorithm.", "Empty Data Set",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show ("In MAX Iterations field enter only positive integers.", "Invalid input",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Clears image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.CreateEmptyImage ();
                this.inputPoints.Clear ();
                ShowFinalLineBtn.IsEnabled = false;
                myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Clear input points",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                Logic.LogMessage ("MainWindow: ClearBtn_Click():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Saves the data set to txt file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDataSetBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "DataSet.txt",
                    Filter = "Text File | *.txt"
                };

                // Show save file dialog box
                Nullable<bool> result = saveFileDialog.ShowDialog ();

                if (result == true)
                {
                    StreamWriter writer = new StreamWriter (saveFileDialog.OpenFile ());
                    for (int i = 0; i < this.inputPoints.Count; i++)
                    {
                        writer.WriteLine (this.inputPoints.ElementAt (i).X + " " +
                                        this.inputPoints.ElementAt (i).Y + " " +
                                        this.inputPoints.ElementAt (i).IsPositive.ToString ());
                    }
                    writer.Dispose ();
                    writer.Close ();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Save Data set",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                Logic.LogMessage ("MainWindow: SaveDataSetBtn_Click():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Loads data set from txt file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadDataSetBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.inputPoints.Clear ();
                this.CreateEmptyImage ();

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text File | *.txt"
                };

                // Show open file dialog box
                Nullable<bool> result = openFileDialog.ShowDialog ();

                if (result == true)
                {
                    using (StreamReader streamReader = new StreamReader (openFileDialog.FileName))
                    {
                        while (streamReader.Peek () >= 0)
                        {
                            string line;
                            string[] lineToArray;
                            line = streamReader.ReadLine ();

                            lineToArray = line.Split (' ');
                            InputPoint currentInputPoint = new InputPoint (Int32.Parse (lineToArray[0]),
                                                                Int32.Parse (lineToArray[1]),
                                                                Convert.ToBoolean (lineToArray[2]));
                            if (currentInputPoint.IsPositive)
                            {
                                this.DrawPointToImage (currentInputPoint.X, currentInputPoint.Y, System.Drawing.Color.Blue);
                            }
                            else
                            {
                                this.DrawPointToImage (currentInputPoint.X, currentInputPoint.Y, System.Drawing.Color.Purple);
                            }

                            this.inputPoints.Add (currentInputPoint);
                        }
                    }
                    myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);
                }
            }
            catch (Exception ex)
            {
                this.inputPoints.Clear ();
                this.CreateEmptyImage ();

                MessageBox.Show (ex.Message, "Load Data set",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                Logic.LogMessage ("MainWindow: LoadDataSetBtn_Click():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Shows only final line and removes all other lines.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowFinalLineBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear image
                CreateEmptyImage ();

                // Draw points.
                for (int i = 0; i < this.inputPoints.Count; i++)
                {
                    if (this.inputPoints.ElementAt (i).IsPositive)
                    {
                        DrawPointToImage (this.inputPoints.ElementAt (i).X, this.inputPoints.ElementAt (i).Y, System.Drawing.Color.Blue);
                    }
                    else
                    {
                        DrawPointToImage (this.inputPoints.ElementAt (i).X, this.inputPoints.ElementAt (i).Y, System.Drawing.Color.Purple);
                    }
                }

                // Draw final line.
                Pen myPen = new Pen (Color.Red);
                DrawLineOnImage (-Logic.BEST[1], Logic.BEST[0], myPen, 2);
                myPen.Dispose ();

                myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: ShowFinalLineBtn_Click():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close ();
                Environment.Exit (0);
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: CloseBtn_Click():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Drags the Main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            if (!myImage.IsMouseOver)
            {
                DragMove ();
            }
        }
        #endregion Events

        #region Methods
        /// <summary>
        /// Starts computing of the algorithm.
        /// </summary>
        private void StartComputing ()
        {
            try
            {
                // Clear data in previous run of algorithm
                Logic.BEST[0] = 0;
                Logic.BEST[1] = 0;
                Logic.COUNT = 0;

                // Clear image
                CreateEmptyImage ();

                // Draw points.
                for (int i = 0; i < this.inputPoints.Count; i++)
                {
                    if (this.inputPoints.ElementAt (i).IsPositive)
                    {
                        DrawPointToImage (this.inputPoints.ElementAt (i).X, this.inputPoints.ElementAt (i).Y, System.Drawing.Color.Blue);
                    }
                    else
                    {
                        DrawPointToImage (this.inputPoints.ElementAt (i).X, this.inputPoints.ElementAt (i).Y, System.Drawing.Color.Purple);
                    }
                }

                double[] output = Logic.IWPalgorithm (this.inputPoints, 2, this.maxIterations, this, this.initialization);

                ShowFinalLineBtn.IsEnabled = true;
                int numberOfIterations = this.maxIterations - Logic.COUNT;
                DrawPointsCheckBox.IsChecked = false;
                DrawPointsCheckBox_Click (null, null);

                // Show image with lines
                myImage.Source = Logic.FromBitmapToBitmapSource (this.imageToShow);

                MessageBox.Show ("w0 = " + output[0] + "\nw1 = " + output[1] +
                    "\nw2 = 1\n\nFinished after " + numberOfIterations.ToString () + " iterations.");
            }
            catch (Exception ex)
            {
                Logic.LogMessage ("MainWindow: StartBtn_Click():\n" + ex.ToString ());
                MessageBox.Show ("Exception:\n\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Draws point to the image.
        /// </summary>
        /// <param name="x">The "x" coordinate.</param>
        /// <param name="y">The "y" coordinate.</param>
        /// <param name="color">Color of point.</param>
        private void DrawPointToImage (int x, int y, System.Drawing.Color color)
        {
            int xBorderOne = x - 2;
            int xBorderTwo = x + 2;
            int yBorderOne = y - 2;
            int yBorderTwo = y + 2;

            if (xBorderOne < 0) xBorderOne = 0;
            if (yBorderOne < 0) yBorderOne = 0;
            if (xBorderTwo > this.imageToShow.Width) xBorderTwo = this.imageToShow.Width - 1;
            if (yBorderTwo > this.imageToShow.Height) yBorderTwo = this.imageToShow.Height - 1;

            for (int i = xBorderOne; i <= xBorderTwo; i++)
            {
                for (int j = yBorderOne; j <= yBorderTwo; j++)
                {
                    this.imageToShow.SetPixel (i, j, color);
                }
            }
        }

        /// <summary>
        /// Creates white image 500 x 500.
        /// </summary>
        private void CreateEmptyImage ()
        {
            this.imageToShow = new Bitmap (500, 500);

            for (int i = 0; i < this.imageToShow.Width; i++)
            {
                for (int j = 0; j < this.imageToShow.Height; j++)
                {
                    this.imageToShow.SetPixel (i, j, System.Drawing.Color.White);
                }
            }
        }

        /// <summary>
        /// Draws a line on image.
        /// </summary>
        public void DrawLineOnImage (double k, double q, Pen myPen)
        {
            DrawLineOnImage (k, q, myPen, 1);
        }

        /// <summary>
        /// Draws line on image.
        /// </summary>
        public void DrawLineOnImage (double k, double q, Pen myPen, int widthOfLine)
        {
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;
            bool usedEquivalent = false;

            int x1B = 0;
            int y1B = 0;
            int x2B = 0;
            int y2B = 0;

            try
            {
                int imageWidth = 500;
                x1 = 0;
                x2 = imageWidth - 1;
                y1 = (int)(k * x1 + q);
                y2 = (int)(k * x2 + q);
                usedEquivalent = false;

                // To avoid Overflow.
                if (y1 < -100000 || y1 > 100000 || y2 < -100000 || y2 > 100000)
                {
                    y1B = 0;
                    y2B = imageWidth - 1;
                    x1B = (int)((y1 - q) / k);
                    x2B = (int)((y2 - q) / k);
                    usedEquivalent = true;

                    using (var graphics = Graphics.FromImage (this.imageToShow))
                    {
                        myPen.Width = widthOfLine;
                        graphics.DrawLine (myPen, x1B, y1B, x2B, y2B);
                    }
                    myPen.Dispose ();
                }
                else
                {
                    // Draw image
                    using (var graphics = Graphics.FromImage (this.imageToShow))
                    {
                        myPen.Width = widthOfLine;
                        graphics.DrawLine (myPen, x1, y1, x2, y2);
                    }
                    myPen.Dispose ();
                }
            }
            catch (Exception ex)
            {
                Logic.COUNT++;

                Logic.LogMessage (ex.ToString ());

                Logic.LogMessage ("k: " + k + Environment.NewLine +
                    "q: " + q + Environment.NewLine + Environment.NewLine);

                Logic.LogMessage (usedEquivalent.ToString () + Environment.NewLine + Environment.NewLine +
                    "x1: " + x1.ToString () + Environment.NewLine +
                    "y1: " + y1.ToString () + Environment.NewLine +
                    "x2: " + x2.ToString () + Environment.NewLine +
                    "y2: " + y2.ToString () + Environment.NewLine);

                Logic.LogMessage (usedEquivalent.ToString () + Environment.NewLine + Environment.NewLine +
                    "x1B: " + x1B.ToString () + Environment.NewLine +
                    "y1B: " + y1B.ToString () + Environment.NewLine +
                    "x2B: " + x2B.ToString () + Environment.NewLine +
                    "y2B: " + y2B.ToString () + Environment.NewLine);
            }
        }
        #endregion Methods
    }
}
