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
        [Display(Name = "Адрес")]
        public string StreetName { get; set; }


        [Display(Name = "Пощенски код")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Град")]
        public string City { get; set; }
        [Display(Name = "Дата на създаване")]
        public DateTime? DateCreated { get; set; }
        [Display(Name = "Дата на промяна")]
        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        [Display(Name = "Потребител")]
        public string UserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }
    }
}
