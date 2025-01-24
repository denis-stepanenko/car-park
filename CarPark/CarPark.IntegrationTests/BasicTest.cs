using Microsoft.AspNetCore.Mvc.Testing;

namespace CarPark.IntegrationTests
{
    public class BasicTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BasicTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Cars")]
        public async Task Should_ReturnSuccessAndCorrectContentType_WhenCallEveryEndpoint(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
        }
    }
}
