using System;
using Xunit;
using Moq;
using Moq.Protected;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould_Protected_Concrete_Class_Possibilities
    {

        [Fact]
        public void ReferFraudRisk_205_Concrete_Class_205()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            Mock<FraudLookupProtected> mockFraudLookup = new Mock<FraudLookupProtected>();

            //STEP - IF YOU UNCOMMENT THE BELOW LINES THE TEST WILL FAIL WITH EXCEPTION
            //      Non-overridable members (here: FraudLookupProtected.IsFraudRisk) may not be used in setup / verification expressions.
            //      Because MOQ doesn't support NON OVERRIDABLE MEMBERS on a CONCRETE CLASS implementations
            //      You can check that ISFRAUDRISK method is PUBLIC in "FraudLookupProtected" concrete class
            //mockFraudLookup.Setup(x => x.IsFraudRisk(It.IsAny<CreditCardApplication>()))
            //               .Returns(true);


            ////STEP - NOTE - Added USING statement for including
            ////          MOQ.PROCTECTED
            mockFraudLookup.Protected()
                           .Setup<bool>("CheckApplication", ItExpr.IsAny<CreditCardApplication>())
                           .Returns(true);

            var sut = new CreditCardApplicationEvaluatorServiceConcreteClassProtected(mockValidator.Object,
                                                         mockFraudLookup.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecisionConcreteClass decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecisionConcreteClass.ReferredToHumanFraudRisk, decision);
        }
    }
}
