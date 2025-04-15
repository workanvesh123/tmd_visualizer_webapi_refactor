namespace core.Models;
public class FaultHistory
{
    public int Id { get; set; }
    public string? FaultTime { get; set; }
    public int FirstFaultCode { get; set; }
    public int SecondFaultCode { get; set; }
    public string? FirstFaultName { get; set; }
    public string? SecondFaultName { get; set; }
} 