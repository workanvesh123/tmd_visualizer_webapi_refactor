namespace core.Models;
public class CSignal
{
  #region Fields
  private string? _bitGroup;
  private List<string>? _signalGroup;
  private List<string>? _bitName;
  private List<string>? _bitDescription;
  private List<string>? _bitDescriptionJP;
  private string? _description;
  private string? _percentConversion;
  private string? _scaledUnits;
  private string? _scaledUnitsCommissioning;
  private double? _scaleFactor;
  private Dictionary<string, string>? _enumGroup;
  #endregion

  #region Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  public CSignal()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  {
    _signalGroup = new List<string>();
    _bitName = new List<string>();
    _bitDescription = new List<string>();
    _bitDescriptionJP = new List<string>();
    _enumGroup = new Dictionary<string, string>();
    _scaledUnits = null;
    _scaleFactor = 1.0;
  }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  public CSignal(List<string>? signalGroup
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

 , List<string>? bitName, List<string>? bitDescription
 , List<string>? bitDescriptionJP, Dictionary<string, string>? enumGroup,
 string? scaledUnits, int? ScaleFactor = 1)
  {
    _signalGroup = signalGroup;
    _bitName = bitName;
    _bitDescription = bitDescription;
    _bitDescriptionJP = bitDescriptionJP;
    _enumGroup = enumGroup;
    _scaledUnits = scaledUnits;
    _scaleFactor = 1.0;
  }
  #endregion

  #region Properties
  /// <summary>
  /// Name of signal
  /// </summary>
  public string? SignalName { get; set; }
  /// <summary>
  /// Address of signal
  /// </summary>
  public string? Address { get; set; }
  /// <summary>
  /// Address of signal in uint
  /// </summary>
  public uint? AddressUint { get; set; }
  /// <summary>
  /// Data type of signal. By Default it is Word type
  /// </summary>
  public string? DataType { get; set; } = Defines.WordType;
  /// <summary>
  /// Display type indicates the type to be shown on display
  /// </summary>
  public string? DisplayType { get; set; }
  /// <summary>
  /// Array length for string signals
  /// </summary> 
  public int? ArrayLength { get; set; } = 0;
  /// <summary>
  /// Value of signal.This is obtained from drive
  /// </summary> 
  public object? Value { get; set; }
  /// <summary>
  /// Description of signal
  /// </summary>
  public string? Description
  {
    get => _description;
    set => _description = (null != value) ? value : "";
  }
  /// <summary>
  /// Type of signal which can be Parameter or Variable
  /// </summary>
  public Defines.SignalEnumType? SignalType { get; set; }
  /// <summary>
  /// Percent conversion type of signal
  /// </summary>
  public string? PercentConversion
  {
    get => _percentConversion;
    set => _percentConversion = value;
  }
  /// <summary>
  /// Scaling unit of signal
  /// </summary>
  public string? ScaledUnit
  {
    get => _scaledUnits;
    set => _scaledUnits = value;
  }

  /// <summary>
  /// Scaling unit of signal for Commissioning
  /// </summary>
  public string? ScaledUnit_Commissioning
  {
    get => _scaledUnitsCommissioning;
    set => _scaledUnitsCommissioning = value;
  }

  /// <summary>
  /// Signal value to be scaled based on Scale factor
  /// </summary>
  public double? ScaleFactor
  {
    get => _scaleFactor;
    set => _scaleFactor = value;
  }
  /// <summary>
  /// Indicates format to be shown like number of digits to be shown after decimal etc
  /// </summary>
  public string? FormatMask { get; set; }

  public Dictionary<string, string?>? EnumGroup
  {
    get => _enumGroup;
    set => _enumGroup = value;
  }

  /// <summary>
  /// null means not a bit group, otherwise expected to be name of bit group
  /// </summary>
  public string? BitGroup
  {
    get => _bitGroup;
    set
    {
      string? newBitGrpName = (null != value) ? value : "";
      if ((newBitGrpName.Length == 0) || (newBitGrpName != _bitGroup))
      {// clear bit descriptors since we dom't have or are changing bit group name 
        _bitName = new List<string>();
        _bitDescription = new List<string>();
        _bitDescriptionJP = new List<string>();
      }
      _bitGroup = newBitGrpName;
      if (_bitName?.Count == 0)
      {
        for (int i = 0; i < 16; i++)
        {
          //_bitName.Add($"bit{i} N.U.");
          _bitName.Add("NU");
          _bitDescription?.Add($"Not used");
          _bitDescriptionJP?.Add($"Not used JP");
        }
      }
    }
  }

  /// <summary>
  /// Gets or sets the signal group List. Nesting indicated by '//'.
  /// </summary>
  /// <value>
  /// The signal group List.
  /// </value>
  public List<string>? SignalGroup
  {
    get => _signalGroup;
    set => _signalGroup = value;
  }

  /// <summary>
  /// 16-element list naming bits 0-15 of a BitGroup
  /// </summary>
  public List<string>? BitNameList
  {
    get => _bitName;
    set
    {
      if (value?.Count != 16)
      {
        throw new System.ArgumentException($"{value} is invalid.  Must be a 16-element List<string>", "BitNameList");
      }
      _bitName = value;
    }
  }

  public void SetBitName(int bitNum, string bitName)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  SetBitName() expects bitNum to be in range 0-15", "bitNum");
    }
    // initialize list if empty
    if (_bitName?.Count == 0)
    {
      for (int i = 0; i < 16; i++)
      {
        _bitName.Add($"bit{i} N.U.");
      }
    }
    if(_bitName != null)
      _bitName[bitNum] = bitName;
  }

  public string GetBitName(int bitNum)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  BitName() expects bitNum to be in range 0-15", "bitNum");
    }

    return _bitName?[bitNum] != null ? _bitName[bitNum] : "";
  }

  /// <summary>
  /// 16-element list of bit descriptions (English).  Bits 0-15 of a BitGroup.  
  /// </summary>
  public List<string>? BitDescriptionList
  {
    get => _bitDescription;
    set
    {
      if (value?.Count != 16)
      {
        throw new System.ArgumentException($"{value} is invalid.  Must be a 16-element List<string>", "BitDescription");
      }
      _bitDescription = value;
    }
  }

  public void SetBitDescription(int bitNum, string? bitName)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  SetBitDescription() expects bitNum to be in range 0-15", "bitNum");
    }
    // initialize list if empty
    if (_bitDescription?.Count == 0)
    {
      for (int i = 0; i < 16; i++)
      {
        _bitDescription.Add($"Not used");
      }
    }
    if (_bitDescription?[bitNum] != null && bitName != null)
      _bitDescription[bitNum] = bitName;
  }


  public string? GetBitDescription(int bitNum)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  BitDescription() expects bitNum to be in range 0-15", "bitNum");
    }

    return (_bitDescription?[bitNum] != null) ? _bitDescription[bitNum] : "";
  }

  public List<string>? BitDescriptionListJP
  {
    get => _bitDescriptionJP;
    set
    {
      if (value?.Count != 16)
      {
        throw new System.ArgumentException($"{value} is invalid.  Must be a 16-element List<string>", "BitDescription");
      }
      _bitDescriptionJP = value;
    }
  }

  public void SetBitDescriptionJP(int bitNum, string? bitName)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  SetBitDescription() expects bitNum to be in range 0-15", "bitNum");
    }
    if (_bitDescriptionJP?.Count == 0)
    {
      for (int i = 0; i < 16; i++)
      {
        _bitDescriptionJP.Add($"Not used");
      }
    }
    if (_bitDescriptionJP != null)
      _bitDescriptionJP[bitNum] = bitName ?? "";
  }

  public string GetBitDescriptionJP(int bitNum)
  {
    if (bitNum < 0 || bitNum > 15)
    {
      throw new System.ArgumentException($"{bitNum} is invalid.  BitDescriptionJP() expects bitNum to be in range 0-15", "bitNum");
    }

    return _bitDescriptionJP != null ? _bitDescriptionJP[bitNum] : "";
  }
  public List<BitDefinition> BitDefinitionsList { get; set; } = new List<BitDefinition>();
  public List<BitDefinition> BitDefinitionsListJP { get; set; } = new List<BitDefinition>();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public CSignal(EventCounter? eventcounter, string? descriptionJP, string? rank, bool? hasBitGroupAltName)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    this.Eventcounter = eventcounter;
    this.DescriptionJP = descriptionJP;
    this.Rank = rank;
    this.HasBitGroupAltName = hasBitGroupAltName;

  }
  public EventCounter? Eventcounter { get; set; }
  public object RawValue { get; set; }
  public string? DescriptionJP { get; set; }
  public string? DefaultValue { get; set; }
  public string? Rank { get; set; }
  public string? BitGroupAltName { get; set; }
  public bool? HasBitGroupAltName { get; set; } = false;
  #endregion
}