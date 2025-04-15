namespace core.Models;
 public class EventCounterData
 {
     public int ID { get; set; }
     public string? JPDescription { get; set; }
     public string? USDescription { get; set; }        
     public string? Count { get; set; }
     public string? Unit { get; set; }
     public string? LastResetTime { get; set; }
     public string? RawCount { get; set; }
 }