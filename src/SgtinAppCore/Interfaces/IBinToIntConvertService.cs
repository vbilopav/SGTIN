namespace SgtinAppCore.Interfaces
{
    public interface IBinToIntConvertService
    {
        byte ToByte(string inputBin);
        ulong ToLong(string inputBin);
    }
}