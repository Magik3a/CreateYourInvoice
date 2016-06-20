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

        public int? ProductInvoiceID { get; set; }

        [Display(Name = "Дата на издаване")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Дата на падеж")]
        public DateTime? InvoiceEndDate { get; set; }
        
        [Display(Name = "Проект")]
        public string Project { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoiceTemp> ProductInvoiceTemps { get; set; }
    }
}
