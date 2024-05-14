namespace StableDiffusion.AI.Core.V1.Images
{
    public class Output
    {
        public Artifact[] Artifacts { get; set; } 

        public string FinalReason { get; set; }
        public string Message { get; set; }
    }
    public class Artifact
    {
        public string Base64 { get; set; }
        public string FinishReason { get; set; }
    }
}
