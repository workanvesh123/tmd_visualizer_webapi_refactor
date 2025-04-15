namespace core.Models;
public class Word
{
  public int ID { get; set; }
  public string? Symbol { get; set; }
  public string Address
  {
    get
    {
      return "&H" + AddressInHex;
    }
  }
  public double PCTValue { get; set; }
  public string? DisplayChar { get; set; }
  public float MIN { get; set; }
  public float MAX { get; set; }
  public string? LongName { get; set; }
  public string? AddressInHex { get; set; }
  public string? DataType { get; set; }
  public UInt64 AddressInUInt64
  {
    get
    {
      return Convert.ToUInt64(AddressInHex, 16);
    }
  }
  public string? FormatMask { get; set; }
}