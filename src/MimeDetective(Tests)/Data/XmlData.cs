using MimeDetective.Storage.Xml.v2;

namespace MimeDetective.Tests;

public static class XmlData {
    public static string ExampleVersion => "1.20";

    public static string Pattern1Ascii => "12345678";
    public static string Pattern1Bytes => "AAFFCCDD";
    public static int Pattern1Position => 11;


    public static string Pattern2Ascii => "87654321";
    public static string Pattern2Bytes => "DDFFEECC";
    public static int Pattern2Position => 99;

    public static string GlobalString1 => "Global 1";
    public static string GlobalString2 => "Global 2";

    public static string CheckStrings => "True";
    public static string Creator => "Tony Valenti";
    public static int DateYear => 2020;
    public static int DateMonth => 1;
    public static int DateDay => 2;
    public static int DateHour => 3;
    public static int DateMinute => 4;
    public static int DateSecond => 5;

    public static string FileExtension => "exe";
    public static string FileDescription => "Applications";
    public static string FileMimeType => "Application/exe";

    public static Definition Example() {
        var example = new Definition {
            Version = ExampleVersion,
            FrontBlock = {
                new() {
                    ASCII = Pattern1Ascii,
                    Bytes = Pattern1Bytes,
                    Position = Pattern1Position
                },
                new() {
                    ASCII = Pattern2Ascii,
                    Bytes = Pattern2Bytes,
                    Position = Pattern2Position
                }
            },
            GlobalStrings = {
                GlobalString1,
                GlobalString2
            },
            General = {
                CheckStrings = CheckStrings,
                Creator = Creator,
                Date = {
                    Year = DateYear,
                    Month = DateMonth,
                    Day = DateDay
                },
                Time = {
                    Hour = DateHour,
                    Min = DateMinute,
                    Sec = DateSecond
                },
                FileNum = 12,
                Refine = "asd"
            },
            Info = {
                AuthorEmail = "Email",
                FileExtension = FileExtension,
                ExtraInfo = {
                    ReferenceUrl = "URL",
                    Remark = "Remark"
                },
                FileType = FileDescription,
                MimeType = FileMimeType,
                Author = "asdf"
            }
        };

        return example;
    }
}
