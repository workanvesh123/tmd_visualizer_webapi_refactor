namespace core.Models;
    public class DataItem
    {
        public string? Name { get; set; }
        public string? DataType { get; set; }
        public int Bytes { get; set; }
        public object? Value { get; set; }
        public CSignal? PDNSignal { get; set; }
        public byte[]? RawData { get; set; }
    }