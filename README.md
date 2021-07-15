# MC.FileDetection
MC.FileDetection is a blazing-fast, low-memory file type detector for .NET.
It uses Magic-Number and Magic-Word signatures to accurately identify over
14,000 different file variants by analyzing a raw stream or array of bytes.

## Installing from Nuget
```
install-package MC.FileDetection
```
Then install one of the definition packs below.


## Getting Started

Create a new engine and use the Micro definition pack:
```
using FileDetection;
var Engine = new ContentDetectionEngineBuilder() {
    Definitions = Data.Micro.All
}.Build();
```


Alternatively, use the Large definition pack:
```
var Engine = new ContentDetectionEngineBuilder() {
    Definitions = new Data.LargeBuilder() {
        UsageType = Data.Licensing.UsageType.PersonalNonCommercial
    }.Build()
}.Build();
```


Read content from a file.
```
//You could also use System.IO.File.ReadAllBytes(FileName) but this is more efficient.
var Content = ContentReader.Default.ReadFromFile(FileName);
```


Analyze the content and get results.
```
var Results = Engine.Detect(Content);
```

Group the results by file extension:
```
var ResultsByExtension = Results.ByFileExtension();
```

Or group the results by mime type:
```
var ResultsByMimeType = Results.ByMimeType();
```
# Definition Packs
Definition packs make it easy to expand or limit the number of definitions that the 
engine will use.  You can use one of the provided definition packs, create a limited
subset of a definition pack, or create entirely new definition packs from scratch.

## MC.FileDetection.Data.Micro
```
install-package MC.FileDetection.Data.Micro
```

This is a small library of definitions.
\
\
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

## MC.FileDetection.Data.Small
```
install-package MC.FileDetection.Data.Small
```

This is a condensed library containing the most common file signatures.
\
\
It is derived from the publicly available [TrID file signatures](https://mark0.net/soft-tridnet-e.html)
which may be used for personal/non-commercial use (free) or with a paid commercial license.

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

## MC.FileDetection.Data.Large
```
install-package MC.FileDetection.Data.Small
```

This library contains the exhaustive set of 14,000+ file signatures.
\
\
It is derived from the publicly available [TrID file signatures](https://mark0.net/soft-tridnet-e.html)
which may be used for personal/non-commercial use (free) or with a paid commercial license.

# Optimizing for Performance
The ```ContentDetectionEngine``` is designed to be a fast, high-speed utility.  In order to achieve
maximum performance and lowest memory usage, there are a few things you want to do.

## 1.  Trim the Data You Don't Need

If you are positive that a file is going to be one of a few different types, create a definition
set that only contains those definitions and trims out unnecessary fields.

```
var AllDefintions = new Data.LargeBuilder() { 
    UsageType = Data.Licensing.UsageType.CommercialPaid
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

var Engine = new ContentDetectionEngineBuilder() {
    Definitions = ScopedDefinitions,
}.Build();
```

## 2.  Slow Initialization = Fast Execution
When the ```ContentDetectionEngine``` is first built, it will perform optimizations to ensure fastest execution.
This is a tax best paid only once.  If you  have a list of files to analyze, build the engine once and reuse it. \
**Do not create a new engine every time you need to detect a single file.**