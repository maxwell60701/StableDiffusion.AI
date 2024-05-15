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

        /// <summary>
        /// generate images async
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="prompt">describe what to generate</param>
        /// <param name="negativePrompt">describe what NOT to generate</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, string? negativePrompt = null, ImageArgs? args = null)
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
                Cfg_scale = args?.Cfg_scale ?? 7,
                Height = args?.Height ?? Height.Large,
                Width = args?.Width ?? Width.Large,
                Sampler = args?.Sampler ?? "K_EULER",
                Samples = args?.Samples ?? 1,
                Steps = args?.Steps ?? 20,
                Text_Prompts = prompts
            };
            return await GenerateImagesCoreAsync(modelName, imageInput);
        }

        private async Task<IEnumerable<byte[]>> GenerateImagesCoreAsync(string modelName, ImageParams input)
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
