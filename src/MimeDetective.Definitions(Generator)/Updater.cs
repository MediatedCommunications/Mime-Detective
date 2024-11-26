using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Tests;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Readers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MimeDetective.Definitions;

[TestClass]
public class Updater {
    private const string DefinitionsUrl = "https://mark0.net/download/triddefs_xml.7z";
    private const string DefsDir = "defs";

    [TestMethod]
    public async Task UpdateDefinitions() {
        var defsRoot = SourceDefinitions.DefinitionRoot();
        var extractPath = Directory.GetParent(defsRoot)?.FullName
            ?? throw new InvalidOperationException("Could not determine the path to the definitions parent directory.");

        using var client = new HttpClient {
            DefaultRequestVersion = HttpVersion.Version20,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };

        var clock = Stopwatch.StartNew();
        using (var response = await client.GetAsync(DefinitionsUrl).ConfigureAwait(false)) {
            clock.Stop();

            response.EnsureSuccessStatusCode();

            // Since the GET defaults to reading the entire contents, this should be a seekable
            // stream.
            await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            Debug.Assert(stream.CanSeek);

            Trace.WriteLine($"Downloaded {stream.Length} bytes in {clock.Elapsed}");

            using var archive = SevenZipArchive.Open(stream);
            var archiveEntry = archive.Entries.FirstOrDefault(
                e => e is { IsDirectory: true, Key: not null }
                    && e.Key.EndsWith(DefsDir, StringComparison.OrdinalIgnoreCase));

            if (archiveEntry is null) {
                throw new InvalidOperationException($"Could not find '{DefsDir}' directory in archive.");
            }

            using var reader = archive.ExtractAllEntries();

            while (reader.MoveToNextEntry()) {
                if (reader.Entry.IsDirectory) {
                    continue;
                }

                reader.WriteEntryToDirectory(extractPath,
                    new() {
                        ExtractFullPath = true,
                        Overwrite = true,
                        PreserveAttributes = true,
                        PreserveFileTime = true
                    });
            }
        }

        if (Directory.Exists(defsRoot)) {
            Directory.Delete(defsRoot, true);
        }

        Directory.Move(Path.Combine(extractPath, DefsDir), defsRoot);
    }
}