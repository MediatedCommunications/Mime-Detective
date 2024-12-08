using System.Collections.Generic;

namespace System.Linq;

internal static class LinqExtensions {
    public static ParallelQuery<T> AsParallel<T>(this IEnumerable<T> source, bool parallel) {
        var ret = source.AsParallel();

        if (parallel) {
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
