
using StableDiffusion.AI.Core.V1;
using StableDiffusion.AI.Core.V1.Images;

namespace StableDiffusion.AI.Core.Tests
{
    [TestClass()]
    public class StableDiffusionServiceTests
    {
        private readonly string _apiKey = "<your-private-key>";
        private readonly string _prompt = "a cute rabbit with sunglasses";
        private readonly string _negativePrompt = "low quality";
        private readonly string _outPutFilePath = "<your-file-path>";

        [TestMethod()]
        public async Task GenerateImageTest()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution1344x768Test()
        {        
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt,Dimension.Resolution1344x768,_negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution768x1344Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution768x1344, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }


        [TestMethod()]
        public async Task GenerateImageWithResolution1024x1024Test()
        {         
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution1024x1024, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution1152x896Test()
        {         
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution1152x896, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution896x1152Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution896x1152, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution1216x832Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution1216x832, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution832x1216Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution832x1216, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution1536x640Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution1536x640, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithResolution640x1536Test()
        {
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt, Dimension.Resolution640x1536, _negativePrompt);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }

        [TestMethod()]
        public async Task GenerateImageWithWidthAndHeightTest()
        {
            int width = 832;
            int height = 1216;
            File.Delete(_outPutFilePath);
            var service = new StableDiffusionService(_apiKey);
            var response = await service.V1.GenerateImagesAsync(Model.X1, _prompt,width,height);
            Console.WriteLine(response);
            File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(_outPutFilePath));
        }
    }
}