# Mime-Detective
Mime-Detective is a blazing-fast, low-memory file type detector for .NET.
It uses Magic-Number and Magic-Word signatures to accurately identify over
14,000 different file variants by analyzing a raw stream or array of bytes.
It also allows you to easily convert between file extensions and mime types.

## Installing from Nuget
```
install-package Mime-Detective
```
Mime-Detective includes a default set of definitions and can be expanded using 
one of the definition packs below.

## Getting Started

Create a new Inspector and use the Micro definition pack:
```
using MimeDetective;
var Inspector = new ContentInspectorBuilder() {
    Definitions = Definitions.Default.All()
}.Build();
```


Alternatively, use the Large definition pack:
```
var Inspector = new ContentInspectorBuilder() {
    Definitions = new Definitions.ExhaustiveBuilder() {
        UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial
    }.Build()
}.Build();
```


Then read content from a file or stream:
```

//This is more efficient than using System.IO.File.ReadAllBytes(FileName).

var Content = ContentReader.Default.ReadFromFile(FileName);
var Content = ContentReader.Default.ReadFromStream(Stream);
```


Analyze the content and group the results by either file extension or mime type:
```
var Results = Inspector.Inspect(Content);

var ResultsByFileExtension = Results.ByFileExtension();
var ResultsByMimeType = Results.ByMimeType();
```

# Definition Packs
Definition packs make it easy to expand or limit the number of definitions that the 
Inspector will use.  You can use one of the provided definition packs, create a limited
subset of a definition pack, or create entirely new definition packs from scratch.

## Default Definitions

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
```
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

```
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
```
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

