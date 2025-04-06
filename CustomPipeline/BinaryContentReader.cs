using Microsoft.Xna.Framework.Content;

using TOutput = CustomPipeline.BinaryContent;

namespace CustomPipeline
{
    public class BinaryContentReader : ContentTypeReader<TOutput>
    {
        protected override TOutput Read(ContentReader input, TOutput existingInstance)
        {
            string[] parts = input.ReadString().Split('\n');

            bool isCompressed = bool.Parse(parts[0]);
            bool isCipher = bool.Parse(parts[1]);
            byte[] data = isCipher ? KanjiByteConverter.FromKanjiText(parts[2]) : KanjiByteConverter.FromText(parts[2]);

            if (isCompressed)
            {
                data = KanjiByteConverter.Decompression(data);
            }

            var result = new TOutput
            {
                UseCompression = isCompressed,
                Data = data
            };

            return result;
        }
    }
}
