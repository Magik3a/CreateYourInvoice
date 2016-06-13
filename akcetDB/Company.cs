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
        public string CompanyName { get; set; }

        [Required]
        [StringLength(500)]
        public string CompanyMol { get; set; }

        [Required]
        [StringLength(500)]
        public string CompanyBulsatat { get; set; }

        [StringLength(500)]
        public string CompanyDescription { get; set; }

        [StringLength(500)]
        public string CompanyPhone { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fakturi> Fakturis { get; set; }
    }
}
