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
        
        [Display(Name = "Дата на фактурата")]
        public DateTime InvoiceDate { get; set; }


        [Display(Name = "Крайна дата")]
        public DateTime InvoiceEndDate { get; set; }

        [Required]
        [Display(Name = "Проект")]
        public string Project { get; set; }

        [Display(Name = "Крайна цена")]
        public int TotalPrice { get; set; }

        [Display(Name = "Дата на създаване")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Дата на промяна")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [Display(Name = "Потребител")]
        public string UserName { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
