using System;
using Xunit;
using Moq;
using Moq.Protected;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_LINQ_TO_MOCKS_multiple_setup_Possibilities
    {
        [Fact]
        public void ReferFraudRisk_206_LINQ_TO_MOCKS_multiple_setup_206()
        {
            //Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //STEP -  clubbing all of the above setups into a single expression
            IFrequentFlyerNumberValidator mockValidator
                = Mock.Of<IFrequentFlyerNumberValidator>
                (
                    validator =>
                    validator.ServiceInformation.License.LicenseKey == "OK" &&
                    validator.IsValid(It.IsAny<string>()) == true
                );


            var sut = new CreditCardApplicationEvaluatorService(mockValidator);

            var application = new CreditCardApplication { Age = 25 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }
    }
}
