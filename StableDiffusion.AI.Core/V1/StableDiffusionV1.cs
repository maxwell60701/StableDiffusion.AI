using Newtonsoft.Json;
using StableDiffusion.AI.Core.HttpClientExtensions;
using StableDiffusion.AI.Core.V1.Exceptions;
using StableDiffusion.AI.Core.V1.Images;

namespace StableDiffusion.AI.Core.V1
{
    public class StableDiffusionV1
    {
        private const string BaseAddress = "https://api.stability.ai/v1";
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;
        public StableDiffusionV1(string apiKey)
        {
            _apiKey = apiKey;
        }

        public StableDiffusionV1(string apiKey, IHttpClientFactory httpClientFactory)
        {
            _apiKey = apiKey;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// generate images async
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="prompt"></param>
        /// <param name="negativePrompt"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, string? negativePrompt = null, ImageArgs? args = null)
        {
            return await GenerateImagesAsync(modelName, prompt, Dimension.Resolution1024x1024.Width, Dimension.Resolution1024x1024.Height, negativePrompt, args);
        }

        /// <summary>
        ///  generate images async with dimension (recommended)
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="prompt"></param>
        /// <param name="dimension"></param>
        /// <param name="negativePrompt"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, Dimension dimension, string? negativePrompt = null, ImageArgs? args = null)
        {
            return await GenerateImagesAsync(modelName, prompt, dimension.Width, dimension.Height, negativePrompt, args);
        }

        /// <summary>
        /// generate images async with width and height
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="prompt"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="negativePrompt"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, int width, int height, string? negativePrompt = null, ImageArgs? args = null)
        {
            var prompts = new List<Prompts>
            {
                new Prompts() { Text = prompt, Weight = 1 }
            };
            if (!string.IsNullOrEmpty(negativePrompt))
            {
                prompts.Add(new Prompts { Text = negativePrompt, Weight = -1 });
            }
            var imageInput = new TextToImageParams
            {
                Cfg_scale = args?.Cfg_scale ?? 7,
                Height = height,
                Width = width,
                Sampler = args?.Sampler ?? "K_EULER",
                Samples = args?.Samples ?? 1,
                Steps = args?.Steps ?? 20,
                Text_Prompts = prompts
            };
            return await GenerateImagesCoreAsync(modelName, imageInput);
        }

        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, Prompts[] prompts, byte[] imageData, int cfg_scale = 7, string sampler = "K_DPM_2_ANCESTRAL", int samples = 1, double image_strength = 0.35, string init_image_mode = "IMAGE_STRENGTH", int steps = 30)
        {
            return await GenerateImagesByImagesCoreAsync(modelName, prompts, imageData, cfg_scale, sampler, samples, image_strength, init_image_mode, steps);
        }

        private async Task<IEnumerable<byte[]>> GenerateImagesCoreAsync(string modelName, TextToImageParams input)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                throw new ArgumentNullException(nameof(modelName));
            }
            using var client = _httpClientFactory.GetClient();
            string requestUrl = $"{BaseAddress}/generation/{modelName}/text-to-image";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var stringContent = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
            var response = await client.PostAsync(requestUrl, stringContent);
            var data = JsonConvert.DeserializeObject<Output>(await response.Content.ReadAsStringAsync());
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidDataException($"StabilityAPI refused to generate: {data?.Message}");
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


        private async Task<IEnumerable<byte[]>> GenerateImagesByImagesCoreAsync(string modelName, Prompts[] prompts, byte[] imageData, int cfg_scale, string sampler, int samples, double image_strength, string init_image_mode, int steps)
        {
            using var client = _httpClientFactory.GetClient();
            using var memoryStream = new MemoryStream(imageData);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseAddress}/generation/{modelName}/image-to-image");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(image_strength.ToString()), "image_strength");
            content.Add(new StringContent(init_image_mode), "init_image_mode");
            content.Add(new StreamContent(memoryStream));
            for (int i = 0; i < prompts.Length; i++)
            {
                content.Add(new StringContent(prompts[i].Text), $"text_prompts[{i}][text]");
                content.Add(new StringContent(prompts[i].Weight.ToString()), $"text_prompts[{i}][weight]");
            }
            content.Add(new StringContent(cfg_scale.ToString()), "cfg_scale");
            content.Add(new StringContent(sampler), "sampler");
            content.Add(new StringContent(samples.ToString()), "samples");
            content.Add(new StringContent(steps.ToString()), "steps");
            request.Content = content;
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<Output>(await response.Content.ReadAsStringAsync());
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidDataException($"StabilityAPI refused to generate: {data?.Message}");
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
