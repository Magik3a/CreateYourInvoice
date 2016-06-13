namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Fakturi")]
    public partial class Fakturi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fakturi()
        {
            ProductInvoices = new HashSet<ProductInvoice>();
        }

        [Key]
        public int InvoiceID { get; set; }

        public int CompanyID { get; set; }

        public int ProductInvoiceID { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceEndDate { get; set; }

        [Required]
        public string Project { get; set; }

        public int TotalPrice { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
