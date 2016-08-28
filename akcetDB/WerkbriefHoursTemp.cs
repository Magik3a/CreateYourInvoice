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

        [Required]
        [Display(Name = "Week")]
        public string Week { get; set; }


        public int ProjectID { get; set; }

        [Display(Name = "Ma")]
        public string Monday { get; set; }

        [Display(Name = "Di")]
        public string Tuesday { get; set; }

        [Display(Name = "Wo")]
        public string Wednesday { get; set; }

        [Display(Name = "Do")]
        public string Thursday { get; set; }

        [Display(Name = "Fr")]
        public string Friday { get; set; }

        [Display(Name = "Za")]
        public string Saturday { get; set; }

        [Display(Name = "Zo")]
        public string Sunday { get; set; }

        [Display(Name = "Total")]
        public string TotalHours { get; set; }
        public virtual Project Project { get; set; }

    }
}