using System.Collections.Generic;
using System.Linq;
using RentMeRentalSystem.Model;

namespace RentMeRentalSystem.View
{
    public class FurnitureListItem : Furniture
    {
        public bool IsChecked { get; set; }

        public int SelectedQuantity { get; set; }

        public List<int> QuantityList => Enumerable.Range(1, this.Quantity).ToList();

        public string StringValue => $"{FurnitureId}, {CategoryName}, {StyleName}, {DailyRentalRate}, {Quantity}";

    }
}