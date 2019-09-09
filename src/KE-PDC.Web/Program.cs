using Microsoft.AspNetCore.Hosting;
using Syncfusion.Licensing;
using System.IO;

namespace KE_PDC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SyncfusionLicenseProvider.RegisterLicense("MjE5NzJAMzEzNjJlMzIyZTMwRWJSOUlwNS9jZjkvVEFjbUNJNnFNbWNNYmFZS3NWVTB0MFl3ZkhwVm9Udz0=");
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())                                 
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
} 