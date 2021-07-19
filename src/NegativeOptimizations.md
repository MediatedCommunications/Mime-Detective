# Negative Optimizations
A negative optimization is a concept that, on the surface, seems as though it would
improve performance, but, after implementing, you discover that it actually does not.

The following are optimizations that have been tried but did not yield faster performance:
## Tree-Based Prefixes
Using the first four bytes of a all signature definitions,
build a tree that will allow quickly eliminating options.

This did not yield a significant performance boost.
Comparing a few bytes is quick and easy.  Navigating a tree structure
is quick too, but not as quick as simply doing a compare and eliminating the results.
Also, the tree structure had to be walked because prefix definitions
can contain holes.  For example, a four byte prefix might look like:
AA BB CC DD
AA BB ?? ??
?? BB ?? DD
?? ?? ?? ??

Because of this, a non-present value needed to be treated as its own
value and evaluated at all child depths.  For example, with the data above,
If you searched for:
00 BB AA DD
that would match the last two records.

And if you searched for:
AA BB CC DD
That would match all four records.




