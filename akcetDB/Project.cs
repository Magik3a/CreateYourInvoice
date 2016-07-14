namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            ProductInvoices = new HashSet<ProductInvoice>();
            ProductInvoiceTemps = new HashSet<ProductInvoiceTemp>();
        }

        public int ProjectID { get; set; }

        [StringLength(128)]
        public string UserID { get; set; }

        [StringLength(500)]
        public string ProjectName { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public bool IsDeleted { get; set; }
        [StringLength(500)]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoiceTemp> ProductInvoiceTemps { get; set; }
    }
}
