using System;

namespace CreditCardApplications
{
    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string frequentFlyerNumber);
        void IsValid(string frequentFlyerNumber, out bool isValid);
        public IServiceInformation ServiceInformation
        {
            get
            {
                throw new NotImplementedException("For demo purposes");
            }
        }

        ValidationMode ValidationMode { get; set; }
    }

    public interface ILicenseData
    {
        string LicenseKey { get; }
    }

    public interface IServiceInformation
    {
        ILicenseData License { get; set; }
    }

    public enum ValidationMode
    {
        Quick,
        Detailed
    }

}