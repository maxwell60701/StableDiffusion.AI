namespace StableDiffusion.AI.Core.HttpClientExtensions
{
    internal static class HttpClientExtension
    {
        internal static HttpClient GetClient(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory != null ? httpClientFactory.CreateClient() : new HttpClient();
        }
    }
}
