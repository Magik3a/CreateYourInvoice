namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductInvoices = new HashSet<ProductInvoice>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(500)]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Date modified")]
        public DateTime DateModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
