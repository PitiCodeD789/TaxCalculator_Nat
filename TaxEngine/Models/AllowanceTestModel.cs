using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngine.Models
{
    public class AllowanceTestModel : TaxCalculatorCommand
    {
        public decimal ExpectedResult { get; set; }
    }
}
