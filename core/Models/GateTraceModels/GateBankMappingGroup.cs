namespace core.Models.GateTraceModels;
  public class GateBankMappingGroup
  {
    public string? Name { get; set; }
    public string? IC { get; set; }
    public List<GateBankMapping> GateBankMappings { get; set; } = new List<GateBankMapping>();
  }
