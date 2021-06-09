using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hand_Rolled_Mock_Objects.Logging_Library.Code;

namespace Hand_Rolled_Mock_Objects.Logging_Library_Tests.Tests
{
    public class MockScrubber : IScrubSensitiveData
    {
        public string From(string messageToScrub)
        {
            FromWasCalled = true;
            return string.Empty;
        }

        public bool FromWasCalled { get; private set; }
    }

        [TestClass]
        public class When_logging
        {
        private MockScrubber _mockScrubber;
        private MockHeader _mockHeader;
        private MockFooter _mockFooter;
        private MockSystemConfig _mockSystemConfig;

        [TestInitialize]
            public void TestMethod()
            {
                _mockScrubber = new MockScrubber();
                _mockHeader = new MockHeader();
                _mockFooter = new MockFooter();
                _mockSystemConfig = new MockSystemConfig();

                var logger = new Logging(_mockScrubber, _mockHeader, 
                    _mockFooter, _mockSystemConfig);
                logger.CreateEntryFor("my message", LogLevel.Info);
            }

            [TestMethod]
            public void sensitive_data_should_be_scrubbed_from_the_log_message()
            {
                Assert.IsTrue(_mockScrubber.FromWasCalled);
            }

            [TestMethod]
            public void entry_headers_should_be_created()
            {

            }

            [TestMethod]
            public void the_system_configuration_should_be_checked_for_stack_logging()
            {
                
            }


            [TestMethod]
            public void entry_footers_should_be_created()
            {
                
            }
        }

    internal class MockSystemConfig : IConfigureSystem
    {
        public bool LogStackFor(LogLevel logLevel)
        {
            return false;
        }
    }

    public class MockFooter : ICreateLogEntryFooter
    {
        public void For(LogLevel logLevel)
        {
            
        }
    }

    public class MockHeader : ICreateLogEntryHeaders
    {
        public void For(LogLevel logLevel)
        {
            
        }
    }
}
