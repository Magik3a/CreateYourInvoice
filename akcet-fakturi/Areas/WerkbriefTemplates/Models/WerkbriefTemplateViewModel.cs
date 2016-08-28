using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace akcet_fakturi.Areas.WerkbriefTemplates.Models
{
    public class WerkbriefTemplateViewModel
    {
        [Display(Name = "Име на компанията")]
        public string UserCompanyName { get; set; }

        [Display(Name = "Телефон")]
        public string UserPhone { get; set; }

        [Display(Name = "Банкова Сметка")]
        public string UserBankAccount { get; set; }


        [Display(Name = "Период")]
        public string Period { get; set; }

        [Display(Name = "Булстат")]
        public string UserBulstat { get; set; }

        [Display(Name = "ДДС номе")]
        public string UserDDsNumber { get; set; }

        [Display(Name = "Адрес")]
        public string UserAddress { get; set; }


        public int CompanyID { get; set; }

        [Display(Name = "Име на компания")]
        public string CompanyName { get; set; }

        [Display(Name = "Адрес на компания")]
        public string CompanyAddress { get; set; }

        [Display(Name = "ДДС номер")]
        public string CompanyDDSNumber { get; set; }

        public List<akcetDB.WerkbriefHoursTemp> WerkbriefHoursTemps = new List<akcetDB.WerkbriefHoursTemp>();
    }
}