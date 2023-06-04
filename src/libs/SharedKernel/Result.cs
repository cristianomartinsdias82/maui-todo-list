namespace SharedKernel
{
    public record Result
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
    }
    public record Result<T> : Result
    {
        public T? Value { get; init; }
    }
}