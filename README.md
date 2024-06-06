# Stable Diffusion AI
  * This is an unofficial library just for developers to get stable diffusion web api easily,net standard 2.0
  
    used as shown below
  
  * 1.generate image by text
  ```c#
     string apiKey = "<your-private-key>";
     string prompt = "a cute rabbit with sunglasses";
     string negativePrompt = "low quality";
     string outPutFilePath = "<your-file-path>";
     File.Delete(outPutFilePath);
     var service = new StableDiffusionService(apiKey);
     var response = await service.V1.GenerateImagesAsync(Model.X1, prompt, negativePrompt);
     Console.WriteLine(response);
     File.WriteAllBytes(outPutFilePath, response?.FirstOrDefault());
  ```      
  * 2.generate image by image    
  ```c#
     string apiKey = "<your-private-key>";
     string outPutFilePath = "<your-file-path>";
     string _inputFilePath = "<your-input-file-path>";
     File.Delete(outPutFilePath);
     var service = new StableDiffusionService(_apiKey);
     using var fileStream = File.OpenRead(_inputFilePath);
     var byteData = new byte[fileStream.Length];
     fileStream.Read(byteData, 0, byteData.Length);
     var response = await service.V1.GenerateImagesAsync(Model.X1, new List<Prompt> { new() { Text = "green,land,grass", Weight = 0.5 } }, byteData);
     Console.WriteLine(response);
     File.WriteAllBytes(_outPutFilePath, response?.FirstOrDefault());
     Assert.IsTrue(File.Exists(_outPutFilePath));
  ```

  ![a cute rabbit with sunglasses](./images/rabbit.jpeg)

   There is a newer version called V2Beta used by stability AI,but it has some bugs requested by C#.I will merge them after they are fixed