using Xunit;
using Moq;
using TechMoveGLMS.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Logging;

namespace TechMoveGLMS.Tests
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ILogger<CurrencyService>> _loggerMock;

        public CurrencyServiceTests()
        {
            _loggerMock = new Mock<ILogger<CurrencyService>>();
        }

        [Fact]
        public async Task GetExchangeRateAsync_ValidResponse_ReturnsRate()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"rates\": {\"ZAR\": 18.50}}"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new CurrencyService(httpClient, _loggerMock.Object);

            // Act
            var rate = await service.GetExchangeRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(18.50m, rate);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ApiError_ReturnsFallbackRate()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.InternalServerError,
               });

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new CurrencyService(httpClient, _loggerMock.Object);

            // Act
            var rate = await service.GetExchangeRateAsync("USD", "ZAR");

            // Assert
            Assert.Equal(19.50m, rate); // 19.50 is the fallback in CurrencyService
        }
    }
}
