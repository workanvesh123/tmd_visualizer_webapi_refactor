namespace core.Models.GateTraceModels;
  public class GateTraceInfo
  {
    public string? HSR { get; set; }
    public string? IC { get; set; }
    public int BankCount { get; set; }
    public int WordInSample { get; set; }
    public int SamplingTime { get; set; }
    public int SamplingTime2 { get; set; }
    public int SamplingCount { get; set; }
    public int FaultSampleCount { get; set; }
    public string? GateBankMappingGroup { get; set; }
    public int FaultDisplayPeriod { get; set; }
    public List<GateTraceBank>? GateTraceBanksList { get; set; }
  }