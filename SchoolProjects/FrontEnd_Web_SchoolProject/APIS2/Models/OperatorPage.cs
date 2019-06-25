/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class OperatorPage
    {
        public static readonly string IMAGE_FIRST = "~/Images/ProcessOne.jpg";
        public static readonly string IMAGE_SECOND = "~/Images/ProcessTwo.png";
        public static readonly string IMAGE_THIRD = "~/Images/ProcessThree.JPG";
        public static readonly string IMAGE_FOURTH = "~/Images/ProcessFour.jpg";

        public static string ManufacturingProcessOneImagePath { get; set; }
        public static string ManufacturingProcessTwoImagePath { get; set; }
        public static string ManufacturingProcessThreeImagePath { get; set; }

        [Required]
        [Display(Name = "Orders waiting for confirmation")]
        public string Orders { get; set; }

        public List<SelectListItem> AllOrders { get; set; }

        public OperatorPage()
        {
            MicroTechDatabaseContext database = new MicroTechDatabaseContext();

            var listOfOrders = database.OrderLists.ToList();

            AllOrders = new List<SelectListItem>();
            string currentOrder = string.Empty;

            for (int i = 0; i < listOfOrders.Count; i++)
            {
                currentOrder = "Processor: " + listOfOrders.ElementAt(i).ProcessorType;
                currentOrder += ", Flash: " + listOfOrders.ElementAt(i).Flash;
                currentOrder += ", RAM: " + listOfOrders.ElementAt(i).RAM;
                currentOrder += ", EEPROM: " + listOfOrders.ElementAt(i).EEPROM;
                currentOrder += ", Freq: " + listOfOrders.ElementAt(i).Frequency;
                currentOrder += ", Number: " + listOfOrders.ElementAt(i).NumberOfChips;

                AllOrders.Add(new SelectListItem { Text = currentOrder, Value = listOfOrders.ElementAt(i).ID.ToString() });
            }

            database.Dispose();
        }
    }
}