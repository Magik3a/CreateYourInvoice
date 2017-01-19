using Tools.Attributes;

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
        [MultiLanguageDisplayName("1044")]
        public string CompanyName { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1045")]
        public string CompanyMol { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1046")]
        public string DdsNumber { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("1008")]
        public string CompanyDescription { get; set; }

        [StringLength(500)]
        [MultiLanguageDisplayName("22")]
        public string CompanyPhone { get; set; }

        [MultiLanguageDisplayName("1047")]
        public bool IsPrimary { get; set; }

        [MultiLanguageDisplayName("1061")]
        public DateTime? DateCreated { get; set; }

        [MultiLanguageDisplayName("1062")]
        public DateTime? DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Address Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fakturi> Fakturis { get; set; }
    }
}
