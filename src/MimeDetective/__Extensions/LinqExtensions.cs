using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq {
    internal static class LinqExtensions {
        public static ParallelQuery<T> AsParallel<T>(this IEnumerable<T> Source, bool Parallel) {
            var ret = Source.AsParallel();

            if (Parallel) {
                ret = ret
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                    ;
            } else {
                ret = ret.WithExecutionMode(ParallelExecutionMode.Default)
                    .WithDegreeOfParallelism(1)
                    ;
            }

            return ret;
        }

    }
}
