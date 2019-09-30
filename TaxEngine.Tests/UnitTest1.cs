using NUnit.Framework;
using TaxEngine;
using TaxEngine.Models;

namespace Tests
{
    public class MyTests
    {
        Engine engine;


        [SetUp] //Initialize before ea tests is run
        public void Setup()
        {
            engine = new TaxEngine.Engine();
            ExemptionModel exemptionModel = new ExemptionModel() { Age = 50, GPF = 500000, IsDisabled = true, LeaveCompensate = 300000, ProvFund = 500000, TeacherFund = 500000, TotalIncome = 500000 };
        }


        static AllowanceTestModel[] AllowanceModelList =
        {
            new AllowanceTestModel
            {
                TotalIncome  = 5000000,
                ApplyWithSpouse  = false,
                IsMarried  = false ,
                IsSpouseEligible  = false,
                EligibleChildBefore2561 = 1,
                EligibleAdoptedChild = 1,
                EligibleChildAfter2561 = 1,
                OwnParent = 2,
                SpouseParent = 0,
                HasNonFamilyDisable = false,
                InFamilyDisable = 0,
                ParentalInsurance = 15000,
                LifeInsurance = 50000,
                HealthInsurance = 15000,
                ProvFund = 10000,
                SavingFund = 13200,
                LTF = 400000,
                RMF = 100000,
                PensionInsurance = 50000,
                GPF = 100000,
                TeacherFund = 100000,
                PoliticalParty = 10000,
                TravelExpense = 20000,
                SocialSecurity = 9000,
                HousingInterest = 100000,
                StartupInvestment = 100000,
                CctvExpense = 0,
                OtherExpense = 9000,
                ExpectedResult = 1141200
            },
        };


        [TestCase(100,50,50)]
        public void Test_Engine_GetTax(decimal m1, decimal m2, decimal expected)
        {
            // Arrange

            // Act
            var result = engine.GetTax(m1, m2);

            // Assert
            Assert.AreEqual(expected,result,$"Expected {expected} but return {result}");
        }


        [TestCase(200000,2500)]
        [TestCase(300000,7500)]
        [TestCase(500000,27500)]
        [TestCase(719000,60350)]
        [TestCase(377000,15200)]
        public void Test_Engine_StairCaseTax(decimal netIncome, decimal expectedTax)
        {
            // Act
            var result = engine.StairCaseTax(netIncome);

            // Assert
            Assert.AreEqual(expectedTax,result);
        }


