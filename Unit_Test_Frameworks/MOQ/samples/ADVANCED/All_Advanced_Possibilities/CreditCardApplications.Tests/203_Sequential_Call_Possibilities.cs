using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_Sequential_Calls_Possibilities
    {
        [Fact]
        public void IncrementLookupCount_203_SEQUENTIAL_CALLS_203()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            mockValidator.SetupSequence(x => x.IsValid(It.IsAny<string>()))
                         .Returns(false)
                         .Returns(true);

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { Age = 25 };

            //STEP - As this is the first call FALSE will be returned from SETUP
            CreditCardApplicationDecision firstDecision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, firstDecision);

            //STEP - As this is the first call TRUE will be returned from SETUP
            CreditCardApplicationDecision secondDecision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, secondDecision);
        }
    }
}
