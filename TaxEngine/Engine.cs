using System;
using System.Collections.Generic;
using System.Text;
using TaxEngine.Models;

namespace TaxEngine
{
    public class Engine
    {
        public TaxResultModel TaxCalculator(TaxCalculatorCommand inputCommand)
        {
            TaxCalculatorCommand command = inputCommand;
            TaxResultModel result = new TaxResultModel();
            result.A1 = command.TotalIncome;
            result.A2 = GetExemption(command);
            result.A3 = result.A1 - result.A2;
            result.A4 = GetPersonalExpenseExemption(command.TotalIncome);
            result.A5 = result.A3 - result.A4;
            result.A6 = GetAllowance(command);
            result.A7 = result.A5 - result.A6;
            result.A8 = EducationDonation(command.EducationDonation, result.A7);
            result.A9 = result.A7 - result.A8;
            result.A10 = OtherDonation(command.OtherDonation,result.A9);
            result.A11 = result.A9 - result.A10;
            result.A12 = StairCaseTax(result.A11);
            return result;
        }


        public decimal GetTax(decimal money1,decimal money2)
        {
            return money1-money2;
        }

        public decimal GetExemption(TaxCalculatorCommand exem)
        {
            TaxCalculatorCommand exemption = exem;
            decimal exempt = 0;
            exemption.TeacherFund = SetLimit(exemption.TeacherFund, 500000);
            exempt += exemption.TeacherFund;

            decimal provFund = 0;
            if (exemption.ProvFund > 10000)
            {
                if (exemption.ProvFund > 500000)
                    provFund = 490000;
                else
                    provFund = exemption.ProvFund - 10000;

                provFund = SetLimit(provFund, exemption.TotalIncome * 0.15m);

                exempt += provFund;
            }

            exemption.GPF = SetLimit(exemption.GPF, 500000);
            exempt += exemption.GPF;

            exempt = SetLimit(exempt, 500000);

            if (exemption.IsDisabled || exemption.Age >= 65)
                exempt += 190000m;

            exemption.LeaveCompensate = SetLimit(exemption.LeaveCompensate, 300000);
            exempt += exemption.LeaveCompensate;

            return exempt;
        }

        public decimal StairCaseTax(decimal netIncome)
        {
            var level7 = 5000000m;
            var level6 = 2000000m;
            var level5 = 1000000m;
            var level4 = 750000m;
            var level3 = 500000m;
            var level2 = 300000m;
            var level1 = 150000m;

            decimal[] staircase = new decimal[] { level7, level6, level5, level4, level3, level2, level1, 0};

            decimal tax = 0;
            decimal percentTax = 0.35m;
            int currentLevel = 0;
            while(netIncome > level1)
            {
                if (netIncome > staircase[currentLevel])
                {
                    tax += (netIncome - staircase[currentLevel]) * percentTax;
                    netIncome = staircase[currentLevel];
                }
                percentTax -= 0.05m;
                currentLevel++;
            }
            return tax;
        }

        public decimal GetPersonalExpenseExemption(decimal totalIncome)
        {
            decimal maxPersonalExpenseExemption = 100000m;
            if (totalIncome >= 200000)
            {
                return maxPersonalExpenseExemption;
            }
            else
            {
                return totalIncome / 2;
            }
        }

        public decimal GetAllowance(TaxCalculatorCommand model)
        {
            decimal allowance = 0;
            //1
            allowance += PersonlAllowance(model.ApplyWithSpouse);
            //2
            if (!model.ApplyWithSpouse)
                allowance += SpouseAllowance(model.IsMarried, model.IsSpouseEligible);
            //3
            allowance += ChildAllowance(model.EligibleChildBefore2561, model.EligibleChildAfter2561, model.EligibleAdoptedChild);
            //4
            allowance += ParentalCareAllowance(model.IsMarried, model.OwnParent, model.SpouseParent);
            //5
            allowance += DisabledCareAllowance(model.HasNonFamilyDisable, model.InFamilyDisable);
            //6, 7.1, 7.2
            allowance += InsuranceAllowance(model.ParentalInsurance, model.LifeInsurance, model.HealthInsurance);
            //8
            allowance += ProvFundAllowance(model.ProvFund);
            //9
            allowance += SavingFundAllowance(model.SavingFund);
            //11
            allowance += LTFAllowance(model.LTF, model.TotalIncome);
            //10
            allowance += InvestmentPackageAllowance(model.RMF, model.GPF, model.ProvFund,model.SavingFund, model.TeacherFund, model.PensionInsurance, model.TotalIncome);
            //13
            allowance += RealEstateAllowance(model.RealEstatePrice);
            //19
            allowance += model.MaternityAllowance;
            //12, 14, 17, 18, 15, 20, 21
            allowance += OtherExpense(model.HousingInterest, model.SocialSecurity, model.TravelExpense, model.StartupInvestment, model.CctvExpense, model.PoliticalParty, model.OtherExpense);
            return allowance;
        }


