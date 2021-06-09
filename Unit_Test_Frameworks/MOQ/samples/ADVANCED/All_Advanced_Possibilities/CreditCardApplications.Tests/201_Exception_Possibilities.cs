using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_Exception_Possibilities
    {
        [Fact]
        public void ReferWhenFrequentFlyerValidationError_201_THROW_EXCEPTION_ON_METHOD_PROPERTY_CALL_201()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator
                = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            //STEP - setup to throw exception
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                         .Throws(new Exception("Custom message"));

            var sut = new CreditCardApplicationEvaluatorServiceException(mockValidator.Object);

            var application = new CreditCardApplication { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

    }
}
