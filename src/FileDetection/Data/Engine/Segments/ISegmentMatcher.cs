using FileDetection.Data.Engine;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// The base interface for detecting if content matches a segment.
    /// </summary>
    public interface ISegmentMatcher
    {
        SegmentMatch Match(ImmutableArray<byte> Content);
    }






}
