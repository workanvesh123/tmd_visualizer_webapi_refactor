namespace core.Services
{
    public class PDNService : IPDNService
    {
        public static Stopwatch sw = new Stopwatch();
        public static string? ProductName { get; set; }
        public static string? PDNPath { get; set; }
        public void WriteReturnValuesToFile(string pdnPath, string outputPath)
        {
            var result = ReadPDNData(pdnPath);
            var output = new
            {
                Signals = result.Item1,
                Faults = result.Item2
            };

            string json = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(outputPath, json);
        }
        public (List<CSignal>, List<Fault>) ReadPDNData(string? pdnPath = null)
        {
            Console.WriteLine($@"Read PDN - start");
            sw.Start();
            List<CSignal> signals = [];
            List<Fault> faults = [];
            pdnPath ??= PDNPath;
            if (string.IsNullOrEmpty(pdnPath) || !File.Exists(pdnPath))
            {
                throw new FileNotFoundException($"The file at path {pdnPath} was not found.");
            }
            try
            {
                string? defaultSignalType = Defines.SignalEnumType.Variable.ToString();
                if (File.Exists(pdnPath))
                {
                    using XmlReader reader = XmlReader.Create(pdnPath);
                    Dictionary<string, List<BitDefinition>> BitGroupDef = new();

                    #region ProductInfo
                    if (reader.ReadToFollowing("product"))
                        ProductName = reader?.GetAttribute("name");
                    #endregion

                    #region SignalDefaults
                    // signalDefaults.dataType
                    if (null != reader && reader.ReadToFollowing("signalDefaults"))
                    {
                        string? dataTypeFmFile = reader?.GetAttribute("dataType")?.ToUpper();
                        defaultSignalType = reader?.GetAttribute("type");
                        GlobalManager.SignalDefaultValue = reader?.GetAttribute("defaultValue");
                        GlobalManager.SignalDefaultRank = reader?.GetAttribute("rank");
                    }
                    else
                    {

                    }
                    #endregion

                    #region Signals
                    // read all signals
                    bool? nodeExists = reader?.ReadToFollowing("signal");
                    if (!(nodeExists.HasValue && nodeExists.Value))
                    {
                        throw new Exception("Unable to find signal definitions in PDN file");
                    }
                    while (reader?.Name == "signal")
                    {
                        CSignal signal = new CSignal()
                        {
                            SignalName = reader.GetAttribute("name"),
                            Address = reader.GetAttribute("address"),
                            AddressUint = Convert.ToUInt32(reader.GetAttribute("address"), 16),
                            Description = reader.GetAttribute("descriptionEnUS"),
                            DescriptionJP = reader.GetAttribute("descriptionJaJP"),
                            PercentConversion = reader.GetAttribute("percentType"),
                            SignalType = (null == reader.GetAttribute("type")) ? Defines.SignalEnumType.Variable
                            : Defines.SignalEnumType.Parameter
                        };

                        if (null != reader.GetAttribute("defaultValue"))
                            signal.DefaultValue = reader.GetAttribute("defaultValue");

                        if (null != reader.GetAttribute("rank"))
                            signal.Rank = reader.GetAttribute("rank");

                        if (null != reader.GetAttribute("dataType"))
                            signal.DataType = reader.GetAttribute("dataType");

                        if (null != reader.GetAttribute("displayType"))
                        {
                            signal.DisplayType = reader.GetAttribute("displayType");
                            if (signal.DisplayType == "String")
                                signal.ArrayLength = Convert.ToUInt16(reader.GetAttribute("arrayLength")) - 1;
                        }
                        signals.Add(signal);
                        // move to next signal node
                        reader.ReadToNextSibling("signal");
                    }
                    #endregion

                    #region BitGroups
                    // Read bitGroups
                    #region Assumptions
                    // NOTE: making the following assumptions that may need to be reevaluated
                    // 1) bitGroups is always before signalGroups, and percentConversions
                    // 2) bitGroupSignalDefaults noOfBits="16"
                    // 3) bitDefaults name="N.U." descriptionEnUS="Not used"
                    // 4) bitGroupSymbolDefaults readOnly="false"
                    // 5) <bitGroupSignals> comes before <bitGroupSymbols>
                    #endregion
                    nodeExists = reader?.ReadToFollowing("bitGroup");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("No <bitGroup> nodes were found in PDN file"); }
                    while (reader?.Name == "bitGroup")
                    {
                        if (null != reader.GetAttribute("name"))
                        {
                            List<BitDefinition> Bits = new List<BitDefinition>();
                            for (int i = 0; i < 16; i++)
                            {
                                Bits.Add(new BitDefinition()
                                { Idx = i, Name = $"bit{i} N.U.", Description = $"Not used" });
                            }
                            string? bitGroupName = reader?.GetAttribute("name");
                            reader?.ReadToDescendant("bitGroupSignals");  // read next node.  expect ot to be <bitGroupSignals>
                            if (reader?.Name != "bitGroupSignals")
                                throw new Exception($"Error in {MethodBase.GetCurrentMethod()}.  Did not find <bitGroupSignals> in <bitGroup> name=id ='{bitGroupName}'.");
                            // read bit nodes
                            Dictionary<int, string?> bitNames = new();
                            Dictionary<int, string?> bitDescriptions = new();
                            Dictionary<int, string?> bitDescriptionsJP = new();
                            reader.ReadToDescendant("bit");  // read next node.  expect ot to be <bit>
                            while (reader?.Name == "bit")
                            {
                                int? idx = int.Parse(reader?.GetAttribute("idx") ?? "-1");
                                string? name = reader?.GetAttribute("name");
                                string? descr = (null == reader?.GetAttribute("descriptionEnUS")) ? "" : reader?.GetAttribute("descriptionEnUS");
                                string? descrJP = (null == reader?.GetAttribute("descriptionJaJP")) ? "" : reader?.GetAttribute("descriptionJaJP");
                                bitNames.Add(idx.Value, name);
                                bitDescriptions.Add(idx.Value, descr);
                                bitDescriptionsJP.Add(idx.Value, descrJP);
                                Bits[idx.Value].Name = name;
                                Bits[idx.Value].Description = descr;
                                Bits[idx.Value].DescriptionJP = descrJP;
                                // move to next node
                                reader?.ReadToNextSibling("bit");
                            }
                            //Collecting Bit groups to use in faults
                            if (bitGroupName != null)
                                BitGroupDef.Add(bitGroupName, Bits);
                            // read bitGroupSymbols
                            reader?.ReadToNextSibling("bitGroupSymbols");
                            if (reader?.Name != "bitGroupSymbols")
                                throw new Exception($"Error in {MethodBase.GetCurrentMethod()}.  Did not find <bitGroupSymbols> in <bitGroup> name=id ='{bitGroupName}'.");
                            reader.ReadToDescendant("symbol");  // read to <bit> node. 
                            if (reader.Name != "symbol")
                                throw new Exception($"Error in {MethodBase.GetCurrentMethod()}.  Did not find <symbol> in <bitGroup> name=id ='{bitGroupName}'.");
                            while (reader.Name == "symbol")
                            {
                                CSignal? signalDef = signals.SingleOrDefault(s => s.SignalName == reader?.GetAttribute("name"));
                                if (signalDef != null)
                                {
                                    signalDef.BitGroup = bitGroupName;
                                    if (reader.GetAttribute("type") != null && reader.GetAttribute("type") == "In")
                                    {
                                        signalDef.BitGroupAltName = reader.GetAttribute("name");
                                        signalDef.HasBitGroupAltName = true;
                                    }
                                    foreach (KeyValuePair<int, string?> entry in bitNames)
                                    {
                                        signalDef.SetBitName(entry.Key, entry.Value ?? "");
                                        signalDef.SetBitDescription(entry.Key, bitDescriptions[entry.Key]);
                                        signalDef.SetBitDescriptionJP(entry.Key, bitDescriptionsJP[entry.Key]);
                                    }
                                }
                                // move to next symbol node
                                reader.ReadToNextSibling("symbol");
                            }
                        }
                        // move to next signal node
                        reader.ReadToNextSibling("bitGroup");
                    }
                    #endregion

                    #region PercentConversionDefaults
                    //Read default scaler
                    if (!(reader != null && reader.ReadToFollowing("percentConversionDefaults")))
                    {
                        throw new Exception($"Error in {MethodBase.GetCurrentMethod()}.  Did not find <percentConversionDefaults> node.");
                    }
                    string? percentValue = reader?.GetAttribute("percentValue");
                    double defaultScaler = double.Parse(percentValue ?? "1");
                    string? formatMask = reader?.GetAttribute("formatMask");
                    #endregion

                    #region PercentConversions
                    // Read percentConversions
                    Dictionary<string, Tuple<double, string, string>> pctConversions = new();
                    nodeExists = reader?.ReadToFollowing("percentConversion");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("Did not find <percentConversion> node in PDN file"); }
                    while (reader?.Name == "percentConversion")
                    {
                        string? pcType = reader?.GetAttribute("type");
                        double? pcScaler = (null == reader?.GetAttribute("percentValue")) ? defaultScaler : double.Parse(reader?.GetAttribute("percentValue") ?? "1");
                        string? pcUnits = (null == reader?.GetAttribute("display")) ? "" : reader?.GetAttribute("display");
                        string? pcMask = (null == reader?.GetAttribute("formatMask")) ? formatMask : reader.GetAttribute("formatMask");
                        pctConversions.Add(pcType ?? "", new Tuple<double, string, string>(pcScaler.Value, pcUnits ?? "", pcMask ?? ""));
                        // nove to next node
                        reader?.ReadToNextSibling("percentConversion");
                        GlobalManager.PercentConversions = pctConversions;
                    }
                    #endregion

                    #region ApplyConversionstoSignals
                    // now apply conversions to signals
                    foreach (var signal in signals)
                    {
                        signal.ScaleFactor = defaultScaler;
                        if ((signal.PercentConversion != null) && (pctConversions.ContainsKey(signal.PercentConversion)))
                        {
                            signal.ScaleFactor = pctConversions[signal.PercentConversion].Item1;
                            signal.ScaledUnit = pctConversions[signal.PercentConversion].Item2;
                            signal.ScaledUnit_Commissioning = "(%)(" + pctConversions[signal.PercentConversion].Item2 + ")";
                            signal.FormatMask = pctConversions[signal.PercentConversion].Item3;
                        }
                        else
                        {
                            var disptype = signal.DisplayType;
                            switch (disptype)
                            {
                                case Defines.IntType:
                                case Defines.WordType:
                                    signal.ScaledUnit = "D";
                                    signal.ScaledUnit_Commissioning = "(D)";
                                    break;
                                case Defines.HexType:
                                    signal.ScaledUnit = "H";
                                    signal.ScaledUnit_Commissioning = "(H)";
                                    break;
                                case Defines.StrType:
                                    signal.ScaledUnit = "C";
                                    signal.ScaledUnit_Commissioning = "(C)";
                                    break;
                                case Defines.RefType:
                                    signal.ScaledUnit = "Ref";
                                    signal.ScaledUnit_Commissioning = "(&)";
                                    break;
                                default:
                                    break;
                            }
                            //signal.ScaledUnit = signal.DataType.Substring(0, 1);
                            signal.ScaleFactor = defaultScaler;
                        }
                    }
                    #endregion

                    #region Faults
                    // Read Faults
                    nodeExists = reader?.ReadToFollowing("fault");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("Did not find <fault> node in PDN file"); }
                    while (reader?.Name == "fault")
                    {
                        Fault fault = new Fault();
                        fault.FaultId = int.Parse(reader?.GetAttribute("id") ?? "-1");
                        string? bitGroup = reader?.GetAttribute("bitGroup");
                        fault.FaultGroup = bitGroup;
                        int bitNo = int.Parse(reader?.GetAttribute("bitNumber") ?? "-1");
                        if (BitGroupDef?.Count > 0)
                        {
                            if (BitGroupDef.ContainsKey(bitGroup ?? ""))
                            {
                                BitDefinition? bit = BitGroupDef[bitGroup ?? ""].SingleOrDefault(b => b.Idx == bitNo);
                                if (bit != null)
                                {
                                    fault.FaultName = bit.Name;
                                    fault.FaultDescription = bit.Description;
                                    fault.FaultDescriptionJP = bit.DescriptionJP;
                                }
                            }
                        }
                        faults.Add(fault);
                        // nove to next node
                        reader?.ReadToNextSibling("fault");
                    }
                    faults.Add(new Fault()
                    {
                        FaultId = -1,
                        FaultDescription = "Manual Fault",
                        FaultDescriptionJP = "手動故障",
                        FaultName = "No detect"
                    });
                    #endregion

                    #region EnumValues
                    // Read enum values                     
                    nodeExists = reader?.ReadToFollowing("enum");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("No <enum> nodes were found in PDN file"); }
                    while (reader?.Name == "enum")
                    {
                        string? enumName = reader.GetAttribute("description");
                        CSignal? signalDef = signals.SingleOrDefault(s => s.SignalName == enumName);
                        if (signalDef != null)
                        {
                            reader.Read();
                            while (reader.NodeType == XmlNodeType.Whitespace)
                            {
                                reader.Read();
                            }
                            while (reader?.Name == "enumLiteral")
                            {
                                signalDef?.EnumGroup?.Add(reader?.GetAttribute("value") ?? "",
                                    reader?.GetAttribute("descriptionEnUS"));
                                // move to next node
                                nodeExists = reader?.ReadToNextSibling("enumLiteral");
                            }
                        }

                        // move to next enum node
                        reader?.ReadToNextSibling("enum");
                    }
                    #endregion

                    #region EventCounter
                    // Read EventCounter values                     
                    nodeExists = reader?.ReadToFollowing("eventCounters");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("No <eventCounter> nodes were found in PDN file"); }
                    string? unit;
                    if (reader != null && reader.ReadToFollowing("eventCounterDefaults"))
                        unit = reader?.GetAttribute("unit");
                    else
                        unit = "";
                    reader?.Read();
                    while (reader?.NodeType == XmlNodeType.Whitespace)
                    {
                        reader.Read();
                    }
                    int id = 0;
                    while (reader?.Name == "eventCounter")
                    {
                        string? SignalName = reader.GetAttribute("signalRef");
                        CSignal? signalDefinition = signals.SingleOrDefault(s => s.SignalName == SignalName);
                        if (signalDefinition != null)
                        {
                            id++;
                            EventCounter eventCounter = new EventCounter
                            {
                                Id = id,
                                ChannelNo = Convert.ToInt32(reader.GetAttribute("channel")),
                                SignalName = reader.GetAttribute("signalRef"),
                                USDescription = reader.GetAttribute("descriptionEnUS"),
                                JPDescription = reader.GetAttribute("descriptionJaJP"),
                                Unit = (signalDefinition.ScaledUnit == string.Empty) ? unit : signalDefinition.ScaledUnit
                            };
                            signalDefinition.Eventcounter = eventCounter;
                        }
                        // move to next eventCounter node
                        reader.ReadToNextSibling("eventCounter");
                    }
                    #endregion

                    #region ffts
                    List<FFT_PDN_Model>? fftList = new();
                    nodeExists = reader?.ReadToFollowing("ffts");
                    if (!(nodeExists.HasValue && nodeExists.Value)) { CustomException.LogCustomMessage("No <ffts> nodes were found in PDN file"); }
                    if (reader != null && reader.ReadToFollowing("fftDefaults"))
                        unit = reader.GetAttribute("unit")?.ToString();
                    reader?.Read();
                    while (reader?.NodeType == XmlNodeType.Whitespace)
                    {
                        reader.Read();
                    }
                    while (reader?.Name == "fft")
                    {
                        fftList.Add(new FFT_PDN_Model
                        {
                            Id = Convert.ToInt32(reader.GetAttribute("id")),
                            DescriptionEnUS = Convert.ToString(reader.GetAttribute("descriptionEnUS")),
                            DescriptionJaJP = Convert.ToString(reader.GetAttribute("descriptionJaJP")),
                            Unit = Convert.ToString(reader.GetAttribute("unit")),
                            Sampling = Convert.ToString(reader.GetAttribute("sampling"))
                        });

                        reader.ReadToNextSibling("fft");
                    }

                    GlobalManager.FFTList = fftList;
                    #endregion ffts

                    #region gatetrace
                    #region gatetraceobject

                    GateTraceObjects gto = new();
                    nodeExists = reader?.ReadToFollowing("gateTrace");
                    if (!(nodeExists.HasValue && nodeExists.Value))
                    {
                        CustomException.LogCustomMessage("No <gateTrace> nodes were found in the XML file");
                    }

                    string? noOfBits;
                    GateTraceBankDefaults gtbd = new GateTraceBankDefaults();
                    if (reader != null && reader.ReadToFollowing("gateTraceBankDefaults"))
                    {
                        noOfBits = reader.GetAttribute("noofBits");
                        gtbd.NoOfBits = string.IsNullOrEmpty(noOfBits) ? 0 : Convert.ToInt32(noOfBits);
                    }

                    gto.GateTraceBankDefault = gtbd;
                    gto.GateTraceInfoList = new List<GateTraceInfo>();

                    reader?.Read();
                    while (reader != null && reader.NodeType == XmlNodeType.Whitespace)
                        reader.Read();

                    while (reader?.Name == "gateTraceInfo")
                    {
                        GateTraceInfo gateTraceInfo = new GateTraceInfo
                        {
                            HSR = reader.GetAttribute("HSR"),
                            IC = reader.GetAttribute("IC"),
                            BankCount = Convert.ToInt32(reader.GetAttribute("bankCount")),
                            WordInSample = Convert.ToInt32(reader.GetAttribute("wordInSample")),
                            SamplingTime = Convert.ToInt32(reader.GetAttribute("samplingTime")),
                            SamplingTime2 = Convert.ToInt32(reader.GetAttribute("samplingTime2")),
                            SamplingCount = Convert.ToInt32(reader.GetAttribute("samplingCount")),
                            FaultSampleCount = Convert.ToInt32(reader.GetAttribute("faultSampleCount")),
                            GateBankMappingGroup = reader.GetAttribute("gateBankMappingGroup"),
                            FaultDisplayPeriod = Convert.ToInt32(reader.GetAttribute("faultDisplayPeriod")),
                            GateTraceBanksList = new List<GateTraceBank>()
                        };

                        reader.Read();
                        while (reader.NodeType == XmlNodeType.Whitespace)
                        {
                            reader.Read();
                        }

                        while (reader.NodeType == XmlNodeType.Element && reader.Name == "gateTraceBank")
                        {
                            GateTraceBank gateTraceBank = new GateTraceBank
                            {
                                Id = Convert.ToInt32(reader.GetAttribute("id")),
                                Bits = new List<Bit>()
                            };

                            reader.Read();
                            while (reader.NodeType == XmlNodeType.Whitespace)
                            {
                                reader.Read();
                            }

                            while (reader.NodeType == XmlNodeType.Element && reader.Name == "bit")
                            {
                                Bit bit = new Bit
                                {
                                    Idx = Convert.ToInt32(reader.GetAttribute("idx")),
                                    DescriptionEnUS = reader.GetAttribute("descriptionEnUS"),
                                    DescriptionJaJP = reader.GetAttribute("descriptionJaJP")
                                };

                                gateTraceBank.Bits.Add(bit);

                                reader.Read();
                                while (reader.NodeType == XmlNodeType.Whitespace)
                                {
                                    reader.Read();
                                }
                            }

                            gateTraceInfo.GateTraceBanksList.Add(gateTraceBank);

                            reader.Read();
                            while (reader.NodeType == XmlNodeType.Whitespace)
                            {
                                reader.Read();
                            }
                        }

                        gto.GateTraceInfoList.Add(gateTraceInfo);
                        reader.ReadToFollowing("gateTraceInfo");
                    }

                    GlobalManager.GateTraceObject = gto;
                    #endregion gatetraceobject

                    #region gatebankmappinggroups
                    GateBankMappingGroups gbmg = new GateBankMappingGroups();
                    List<GateBankMappingGroup> gateBankMappingGroups = new List<GateBankMappingGroup>();
                    nodeExists = reader?.ReadToFollowing("gateBankMappingGroups");
                    if (!(nodeExists.HasValue && nodeExists.Value))
                        CustomException.LogCustomMessage("No <gateBankMappingGroups> nodes were found in the XML file");
                    reader?.Read();
                    while (reader?.NodeType == XmlNodeType.Whitespace)
                        reader.Read();
                    while (reader?.NodeType == XmlNodeType.Element && reader.Name == "gateBankMappingGroup")
                    {
                        GateBankMappingGroup gateBankMappingGroup = new GateBankMappingGroup
                        {
                            Name = reader.GetAttribute("name"),
                            IC = reader.GetAttribute("IC")
                        };

                        reader.Read();
                        while (reader.NodeType == XmlNodeType.Whitespace)
                        {
                            reader.Read();
                        }
                        while (reader?.NodeType == XmlNodeType.Element && reader.Name == "gateBankMapping")
                        {
                            GateBankMapping gateBankMapping = new GateBankMapping
                            {
                                Id = Convert.ToInt32(reader.GetAttribute("id")),
                                DescriptionEnUS = reader.GetAttribute("descriptionEnUS"),
                                DescriptionJaJP = reader.GetAttribute("descriptionJaJP"),
                            };

                            reader.Read();
                            while (reader.NodeType == XmlNodeType.Whitespace)
                            {
                                reader.Read();
                            }
                            while (reader?.NodeType == XmlNodeType.Element && reader.Name == "gate")
                            {
                                Gate gate = new()
                                {
                                    Idx = Convert.ToInt32(reader.GetAttribute("idx")),
                                    Bank = Convert.ToInt32(reader.GetAttribute("bank")),
                                    Bit = Convert.ToChar(reader != null ? reader.GetAttribute("bit") : '0'),
                                };

                                gateBankMapping.Gates.Add(gate);

                                reader?.Read();
                                while (reader?.NodeType == XmlNodeType.Whitespace)
                                {
                                    reader.Read();
                                }
                            }

                            gateBankMappingGroup.GateBankMappings.Add(gateBankMapping);

                            reader?.Read();
                            while (reader?.NodeType == XmlNodeType.Whitespace)
                                reader.Read();
                        }

                        gateBankMappingGroups.Add(gateBankMappingGroup);

                        reader?.Read();
                        while (reader?.NodeType == XmlNodeType.Whitespace)
                        {
                            reader.Read();
                        }
                    }

                    gbmg.GateBankMappingGroupsList = gateBankMappingGroups;

                    GlobalManager.GateBankMappingGroupsList = gbmg;
                    #endregion gatebankmappinggroups
                    #endregion gatetrace
                }
            }
            catch (Exception a_Exception)
            {
                CustomException.LogExceptionInfo("Exception in method ReadPDNData", a_Exception);
            }
            sw.Stop();
            Console.WriteLine($@"total time - {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($@"signals - {signals.Count} && faults - {faults.Count}");
            Console.WriteLine($@"Read PDN - end");
            return (signals, faults);
        }
    }
}