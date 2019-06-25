/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Ladislav Pomsar
/// </summary>

namespace APIS2.Models
{
    public class QualityControlProtocol
    {
        public int NumberOfChipsWithTooLowBinning { get; set; }
        public int NumberOfChipsWithTooHighBinning { get; set; }
        public int NumberOfFaultyRAMChips { get; set; }
        public int NumberOfFaultyEEPROMChips { get; set; }
        public int NumberOfFaultyFlashChips { get; set; }
    }
}