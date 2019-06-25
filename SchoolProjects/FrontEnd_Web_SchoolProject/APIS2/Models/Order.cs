/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Models
{
    using System;

    public class Order
    {
        public Order() {}

        public Order(Order order)
        {
            this.OrderNumber = order.OrderNumber;
            this.Quantity = order.Quantity;
            this.Priority = order.Priority;
            this.ChipName = order.ChipName;
            this.FlashSize = order.FlashSize;
            this.RAMSize = order.RAMSize;
            this.EEPROMSize = order.EEPROMSize;
            this.Frequency = order.Frequency;
        }

        public int OrderNumber { get; set; }
        public int Quantity { get; set; }
        public int Priority { get; set;  }
        public String ChipName { get; set; }
        public int FlashSize { get; set; }
        public int RAMSize { get; set; }
        public int EEPROMSize { get; set; }
        public int Frequency { get; set; }
    }
}