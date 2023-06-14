namespace Application.IServices
{
    public interface IPingService
    {
        Task<long> PingAsync(string ipAddress);
    }
}
