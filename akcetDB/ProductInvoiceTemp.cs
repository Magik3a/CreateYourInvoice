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

        public int ProductID { get; set; }

        public int DdsID { get; set; }

        public int ProjectID { get; set; }
        
        public decimal ProductPrice { get; set; }
       
        public decimal Quanity { get; set; }

        public decimal ProductTotalPrice { get; set; }


        public virtual FakturiTemp FakturiTemp { get; set; }

        public virtual Project Project { get; set; }
    }
}