        [TestCase(30,0,false, 0, 0, 0, 0,0)] // Zeroed out
        [TestCase(50,5000000,true, 3000000, 5000000, 5000000, 5000000,990000)] // Everything Excess
        [TestCase(50,500000,true, 300000, 500000, 500000, 500000,990000)] // Everything maxed out
        [TestCase(30,1000000,false, 0, 0, 0, 0, 500000)] // Excess GPF
        [TestCase(30,5000,false, 0, 0, 0, 0, 5000)] // Normal GPF
        [TestCase(30,0,true, 0, 0, 0, 0,190000)] // Is Disabled
        [TestCase(65,0,false, 0, 0, 0, 0,190000)] // Is Over-Age
        [TestCase(30,0,false, 0, 500000, 0, 1000, 150)] // Max ProvFund only 15% of total income
        [TestCase(30,5000,false, 0, 500000, 0, 1000, 5150)] // Max ProvFund only 15% of total income + GPF
        [TestCase(30,0,false, 0, 500000, 0, 1000, 150)]
        [TestCase(30, 0, false, 300, 500000, 0, 1000, 450)] // LeaveCompensate + MaxProvFund at 15% of Income
        public void Test_Engine_GetExemption(
            int age, decimal gPF, bool isDisabled, decimal leaveCompensate, decimal provFund, decimal teacherFund, decimal totalIncome, decimal expectedResult)
        {
            TaxCalculatorCommand exemption = new TaxCalculatorCommand() { Age = age, GPF = gPF, IsDisabled = isDisabled, LeaveCompensate = leaveCompensate, ProvFund = provFund, TeacherFund = teacherFund, TotalIncome = totalIncome };
            var result = engine.GetExemption(exemption);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(100000,50000)] // Under limit
        [TestCase(1000000,100000)] // Over Limitation
        public void Test_Engine_GetPersonalExpenseExemption(decimal totalIncome, decimal expectedResult)
        {
            var result = engine.GetPersonalExpenseExemption(totalIncome);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(1,1,1,120000)] // Normal
        [TestCase(1,1,3,120000)] // Excess Adopted w/ real child
        [TestCase(0,0,5,90000)] // Excess Adopted
        [TestCase(3,3,0,270000)] // All Real
        [TestCase(0,3,0,150000)] // All > 2561 Child
        [TestCase(0,0,3,90000)] // All Adopt
        [TestCase(3,0,0,90000)] // All < 2561 Child
        [TestCase(0,0,0,0)] // No Child
        public void Test_Engine_ChildAllowance(int eligibleChildBefore2561, int eligibleChildAfter2561, int eligibleAdoptedChild, decimal expectedResult)
        {
            var result = engine.ChildAllowance(eligibleChildBefore2561, eligibleChildAfter2561, eligibleAdoptedChild);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(true,2,2,120000)] // Maxed out
        [TestCase(true,3,4,120000)] // Excess value
        [TestCase(false,2,2,60000)] // Not Married
        [TestCase(true,2,0,60000)] // Married only ownParent
        [TestCase(true,0,2,60000)] // Married no ownParent
        [TestCase(false,0,2,0)] // not Married no ownParent
        public void Test_Engine_ParentalCareAllowance(bool isMarried, int ownParent, int spouseParent, decimal expectedResult)
        {
            var result = engine.ParentalCareAllowance(isMarried,ownParent,spouseParent);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(true,3,240000)]
        [TestCase(true,0,60000)]
        [TestCase(false,3,180000)]
        [TestCase(false,1,60000)]
        [TestCase(false,0,0)]
        public void Test_Engine_DisabledCareAllowance(bool hasNonFamilyDisabled, int inFamily, decimal expectedResult)
        {
            var result = engine.DisabledCareAllowance(hasNonFamilyDisabled, inFamily);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(15000, 85000, 15000, 115000)] // Maxed out
        [TestCase(0, 100000, 15000, 100000)] // Excess life insurance
        [TestCase(15000, 15000, 15000, 45000)] // Normal
        [TestCase(0,0,0,0)] // No insurance
        public void Test_Engine_InsuranceAllowance(decimal parentalInsurance, decimal lifeInsurance, decimal healthInsurance, decimal expectedResult)
        {
            var result = engine.InsuranceAllowance(parentalInsurance, lifeInsurance, healthInsurance);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(50000,10000)]
        [TestCase(5000,5000)]
        [TestCase(0,0)]
        public void Test_Engine_ProvFundAllowance(decimal provFund, decimal expectedResult)
        {
            var result = engine.ProvFundAllowance(provFund);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(20000, 13200)]
        [TestCase(10000, 10000)]
        [TestCase(0,0)]
        public void Test_Engine_SavingFund(decimal savingFund, decimal expectedResult)
        {
            var result = engine.SavingFundAllowance(savingFund);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(500000,10000000,500000)] // Maxed out
        [TestCase(500000,1000000,150000)] // At 15% 
        [TestCase(5000000,10000000,500000)] // 15% Exceed 500,000
        [TestCase(0, 0, 0)] // Zeroed out
        public void Test_Engine_LTFAllowance(decimal lTF,decimal totalIncomeA5, decimal expectedResult)
        {
            var result = engine.LTFAllowance(lTF,totalIncomeA5);
            Assert.AreEqual(expectedResult, result);
        }

        //[TestCase(500000, 500000, 500000, 500000, 500000, 10000000, 0)] // Other Invest Exceed 500,000
        //[TestCase(500000, 100000, 100000, 100000, 500000, 10000000, 200000)] // Capped by Other Invest (300,000)
        //[TestCase(500000, 0, 0, 0, 0, 100000, 15000)] // 15% Cap RMF
        //[TestCase(0, 0, 0, 0, 5000000, 100000, 15000)] // 500000 Cap TeacherFund
        //[TestCase(5000, 1000, 1000, 1000, 1000, 100000, 6000)] // Min RMF + Pension
        //[TestCase(0, 0, 0, 0, 0, 0, 0)] // Zeroed out
        //public void Test_Engine_InvestmentPackageAllowance(decimal rMF, decimal gPF, decimal provFund, decimal teacherFund, decimal pensionInsurance, decimal totalIncomeA5, decimal expectedResult)
        //{
        //    var result = engine.InvestmentPackageAllowance(rMF, gPF, provFund, teacherFund, pensionInsurance, totalIncomeA5);
        //    Assert.AreEqual(expectedResult, result);
        //}


