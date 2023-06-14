using Application.IServices;

namespace WFA.Ui
{
    public partial class MainForm : WFF.BaseForm.Forms
    {
        private readonly IPingService _pingService;
        private readonly IIpAddressService _ipAddressService;
        private readonly IDnsService _dnsService;
        private readonly System.Windows.Forms.Timer _timer;

        public MainForm(IPingService pingService, IIpAddressService ipAddressService, IDnsService dnsService)
        {
            InitializeComponent();
            _pingService = pingService;
            _ipAddressService = ipAddressService;
            _dnsService = dnsService;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 400;
            _timer.Tick += async (sender, e) => await UpdatePingLabels();
            _timer.Start();
        }

        private async Task UpdatePingLabels()
        {
            await UpdatePingLabel(DnsLabel1, "Google MiddleEast", "8.8.8.8");
            await UpdatePingLabel(DnsLabel2, "Google Europe", "4.2.2.4");
            await UpdatePingLabel(DnsLabel24, "Cloudflare", "1.1.1.1");
            await UpdatePingLabel(DnsLabel25, "Cloudflare", "1.0.0.1");
            await UpdatePingLabel(DnsLabel26, "OpenDNS", "208.67.222.222");
            await UpdatePingLabel(DnsLabel28, "OpenDNS", "208.67.220.220");
            await UpdatePingLabel(DnsLabel27, "Network", _ipAddressService.GetDefaultGatewayIpAddress());
            await UpdatePingLabel(DnsLabel3, "Shecan", "178.22.122.100");
            await UpdatePingLabel(DnsLabel4, "403", "10.202.10.202");
            await UpdatePingLabel(DnsLabel5, "Radar", "10.202.10.10");
            await UpdatePingLabel(DnsLabel6, "213", "213.202.254.131");
            await UpdatePingLabel(DnsLabel7, "213", "213.202.230.138");
            await UpdatePingLabel(DnsLabel8, "213", "213.202.225.16");
            await UpdatePingLabel(DnsLabel9, "213", "213.202.233.251");
            await UpdatePingLabel(DnsLabel10, "213", "213.202.230.29");
            await UpdatePingLabel(DnsLabel11, "213", "213.202.242.49");
            await UpdatePingLabel(DnsLabel12, "85", "85.114.136.180");
            await UpdatePingLabel(DnsLabel13, "85", "85.114.146.107");
            await UpdatePingLabel(DnsLabel14, "89", "89.163.140.33");
            await UpdatePingLabel(DnsLabel15, "89", "89.163.206.130");
            await UpdatePingLabel(DnsLabel16, "89", "89.163.140.152");
            await UpdatePingLabel(DnsLabel17, "89", "89.163.145.212");
            await UpdatePingLabel(DnsLabel18, "94", "94.130.133.90");
            await UpdatePingLabel(DnsLabel19, "94", "94.130.142.179");
            await UpdatePingLabel(DnsLabel20, "78", "78.31.67.178");
            await UpdatePingLabel(DnsLabel21, "81", "81.30.144.77");
            await UpdatePingLabel(DnsLabel22, "88", "88.99.1.168");
            await UpdatePingLabel(DnsLabel23, "193", "193.111.198.50");
            // 195.201.107.175
            // 176.9.29.122
        }

        private async Task UpdatePingLabel(Label label, string serverName, string serverIp)
        {
            long pingResult = await _pingService.PingAsync(serverIp);

            if (pingResult != 0)
                label.Text = $"{serverName}: {pingResult} ms";
            else
                label.Text = $"Faild to connect to {serverName}";

            if (pingResult == 0)
            {
                label.ForeColor = Color.Purple;
            }
            else if (pingResult < 40)
            {
                label.ForeColor = Color.Green;
            }
            else if (pingResult <= 110)
            {
                label.ForeColor = Color.LightGreen;
            }
            else if (pingResult <= 180)
            {
                label.ForeColor = Color.Yellow;
            }
            else
            {
                label.ForeColor = Color.Red;
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateIpAddresses();
            UpdateDnsServersAsync();
        }

        private async void UpdateDnsServersAsync()
        {
            var dnsServers = await _dnsService.GetActiveDnsServersAsync();

            if (dnsServers != null && dnsServers.Length > 0)
            {
                PdnsLabel.Text = $"Primary DNS: {dnsServers[0]}";
                SdnsLabel.Text = dnsServers.Length > 1 ? $"Secondary DNS: {dnsServers[1]}" : "Secondary DNS: Not Set";
            }
            else
            {
                PdnsLabel.Text = "Primary DNS: Not Set";
                SdnsLabel.Text = "Secondary DNS: Not Set";
            }

            if (dnsServers[0] == "178.22.122.100")
            {
                CurrentDns.Text = "Current DNS is : Shecan";
            }
            else if (dnsServers[0] == "10.202.10.10")
            {
                CurrentDns.Text = "Current DNS is : Radar";
            }
            else if (dnsServers[0] == "10.202.10.202")
            {
                CurrentDns.Text = "Current DNS is : 403";
            }
            else
            {
                CurrentDns.Text = "Cant Recegonize";
            }
        }

        private void UpdateIpAddresses()
        {
            var publicIpAddress = _ipAddressService.GetPublicIpAddress();
            var localIpAddress = _ipAddressService.GetLocalIpAddress();

            if (!string.IsNullOrWhiteSpace(publicIpAddress))
            {
                IPPLabel.Text = $"Public IP Address: {publicIpAddress}";
            }
            else
            {
                IPPLabel.Text = "Public IP Address: Not found";
            }

            if (!string.IsNullOrWhiteSpace(localIpAddress))
            {
                IPLLabel.Text = $"Local IP Address: {localIpAddress}";
            }
            else
            {
                IPLLabel.Text = "Local IP Address: Not found";
            }
        }

        private async void DnsSetBtn1_Click(object sender, EventArgs e)
        {
            _dnsService.SetDnsServers("178.22.122.100", "185.51.200.2");
            UpdateIpAddresses();
            await Task.Delay(TimeSpan.FromSeconds(1));
            UpdateDnsServersAsync();
        }

        private async void DnsSetBtn2_Click(object sender, EventArgs e)
        {
            _dnsService.SetDnsServers("10.202.10.10", "10.202.10.11");
            UpdateIpAddresses();
            await Task.Delay(TimeSpan.FromSeconds(1));
            UpdateDnsServersAsync();
        }

        private async void DnsSetBtn3_Click(object sender, EventArgs e)
        {
            _dnsService.SetDnsServers("10.202.10.202", "10.202.10.102");
            UpdateIpAddresses();
            await Task.Delay(TimeSpan.FromSeconds(1));
            UpdateDnsServersAsync();
        }
    }
}
