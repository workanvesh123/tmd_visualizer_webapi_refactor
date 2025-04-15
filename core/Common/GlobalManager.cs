namespace core.Common
{
    public static class GlobalManager
    {
        public static string? SignalDefaultValue { get; set; }
        public static string? SignalDefaultRank { get; set; }
        public static Dictionary<string, Tuple<double, string, string>> PercentConversions { get; set; } = [];
        public static List<FFT_PDN_Model>? FFTList { get; set; } = [];
        public static GateTraceObjects? GateTraceObject { get; set; } = new();
        public static GateBankMappingGroups? GateBankMappingGroupsList { get; set; } = new();

    }
}