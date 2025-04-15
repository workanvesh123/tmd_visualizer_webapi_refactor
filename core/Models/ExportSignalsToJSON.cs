namespace core.Models;
  public class ExportSignalsToJSON
  {
    public string signal_name { get; set; } = string.Empty;
    public Dictionary<object, object> attrs { get; set; } = new Dictionary<object, object>();
    public object? value{ get; set; }
    public string description_enus { get; set; } = string.Empty;
    public string description_jajp { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string unit { get; set; } = string.Empty;
    public string color { get; set; } = string.Empty;
    public int sort_id { get; set; }
  }
