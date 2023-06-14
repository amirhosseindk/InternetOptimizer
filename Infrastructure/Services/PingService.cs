using Application.IServices;
using System.Net.NetworkInformation;

namespace Infrastructure.Services
{
    public class PingService : IPingService
    {
        public async Task<long> PingAsync(string ipAddress)
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ipAddress);
            return reply.RoundtripTime;
        }
    }
}
