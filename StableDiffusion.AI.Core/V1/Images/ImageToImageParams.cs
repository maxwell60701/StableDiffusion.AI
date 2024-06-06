using System;
using System.Collections.Generic;
using System.Text;

namespace StableDiffusion.AI.Core.V1.Images
{
    public class ImageToImageParams
    {
        /// <summary>
        /// How strongly to scale prompt input.Higher CFG scales tend to produce more contrast, and lower CFG scales produce less contrast.
        /// </summary>
        public int Cfg_scale { get; set; }
        public int Height { get; set; }

        public int Width { get; set; }

        public int Samples { get; set; }

        public string Sampler { get; set; }

        /// <summary>
        /// How many times to run the model.More steps = better quality, but more time.\n20 is a good baseline for speed, 40 is good for maximizing quality.You can go much higher, but it quickly becomes pointless above 70 or so
        /// </summary>
        public int Steps { get; set; }

        public IEnumerable<Prompt> Text_Prompts { get; set; }
    }
}
