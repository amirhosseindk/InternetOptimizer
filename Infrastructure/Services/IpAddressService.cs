using Application.IServices;
using System.Net;
using System.Net.NetworkInformation;

namespace Infrastructure.Services
{
    public class IpAddressService : IIpAddressService
    {
        public string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address not found.");
        }

        public string GetPublicIpAddress()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    string publicIpAddress = webClient.DownloadString("https://api.ipify.org");
                    return publicIpAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string GetDefaultGatewayIpAddress()
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var gatewayAddress = networkInterface.GetIPProperties().GatewayAddresses;
                    if (gatewayAddress.Any())
                    {
                        foreach (var address in gatewayAddress)
                        {
                            if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return address.Address.ToString();
                            }
                        }
                    }
                }
            }
            return "No IPv4 gateway found.";
        }
    }
}
