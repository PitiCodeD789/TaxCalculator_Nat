using System;
using System.Reflection;
using TaxEngine.Models;

namespace TaxEngine.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is uTax - Tax Calculator");
            //TaxCalculatorCommand command = new TaxCalculatorCommand
            //{
            //    TotalIncome = 5000000,
            //    IsDisabled = false,
            //    Age = 30,
            //    TeacherFund = 100000,
            //    ProvFund = 100000,
            //    GPF = 100000,
            //    LeaveCompensate = 0,
            //    ApplyWithSpouse = false,
            //    IsMarried = false,
            //    IsSpouseEligible = false,
            //    EligibleChildBefore2561 = 1,
            //    EligibleAdoptedChild = 1,
            //    EligibleChildAfter2561 = 1,
            //    OwnParent = 2,
            //    SpouseParent = 0,
            //    HasNonFamilyDisable = false,
            //    InFamilyDisable = 0,
            //    ParentalInsurance = 15000,
            //    LifeInsurance = 70000,
            //    HealthInsurance = 15000,
            //    SavingFund = 13200,
            //    LTF = 400000,
            //    RMF = 200000,
            //    PensionInsurance = 20000,
            //    PoliticalParty = 10000,
            //    TravelExpense = 20000,
            //    SocialSecurity = 9000,
            //    HousingInterest = 100000,
            //    StartupInvestment = 100000,
            //    CctvExpense = 0,
            //    OtherExpense = 9000,
            //    EducationDonation = 0,
            //    OtherDonation = 0
            //};


            //TaxCalculatorCommand command = new TaxCalculatorCommand
            //{
            //    TotalIncome = 100000000,
            //    ProvFund = 600000,
            //    OwnParent = 2,
            //    InFamilyDisable = 1,
            //    LifeInsurance = 120000,
            //    HealthInsurance = 16000,
            //    LTF = 600000,
            //    RMF = 600000,
            //    TravelExpense = 60000,
            //    SocialSecurity = 9000,
            //    HousingInterest = 200000,
            //    StartupInvestment = 200000,
            //    OtherExpense = 40000,
            //    EducationDonation = 5000000,
            //    OtherDonation = 10000000,
            //    MaternityAllowance = 100000,
            //    RealEstatePrice = 4000000 // พังที่ Realestate ไม่ได้เช็ค เกิน 3000000
            //};

            TaxCalculatorCommand command = new TaxCalculatorCommand
            {
                TotalIncome = 100000000,
                //IsDisabled = false,
                //Age = 30,
                //TeacherFund = 100000,
                //ProvFund = 100000,
                //GPF = 100000,
                //LeaveCompensate = 0,
                //ApplyWithSpouse = false,
                //IsMarried = false,
                //IsSpouseEligible = false,
                //EligibleChildBefore2561 = 1,
                //EligibleAdoptedChild = 1,
                //EligibleChildAfter2561 = 1,
                //OwnParent = 2,
                //SpouseParent = 0,
                //HasNonFamilyDisable = false,
                //InFamilyDisable = 0,
                //ParentalInsurance = 15000,
                //LifeInsurance = 70000,
                //HealthInsurance = 15000,
                //SavingFund = 13200,
                //LTF = 400000,
                //RMF = 200000,
                //PensionInsurance = 20000,
                //PoliticalParty = 10000,
                //TravelExpense = 20000,
                //SocialSecurity = 9000,
                //HousingInterest = 100000,
                //StartupInvestment = 100000,
                //CctvExpense = 0,
                //OtherExpense = 9000,
                //EducationDonation = 0,
                //OtherDonation = 0
            };
            Console.WriteLine("สวัสดี");
            Engine engine = new Engine();
            var result = engine.TaxCalculator(command);

            int i = 1;
            Console.WriteLine($"A{i}: {result.A1:n}"); i++;
            Console.WriteLine($"A{i}: {result.A2:n}"); i++;
            Console.WriteLine($"A{i}: {result.A3:n}"); i++;
            Console.WriteLine($"A{i}: {result.A4:n}"); i++;
            Console.WriteLine($"A{i}: {result.A5:n}"); i++;
            Console.WriteLine($"A{i}: {result.A6:n}"); i++;
            Console.WriteLine($"A{i}: {result.A7:n}"); i++;
            Console.WriteLine($"A{i}: {result.A8:n}"); i++;
            Console.WriteLine($"A{i}: {result.A9:n}"); i++;
            Console.WriteLine($"A{i}: {result.A10:n}"); i++;
            Console.WriteLine($"A{i}: {result.A11:n}"); i++;
            Console.WriteLine($"A{i}: {result.A12:n}"); i++;

        }
    }
}
