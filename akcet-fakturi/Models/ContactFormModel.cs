using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace akcet_fakturi.Models
{
    public class ContactFormModel
    {
        [Required]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Емайл")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Съобщение")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

    }
}