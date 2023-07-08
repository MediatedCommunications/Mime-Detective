# Mime-Detective
Mime-Detective is a blazing-fast, low-memory file type detector for .NET.
It uses Magic-Number and Magic-Word signatures to accurately identify over
14,000 different file variants by analyzing a raw stream or array of bytes.
It also allows you to easily convert between file extensions and mime types.

# Overview
There are three main ways you can use Mime-Detective.
* The ```Default``` definition pack which includes a very small set of detection rules.
* The ```Condensed``` definition pack which includes an expanded set of detection rules.
* The ```Exhaustive``` definition pack which includes over 14,000 detection rules.

More information on these definitions is included toward the end of this file.

## Installing from Nuget
### Installing the ```Default``` (Small) Definition Pack
```bash
install-package Mime-Detective
```

### Installing the ```Condensed``` (Medium) Definition Pack
```bash
install-package Mime-Detective
install-package Mime-Detective.Definitions.Condensed
```

### Installing the ```Exhaustive``` (Large) Definition Pack
```bash
install-package Mime-Detective
install-package Mime-Detective.Definitions.Exhaustive
```

## Create the ```ContentInspector```
### Create the ```Default``` ```ContentInspector```
```csharp
using MimeDetective;
var Inspector = new ContentInspectorBuilder() {
    Definitions = Definitions.Default.All()
}.Build();
```

### Create the ```Condensed``` ```ContentInspector```
```csharp
using MimeDetective;
var Inspector = new ContentInspectorBuilder() {
    Definitions = new Definitions.CondensedBuilder() {
        UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial
    }.Build()
}.Build();
```

### Create the ```Exhaustive``` ```ContentInspector```
```csharp
using MimeDetective;
var Inspector = new ContentInspectorBuilder() {
    Definitions = new Definitions.ExhaustiveBuilder() {
        UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial
    }.Build()
}.Build();
```

## Inspect Content
Once you have a ```ContentInspector``` you can use it to inspect a stream, file, or array of bytes:

```csharp
var Results = Inspector.Inspect(ContentByteArray);
var Results = Inspector.Inspect(ContentStream);
var Results = Inspector.Inspect(ContentFileName);
```

## Group Results by File Extension or Mime Type
```csharp
var ResultsByFileExtension = Results.ByFileExtension();
var ResultsByMimeType = Results.ByMimeType();
```

# Definition Packs
Definition packs make it easy to expand or limit the number of definitions that the 
Inspector will use.  You can use one of the provided definition packs, create a limited
subset of a definition pack, or create entirely new definition packs from scratch.

## ```Default``` Definitions

The default definitions are included with the Mime-Detective nuget package
and are located in the ```MimeDetective.Definitions.Default``` static class.
You can create a copy of all definitions by calling ```MimeDetective.Definitions.Default.All()```
or just a limited subset by calling something like ```MimeDetective.Definitions.Default.FileTypes.Documents.All()```.

It can be used by anyone for any purpose and requires no additional licensing.

| Type          | Extensions
|---------------|-----------
|Archives       | ```7z bz2 gz rar tar zip```
|Audio          | ```flac m4a mid midi mp3 ogg wav```
|Cryptographic  | ```aes pkr skr```
|Documents      | ```doc docx dwg pdf ppt pptx rtf xls xlsx```
|Disk Images    | ```bin dmg iso toast vcd```
|Email Files    | ```eml pst```
|Executables    | ```dll exe elf coff```
|Images         | ```bmp gif ico jpeg jpg png psd tiff```
|Text           | ```txt```
|Video          | ```3gp flv mov mp4```
|Xml            | ```xml```

## Mime-Detective.Definitions.Condensed
```
install-package Mime-Detective.Definitions.Condensed
```

