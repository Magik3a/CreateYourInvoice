namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductInvoice")]
    public partial class ProductInvoice
    {
        public int ProductInvoiceID { get; set; }

        public int InvoiceID { get; set; }

        public int ProductID { get; set; }

        public int DdsID { get; set; }

        public int ProjectID { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public virtual DD DD { get; set; }

        public virtual Fakturi Fakturi { get; set; }

        public virtual Product Product { get; set; }

        public virtual Project Project { get; set; }
    }
}
