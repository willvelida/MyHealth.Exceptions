using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyHealth.Exceptions.Functions;
using MyHealth.Exceptions.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyHealth.Exceptions.UnitTests.FunctionTests
{
    public class ProcessExceptionMessageShould
    {
        private readonly Mock<ILogger<ProcessExceptionMessage>> _loggerMock;
        private readonly Mock<ISendGridService> _sendGridServiceMock;

        private ProcessExceptionMessage _func;

        public ProcessExceptionMessageShould()
        {
            _loggerMock = new Mock<ILogger<ProcessExceptionMessage>>();
            _sendGridServiceMock = new Mock<ISendGridService>();

            _func = new ProcessExceptionMessage(_loggerMock.Object, _sendGridServiceMock.Object);
        }

        [Fact]
        public async Task ShouldSendExceptionEmail()
        {
            // Arrange
            var exception = new Exception();
            var testExceptionString = JsonConvert.SerializeObject(exception);

            // Act
            await _func.Run(testExceptionString);

            // Assert
            _sendGridServiceMock.Verify(x => x.SendExceptionEmail(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ShouldLogErrorOnSendGridException()
        {
            // Arrange
            var exception = new Exception("SendGridServiceFailed");
            var testExceptionString = JsonConvert.SerializeObject(exception);

            _sendGridServiceMock.Setup(x => x.SendExceptionEmail(It.IsAny<Exception>())).Throws(exception);

            // Act
            await _func.Run(testExceptionString);

            // Assert
            _loggerMock.Verify
            (
                x => x.Log
                (
                    LogLevel.Error,
                    0,
                    It.Is<object>(o => o.ToString() == "Exception thrown in ProcessExceptionMessage: SendGridServiceFailed"),
                    null,
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Once
            );
        }
    }
}