        //---------------------------------------Allowance---------------------------------------//
        
        //1
        public decimal PersonlAllowance(bool ApplyWithSpouse)
        {
            decimal allowance = 60000;
            if (ApplyWithSpouse)
                allowance += 60000;
            return allowance;
        }

        //2
        public decimal SpouseAllowance(bool isMarried, bool isSpouseEligible)
        {
            if (!isMarried)
            {
                return 0;
            }
            if (!isSpouseEligible)
            {
                return 0;
            }
            return 60000;
        }

        //3
        public decimal ChildAllowance(int eligibleChildBefore2561, int eligibleChildAfter2561, int eligibleAdoptedChild)
        {
            decimal allowance = 0;
            int totalChild = eligibleChildBefore2561 + eligibleChildAfter2561;

            if (totalChild >= 3)
                eligibleAdoptedChild = 0;
            else if (totalChild < 3 && eligibleAdoptedChild != 0)
                eligibleAdoptedChild = 3 - totalChild;

            if (totalChild == 0)
                allowance += eligibleAdoptedChild * 30000;
            else if (eligibleChildBefore2561>0)
                allowance = eligibleChildBefore2561 * 30000 + eligibleChildAfter2561 * 60000 + eligibleAdoptedChild * 30000;
            else
            {
                allowance = 30000; // FirstChild
                allowance += (eligibleChildAfter2561 - 1) * 60000 + eligibleAdoptedChild * 30000;
            }

            return allowance;
        }

        //4
        public decimal ParentalCareAllowance(bool isMarried, int ownParent, int spouseParent)
        {
            ownParent = (ownParent > 2) ? 2 : ownParent;
            spouseParent = (spouseParent > 2) ? 2 : spouseParent;

            decimal allowancePerParent = 30000m;

            if (!isMarried)
                return allowancePerParent * ownParent;

            int totalParent = ownParent + spouseParent;

            return allowancePerParent * totalParent;
        }

        //5
        public decimal DisabledCareAllowance(bool hasNonFamilyDisabled, int inFamily)
        {
            decimal allowance = inFamily * 60000m;
            if (hasNonFamilyDisabled)
                allowance += 60000m;

            return allowance;
        }

        //6, 7.1, 7.2
        public decimal InsuranceAllowance(decimal parentalInsurance, decimal lifeInsurance, decimal healthInsurance)
        {
            decimal allowance = 0;
            allowance += SetLimit(parentalInsurance,15000);
            healthInsurance = SetLimit(healthInsurance, 15000);
            allowance += healthInsurance;

            if ((healthInsurance + lifeInsurance) > 100000)
                allowance += 100000 - healthInsurance;
            else
                allowance += lifeInsurance;

            return allowance;
        }

        //8
        public decimal ProvFundAllowance(decimal provFund)
        {
            decimal lowerLimit = 10000;
            if (provFund > lowerLimit)
            {
                return lowerLimit;
            }
            return provFund;
        }

        //9
        public decimal SavingFundAllowance(decimal savingFund)
        {
            return SetLimit(savingFund, 13200);
        }

        //11
        public decimal LTFAllowance(decimal lTF, decimal totalIncomeA5)
        {
            decimal lTFLimitation = 500000;
            lTF = SetLimit(lTF, totalIncomeA5 * 0.15m);
            lTF = SetLimit(lTF, lTFLimitation);
            return lTF;
        }

