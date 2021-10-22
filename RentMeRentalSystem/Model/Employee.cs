using System;

namespace RentMeRentalSystem.Model
{
    /// <summary>
    /// 10/20/2021 The employee
    /// </summary>
    /// <seealso cref="RentMeRentalSystem.Model.Individual" />
    /// <author>
    /// Eboni Walker
    /// </author>
    public class Employee : Individual
    {
        /// <summary>
        /// Gets or sets the username of the employee.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }
    }
}