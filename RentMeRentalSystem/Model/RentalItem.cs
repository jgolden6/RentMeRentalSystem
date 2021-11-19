namespace RentMeRentalSystem.Model
{
    public class RentalItem
    {
        #region Properties

        public string RentalId { get; set; }

        public Furniture FurnitureItem { get; set; }

        public string FurnitureId => this.FurnitureItem.FurnitureId;

        public int Quantity { get; set; }

        #endregion
    }
}