using Tools.Attributes;

namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WerkbriefHours")]
    public partial class WerkbriefHours
    {
        [Key]
        public int WerkbriefHoursID { get; set; }

        public int WerkbriefID { get; set; }

        [Required]
        [MultiLanguageDisplayName("1030")]
        public string Week { get; set; }

        public int ProductID { get; set; }

        public int ProjectID { get; set; }


        [MultiLanguageDisplayName("1031")]
        public string Monday { get; set; }

        [MultiLanguageDisplayName("1032")]
        public string Tuesday { get; set; }

        [MultiLanguageDisplayName("1033")]
        public string Wednesday { get; set; }

        [MultiLanguageDisplayName("1034")]
        public string Thursday { get; set; }

        [MultiLanguageDisplayName("1035")]
        public string Friday { get; set; }

        [MultiLanguageDisplayName("1036")]
        public string Saturday { get; set; }

        [MultiLanguageDisplayName("1037")]
        public string Sunday { get; set; }

        [MultiLanguageDisplayName("1038")]
        public string TotalHours { get; set; }
        public virtual Project Project { get; set; }

    }
}
