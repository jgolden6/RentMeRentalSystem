using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentMeRentalSystem.Model
{
    public class Furniture
    {
        public string FurnitureId { get; set; }

        public string CategoryName { get; set; }

        public string StyleName { get; set; }

        public double DailyRentalRate { get; set; }

        public int Quantity { get; set; }
    }
}
