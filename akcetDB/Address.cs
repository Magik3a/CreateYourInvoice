using Tools.Attributes;

namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            Companies = new HashSet<Company>();
        }

        [Key]
        public int IdAddress { get; set; }

        [StringLength(128)]
        public string UserID { get; set; }

        [Required]
        [StringLength(500)]
        [MultiLanguageDisplayName("1026")]
        public string StreetName { get; set; }


        [MultiLanguageDisplayName("1048")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(500)]
        [MultiLanguageDisplayName("1049")]
        public string City { get; set; }

        [MultiLanguageDisplayName("1061")]
        public DateTime? DateCreated { get; set; }

        [MultiLanguageDisplayName("1062")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }
    }
}
