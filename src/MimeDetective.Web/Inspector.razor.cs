using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MimeDetective.Web;

public sealed partial class Inspector : ComponentBase {
    private const int MaxInspectedBytes = InspectorCatalog.MaxInspectedBytes;

    [Inject]
    public required InspectorCatalog Catalog { get; set; }

    private DefinitionSet _set = DefinitionSet.Minimal;
    private bool _busy;
    private bool _dragging;
    private int _dragDepth;
    private string? _status;
    private string? _error;
    private byte[]? _content;
    private string? _fileName;
    private long _fileSize;
    private string? _browserType;
    private InspectionReport? _report;
    private int _requestId;

    private string WorkingDetail => _set switch {
        DefinitionSet.Exhaustive => "The exhaustive library is loaded once, then kept for the rest of this visit.",
        DefinitionSet.Condensed => "Common signatures are being prepared.",
        _ => "The built-in signatures are being prepared.",
    };

    private async Task ChangeSet(DefinitionSet set) {
        if (_busy || _set == set) {
            return;
        }

        _set = set;
        if (_content is not null) {
            await InspectAsync();
        }
    }

    private void OnDragEnter() {
        _dragDepth++;
        _dragging = true;
    }

    private void OnDragLeave() {
        _dragDepth = Math.Max(0, _dragDepth - 1);
        _dragging = _dragDepth > 0;
    }

    private void OnDrop() {
        _dragDepth = 0;
        _dragging = false;
    }

    private async Task OnFile(InputFileChangeEventArgs args) {
        var files = args.GetMultipleFiles(1);
        if (files.Count == 0 || _busy) {
            return;
        }

        var file = files[0];

        var request = ++_requestId;
        _busy = true;
        _error = null;
        _report = null;
        _status = "Reading file…";
        await InvokeAsync(StateHasChanged);
        await Task.Delay(1);

        try {
            var content = await ReadPrefixAsync(file);
            if (request != _requestId) {
                return;
            }

            _content = content;
            _fileName = file.Name;
            _fileSize = file.Size;
            _browserType = file.ContentType;
            await InspectAsync(request);
        } catch (Exception ex) {
            if (request != _requestId) {
                return;
            }

            _error = ex.Message;
            _status = null;
            _busy = false;
        }
    }

    private async Task InspectAsync(int? request = null) {
        if (_content is null || _fileName is null) {
            return;
        }

        request ??= ++_requestId;
        _busy = true;
        _error = null;
        _report = null;
        _status = _set switch {
            DefinitionSet.Exhaustive => "Preparing exhaustive definitions…",
            DefinitionSet.Condensed => "Preparing condensed definitions…",
            _ => "Preparing default definitions…",
        };

        await InvokeAsync(StateHasChanged);
        await Task.Delay(1);
        if (request != _requestId || _content is null || _fileName is null) {
            return;
        }

        try {
            var inspector = Catalog.Get(_set);
            if (request != _requestId || _content is null || _fileName is null) {
                return;
            }

            _status = "Inspecting contents…";
            await InvokeAsync(StateHasChanged);
            await Task.Delay(1);
            if (request != _requestId || _content is null || _fileName is null) {
                return;
            }

            var content = _content;
            var fileName = _fileName;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var matches = inspector.Inspect(content);
            stopwatch.Stop();

            if (request != _requestId) {
                return;
            }

            _report = InspectionReport.Create(
                matches,
                fileName,
                _fileSize,
                content.Length,
                _browserType,
                _set,
                stopwatch.Elapsed);
            _status = null;
        } catch (Exception ex) {
            if (request != _requestId) {
                return;
            }

            _error = ex.Message;
            _status = null;
        } finally {
            if (request == _requestId) {
                _busy = false;
            }
        }
    }

    private static async Task<byte[]> ReadPrefixAsync(IBrowserFile file) {
        var length = (int)Math.Min(Math.Max(file.Size, 0), MaxInspectedBytes);
        await using var stream = file.OpenReadStream(maxAllowedSize: Math.Max(file.Size, 1));
        if (length == 0) {
            return [];
        }

        var buffer = new byte[length];
        var offset = 0;
        while (offset < buffer.Length) {
            var read = await stream.ReadAsync(buffer.AsMemory(offset));
            if (read == 0) {
                break;
            }

            offset += read;
        }

        return offset == buffer.Length ? buffer : buffer[..offset];
    }

    private static string Describe(DefinitionSet set) => set switch {
        DefinitionSet.Minimal => "A small built-in set: archives, documents, images, audio, video, and executables.",
        DefinitionSet.Condensed => "The most common file signatures, for personal, non-commercial use.",
        DefinitionSet.Exhaustive => "More than 14,000 signatures, for personal, non-commercial use. The first run loads the full library.",
        _ => string.Empty,
    };
}
