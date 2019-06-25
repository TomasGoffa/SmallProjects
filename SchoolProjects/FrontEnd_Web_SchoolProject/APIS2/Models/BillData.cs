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
    using System.ComponentModel.DataAnnotations;
    using PdfSharp.Drawing;

    public class BillData
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "E-mail address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Street number")]
        public string StreetNumber { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        public CustomerPage order;

        public BillData()
        {

        }

        public MemoryStream CreateBillInPdf()
        {
            try
            {
                MyFontResolver.Apply();

                string bill = string.Empty;

                PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument();
                pdf.Info.Title = "CustomersBill";
                PdfSharp.Pdf.PdfPage pdfPage = pdf.AddPage();
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                XFont font = new XFont("Arial", 12, XFontStyle.Regular);
                XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                int xCor = 0;

                bill = "SELLER:";
                graph.DrawString(bill, headerFont, XBrushes.Black, new XRect(0, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "MicroTech";
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "Letna 9";
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "Kosice, Slovakia";
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "04001";
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "tomas.goffa@microtech.com";
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                bill = "SHIPPING ADDRESS:";
                xCor += 26;
                graph.DrawString(bill, headerFont, XBrushes.Black, new XRect(0, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = FirstName + " " + LastName;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = Street + " " + StreetNumber;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = City + ", " + Country;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = PostalCode;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = PhoneNumber;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = EmailAddress;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                bill = "DESCRIPTION:";
                xCor += 26;
                graph.DrawString(bill, headerFont, XBrushes.Black, new XRect(0, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "Processor type:\t" + this.order.ProcessorType;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "Flash:\t" + this.order.FlashMemory;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "RAM:\t" + this.order.RamMemory;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "EEPROM:\t" + this.order.Eeprom;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                bill = "Frequency:\t" + this.order.Frequency;
                xCor += 13;
                graph.DrawString(bill, font, XBrushes.Black, new XRect(12, xCor, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        pdf.Save(memoryStream, false);

                        return memoryStream;
                    }
                }
                catch
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string toString = ex.ToString();

                int a = 5;
                a++;
                return null;
            }
        }
    }
}