/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SaveOutputView.xaml
    /// </summary>
    public partial class SaveOutputView : Window
    {
        public SaveOutputView ()
        {
            InitializeComponent ();
        }

        private void MoveWindow (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove ();
        }
    }
}
