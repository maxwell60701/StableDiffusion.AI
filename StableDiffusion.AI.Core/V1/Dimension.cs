namespace StableDiffusion.AI.Core.V1
{
    public readonly struct Dimension(int width, int height)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;

        public static readonly Dimension Resolution1024x1024 = new(1024, 1024);

        public static readonly Dimension Resolution1152x896 = new(1152, 896);

        public static readonly Dimension Resolution896x1152 = new(896, 1152);

        public static readonly Dimension Resolution1216x832 = new(1216, 832);

        public static readonly Dimension Resolution832x1216 = new(832, 1216);

        public static readonly Dimension Resolution1344x768 = new(1344, 768);

        public static readonly Dimension Resolution768x1344 = new(768, 1344);

        public static readonly Dimension Resolution1536x640 = new(1536, 640);

        public static readonly Dimension Resolution640x1536 = new(640, 1536);

    }

}
