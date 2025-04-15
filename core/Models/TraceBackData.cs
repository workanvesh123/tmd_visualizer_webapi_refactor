namespace core.Models;
public class TraceBackData
{
  //name                                                                       type       postion(hex)    postion(int)    size
  //Index datastrg confirmation//(0xFFFF：empty 0x0057：waveform)				 uint16     0x00000000 		 0		         2
  //Soft Version(8 characters)                        						 char       0x00000002       2               8
  //Database version                                  						 char       0x0000000A       10              2
  //Standard traceback Ch.1～70 //Symbol address                          	  	 uint32     0x0000000C       12              280    
  //High-speed traceback Ch.1～64 //Symbol address                        	  	 uint32     0x00000124       292             256    
  //Long Traceback Ch.1～32 //Symbol address                              	  	 uint32     0x00000224       548             128    
  //Standard traceback Ch.1～70  data(1024 sampling)                      	  	 uint32     0x000002A4       676             286720    
  //High-speed traceback Ch.1～64 //Waveform data(256 sampling)//() 			            0x000462A4       287396          65536
  //Long Traceback Ch.1～32 //Waveform data(1024 sampling)//					            0x000562A4 	     352932          131072
  //High-speed traceback sampling period[10cnt / us]//(256 samplings,          int16      0x000762A4       484004          512  
  //Standard traceback sampling period[1cnt / us]     						 int32      0x000764A4       484516          4
  //High-speed traceback sampling period[1cnt / us]   						 int32      0x000764A8       484520          4
  //Long traceback sampling period[1cnt / us]         						 int32      0x000764AC       484524          4
  //Sampling No.after Standard traceback trigger      						 int16      0x000764B0       484528          2
  //Sampling No. after High-speed traceback trigger   						 int16      0x000764B2       484530          2
  //Sampling No. after Long traceback trigger         						 int16      0x000764B4       484532          2
  //FI Code//(30 elements in order from the first fault)       				 int16      0x000764B6       484534          60
  //FI Code time series data//(30 elements in order from the first fault)      int16      0x000764F2       484594          60  
  //18(HOUR)                                            						 int16      0x0007652E       484654          2
  //19(MIN)                                             						 int16      0x00076530       484656          2
  //20(SEC)                                             						 int16      0x00076532       484658          2
  //21(MONTH)                                           						 int16      0x00076534       484660          2
  //22(DAY)                                             						 int16      0x00076536       484662          2
  //23(YEAR)                                            						 int16      0x00076538       484664          2
  //24(MSEC)                                            						 int16      0x0007653A       484666          2
  //Dummy Area(6,852Byte) ―                           						            0x0007653C       484668          6852
  public int Id { get; set; }
  public short Index { get; set; }
  public object? SoftwareVersion { get; set; }
  public object? DBVersion { get; set; }
  public object? StandardTraceBackSymbolAddress { get; set; }
  public object? HighSpeedTraceBackSymbolAddress { get; set; }
  public object? LongTraceBackSymbolAddress { get; set; }
  public object? StandardTraceBackWaveformData { get; set; }
  public object? HighSpeedTraceBackWaveformData { get; set; }
  public object? LongTraceBackWaveformData { get; set; }
  public object? HighSpeedTraceBacSamplingPeriod_10Cnt { get; set; }
  public object? StandardTraceBackSamplingPeriod_1Cnt { get; set; }
  public object? HighSpeedTraceBacSamplingPeriod_1Cnt { get; set; }
  public object? LongTraceBackSamplingPeriod_1Cnt { get; set; }
  public object? StandardTraceBack_SamplingNumber_AfterTrigger { get; set; }
  public object? HighSpeedTraceBack_SamplingNumber_AfterTrigger { get; set; }
  public object? LongTraceBack_SamplingNumber_AfterTrigger { get; set; }
  public object? FI_Code_TimeSeries { get; set; }
  public object? FI_Code { get; set; }
  public short Hour { get; set; }
  public short Minute { get; set; }
  public short Second { get; set; }
  public short Month { get; set; }
  public short Day { get; set; }
  public short Year { get; set; }
  public short MSEC { get; set; }
  public object? DummyArea { get; set; }

