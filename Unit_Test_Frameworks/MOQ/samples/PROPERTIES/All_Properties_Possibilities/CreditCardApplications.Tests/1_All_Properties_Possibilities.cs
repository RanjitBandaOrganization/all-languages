using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void ReferWhenLicenseKeyExpired_100_PROPERTY_RETURNS_VALUE_100()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberBASIC>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //STEP - Property is mocked to return string value == EXPIRED 
            mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluatorServiceBASIC(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }


        [Fact]
        public void ReferWhenLicenseKeyExpired_101_HIERARCHICAL_PROPERTY_RETURNS_VALUE_101()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //STEP - HIERARCHICAL Property is mocked to return string value == EXPIRED 
            //var mockLicenseData = new Mock<ILicenseData>();
            //mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");
            //var mockServiceInfo = new Mock<IServiceInformation>();
            //mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);
            //mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);
            // OR
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey)
                         .Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }


        [Fact]
        public void ReferWhenLicenseKeyExpired_102_HIERARCHICAL_PROPERTY_DEFAULT_MOCK_SETTING_102()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //STEP - In the HIERARCHICAL PROPERTY instead of setting up the entire structure JUST
            //      setting DEFAULT VALUE is sufficient.
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");
                                    //OR
            mockValidator.DefaultValue = DefaultValue.Mock;

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void UseDetailedLookupForOlderApplications_103_CHANGE_TRACKING__PROPERTIES_SAVING_AFTER_CHANGING_VALUE_103()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //STEP - The default MOCK behaviour is empty
            //  Any method called for which SETUP is not done will return NULL
            //  Similarly Properties also.    
            //  Please check the image - "SetupProperty vs SetupAllProperties.png",
            //  For more information
            //  So the following line is required 
            mockValidator.SetupAllProperties();
                        //OR
            //mockValidator.SetupProperty(x => x.ValidationMode);

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }

        [Fact]
        public void CheckLicenseKeyForLowIncomeApplications_104_PROPERTY_GET_TIMES_CALLED_104()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 99_000 };

            sut.Evaluate(application);
            //STEP - Verify Get property call called at least once
            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey, Times.Once);
        }

        [Fact]
        public void SetDetailedLookupForOlderApplications_105_PROPERTY_SET_TIMES_CALLED_105()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            sut.Evaluate(application);
            //STEP - Verify Set property call called at least once
            mockValidator.VerifySet(x => x.ValidationMode = It.IsAny<ValidationMode>(), Times.Once);
        }
    }
}
