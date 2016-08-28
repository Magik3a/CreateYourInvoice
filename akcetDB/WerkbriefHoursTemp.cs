namespace akcetDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WerkbriefHoursTemp")]
    public partial class WerkbriefHoursTemp
    {
        public int WerkbriefHoursIDTemp { get; set; }

        public int WerkbriefIDTemp { get; set; }

        public string Week { get; set; }


        public int ProjectID { get; set; }

        public int Monday { get; set; }

        public int Tuesday { get; set; }

        public int Wednesday { get; set; }

        public int Thursday { get; set; }

        public int Friday { get; set; }

        public int Saturday { get; set; }

        public int Sunday { get; set; }

        public int TotalHours { get; set; }
        public virtual Project Project { get; set; }

    }
}