using Application.IServices;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class DnsService : IDnsService
    {
        public async Task<string[]> GetActiveDnsServersAsync()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dns_servers.txt");
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "get_dns.ps1");
            string script = @"
                $activeNetworkAdapters = Get-CimInstance -Class Win32_NetworkAdapterConfiguration | Where-Object { $_.IPAddress -ne $null }
                foreach ($adapter in $activeNetworkAdapters) {
                    if ($adapter.DNSServerSearchOrder) {
                        foreach ($dns in $adapter.DNSServerSearchOrder) {
                            $dns | Out-File -FilePath '" + filePath + @"' -Append
                        }
                    } else {
                        'No DNS servers' | Out-File -FilePath '" + filePath + @"' -Append
                    }
                }
            ";
            try
            {
                File.WriteAllText(scriptPath, script);
                RunPowershellScript(scriptPath);
                await Task.Delay(TimeSpan.FromSeconds(1));
                string[] dnsServers = await File.ReadAllLinesAsync(filePath);
                File.Delete(filePath);
                return dnsServers;
            }
            catch (Exception ex)
            {
                string[] error = new string[1];
                error[0] = ex.Message;
                return error;
            }
        }


        public void SetDnsServers(string primaryDns, string secondaryDns)
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "set_dns.ps1");
            string script = @"
                $activeNetworkAdapters = Get-CimInstance -Class Win32_NetworkAdapterConfiguration | Where-Object { $_.IPAddress -ne $null }
                foreach ($adapter in $activeNetworkAdapters) 
                {
                    $adapter | Set-DnsClientServerAddress -ServerAddresses ('" + primaryDns + @"', '" + secondaryDns + @"')
                }
            ";
            File.WriteAllText(scriptPath, script);
            RunPowershellScript(scriptPath);
        }

        private void RunPowershellScript(string scriptPath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                Verb = "runas",
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true,
            });
        }
    }
}
