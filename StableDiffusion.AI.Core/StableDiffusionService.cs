using StableDiffusion.AI.Core.V1;

namespace StableDiffusion.AI.Core
{
    public class StableDiffusionService
    {
        public StableDiffusionV1 V1 { get; private set; }

        public StableDiffusionService(string apiKey)
        {
            V1 = new StableDiffusionV1(apiKey);
        }

        public StableDiffusionService(string apiKey, IHttpClientFactory httpClientFactory)
        {
            V1 = new StableDiffusionV1(apiKey, httpClientFactory);
        }
    }
}
