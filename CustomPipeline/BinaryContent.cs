namespace CustomPipeline
{
    public class BinaryContent
    {
        public bool UseCompression { get; set; }
        public bool UseEncryption { get; set; }
        public byte[] Data { get; set; } = null;
    }
}
