using BlockStorm.BinanceConnector.Spot;
using BlockStorm.EFModels;
using Newtonsoft.Json;
using System.Text;

namespace BlockStorm.Arb.UpdateTokenPriceSvc
{
    public class Worker : BackgroundService
    {
        private readonly List<string> excludedTokens = new();

        
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            excludedTokens.Add("0xdAC17F958D2ee523a2206206994597C13D831ec7"); //USDT
            excludedTokens.Add("0x2b591e99afE9f32eAA6214f7B7629768c40Eeb39"); //HEX
            excludedTokens.Add("0xD46bA6D942050d489DBd938a2C909A5d5039A161"); //AMPL
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await UpdateTokenPriceAsync();
                await Task.Delay(1000*60*60, stoppingToken);
            }
        }

        async Task UpdateTokenPriceAsync()
        {
            var context = new BlockchainContext();
            var topTokens = context.Tokens.Where(t => t.IsTopToken == true).ToList();
            var sb = new StringBuilder();
            sb.Append('[');
            foreach ( var topToken in topTokens )
            {
                if (excludedTokens.Contains(topToken.TokenAddress)) continue;
                sb.Append("\"" + topToken.PriceSymbol.Trim() + "\",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(']');
            var httpClient = new HttpClient();
            var market = new Market(httpClient);
            var result = await market.SymbolPriceTicker(null, sb.ToString());
            List<SymbolPrice> sps = JsonConvert.DeserializeObject<List<SymbolPrice>>(result);
            foreach (var sp in sps)
            {
                var token = topTokens.Where(t => t.PriceSymbol.Trim() == sp.Symbol).FirstOrDefault();
                if (token == null) continue;
                token.PriceUsdt = sp.Price;
                token.LastUpdate=DateTime.Now;
                context.Tokens.Update(token);
            }
            context.SaveChanges();
        }
    }

    public class SymbolPrice
    {
        public string Symbol { get; set; }
        public Decimal Price { get; set; }
    }
}