        [TestCase(200000,100000)]
        [TestCase(50000,50000)]
        [TestCase(0,0)]
        public void Test_Engine_HousingLoanInterestAllowance(decimal interest, decimal expectedResult)
        {
            var result = engine.HousingLoanInterestAllowance(interest);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(1000000,90000,150000,1000000,200,100000,150000,254400)] // Exceed limit
        [TestCase(100000,9000,20000,100000,200,10000,15000, 254400)] // At limit
        [TestCase(50000,4500,5000,50000,200,5000,5000,119900)] 
        [TestCase(500000, 0,0,0,0,0,0,100000)] // housing
        [TestCase(0, 500000, 0,0,0,0,0,9000)] // ss
        [TestCase(0,0, 500000, 0,0,0,0,20000)] // travel
        [TestCase(0,0,0, 500000, 0,0,0,100000)] // startup
        [TestCase(0,0,0,0, 500000, 0,0, 1000000)] // cctv
        [TestCase(0,0,0,0,0, 500000, 0,10000)] // political
        [TestCase(0,0,0,0,0,0, 500000, 15000)] // other
        [TestCase(0,0,0,0,0,0,0,0)] // At minimum
        public void Test_Engine_OtherExpense(decimal housingInterest, decimal socialSecurity, decimal travelExpense, decimal startupInvestment, decimal cctvExpense, decimal politicalParty, decimal otherExpense, decimal expectedResult)
        {
            var result = engine.OtherExpense(housingInterest, socialSecurity, travelExpense, startupInvestment, cctvExpense, politicalParty, otherExpense);
            Assert.AreEqual(expectedResult, result);
        }


        [TestCase(20000, 20000, 100000, 19000)] // Maxed out
        [TestCase(2500, 5000, 100000, 10000)] // 
        [TestCase(0, 0, 0, 0)] // Zeroed out
        public void Test_Engine_Donation(decimal educationDonation, decimal otherDonation, decimal totalIncomeA7, decimal expectedResult)
        {
            var A8 = engine.EducationDonation(educationDonation, totalIncomeA7);
            var A9 = totalIncomeA7 - A8;
            var A10 = engine.OtherDonation(otherDonation, A9);
            var result = A8 + A10;
            Assert.AreEqual(expectedResult, result);
        }


        [Test]
        [TestCaseSource("AllowanceModelList")]
        public void Test_Engine_GetAllowance(AllowanceTestModel model)
        {
            var result = engine.GetAllowance(model);
            Assert.AreEqual(model.ExpectedResult, result);
        }

        //[TestCase(5000000,200000)]
        //[TestCase(1000000,200000)]
        //[TestCase(10000000,0)]
        //[TestCase(0,0)]
        //public void Test_Engine_RealEstateAllowance(decimal realEstatePrice, decimal expectedResult)
        //{
        //    var result = engine.RealEstateAllowance(realEstatePrice);
        //    Assert.AreEqual(expectedResult, result);
        //}

        static TaxCalculatorCommand[] CommandList =
{
            new TaxCalculatorCommand
            {
                TotalIncome = 5000000,
                IsDisabled = false,
                Age = 30,
                TeacherFund = 100000,
                ProvFund = 100000,
                GPF = 100000,
                LeaveCompensate = 0,
                ApplyWithSpouse = false,
                IsMarried = false,
                IsSpouseEligible = false,
                EligibleChildBefore2561 = 1,
                EligibleAdoptedChild = 1,
                EligibleChildAfter2561 = 1,
                OwnParent = 2,
                SpouseParent = 0,
                HasNonFamilyDisable = false,
                InFamilyDisable = 0,
                ParentalInsurance = 15000,
                LifeInsurance = 50000,
                HealthInsurance = 15000,
                SavingFund = 13200,
                LTF = 400000,
                RMF = 300000,
                PensionInsurance = 50000,
                PoliticalParty = 10000,
                TravelExpense = 20000,
                SocialSecurity = 9000,
                HousingInterest = 100000,
                StartupInvestment = 100000,
                CctvExpense = 0,
                OtherExpense = 9000,
                EducationDonation = 0,
                OtherDonation = 0,
                ExpectedResult = 784100m,
            },
            new TaxCalculatorCommand
            {
                TotalIncome = 5000000,
                IsDisabled = false,
                Age = 30,
                TeacherFund = 100000,
                ProvFund = 100000,
                GPF = 100000,
                LeaveCompensate = 0,
                ApplyWithSpouse = false,
                IsMarried = false,
                IsSpouseEligible = false,
                EligibleChildBefore2561 = 1,
                EligibleAdoptedChild = 1,
                EligibleChildAfter2561 = 1,
                OwnParent = 2,
                SpouseParent = 0,
                HasNonFamilyDisable = false,
                InFamilyDisable = 0,
                ParentalInsurance = 15000,
                LifeInsurance = 50000,
                HealthInsurance = 15000,
                SavingFund = 13200,
                LTF = 400000,
                RMF = 100000,
                PensionInsurance = 50000,
                PoliticalParty = 10000,
                TravelExpense = 20000,
                SocialSecurity = 9000,
                HousingInterest = 100000,
                StartupInvestment = 100000,
                CctvExpense = 0,
                OtherExpense = 9000,
                EducationDonation = 0,
                OtherDonation = 0,
                ExpectedResult = 805640
            }
        };

        [Test]
        [TestCaseSource("CommandList")]
        public void Test_Engine_OverallTest(TaxCalculatorCommand model)
        {
            var result = engine.TaxCalculator(model);
            Assert.AreEqual(model.ExpectedResult, result.A12);
        }
    }
}