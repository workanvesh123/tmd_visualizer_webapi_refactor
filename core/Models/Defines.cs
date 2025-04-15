namespace core.Models;

public static class Defines
{
    #region "Enum"
    /// <summary>
    /// Represents various types of signal
    /// </summary>
    public enum SignalEnumType
    {
        Parameter,
        Variable
    }
    #endregion

    #region Constants
    #region DatatypeConstants
    public const string FloatType = "Float";
    public const string IntType = "Integer";
    public const string DWordType = "DoubleWord";
    public const string WordType = "Word";
    public const string ByteType = "Byte";
    public const string HexType = "Hex";
    public const string RefType = "Reference";
    public const string StrType = "String";

    public const string Int16Type = "Int16";
    public const string Int8Type = "Int8";
    public const string Int32Type = "Int32";
    public const string Int64Type = "Int64";
    public const string UInt16Type = "UInt16";
    public const string UInt8Type = "UInt8";
    public const string UInt32Type = "UInt32";
    public const string UInt64Type = "UInt64";
    public const string Hex32Type = "Hex32";
    public const string Hex16Type = "Hex16";
    public const string DumType = "Dummy";
    #endregion
    #endregion
}