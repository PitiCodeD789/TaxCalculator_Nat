using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngine.Models
{
    public class ExemptionModel
    {
        public decimal TotalIncome { get; set; }
        public bool IsDisabled { get; set; }
        public int Age { get; set; }
        public decimal TeacherFund { get; set; }
        public decimal ProvFund { get; set; }
        public decimal GPF { get; set; }
        public decimal LeaveCompensate { get; set; }
    }
}
