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
                    options.ServiceName = "定时更新Token价格";

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