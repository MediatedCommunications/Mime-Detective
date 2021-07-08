using FileDetection.Data.Trid.v2;

namespace FileDetection.Data.Trid.v2
{
    public static class DefinitionExtensions
    {
        public static Engine.Definition Modernize(this Definition This)
        {
            return TridConverter.Convert(This);
        }
    }
}
