namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Fakturis = new HashSet<Fakturi>();
        }

        public int CompanyID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int IdAddress { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Company manager")]
        public string CompanyMol { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Vat number")]
        public string DdsNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string CompanyDescription { get; set; }

        [StringLength(500)]
        [Display(Name = "Email")]
        public string CompanyPhone { get; set; }

        [Display(Name = "Work in it")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fakturi> Fakturis { get; set; }
    }
}
