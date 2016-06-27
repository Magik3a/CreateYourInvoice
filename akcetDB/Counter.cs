using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace akcetDB
{
    [Table("InvoiceCounter")]
    public class Counter
    {
        [Key]
        public int CounterId { get; set; }

        public string Year { get; set; }

        public int CounterValue { get; set; }
    }
}