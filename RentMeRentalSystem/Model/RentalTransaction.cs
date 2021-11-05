using System;
using System.Collections.Generic;
using System.Linq;

namespace RentMeRentalSystem.Model
{
    public class RentalTransaction : Transaction
    {
        #region Properties

        public DateTime RentalDate { get; set; }

        #endregion
    }
}