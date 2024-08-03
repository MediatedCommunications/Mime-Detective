namespace MimeDetective.Benchmark.Support;

public sealed class BenchmarkParameter<T>(string name, T value) {
    public string Name { get; } = name;
    public T Value { get; } = value;

    public override string ToString() {
        return this.Name;
    }
}
