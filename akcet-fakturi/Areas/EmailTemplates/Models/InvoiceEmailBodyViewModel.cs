using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace akcet_fakturi.Areas.EmailTemplates.Models
{
    public class InvoiceEmailBodyViewModel
    {
        public string FakturaNumber { get; set; }

        public string FakturaDate { get; set; }

        public string Total { get; set; }

        public string FakturaEndDate { get; set; }
    }
}