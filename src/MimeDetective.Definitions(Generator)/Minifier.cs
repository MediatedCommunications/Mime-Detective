using MimeDetective.Definitions;
using MimeDetective.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions
{
    [TestClass]
    public class Minifier
    {


        [TestMethod]
        public void Minify()
        {
            var A = new PrefixSegment()
            {
                Start = 1,
                Pattern = [2, 3, 4, 5, 6, 7, 8, 9, 10]
            };

            var B = new PrefixSegment()
            {
                Start = 2,
                Pattern = [3, 4, 0, 0, 7, 8, 9]
            };

            var Data = DefinitionExtensions.Intersection(A, B);

            Data.Equals(Data);

        }

    }
}
