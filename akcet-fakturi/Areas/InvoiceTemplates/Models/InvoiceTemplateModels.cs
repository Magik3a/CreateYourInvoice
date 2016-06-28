using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using akcetDB;

namespace akcet_fakturi.Areas.InvoiceTemplates.Models
{
    public class InvoiceTemplateModels
    {
        [Display(Name = "Име на компанията")]
        public string UserCompanyName { get; set; }

        [Display(Name = "Телефон")]
        public string UserPhone { get; set; }

        [Display(Name = "Банкова Сметка")]
        public string UserBankAccount { get; set; }

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


        [Display(Name = "Общо без ддс")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Decimal TotalWithoutDDS { get; set; }


        [Display(Name = "Общо с ддс")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Decimal TotalWithDDS { get; set; }


        [Display(Name = "Общо ддс")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Decimal TotalDDS { get; set; }

        //ддс
        public List<akcetDB.DD> ListDds { get; set; }
        
        //ддс


        [Display(Name = "Дата на издаване")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Дата на падеж")]
        public string InvoiceEndDate { get; set; }

        [Display(Name = "Период")]
        public string Period { get; set; }

        [Display(Name = "Фактура номер:")]
        public string InvoiceNumber { get; set; }

        public List<ProductInvoiceTemp> ProductsListTemp = new List<ProductInvoiceTemp>();
    }
}