# MC.FileDetection
MC.FileDetection is a blazing-fast, low-memory file type detector for .NET.
It uses Magic-Number and Magic-Word signatures to accurately identify over
14,000 different file variants by analyzing a raw stream or array of bytes.

## Installing from Nuget
Install MC.FileDetection:
```
install-package MC.FileDetection
```
Then install one of two definition files below.


## Getting Started
```
using FileDetection;

//Create a new engine and use the Small set.
var Engine = new FileDetectionEngine()
{
    Definitions = Data.Small.Definitons()
};


//Read content from a file.
//You could also use System.IO.File.ReadAllBytes(FileName) but this is more efficient.
var Content = ContentReader.Default.ReadFromFile(FileName);

//Analyze the content and get results.
var Results = Engine.Detect(Content);

//Group the results by file extension
var ResultsByExtension = Results.ByFileExtension();

//Or group the results by mime type
var ResultsByMimeType = Results.ByMimeType();

```

# MC.FileDetection.Data.Small
```
install-package MC.FileDetection.Data.Small
```

This is a condensed library containing the most common file signatures:

| Type          | Extensions
|---------------|-----------
|Audio          | ```aif cda mid midi mp3 mpa ogg wav wma wpl```
|Video          | ```3g2 3gp avi flv h264 m4v mkv mov mp4 mpg mpeg rm swf vob wmv```
|Archives       | ```7z  arj cab deb pkg rar rpm tar.gz z zip```
|Disk Images    | ```bin dmg iso toast vcd```
|Email Files    | ```eml emlx msg oft ost pst vcf```
|Executables    | ```apk exe com jar msi```
|Fonts          | ```fnt fon otf ttf```
|Images         | ```ai bmp cur gif ico icns jpg jpeg png ps psd svg tif tiff```
|Presentations  | ```key odp pps ppt pptx```
|Spreadsheets   | ```ods xls xlsm xlsx```
|Documents      | ```doc docx odt pdf rtf tex wpd```



# MC.FileDetection.Data.Large
```
install-package MC.FileDetection.Data.Small
```

This library contains the exhaustive set of 14,000+ file signatures.
