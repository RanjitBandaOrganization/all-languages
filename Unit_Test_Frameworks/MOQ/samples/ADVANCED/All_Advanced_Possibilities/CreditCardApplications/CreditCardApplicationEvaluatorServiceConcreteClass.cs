namespace CreditCardApplications
{
    public class CreditCardApplicationEvaluatorServiceConcreteClass
    {
        private readonly IFrequentFlyerNumberValidator _validator;
        private readonly FraudLookup _fraudLookup;

        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshhold = 100_000;
        private const int LowIncomeThreshhold = 20_000;

        public CreditCardApplicationEvaluatorServiceConcreteClass(IFrequentFlyerNumberValidator validator,
                FraudLookup fraudLookup = null)
        {
            _validator = validator ??
                throw new System.ArgumentNullException(nameof(validator));

            _fraudLookup = fraudLookup;
        }

        public CreditCardApplicationDecisionConcreteClass Evaluate(CreditCardApplication application)
        {
            if (_fraudLookup != null && _fraudLookup.IsFraudRisk(application))
            {
                return CreditCardApplicationDecisionConcreteClass.ReferredToHumanFraudRisk;
            }

            if (application.GrossAnnualIncome >= HighIncomeThreshhold)
            {
                return CreditCardApplicationDecisionConcreteClass.AutoAccepted;
            }

            var isValidFrequentFlyerNumber =
                _validator.IsValid(application.FrequentFlyerNumber);

            if (!isValidFrequentFlyerNumber)
            {
                return CreditCardApplicationDecisionConcreteClass.ReferredToHuman;
            }

            if (application.Age <= AutoReferralMaxAge)
            {
                return CreditCardApplicationDecisionConcreteClass.ReferredToHuman;
            }

            if (application.GrossAnnualIncome < LowIncomeThreshhold)
            {
                return CreditCardApplicationDecisionConcreteClass.AutoDeclined;
            }

            return CreditCardApplicationDecisionConcreteClass.ReferredToHuman;
        }
    }
}