This is a condensed library containing the most common file signatures.
\
\
It is derived from the publicly available [TrID file signatures](https://mark0.net/soft-tridnet-e.html)
which may be used for personal/non-commercial use (free) or with a [paid commercial license](https://mark0.net) (usually around 300€).

Create a copy of these definitions by using the following code:
```
var AllDefintions = new Definitions.CondensedBuilder() { 
    UsageType = Data.Licensing.UsageType.PersonalNonCommercial //Change this to be your usage type
}.Build();
```


| Type          | Extensions
|---------------|-----------
|Audio          | ```aif cda mid midi mp3 mpa ogg wav wma wpl```
|Video          | ```3g2 3gp avi flv h264 m4v mkv mov mp4 mpg mpeg rm swf vob wmv```
|Archives       | ```7z arj cab deb pkg rar rpm tar.gz z zip```
|Disk Images    | ```bin dmg iso toast vcd```
|Email Files    | ```eml emlx msg oft ost pst vcf```
|Executables    | ```apk exe com jar msi```
|Fonts          | ```fnt fon otf ttf```
|Images         | ```ai bmp cur gif ico icns jpg jpeg png ps psd svg tif tiff```
|Presentations  | ```key odp pps ppt pptx```
|Spreadsheets   | ```ods xls xlsm xlsx```
|Documents      | ```doc docx odt pdf rtf tex wpd```

## Mime-Detective.Definitions.Exhaustive
```
install-package Mime-Detective.Definitions.Exhaustive
```

This library contains the exhaustive set of 14,000+ file signatures.
\
\
It is derived from the publicly available [TrID file signatures](https://mark0.net/soft-tridnet-e.html)
which may be used for personal/non-commercial use (free) or with a [paid commercial license](https://mark0.net) (usually around 300€).

Create a copy of these definitions by using the following code:
```csharp
var AllDefintions = new Definitions.ExhaustiveBuilder() { 
    UsageType = Data.Licensing.UsageType.PersonalNonCommercial //Change this to be your usage type
}.Build();
```

# Optimizing/Balancing Performance and Memory
The ```ContentInspector``` is designed to be a fast, high-speed utility.  In order to achieve
maximum performance and lowest memory usage, there are a few things you want to do.

## 1.  Trim the Data You Don't Need

If you are positive that a file is going to be one of a few different types, create a definition
set that only contains those definitions and trim out unnecessary fields.

```csharp
var AllDefintions = new Definitions.ExhaustiveBuilder() { 
    UsageType = Data.Licensing.UsageType.PersonalNonCommercial
}.Build();

var Extensions = new[]{
    "aif", "cda","mid", "midi","mp3", "mpa", "ogg","wav","wma", "wpl",
}.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase);

var ScopedDefinitions = AllDefinitions
    .ScopeExtensions(Extensions) //Limit results to only the extensions provided
    .TrimMeta() //If you don't care about the meta information (definition author, creation date, etc)
    .TrimDescription() //If you don't care about the description
    .TrimMimeType() //If you don't care about the mime type
    .ToImmutableArray()
    ;

var Inspector = new ContentInspectorBuilder() {
    Definitions = ScopedDefinitions,
}.Build();
```

## 2.  Slow Initialization = Fast Execution
When the ```ContentInspector``` is first built, it will perform optimizations to ensure fastest execution.
This is a tax best paid only once.  If you  have a list of files to analyze, build the Inspector once and reuse it. \
**Do not create a new Inspector every time you need to detect a single file.**

## 3.  Parallel = True/False
The ```ContentInspectorBuilder.Parallel``` option controlls whether multiple threads will be used
to perform detections.  If you have lots of definitions or want to make optimal usage of your CPU, this should be set to ```true```.
If you have a low number of definitions or you want more balanced CPU usage, set this to ```false```.

## 4.  Read Definitions Once
Materializing definitions causes a new instance of each definition to be created.  If you are going to use the 
same definitions for multiple purposes, load them once and reuse them.
```csharp
var AllDefintions = new Definitions.ExhaustiveBuilder() { 
    UsageType = Data.Licensing.UsageType.PersonalNonCommercial
}.Build();

var Inspector = new ContentInspectorBuilder() {
    Definitions = AllDefintions,
}.Build();

var MimeTypeToFileExtensions = new MimeTypeToFileExtensionLookupBuilder() {
    Definitions = AllDefintions,
}.Build();

var FileExtensionToMimeTypes = new FileExtensionToMimeTypeLookupBuilder() {
    Definitions = AllDefintions,
}.Build();


```
## Benchmark
``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 2700, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  Job-QGXQKV : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT

Platform=X64  Runtime=.NET 6.0  

```
|     Method |             TestFile |         Mean |      Error |     StdDev |       Median |  Ratio | RatioSD |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|----------- |--------------------- |-------------:|-----------:|-----------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|----------:|
|    **Default** | **MindM(...)x.xml [31]** |     **2.757 ms** |  **0.0533 ms** |  **0.0692 ms** |     **2.752 ms** |   **1.00** |    **0.00** |   **644.5313** |   **550.7813** |  **515.6250** |     **10 MB** |
|  Condensed | MindM(...)x.xml [31] |   106.056 ms |  2.0622 ms |  2.3748 ms |   106.228 ms |  38.41 |    1.19 |  4000.0000 |  2000.0000 | 1000.0000 |     37 MB |
| Exhaustive | MindM(...)x.xml [31] | 1,056.068 ms | 18.2700 ms | 17.0898 ms | 1,058.858 ms | 385.27 |   10.59 | 36000.0000 | 15000.0000 | 4000.0000 |    236 MB |
|            |                      |              |            |            |              |        |         |            |            |           |           |
|    **Default** |         **MixedExe.exe** |     **9.583 ms** |  **0.8167 ms** |  **2.4080 ms** |    **10.818 ms** |   **1.00** |    **0.00** |  **1109.3750** |  **1023.4375** |  **976.5625** |     **11 MB** |
|  Condensed |         MixedExe.exe |   115.825 ms |  2.2480 ms |  3.2951 ms |   115.130 ms |  12.74 |    4.48 |  4750.0000 |  2500.0000 | 1250.0000 |     38 MB |
| Exhaustive |         MixedExe.exe | 1,048.981 ms | 20.4143 ms | 31.1748 ms | 1,054.640 ms | 114.04 |   39.50 | 36000.0000 | 15000.0000 | 4000.0000 |    236 MB |
|            |                      |              |            |            |              |        |         |            |            |           |           |
|    **Default** |     **imagesBy7zip.zip** |    **56.191 ms** |  **0.3438 ms** |  **0.3216 ms** |    **56.173 ms** |   **1.00** |    **0.00** |   **900.0000** |   **800.0000** |  **700.0000** |     **25 MB** |
|  Condensed |     imagesBy7zip.zip |   165.268 ms |  1.9812 ms |  1.7563 ms |   165.071 ms |   2.94 |    0.04 |  6000.0000 |  4000.0000 | 3000.0000 |     52 MB |
| Exhaustive |     imagesBy7zip.zip | 1,133.915 ms | 14.8815 ms | 13.9202 ms | 1,137.890 ms |  20.18 |    0.27 | 35000.0000 | 14000.0000 | 4000.0000 |    251 MB |
|            |                      |              |            |            |              |        |         |            |            |           |           |
|    **Default** | **micro(...)f.pdf [23]** |     **5.161 ms** |  **0.1056 ms** |  **0.3046 ms** |     **5.161 ms** |   **1.00** |    **0.00** |  **1109.3750** |  **1023.4375** |  **976.5625** |     **12 MB** |
|  Condensed | micro(...)f.pdf [23] |   118.030 ms |  1.9950 ms |  1.8661 ms |   118.048 ms |  23.07 |    1.83 |  4800.0000 |  2400.0000 | 1200.0000 |     39 MB |
| Exhaustive | micro(...)f.pdf [23] | 1,054.044 ms | 19.6671 ms | 18.3966 ms | 1,059.507 ms | 205.84 |   13.67 | 36000.0000 | 15000.0000 | 4000.0000 |    238 MB |
|            |                      |              |            |            |              |        |         |            |            |           |           |
|    **Default** |             **test.bmp** |    **13.465 ms** |  **0.3585 ms** |  **1.0572 ms** |    **13.742 ms** |   **1.00** |    **0.00** |  **1046.8750** |   **953.1250** |  **921.8750** |     **22 MB** |
|  Condensed |             test.bmp |   119.457 ms |  2.3831 ms |  5.8904 ms |   119.411 ms |   9.12 |    0.96 |  4666.6667 |  2333.3333 | 1333.3333 |     49 MB |
| Exhaustive |             test.bmp | 1,067.179 ms | 20.8197 ms | 19.4748 ms | 1,069.120 ms |  87.67 |    3.37 | 36000.0000 | 15000.0000 | 4000.0000 |    254 MB |
|            |                      |              |            |            |              |        |         |            |            |           |           |
|    **Default** |           **wavVLC.wav** |     **8.037 ms** |  **0.2070 ms** |  **0.6104 ms** |     **8.015 ms** |   **1.00** |    **0.00** |   **765.6250** |   **687.5000** |  **640.6250** |     **15 MB** |
|  Condensed |           wavVLC.wav |   123.661 ms |  1.4984 ms |  1.3283 ms |   123.824 ms |  15.39 |    0.84 |  5000.0000 |  3000.0000 | 2000.0000 |     45 MB |
| Exhaustive |           wavVLC.wav | 1,054.265 ms | 20.5831 ms | 20.2153 ms | 1,060.961 ms | 132.62 |    8.27 | 36000.0000 | 15000.0000 | 4000.0000 |    243 MB |