  //MVe3/10e3
  //name                                                                      |type        |postion(hex)          | postion(int)    |size
  //Index for data storage confirmation(0xFFFF：empty、0x0057：waveform )        uint16      0x00000000             0                   2 
  //Soft Version(8 characters)                                                   char        0x00000002             2                  8  
  //Database version                                                             char        0x0000000A            10                  2 
  //Standard Traceback Ch.1～70 Symbol address                                   uint32      0x0000000C            12                  280 
  //High-speed Traceback Ch.1～64 Symbol address                                 uint32      0x00000124            292                 256 
  //Long Traceback Ch.1～32 Symbol address                                       uint32      0x00000224            548                 128 
  //Standard Cell Traceback Ch.1～72 Symbol address                              uint32      0x000002A4            676                 288 
  //High-speed Cell Traceback Ch.1～76 Symbol address                            uint32      0x000003C4            964                 304 
  //Standard Traceback Ch.1～70 Waveform data(1024 sampling)                     uint32      0x000004F4            1268                286720 
  //High-speed Traceback Ch.1～64 Waveform data(256 sampling)                     uint32     0x000464F4            287988              65536 
  //Long Traceback Ch.1～32 Waveform data(1024 sampling)                          uint32     0x000564F4            353524              131072 
  //Standard Cell Traceback Ch.1～72 Waveform data(1024 sampling)                 uint32     0x000764F4            484596              294912 
  //High-speed Cell Traceback Ch.1～76 Waveform data(256 sampling)                uint32     0x000BE4F4            779508              77824 
  //High-speed Traceback Sampling Period[10cnt / us] (256 samplingsling)         int16       0x000D14F4            857332               512 
  //Standard traceback sampling period[1cnt / us]                                 int32      0x000D16F4            857844               4 
  //High-speed traceback sampling period[1cnt / us]                               int32      0x000D16F8            857848               4 
  //Long traceback sampling period[1cnt / us]                                     int32      0x000D16FC            857852               4 
  //Standard Cell traceback sampling period[1cnt / us]                            int32      0x000D1700            857856               4 
  //High-speed Cell traceback sampling period[1cnt / us]                          Int32      0x000D1704            857860               4 
  //Sampling No.after Standard traceback trigger                                  int16      0x000D1708            857864               2 
  //Sampling No. after High-speed traceback trigger                               int16      0x000D170A            857866               2 
  //Sampling No. after Long traceback trigger                                     int16      0x000D170C            857868               2 
  //Sampling No. after Standard Cell traceback trigger                            int16      0x000D170E            857870               2 
  //Sampling No. after High-speed Cell traceback trigger                          int16      0x000D1710            857872               2 
  //FI Code (30 elements in order from the first fault)                           int16      0x000D1712            857874               60 
  //FI Code time series data(30 elements in)                                      int16      0x000D174E            857934               60 
  //(HOUR)                                                                        int16      0x000D178A            857994               2 
  //(MIN)                                                                         int16      0x000D178C            857996               2 
  //(SEC)                                                                         int16      0x000D178E            857998               2 
  //(MONTH)                                                                       int16      0x000D1790            858000               2 
  //(DAY)                                                                         int16      0x000D1792            858002               2 
  //(YEAR)                                                                        int16      0x000D1794            858004               2 
  //(MSEC)                                                                        int16      0x000D1796            858006               2 
  //Dummy area(6,852Byte) ―                                                        --        0x000D1798            858008               26728 

  //70e3
  //name                                                                       type       postion(hex)    postion(int)    size
  //Index datastrg confirmation//(0xFFFF：empty 0x0057：waveform)				 uint16     0x00000000 		 0		         2
  //Soft Version(8 characters)                        						 char       0x00000002       2               8
  //Database version                                  						 char       0x0000000A       10              2
  //Standard traceback Ch.1～70 //Symbol address                          	  	 uint32     0x0000000C       12              280    
  //High-speed traceback Ch.1～64 //Symbol address                        	  	 uint32     0x00000124       292             256    
  //Long Traceback Ch.1～32 //Symbol address                              	  	 uint32     0x00000224       548             128    
  //Standard traceback Ch.1～70  data(1024 sampling)                      	  	 uint32     0x000002A4       676             286720    
  //High-speed traceback Ch.1～64 //Waveform data(256 sampling)//() 			            0x000462A4       287396          65536
  //Long Traceback Ch.1～32 //Waveform data(1024 sampling)//					            0x000562A4 	     352932          131072
  //High-speed traceback sampling period[10cnt / us]//(256 samplings,          int16      0x000762A4       484004          512  
  //Standard traceback sampling period[1cnt / us]     						 int32      0x000764A4       484516          4
  //High-speed traceback sampling period[1cnt / us]   						 int32      0x000764A8       484520          4
  //Long traceback sampling period[1cnt / us]         						 int32      0x000764AC       484524          4
  //Sampling No.after Standard traceback trigger      						 int16      0x000764B0       484528          2
  //Sampling No. after High-speed traceback trigger   						 int16      0x000764B2       484530          2
  //Sampling No. after Long traceback trigger         						 int16      0x000764B4       484532          2
  //FI Code//(30 elements in order from the first fault)       				 int16      0x000764B6       484534          60
  //FI Code time series data//(30 elements in order from the first fault)      int16      0x000764F2       484594          60  
  //18(HOUR)                                            						 int16      0x0007652E       484654          2
  //19(MIN)                                             						 int16      0x00076530       484656          2
  //20(SEC)                                             						 int16      0x00076532       484658          2
  //21(MONTH)                                           						 int16      0x00076534       484660          2
  //22(DAY)                                             						 int16      0x00076536       484662          2
  //23(YEAR)                                            						 int16      0x00076538       484664          2
  //24(MSEC)                                            						 int16      0x0007653A       484666          2
  //Dummy Area(6,852Byte) ―                           						            0x0007653C       484668          6852
  //Gate Trace Area                           						                    0x00078000       491520          262144
}