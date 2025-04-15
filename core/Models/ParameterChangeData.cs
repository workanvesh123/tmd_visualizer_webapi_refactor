namespace core.Models;
   public class ParameterChangeData
   {
      
       public int BankNo { get; set; }
       public string? User { get; set; }
       public string? ChangedTime { get; set; }
       public string? ParameterAddress { get; set; }
       public object? BeforeValue { get; set; }
       public object? AfterValue { get; set; }
       public string? Unit { get; set; }
   }