        //10, 7.3
        public decimal InvestmentPackageAllowance(decimal rMF, decimal gPF, decimal provFund, decimal savingFund, decimal teacherFund, decimal pensionInsurance, decimal totalIncomeA5)
        {
            if (rMF < totalIncomeA5 * 0.03m && rMF < 5000)
                rMF = 0;


            decimal totalInvestLimitation = 500000;
            decimal totalInvest = gPF + provFund + teacherFund + savingFund;

            if (totalInvest >= totalInvestLimitation)
                return 0;

            decimal leftover = totalInvestLimitation - totalInvest;
            decimal rMFAndPension = rMF + pensionInsurance;
            decimal allowance = SetLimit(rMFAndPension, leftover);
            allowance = SetLimit(allowance, totalIncomeA5 * 0.15m);
            return allowance;

            //if (rMF < totalIncomeA5*0.03m && rMF < 5000 )
            //{
            //    rMF = 0;
            //}
            //decimal totalInvestLimitation = 500000;
            //decimal totalInvest = rMF + gPF + provFund + teacherFund + pensionInsurance;
            //totalInvest = SetLimit(totalInvest, totalInvestLimitation);
            //totalInvest = SetLimit(totalInvest, totalIncomeA5 * 0.15m);
            //return totalInvest;
        }

        public decimal RealEstateAllowance(decimal realEstatePrice)
        {
            if (realEstatePrice > 5000000 || realEstatePrice < 200000)
                return 0;

            return 200000;
        }

        //12, 14, 15, 17, 18,20, 21
        public decimal OtherExpense(decimal housingInterest, decimal socialSecurity, decimal travelExpense, decimal startupInvestment, decimal cctvExpense, decimal politicalParty, decimal otherExpense)
        {
            decimal otherAllowance = 0;
            otherAllowance += HousingLoanInterestAllowance(housingInterest);
            otherAllowance += SocialSecurityAllowance(socialSecurity);
            otherAllowance += travelAllowance(travelExpense);
            otherAllowance += StartupInvestmentAllowance(startupInvestment);
            otherAllowance += cctvAllowance(cctvExpense);
            otherAllowance += PoliticalPartyAllowance(politicalParty);
            otherAllowance += ProductsAndServicesExpenseAllowance(otherExpense);
            return otherAllowance;
        }

        #region OtherExpense
        public decimal HousingLoanInterestAllowance(decimal housingInterest)
        {
            decimal housingInterestLimit = 100000;
            housingInterest = SetLimit(housingInterest, housingInterestLimit);
            return housingInterest;
        }

        public decimal SocialSecurityAllowance(decimal socialSecurity)
        {
            decimal socialSecurityLimit = 9000;
            socialSecurity = SetLimit(socialSecurity, socialSecurityLimit);
            return socialSecurity;
        }

        public decimal travelAllowance(decimal travelExpense)
        {
            decimal travelExpenseLimit = 20000;
            travelExpense = SetLimit(travelExpense, travelExpenseLimit);
            return travelExpense;
        }

        public decimal StartupInvestmentAllowance(decimal startupInvestment)
        {
            decimal startupInvestmentLimit = 100000;
            startupInvestment = SetLimit(startupInvestment, startupInvestmentLimit);
            return startupInvestment;
        }

        public decimal cctvAllowance(decimal cctvExpense)
        {
            cctvExpense = cctvExpense * 2;
            return cctvExpense;
        }

        public decimal PoliticalPartyAllowance(decimal politicalParty)
        {
            decimal politicalPartyLimit = 10000;
            politicalParty = SetLimit(politicalParty, politicalPartyLimit);
            return politicalParty;
        }

        public decimal ProductsAndServicesExpenseAllowance(decimal otherExpense)
        {
            decimal expenseLimit = 15000;
            otherExpense = SetLimit(otherExpense, expenseLimit);
            return otherExpense;
        }
        #endregion 

        public decimal EducationDonation(decimal educationDonation, decimal totalIncomeA7)
        {
            educationDonation = educationDonation * 2;
            educationDonation = SetLimit(educationDonation, totalIncomeA7 * 0.1m);
            return educationDonation;
        }

        public decimal OtherDonation(decimal otherDonation, decimal totalIncomeA9)
        {
            otherDonation = SetLimit(otherDonation, totalIncomeA9 * 0.1m);
            return otherDonation;
        }





        decimal SetLimit(decimal input, decimal limit)
        {
            if (input > limit)
                return limit;
            return input;
        }
    }
}
