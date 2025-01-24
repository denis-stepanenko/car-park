using System.Text.RegularExpressions;

namespace CarPark.IntegrationTests.Infrastructure
{
    public static class AntiForgeryTokenExtractor
    {
        public static string Field { get; } = "AntiForgeryField";

        public static async Task<string> Extract(HttpResponseMessage response)
        {
            var html = await response.Content.ReadAsStringAsync();

            var requestVerificationTokenMatch = Regex.Match(html, $@"\<input name=""{Field}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            else
                throw new ArgumentException($"Anti forgery token '{Field}' не найден", nameof(html));
        }
    }
}
