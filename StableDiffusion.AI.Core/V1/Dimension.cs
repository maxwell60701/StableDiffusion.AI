namespace StableDiffusion.AI.Core.V1
{
    public struct Dimension
    {
        public int Width { get; }
        public int Height { get; }
        public Dimension(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static readonly Dimension Resolution1024x1024 = new Dimension(1024, 1024);

        public static readonly Dimension Resolution1152x896 = new Dimension(1152, 896);

        public static readonly Dimension Resolution896x1152 = new Dimension(896, 1152);

        public static readonly Dimension Resolution1216x832 = new Dimension(1216, 832);

        public static readonly Dimension Resolution832x1216 = new Dimension(832, 1216);

        public static readonly Dimension Resolution1344x768 = new Dimension(1344, 768);

        public static readonly Dimension Resolution768x1344 = new Dimension(768, 1344);

        public static readonly Dimension Resolution1536x640 = new Dimension(1536, 640);
            
        public static readonly Dimension Resolution640x1536 = new Dimension(640, 1536);

    }

}
