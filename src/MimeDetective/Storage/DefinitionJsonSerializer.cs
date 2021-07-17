using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MimeDetective.Storage
{

    /// <summary>
    /// Read and write <see cref="Definition"/>s in raw JSON form.
    /// </summary>
    public static partial class DefinitionJsonSerializer
    {
        private static JsonSerializerOptions JsonOptions()
        {
            var ret = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
                Converters =
                {
                    new ImmutableByteArrayToHexStringConverter(),
                }
            };

            return ret;
        }

        public static Definition[] FromJson(string Content)
        {
            return FromJson(default, Content);
        }

        public static Definition[] FromJson(JsonSerializerOptions? Options, string Content)
        {
            var MyOptions = Options ?? JsonOptions();
            var ret = System.Text.Json.JsonSerializer.Deserialize<Definition[]>(Content, MyOptions)
                ?? Array.Empty<Definition>()
                ;

            return ret;
        }

        public static Definition[] FromJsonFile(string FileName)
        {
            return FromJson(default, FileName);
        }

        public static Definition[] FromJsonFile(JsonSerializerOptions? Options, string FileName)
        {
            var Content = System.IO.File.ReadAllText(FileName);

            var ret = FromJson(Options, Content);

            return ret;
        }


        public static string ToJson(params Definition[] Values)
        {
            return ToJson(default, Values.AsEnumerable());
        }

        public static string ToJson(JsonSerializerOptions? Options, params Definition[] Values)
        {
            return ToJson(Options, Values.AsEnumerable());
        }

        public static string ToJson(IEnumerable<Definition> Values)
        {
            return ToJson(default, Values);
        }

        public static string ToJson(JsonSerializerOptions? Options, IEnumerable<Definition> Values)
        {
            var MyOptions = Options ?? JsonOptions();

            var ret = System.Text.Json.JsonSerializer.Serialize(Values, MyOptions);

            return ret;
        }

        public static void ToJsonFile(string FileName, params Definition[] Values)
        {
            ToJsonFile(default, FileName, Values.AsEnumerable());
        }

        public static void ToJsonFile(JsonSerializerOptions? Options, string FileName, params Definition[] Values)
        {
            ToJsonFile(Options, FileName, Values.AsEnumerable());
        }

        public static void ToJsonFile(string FileName, IEnumerable<Definition> Values)
        {
            ToJsonFile(default, FileName, Values);
        }

        public static void ToJsonFile(JsonSerializerOptions? Options, string FileName, IEnumerable<Definition> Values)
        {
            var Content = ToJson(Options, Values);
            System.IO.File.WriteAllText(FileName, Content);
        }

    }
}
