namespace BlockStorm.Arb.UpdateTokenPriceSvc
{
    using Microsoft.Extensions.Hosting;
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options=>
                {
                    options.ServiceName = "��ʱ����Token�۸�";

                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}