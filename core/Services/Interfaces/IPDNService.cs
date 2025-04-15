namespace core.Services.Interfaces
{
    public interface IPDNService
    {
        (List<CSignal>, List<Fault>) ReadPDNData(string pdnPath);
    }
}