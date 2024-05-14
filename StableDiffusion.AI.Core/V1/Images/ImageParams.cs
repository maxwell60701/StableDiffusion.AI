namespace StableDiffusion.AI.Core.V1.Images
{
    public class ImageParams
    {
        public int Cfg_scale { get; set; }
        public int Height { get; set; }

        public int Width { get; set; }

        public int Samples { get; set; }

        public string Sampler { get; set; }
        public int Steps { get; set; }

        public IEnumerable<Prompts> Text_Prompts { get; set; }
    }

    public class ImageArgs
    {
        public int Cfg_scale { get; set; }
        public int Height { get; set; }

        public int Width { get; set; }

        public int Samples { get; set; }

        public string Sampler { get; set; }

        public int Steps { get; set; }
    }
    public class Prompts
    {
        public string Text { get; set; }
        public int Weight { get; set; }
    }

}
