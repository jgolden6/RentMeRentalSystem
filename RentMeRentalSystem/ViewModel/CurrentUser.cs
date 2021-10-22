namespace RentMeRentalSystem.ViewModel
{
    public class CurrentUser
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the identifier number of the current user.
        /// </summary>
        /// <value>
        ///     The identifier number.
        /// </value>
        public static string IdNumber { get; set; }

        /// <summary>
        ///     Gets or sets the first name.
        /// </summary>
        /// <value>
        ///     The fname.
        /// </value>
        public static string Fname { get; set; }

        /// <summary>
        ///     Gets or sets the last name.
        /// </summary>
        /// <value>
        ///     The lname.
        /// </value>
        public static string Lname { get; set; }

        /// <summary>
        ///     Sets The full name of the current user.
        /// </summary>
        /// <value>
        ///     The The full name of the current user.
        /// </value>
        public static string FullName { get; set; }

        /// <summary>
        ///     Gets or sets the username of the employee.
        /// </summary>
        /// <value>
        ///     The username.
        /// </value>
        public static string Username { get; set; }

        #endregion
    }
}