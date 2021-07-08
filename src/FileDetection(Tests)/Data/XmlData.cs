using FileDetection.Data.Trid.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Tests
{

    public static class XmlData
    {
        
        public static string ExampleVersion => "1.20";

        public static string Pattern1_ASCII = "12345678";
        public static string Pattern1_Bytes = "AAFFCCDD";
        public static int Pattern1_Position = 11;


        public static string Pattern2_ASCII = "87654321";
        public static string Pattern2_Bytes = "DDFFEECC";
        public static int Pattern2_Position = 99;

        public static string GlobalString1 => "Global 1";
        public static string GlobalString2 => "Global 2";

        public static string CheckStrings => "True";
        public static string Creator => "Tony Valenti";
        public static int Date_Year => 2020;
        public static int Date_Month => 1;
        public static int Date_Day => 2;
        public static int Date_Hour => 3;
        public static int Date_Minute => 4;
        public static int Date_Second => 5;

        public static string File_Extension => "exe";
        public static string File_Description => "Applications";
        public static string File_MimeType => "Application/exe";

        public static Definition Example()
        {
            var Example = new Definition()
            {
                Version = ExampleVersion,
                FrontBlock =
                        {
                            new Pattern()
                            {
                                ASCII = Pattern1_ASCII,
                                Bytes = Pattern1_Bytes,
                                Position = Pattern1_Position,
                            },
                            new Pattern()
                            {
                                ASCII = Pattern2_ASCII,
                                Bytes = Pattern2_Bytes,
                                Position = Pattern2_Position,
                            }
                        },
                GlobalStrings =
                {
                    GlobalString1,
                    GlobalString2,
                },

                General = {
                    CheckStrings = CheckStrings,
                    Creator = Creator,
                    Date = {
                                Year = Date_Year,
                                Month = Date_Month,
                                Day = Date_Day,
                            },
                    Time =
                            {
                                Hour = Date_Hour,
                                Min = Date_Minute,
                                Sec = Date_Second,
                            },
                    FileNum = 12,
                    Refine = "asd",
                },
                Info =
                        {
                            AuthorEmail = "Email",
                            FileExtension = File_Extension,
                            ExtraInfo = {
                                ReferenceUrl = "URL",
                                Remark = "Remark",
                            },
                            FileType = File_Description,
                            MimeType = File_MimeType,
                            Author = "asdf",
                        }

            };

            return Example;
        }
        
    }
}
