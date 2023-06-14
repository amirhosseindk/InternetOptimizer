namespace Application.IServices
{
    public interface IDnsService
    {
        Task<string[]> GetActiveDnsServersAsync();
        void SetDnsServers(string primaryDns, string secondaryDns);
    }
}
