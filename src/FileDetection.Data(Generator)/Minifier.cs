using FileDetection.Data;
using FileDetection.Data.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data
{
    [TestClass]
    public class Minifier
    {


        [TestMethod]
        public void Minify()
        {
            var A = new BeginningSegment()
            {
                Start = 1,
                Pattern = new byte[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToImmutableArray()
            };

            var B = new BeginningSegment()
            {
                Start = 2,
                Pattern = new byte[] { 3, 4, 0, 0, 7, 8, 9 }.ToImmutableArray()
            };

            var Data = DefinitionExtensions.Intersection(A, B);

        }

    }
}
