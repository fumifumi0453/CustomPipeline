using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TInput = CustomPipeline.BinaryContent;

namespace CustomPipeline
{
    [ContentTypeWriter]
    public class BinaryContentWriter : ContentTypeWriter<TInput>
    {
        protected override void Write(ContentWriter output, TInput value)
        {
            var bytes = value.Data;

            if (value.UseCompression)
            {
                bytes = KanjiByteConverter.Compression(bytes);
            }

            var text = value.UseEncryption ? KanjiByteConverter.ToKanjiText(bytes) : KanjiByteConverter.ToText(bytes);

            output.Write(string.Join("\n", value.UseCompression, value.UseEncryption, text));
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "CustomPipeline.BinaryContentReader, CustomPipeline";
        }

    }
}
