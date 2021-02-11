using FluentAssertions;
using Moq;
using PoliWebSearch.Parser.Infra.Services.Clock;
using PoliWebSearch.Parser.Infra.Services.Log;
using Xunit;

namespace PoliWebSearch.Parser.UnitTests.FileParsers.Infra.Services.Clock
{

    public class ClockServiceUnitTests
    {
        private ClockService clockService;
        private Mock<ILogService> logServiceMock = new Mock<ILogService>();

        public static int HelperVariable = 0;

        public ClockServiceUnitTests()
        {
            logServiceMock.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()));
            clockService = new ClockService(
                logServiceMock.Object
            );
        }

        [Fact]
        public void WhenExecuteWithStopWatch_ShouldExecuteAction()
        {
            // Arrange
            HelperVariable = 0;

            // Act
            clockService.ExecuteWithStopWatch("MyActionName", () => { HelperVariable++; });

            // Assert
            HelperVariable.Should().Be(1);
        }
    }
}
