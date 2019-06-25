/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Ladislav Pomsar
/// </summary>

namespace APIS2.Models
{
    public class FinalManufacturingReport
    {
        public Order OriginalOrder { get; set; }
        public Order Manufactured { get; set; }
        public QualityControlProtocol ManufacturingQualityProtocol { get; set; }
    }
}