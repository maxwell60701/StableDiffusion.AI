namespace StableDiffusion.AI.Core.V1.Exceptions
{
    [Serializable]
    internal class InitImageModeException : Exception
    {
        public InitImageModeException() : base() { }
        public InitImageModeException(string message) : base(message) { }
        public InitImageModeException(string message, Exception inner) : base(message, inner) { }
    }
}
