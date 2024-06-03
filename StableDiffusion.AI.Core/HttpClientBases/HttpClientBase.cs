namespace StableDiffusion.AI.Core.HttpClientBases
{
    internal static class HttpClientBase
    {
        internal static HttpClient GetClient(IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory != null ? httpClientFactory.CreateClient() : new HttpClient();
        }
    }
}
