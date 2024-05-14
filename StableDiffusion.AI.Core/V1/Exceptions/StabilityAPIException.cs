namespace StableDiffusion.AI.Core.V1.Exceptions
{
    [Serializable]
    public class StabilityAPIException : Exception
    {
        public StabilityAPIException() : base() { }
        public StabilityAPIException(string message) : base(message) { }
        public StabilityAPIException(string message, Exception inner) : base(message, inner) { }
    }
}
