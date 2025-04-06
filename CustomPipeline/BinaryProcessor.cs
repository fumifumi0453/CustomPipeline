using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.ComponentModel;

using TInput = System.Array;
using TOutput = CustomPipeline.BinaryContent;

namespace CustomPipeline
{
    [ContentProcessor(DisplayName = "BinaryProcessor - FumiFumi")]
    internal class BinaryProcessor : ContentProcessor<TInput, TOutput>
    {
        [DisplayName("Compression")]
        public bool UseCompression { get; set; } = true;

        [DisplayName("Encryption")]
        public bool UseEncryption { get; set; } = false;

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            var bytes = new byte[input.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(input.GetValue(i) as byte? ?? 0);
            }

            var result = new TOutput
            {
                UseCompression = UseCompression,
                UseEncryption = UseEncryption,
                Data = bytes
            };

            return result;
        }
    }
}