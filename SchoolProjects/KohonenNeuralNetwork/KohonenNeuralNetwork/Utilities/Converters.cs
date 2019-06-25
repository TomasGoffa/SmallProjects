/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    public static class Converters
    {
        /// <summary>
        /// Convert a Bitmap image to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Bitmap Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource ToBitmapSource (Bitmap image)
        {
            var handle = image.GetHbitmap ();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap (handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions ());
            }

            finally
            {
                DeleteObject (handle);
            }
        }

        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport ("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs (UnmanagedType.Bool)]
        public static extern bool DeleteObject ([In] IntPtr hObject);
    }
}
