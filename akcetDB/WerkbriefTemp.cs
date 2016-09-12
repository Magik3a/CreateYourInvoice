namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WerkbriefTemp")]
    public partial class WerkbriefTemp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WerkbriefTemp()
        {
            WerkbriefHoursTemps = new HashSet<WerkbriefHoursTemp>();
        }

        [Key]
        public int WerkbriefIDTemp { get; set; }

        public int? CompanyID { get; set; }

        [Display(Name = "Date created")]
        public string WerkbriefDate { get; set; }

        [Display(Name = "Payment end day")]
        public string WerkbriefEndDate { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        public int? TotalHours { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }


        [StringLength(128)]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WerkbriefHoursTemp> WerkbriefHoursTemps { get; set; }
    }
}
