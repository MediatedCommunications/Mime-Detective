using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;

namespace MimeDetective.Engine {
    internal abstract class StringSegmentMatcherProvider {
        public abstract StringSegmentMatcher CreateMatcher(StringSegment Segment);
    }

}
