using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentMeRentalSystem.Model
{

    /// <summary>
    /// 10/20/2021 The customer
    /// </summary>
    /// <seealso cref="RentMeRentalSystem.Model.Individual" />
    /// <author>
    /// Eboni Walker
    /// </author>
    public class Customer : Individual
    {

        /// <summary>
        /// Gets or sets the customer's registration date.
        /// </summary>
        /// <value>
        /// The registration date.
        /// </value>
        public DateTime RegistrationDate { get; set; }

        public override string ToString()
        {
            return $"{FullName}, {IdNumber}, {PhoneNumber}";
        }
    }
}
