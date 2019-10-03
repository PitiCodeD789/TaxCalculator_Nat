using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TaxEngine;
using TaxEngine.Models;
using Xamarin.Forms;

namespace TaxCalculator
{
    public class TaxCalculatorViewModel : INotifyPropertyChanged
    {
        public TaxCalculatorViewModel()
        {
            Model = new TaxCalculatorCommand
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
                LifeInsurance = 70000,
                HealthInsurance = 15000,
                SavingFund = 13200,
                LTF = 400000,
                RMF = 200000,
                PensionInsurance = 20000,
                PoliticalParty = 10000,
                TravelExpense = 20000,
                SocialSecurity = 9000,
                HousingInterest = 100000,
                StartupInvestment = 100000,
                CctvExpense = 0,
                OtherExpense = 9000,
                EducationDonation = 0,
                OtherDonation = 0
            };
            ResetValueCommand = new Command(GetTax);
        }
        public Command ResetValueCommand { get; set; }

        private TaxCalculatorCommand _calculatorCommand;
        public TaxCalculatorCommand Model
        {
            get { return _calculatorCommand; }
            set { _calculatorCommand = value; }
        }

        private TaxResultModel resultModel;

        public TaxResultModel ResultModel
        {
            get { return resultModel; }
            set { resultModel = value; }
        }

        public void GetTax()
        {
            Engine engine = new Engine();
            ResultModel = engine.TaxCalculator(Model);
            OnPropertyChanged("ResultModel");
            OnPropertyChanged("Model");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
