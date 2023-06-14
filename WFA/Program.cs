using Application.IServices;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using WFA.Ui;

namespace WFA
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            using var provider = services.BuildServiceProvider();
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.Run(provider.GetRequiredService<MainForm>());
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IPingService, PingService>();
            services.AddSingleton<IIpAddressService, IpAddressService>();
            services.AddSingleton<IDnsService, DnsService>();
            services.AddSingleton<MainForm>();
        }
    }
}