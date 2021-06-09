using System;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications_01_CREATING_FIRST_MOCK_01()
        {
            //STEP1 - Create mock object for the interface
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            //STEP2 - Inject this MOCK object instance into the instace so that default
            // values will be supplied to all method and property calls
            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            //SUT - system under test
            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications_02_METHOD_RETURNS_VALUE_02()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            //STEP - Do required setup, so that if METHOD calls 
            //      with desired inputs then it RETURNS this value
            mockValidator.Setup(x => x.IsValid("x")).Returns(true);

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "x"
            };
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }


        [Fact]
        public void ReferInvalidFrequentFlyerApplications_03_STRICT_MOCK_IF_SETUP_DOESNOT_EXISTS_THEN_TEST_FAILS_03()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);

            //STEP - As Strick is passed during mock object creation
            //      So setup is required if that method is called else this will fail
            //      So usually this is not recommended.
            // Check the Images Folder for more details "Mock_Mode_Strict_Loose.png"
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications_04_METHOD_ARGUMENTS_MATCHING_POSSIBILITIES_04()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            //STEP - Instead of hard coding the method arguments as above
            //      We can provide generic parameters matching as below

            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith('x'))))
            //            .Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsIn("x", "y", "z"))).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsInRange("b", "z", Range.Inclusive)))
            //             .Returns(true);
            mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]",
                                System.Text.RegularExpressions.RegexOptions.None)))
                         .Returns(true);

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }


        [Fact]
        public void DeclineLowIncomeApplicationsOutDemo_04_OUT_PARAMETER_TEST_04()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            //SETUP - with OUT parameter
            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ShouldValidateFrequentFlyerNumberForLowIncomeApplications_05_VERIFY_METHOD_CALL_05()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { FrequentFlyerNumber = "q" };

            sut.Evaluate(application);

            //SETUP - The following line make sures to test ISVALID method is called
            // NOTE - though we have not done any setup on this method the below ASSERT worked
            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void NotValidateFrequentFlyerNumberForHighIncomeApplications_06_VERIFY_METHOD_NOT_CALLED_06()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void ValidateFrequentFlyerNumberForLowIncomeApplications_07_VERIFY_METHOD_TIMES_CALLED_07()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluatorService(mockValidator.Object);

            var application = new CreditCardApplication { FrequentFlyerNumber = "q" };

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Once);
        }
    }
}
