# Stable Diffusion AI
  * This is an unOfficial library just for developers to get stable diffusion ai easily,targetFramework is net 8
  
    used as shown below
  ```
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
  ![a cute rabbit with sunglasses](./images/rabbit.jpeg)

   There is a newer version called V2Beta used by stability AI,but it has some bugs requested by C#.I will merge them after they are fixed