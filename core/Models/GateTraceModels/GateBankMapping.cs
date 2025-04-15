namespace core.Models.GateTraceModels;
  public class GateBankMapping
  {
    public int Id { get; set; }
    public string? DescriptionEnUS { get; set; }
    public string? DescriptionJaJP { get; set; }
    public List<Gate> Gates { get; set; } = new List<Gate>();
  }