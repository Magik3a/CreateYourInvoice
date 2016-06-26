namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FakturiTemp")]
    public partial class FakturiTemp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FakturiTemp()
        {
            ProductInvoiceTemps = new HashSet<ProductInvoiceTemp>();
        }

        [Key]
        public int InvoiceIDTemp { get; set; }

        public int? CompanyID { get; set; }

        [Display(Name = "Дата на издаване")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Дата на падеж")]
        public string InvoiceEndDate { get; set; }

        public int? TotalPrice { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        [Display(Name = "Период")]
        public string Period { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoiceTemp> ProductInvoiceTemps { get; set; }
    }
}
