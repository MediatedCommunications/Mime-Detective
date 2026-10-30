using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Storage;

namespace MimeDetective.Definitions.Generator
{
    [TestClass]
    public class Minifier
    {


        [TestMethod]
        public void Minify()
        {
            var A = new OffsetRule()
            {
                Start = 1,
                Value = [2, 3, 4, 5, 6, 7, 8, 9, 10,],
            };

            var B = new OffsetRule()
            {
                Start = 2,
                Value = [3, 4, 0, 0, 7, 8, 9,],
            };

            var Data = DefinitionExtensions.Intersection(A, B);

            Data.Equals(Data);

        }

    }
}
