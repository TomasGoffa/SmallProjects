/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Ladislav Pomsar
/// </summary>

namespace APIS2.Models
{
    using System;
    using System.IO;
    using System.Reflection;
    using PdfSharp.Fonts;

    public class MyFontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Ignore case of font names.
            var name = familyName.ToLower().TrimEnd('#');

            // Deal with the fonts we know.
            switch (name)
            {
                case "arial":
                    if (isBold)
                    {
                        if (isItalic)
                            return new FontResolverInfo("Arial#bi");
                        return new FontResolverInfo("Arial#b");
                    }
                    if (isItalic)
                        return new FontResolverInfo("Arial#i");
                    return new FontResolverInfo("Arial#");
            }

            // We pass all other font requests to the default handler.
            // When running on a web server without sufficient permission, you can return a default font at this stage.
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        /// <summary>
        /// Return the font data for the fonts.
        /// </summary>
        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Arial#":
                    return FontHelper.Arial;

                case "Arial#b":
                    return FontHelper.ArialBold;

                case "Arial#i":
                    return FontHelper.ArialItalic;

                case "Arial#bi":
                    return FontHelper.ArialBoldItalic;
            }

            return null;
        }


        internal static MyFontResolver OurGlobalFontResolver = null;

        /// <summary>
        /// Ensure the font resolver is only applied once (or an exception is thrown)
        /// </summary>
        internal static void Apply()
        {
            if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
            {
                if (OurGlobalFontResolver == null)
                    OurGlobalFontResolver = new MyFontResolver();

                GlobalFontSettings.FontResolver = OurGlobalFontResolver;
            }
        }
    }


    /// <summary>
    /// Helper class that reads font data from embedded resources.
    /// </summary>
    public static class FontHelper
    {
        public static byte[] Arial
        {
            get { return LoadFontData("APIS2.fonts.arial.arial.ttf"); }
        }

        public static byte[] ArialBold
        {
            get { return LoadFontData("APIS2.fonts.arial.arialbd.ttf"); }
        }

        public static byte[] ArialItalic
        {
            get { return LoadFontData("APIS2.fonts.arial.ariali.ttf"); }
        }

        public static byte[] ArialBoldItalic
        {
            get { return LoadFontData("APIS2.fonts.arial.arialbi.ttf"); }
        }

        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        static byte[] LoadFontData(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts
            //var ourResources = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}