/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using APIS2.Models;

    public class HomeController : Controller
    {
        private static MemoryStream pdfMemoryStream;

        [HttpGet]
        public ActionResult Index()
        {
            CustomerPage customerPage = new CustomerPage();
            return View(customerPage);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Order")]
        public ActionResult Order(CustomerPage customerPage)
        {
            // Check if combination of chip attributes set by user is possible
            bool validated = false;
            MicroTechDatabaseContext database = new MicroTechDatabaseContext();
            var listOfCombinations = database.OutputResources.ToList();
            database.Dispose();

            for (int i = 0; i < listOfCombinations.Count; i++)
            {
                if (customerPage.ProcessorType.Equals(listOfCombinations.ElementAt(i).ProcessorType) &&
                    customerPage.FlashMemory.Equals(listOfCombinations.ElementAt(i).Flash) &&
                    customerPage.RamMemory.Equals(listOfCombinations.ElementAt(i).RAM) &&
                    customerPage.Eeprom.Equals(listOfCombinations.ElementAt(i).EEPROM) &&
                    customerPage.Frequency.Equals(listOfCombinations.ElementAt(i).Frequency))
                {
                    validated = true;
                }
            }

            if (validated)
            {
                CreatedOrder.ProcessorType = customerPage.ProcessorType;
                CreatedOrder.FlashMemory = customerPage.FlashMemory;
                CreatedOrder.RAMMemmory = customerPage.RamMemory;
                CreatedOrder.EEPROM = customerPage.Eeprom;
                CreatedOrder.Frequency = customerPage.Frequency;
                CreatedOrder.NumberOfChips = customerPage.NumberOfChips;

                pdfMemoryStream = null;
                BillData billData = new BillData();
                return View(billData);
            }
            else
            {
                ViewBag.Message = "Sorry, but this combination of attributes is not possible." + Environment.NewLine +
                    "Try different combination.";
                return View("Index", customerPage);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Login")]
        public ActionResult LoggingView(CustomerPage customerPage)
        {
            Logging logging = new Logging();
            return RedirectToAction("../Logging/LoggingView", "Logging", logging);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ConfirmOrder")]
        public ActionResult ConfirmOrder(BillData myBillData)
        {
            myBillData.order = new CustomerPage
            {
                ProcessorType = CreatedOrder.ProcessorType,
                FlashMemory = CreatedOrder.FlashMemory,
                RamMemory = CreatedOrder.RAMMemmory,
                Eeprom = CreatedOrder.EEPROM,
                Frequency = CreatedOrder.Frequency,
                NumberOfChips = CreatedOrder.NumberOfChips
            };

            MicroTechDatabaseContext database = new MicroTechDatabaseContext();
            var idS = database.OrderLists.ToList();

            int maxID = 0;

            for (int i = 0; i < idS.Count; i++)
            {
                if (maxID <= idS.ElementAt(i).ID)
                {
                    maxID = idS.ElementAt(i).ID;
                }
            }

            database.OrderLists.Add(new OrderList()
            {
                ID = maxID + 1,
                ProcessorType = myBillData.order.ProcessorType,
                Flash = myBillData.order.FlashMemory,
                RAM = myBillData.order.RamMemory,
                EEPROM = myBillData.order.Eeprom,
                Frequency = myBillData.order.Frequency,
                NumberOfChips = myBillData.order.NumberOfChips.ToString(),

                FirstName = myBillData.FirstName,
                LastName = myBillData.LastName,
                Email = myBillData.EmailAddress,
                Phone = myBillData.PhoneNumber,
                Country = myBillData.Country,
                City = myBillData.City,
                Street = myBillData.Street,
                StreetNumber = myBillData.StreetNumber,
                PostalCode = myBillData.PostalCode
            });

            database.SaveChanges();
            database.Dispose();

            pdfMemoryStream = myBillData.CreateBillInPdf();

            ViewBag.Message = "You order was confirmed";
            return View("Order", myBillData);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DownloadBill")]
        public ActionResult DownloadBill(BillData myBillData)
        {
            if (pdfMemoryStream != null)
            {
                // byte[] bytes = pdfMemoryStream.ToArray();
                // pdfMemoryStream.Seek(0, SeekOrigin.Begin);

                return File(pdfMemoryStream, "application/pdf", "Bill.pdf");

                // return File(bytes, "application/pdf", "Bill.pdf");
                // return File(bytes, MediaTypeNames.Application.Octet, "Bill.pdf");
            }
            else
            {
                return View("Order", myBillData);
            }
        }
    }
}