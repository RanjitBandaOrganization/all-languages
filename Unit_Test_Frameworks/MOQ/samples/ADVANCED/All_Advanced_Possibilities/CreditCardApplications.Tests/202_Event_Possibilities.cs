using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_Event_Possibilities
    {
        [Fact]
        public void IncrementLookupCount_202_EVENT_RAISES_MANUAL_AUTOMATIC_IN_DEPENDENT_aka_MOCKED__CLASS_202()
        {
            Mock<IFrequentFlyerNumberValidatorEvent> mockValidator = new Mock<IFrequentFlyerNumberValidatorEvent>();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            //STEP - raising EVENTS automatically on the MOCK object 
            //      when the SETUP is met
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                         .Returns(true)
                         .Raises(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);

            var sut = new CreditCardApplicationEvaluatorServiceEvent(mockValidator.Object);

            var application = new CreditCardApplication { FrequentFlyerNumber = "x", Age = 25 };

            sut.Evaluate(application);

            //STEP - raising EVENTS manually
            //mockValidator.Raise(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);

            Assert.Equal(1, sut.ValidatorLookupCount);
        }
    }
}
