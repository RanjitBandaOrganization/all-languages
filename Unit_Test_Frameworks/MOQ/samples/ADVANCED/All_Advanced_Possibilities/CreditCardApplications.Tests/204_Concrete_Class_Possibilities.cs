using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_Concrete_Class_Possibilities
    {
        [Fact]
        public void ReferFraudRisk_204_Concrete_Class_204()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
               new Mock<IFrequentFlyerNumberValidator>();

            //STEP - setting up the concrete class
            // NOTE - Mock framework works with classes also not only
            //          with interfaces

            //NOTE2 - Also note that the ISFRAUDRISK is virtual meaning that it is overridable
            //      MOQ only supports OVERRIDABLE methods setups on CONCRETE CLASSES
            Mock<FraudLookup> mockFraudLookup = new Mock<FraudLookup>();
            mockFraudLookup.Setup(x => x.IsFraudRisk(It.IsAny<CreditCardApplication>()))
                           .Returns(true);

            var sut = new CreditCardApplicationEvaluatorServiceConcreteClass(mockValidator.Object,
                                                         mockFraudLookup.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecisionConcreteClass decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecisionConcreteClass.ReferredToHumanFraudRisk, decision);
        }
    }
}
