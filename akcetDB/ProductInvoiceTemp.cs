namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductInvoiceTemp")]
    public partial class ProductInvoiceTemp
    {
        [Key]
        public int ProductInvoiceID { get; set; }

        public int InvoiceIDTemp { get; set; }

        public int? ProductID { get; set; }

        public int? DdsID { get; set; }

        public int? ProductPrice { get; set; }

        public int? Quanity { get; set; }

        public virtual FakturiTemp FakturiTemp { get; set; }
    }
}
