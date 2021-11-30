using System.Runtime.Serialization;

namespace RentMeRentalSystem.Model
{
    
    public class RentalItem
    {
        #region Properties
        
        public string RentalId { get; set; }
        
        public string FurnitureId { get; set; }
        
        public int Quantity { get; set; }

        #endregion
    }
}