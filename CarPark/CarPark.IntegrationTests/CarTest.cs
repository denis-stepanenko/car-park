using CarPark.IntegrationTests.Infrastructure;
using HtmlAgilityPack;
using MongoDB.Driver;

namespace CarPark.IntegrationTests
{
    public class CarTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CarTest(CustomWebApplicationFactory<Program> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async void Should_ShowCars_When_PageNumberIs1()
        {
            var response = await _client.GetAsync("/Cars");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal("text/html; charset=utf-8", response.Content?.Headers?.ContentType?.ToString());
            Assert.Contains("BMW", html);
            Assert.Contains("M4", html);
            Assert.Contains("M5", html);
            Assert.Contains("M8", html);
        }

        [Fact]
        public async void Should_FailToAdd_When_MakeAndNameAreNotUnique()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Create",
                new()
                {
                    { "Item.Make", "Toyota" },
                    { "Item.Name", "Camry" }
                });

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var validationSummaryErrors = doc.DocumentNode.Descendants(0)
                .Where(n => n.HasClass("validation-summary-errors"))
                .SingleOrDefault()?
                .InnerHtml;

            Assert.Contains("Такой автомобиль уже существует", validationSummaryErrors);
        }

        [Fact]
        public async void Should_Add_When_MakeAndNameAreUnique()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Create",
                new()
                {
                    { "Item.Make", "Toyota" },
                    { "Item.Name", "Wildlander" }
                });

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var validationSummaryErrors = doc.DocumentNode.Descendants(0)
                .Where(n => n.HasClass("validation-summary-errors"))
                .SingleOrDefault()?
                .InnerHtml;

            Assert.Null(validationSummaryErrors);
        }

        [Fact]
        public async void Should_Update_When_MakeAndNameAreUnique()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Edit/678a613c95634c673d425942",
                new()
                {
                    { "Item.Id", "678a613c95634c673d425942" },
                    { "Item.Make", "Toyota" },
                    { "Item.Name", "Rav4" }
                });

            Assert.Contains("Rav4", html);
        }

        [Fact]
        public async void Should_FailToUpdate_When_ThereIsAnotherCarWithSuchMakeAndName()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Edit/678a613c95634c673d425938",
                new()
                {
                    { "Item.Id", "678a613c95634c673d425938" },
                    { "Item.Make", "Toyota" },
                    { "Item.Name", "Camry" }
                });

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var validationSummaryErrors = doc.DocumentNode.Descendants(0)
                .Where(n => n.HasClass("validation-summary-errors"))
                .SingleOrDefault()?
                .InnerHtml;

            Assert.Contains("Такой автомобиль уже существует", validationSummaryErrors);
        }

        [Fact]
        public async void Should_FailToUpdate_When_MakeAndNameAreEmpty()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Edit/678a613c95634c673d425941",
                new()
                {
                    { "Item.Id", "678a613c95634c673d425941" },
                    { "Item.Make", "" },
                    { "Item.Name", "" }
                });

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var validationErrors = doc.DocumentNode.Descendants(0)
                .Where(n => n.HasClass("field-validation-error"))
                .Select(x => x.InnerHtml);

            Assert.Contains("Введите наименование", validationErrors);
            Assert.Contains("Введите производителя", validationErrors);
        }

        [Theory]
        [InlineData("678a613c95634c673d425934", "M4")]
        [InlineData("678a613c95634c673d425935", "M5")]
        [InlineData("678a613c95634c673d425936", "M8")]
        [InlineData("678a613c95634c673d425939", "Hilux")]
        public async void Should_AskForDeletion_When_IdIsPassed(string id, string expected)
        {
            var response = await _client.GetAsync("/Cars/Delete/" + id);
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal("text/html; charset=utf-8", response.Content?.Headers?.ContentType?.ToString());
            Assert.Contains("удалить", html);
            Assert.Contains(expected, html);
        }

        [Fact]
        public async void Should_Delete_When_IdIsPassed()
        {
            string html = await _client.PostExtractingAntiForgeryTokenAsync("/Cars/Delete/678a613c95634c673d425940",
                new()
                {
                    { "Item.Id", "678a613c95634c673d425940" },
                });


            Assert.DoesNotContain("678a613c95634c673d425940", html);
        }
    }
}