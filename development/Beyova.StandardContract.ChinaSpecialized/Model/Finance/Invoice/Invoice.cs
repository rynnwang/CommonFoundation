using System.Collections.Generic;

namespace Beyova.ChinaSpecialized
{
    /// <summary>
    /// Class Invoice.
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Gets or sets the buyer title.
        /// </summary>
        /// <value>
        /// The buyer title.
        /// </value>
        public InvoiceTitle BuyerTitle { get; set; }

        /// <summary>
        /// Gets or sets the seller title.
        /// </summary>
        /// <value>
        /// The seller title.
        /// </value>
        public InvoiceTitle SellerTitle { get; set; }

        /// <summary>
        /// Gets or sets the reference number.
        /// </summary>
        /// <value>
        /// The reference number.
        /// </value>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the issued date.
        /// </summary>
        /// <value>
        /// The issued date.
        /// </value>
        public Date IssuedDate { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<InvoiceRowItem> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        public Invoice()
        {
            Items = new List<InvoiceRowItem>();
        }
    }
}