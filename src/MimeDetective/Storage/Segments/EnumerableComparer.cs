using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MimeDetective.Storage {

    internal class EnumerableComparer<TElement> : SequenceComparer<IEnumerable<TElement>, TElement> {

    }

}
