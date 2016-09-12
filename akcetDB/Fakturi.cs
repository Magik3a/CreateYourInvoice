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

        [Display(Name = "Invoice date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Invoice end date")]
        public DateTime InvoiceEndDate { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "Total price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Invoice number")]
        public string FakturaNumber { get; set; }

        [Display(Name = "Content")]
        public string FakturaHtml { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
