namespace core.Models;
public class Fault
{
  public int? FaultId { get; set; }
  public string? FaultName { get; set; }
  public string? FaultDescription { get; set; } = string.Empty;
  public string? FaultDescriptionJP { get; set; } = string.Empty;
  public string? FaultGroup { get; set; }
}