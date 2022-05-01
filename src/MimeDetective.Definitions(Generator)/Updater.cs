using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Tests;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MimeDetective.Definitions
{
    [TestClass]
    public class Updater
    {
        private const string DEFINITIONS_URL = "https://mark0.net/download/triddefs_xml.7z";
        private const string DEFS_DIR = "defs";

        [TestMethod]
        public async Task UpdateDefinitions()
        {
            var defsRoot = SourceDefinitions.DefinitionRoot();
            var extractPath = Directory.GetParent(defsRoot)?.FullName;

            if (extractPath == null)
            {
                throw new InvalidOperationException("Could not determine the path to the definitions parent directory.");
            }

            using var client = new HttpClient();
            await using var stream = await client.GetStreamAsync(DEFINITIONS_URL).ConfigureAwait(false);

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            using var archive = SevenZipArchive.Open(ms);
            var archiveEntry = archive.Entries.FirstOrDefault(e => e.IsDirectory && e.Key.EndsWith(DEFS_DIR, StringComparison.OrdinalIgnoreCase));

            if (archiveEntry == null)
            {
                throw new Exception($"Could not find '{DEFS_DIR}' directory in archive.");
            }

            using var reader = archive.ExtractAllEntries();

            while (reader.MoveToNextEntry())
            {
                if (reader.Entry.IsDirectory)
                {
                    continue;
                }

                reader.WriteEntryToDirectory(extractPath, new ExtractionOptions { ExtractFullPath = true, Overwrite = true, PreserveAttributes = true, PreserveFileTime = true });
            }

            if (Directory.Exists(defsRoot))
            {
                Directory.Delete(defsRoot, true);
            }

            Directory.Move(Path.Combine(extractPath, DEFS_DIR), defsRoot);
        }
    }
}