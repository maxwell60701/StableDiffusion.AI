using Newtonsoft.Json;
using StableDiffusion.AI.Core.HttpClientExtensions;
using StableDiffusion.AI.Core.V1.Enums;
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
        /// <param name="modelName">model name</param>
        /// <param name="prompt">text prompt for generation</param>
        /// <param name="negativePrompt">prompt you do not need to generate</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, string? negativePrompt = null, ImageArgs? args = null)
        {
            return await GenerateImagesAsync(modelName, prompt, Dimension.Resolution1024x1024.Width, Dimension.Resolution1024x1024.Height, negativePrompt, args);
        }

        /// <summary>
        ///  generate images async with dimension (recommended)
        /// </summary>
        /// <param name="modelName">model name</param>
        /// <param name="prompt">text prompt for generation</param>
        /// <param name="dimension">Dimension for the image to generate</param>
        /// <param name="negativePrompt">prompt you do not need to generate</param>
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
        /// <param name="prompt">text prompt for generation</param>
        /// <param name="width">Width of the image to generate, in pixels, in an increment divisible by 64</param>
        /// <param name="height">Height of the image to generate, in pixels, in an increment divisible by 64</param>
        /// <param name="negativePrompt">prompt you do not need to generate</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, string prompt, int width, int height, string? negativePrompt = null, ImageArgs? args = null)
        {
            var prompts = new List<Prompt>
            {
                new Prompt() { Text = prompt, Weight = 1 }
            };
            if (!string.IsNullOrEmpty(negativePrompt))
            {
                prompts.Add(new Prompt { Text = negativePrompt, Weight = -1 });
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

        /// <summary>
        /// generate images by image
        /// </summary>
        /// <param name="modelName">model name</param>
        /// <param name="prompts">An array of text prompts to use for generation</param>
        /// <param name="imageData">Image used to initialize the diffusion process, in lieu of random noise.</param>
        /// <param name="cfg_scale">How strictly the diffusion process adheres to the prompt text (higher values keep your image closer to your prompt)</param>
        /// <param name="sampler">Which sampler to use for the diffusion process. If this value is omitted we'll automatically select an appropriate sampler for you</param>
        /// <param name="samples">Number of images to generate</param>
        /// <param name="image_strength">How much influence the init_image has on the diffusion process. Values close to 1 will yield images very similar to the init_image while values close to 0 will yield images wildly different than the init_image. The behavior of this is meant to mirror DreamStudio's "Image Strength" slider</param>
        /// <param name="init_image_mode">Whether to use image_strength or step_schedule_* to control how much influence the init_image has on the result</param>
        /// <param name="steps">Number of diffusion steps to run</param>
        /// <returns></returns>
        public async Task<IEnumerable<byte[]>> GenerateImagesAsync(string modelName, IList<Prompt> prompts, byte[] imageData, int cfg_scale = 7, string sampler = "K_DPM_2_ANCESTRAL", int samples = 1, double image_strength = 0.35, double step_schedule_start = 0.65, InitImageMode init_image_mode = InitImageMode.IMAGE_STRENGTH, int steps = 30)
        {
            return await GenerateImagesByImagesCoreAsync(modelName, prompts, imageData, cfg_scale, sampler, samples, image_strength, step_schedule_start, init_image_mode, steps);
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

        private async Task<IEnumerable<byte[]>> GenerateImagesByImagesCoreAsync(string modelName, IList<Prompt> prompts, byte[] imageData, int cfg_scale, string sampler, int samples, double image_strength, double step_schedule_start, InitImageMode init_image_mode, int steps)
        {
            using var client = _httpClientFactory.GetClient();
            using var memoryStream = new MemoryStream(imageData);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseAddress}/generation/{modelName}/image-to-image");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var content = new MultipartFormDataContent
            {
                { new StringContent(init_image_mode.ToString()), "init_image_mode" },
                { new StreamContent(memoryStream), "init_image" }
            };
            if (init_image_mode == InitImageMode.IMAGE_STRENGTH)
            {
                content.Add(new StringContent(image_strength.ToString()), "image_strength");
            }
            else if (init_image_mode == InitImageMode.STEP_SCHEDULE)
            {
                content.Add(new StringContent(step_schedule_start.ToString()), "step_schedule_start");
            }
            else
            {
                throw new InitImageModeException("init_image_mode: must be one of IMAGE_STRENGTH or STEP_SCHEDULE");
            }
            for (int i = 0; i < prompts.Count; i++)
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
