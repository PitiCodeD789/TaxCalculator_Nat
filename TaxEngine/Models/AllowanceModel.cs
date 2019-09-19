using System;
using System.Collections.Generic;
using System.Text;

namespace TaxEngine.Models
{
    public class AllowanceModel
    {
        public decimal TotalIncomeA5 { get; set; }
        public bool ApplyWithSpouse { get; set; }
        public bool IsMarried { get; set; }
        public bool IsSpouseEligible { get; set; }
        public int EligibleChildBefore2561 { get; set; }
        public int EligibleAdoptedChild { get; set; }
        public int EligibleChildAfter2561 { get; set; }
        public int OwnParent { get; set; }
        public int SpouseParent { get; set; }
        public bool HasNonFamilyDisable { get; set; }
        public int InFamilyDisable { get; set; }
        public decimal ParentalInsurance { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal HealthInsurance { get; set; }
        public decimal ProvFund { get; set; }
        public decimal SavingFund { get; set; }
        public decimal LTF { get; set; }
        public decimal RMF { get; set; }
        public decimal PensionInsurance { get; set; }
        public decimal GPF { get; set; }
        public decimal TeacherFund { get; set; }
        public decimal PoliticalParty { get; set; }
        public decimal TravelExpense { get; set; }
        public decimal SocialSecurity { get; set; }
        public decimal HousingInterest { get; set; }
        public decimal StartupInvestment { get; set; }
        public decimal CctvExpense { get; set; }
        public decimal OtherExpense { get; set; }
    }
}
