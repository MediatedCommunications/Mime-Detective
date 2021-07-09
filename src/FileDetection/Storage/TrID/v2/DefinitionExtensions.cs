using FileDetection.Storage.Trid.v2;

namespace FileDetection.Storage.Trid.v2
{
    public static class DefinitionExtensions
    {
        public static Storage.Definition Modernize(this Definition This)
        {
            return TridConverter.Convert(This);
        }
    }
}
