using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

using TImport = System.Array;

namespace CustomPipeline
{
    [ContentImporter(".*", DisplayName = "Binary Importer - FumiFumi", DefaultProcessor = nameof(BinaryProcessor))]
    public class BinaryImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllBytes(filename);
        }
    }
    }
