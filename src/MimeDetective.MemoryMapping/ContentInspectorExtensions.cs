using MimeDetective.Engine;
using System.Collections.Immutable;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace MimeDetective.MemoryMapping;

public static class ContentInspectorExtensions {
    /// <summary>
    /// </summary>
    /// <param name="This"></param>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> InspectMemoryMapped(this ContentInspector This, string FilePath) {
        using var file = new FileStream(FilePath, FileMode.Open, FileAccess.Read,
            FileShare.Read, 4096, FileOptions.RandomAccess);
        using var mapping = MemoryMappedFile.CreateFromFile(file, null, 0,
            MemoryMappedFileAccess.Read, HandleInheritability.None, true);
        using var view = mapping.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
        //using var unsafeMemoryManager =
        //    new UnsafeMemoryMappedViewMemory(view.SafeMemoryMappedViewHandle, checked((int)file.Length));
        //return This.Inspect(unsafeMemoryManager.Memory);

        unsafe {
            byte* ptr = null;
            view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
            try {
                return This.Inspect(new(ptr, checked((int)file.Length)));
            } finally {
                view.SafeMemoryMappedViewHandle.ReleasePointer();
            }
        }
    }
}
