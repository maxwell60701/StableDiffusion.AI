using Newtonsoft.Json;
using StableDiffusion.AI.Core.V1.Exceptions;
using StableDiffusion.AI.Core.V1.Images;
using System.Net.Http.Json;

namespace StableDiffusion.AI.Core.V1
{
    public class StableDiffusionV1
    {
        private const string BaseAddress = "https://api.stability.ai/v1";
        private readonly string _apiKey;
        public StableDiffusionV1(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<IEnumerable<byte[]>> GenerateImageAsync(string modelName, string prompt, string? negativePrompt = null, ImageArgs? args = null)
        {
            var prompts = new List<Prompts>
            {
                new() { Text = prompt, Weight = 1 }
            };
            if (!string.IsNullOrEmpty(negativePrompt))
            {
                prompts.Add(new Prompts { Text = negativePrompt, Weight = -1 });
            }
            var imageInput = new ImageParams
            {
                Cfg_scale = args?.Cfg_scale ?? 5,
                Height = args?.Height ?? 1024,
                Width = args?.Width ?? 1024,
                Sampler = args?.Sampler ?? "K_EULER",
                Samples = args?.Samples ?? 1,
                Steps = args?.Steps ?? 10,
                Text_Prompts = prompts
            };
            return await GenerateImageCoreAsync(modelName, imageInput);
        }

        private async Task<IEnumerable<byte[]>> GenerateImageCoreAsync(string modelName, ImageParams input)
        {
            ArgumentNullException.ThrowIfNull(modelName);
            ArgumentNullException.ThrowIfNull(input);

            using var client = new HttpClient();
            string requestUrl = $"{BaseAddress}/generation/{modelName}/text-to-image";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var response = await client.PostAsJsonAsync(requestUrl, input);
            response.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<Output>(await response.Content.ReadAsStringAsync());

            ArgumentNullException.ThrowIfNull(data);
            if (!string.IsNullOrEmpty(data?.Message))
            {
                throw new InvalidDataException($"StabilityAPI refused to generate: {data.Message}");
            }
            var list = new List<byte[]>();
            foreach (var img in data?.Artifacts)
            {
                if (img.FinishReason == "ERROR")
                {
                    throw new StabilityAPIException("StabilityAPI returned error for request");
                }
                var byteData = Convert.FromBase64String(img.Base64.ToString());
                list.Add(byteData);
            }
            return list;
        }
    }
}
