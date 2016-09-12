namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DDS")]
    public partial class DD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DD()
        {
            ProductInvoices = new HashSet<ProductInvoice>();
        }

        [Key]
        public int DdsID { get; set; }

        [Display(Name = "Name")]
        public string DdsName { get; set; }


        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Zero value?")]
        public bool IsNullValue { get; set; }

        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInvoice> ProductInvoices { get; set; }
    }
}
