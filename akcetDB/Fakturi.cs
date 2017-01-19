using Tools.Attributes;

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

        [StringLength(128)]
        public string UserID { get; set; }

        public int CompanyID { get; set; }

        [MultiLanguageDisplayName("1005")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [MultiLanguageDisplayName("1006")]
        public DateTime InvoiceEndDate { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1007")]
        public string Period { get; set; }

        [MultiLanguageDisplayName("1013")]
        public decimal TotalPrice { get; set; }

        [MultiLanguageDisplayName("1061")]
        public DateTime? DateCreated { get; set; }

        [MultiLanguageDisplayName("1062")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1")]
        public string UserName { get; set; }

        [MultiLanguageDisplayName("1060")]
        public string FakturaNumber { get; set; }

        [MultiLanguageDisplayName("1019")]
        public string FakturaHtml { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
