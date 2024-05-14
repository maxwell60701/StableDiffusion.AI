﻿
using StableDiffusion.AI.Core.V1.Images;

namespace StableDiffusion.AI.Core.Tests
{
    [TestClass()]
    public class StableDiffusionServiceTests
    {
        [TestMethod()]
        public async Task GenerateImageTest()
        {
            string apiKey = "<your-private-key>";
            string prompt = "a cute rabbit with sunglasses";
            string outPutFilePath = "<your-file-path>";
            File.Delete(outPutFilePath);
            var service = new StableDiffusionService(apiKey);
            var response = await service.V1.GenerateImageAsync(Model.X1, prompt);
            Console.WriteLine(response);
            File.WriteAllBytes(outPutFilePath, response?.FirstOrDefault());
            Assert.IsTrue(File.Exists(outPutFilePath));
        }
    }
}