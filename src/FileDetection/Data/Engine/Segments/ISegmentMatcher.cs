using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Engine
{

    /// <summary>
    /// The base interface for detecting if content matches a segment.
    /// </summary>
    internal interface ISegmentMatcher
    {
        SegmentMatch Match(ImmutableArray<byte> Content);
    }






}
