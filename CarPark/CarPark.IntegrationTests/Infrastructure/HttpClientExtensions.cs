namespace CarPark.IntegrationTests.Infrastructure
{
    public static class HttpClientExtensions
    {
        public static async Task<string> PostExtractingAntiForgeryTokenAsync(this HttpClient client, string url, Dictionary<string, string> data)
        {
            var initialRes = await client.GetAsync(url);
            var antiForgeryToken = await AntiForgeryTokenExtractor.Extract(initialRes);

            var formData = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.Field, antiForgeryToken }
            };

            foreach (var pair in data)
            {
                formData.Add(pair.Key, pair.Value);
            }

            var content = new FormUrlEncodedContent(formData);

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            return html;
        }
    }
}
