using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AgeCalculatorPlus.Services
{
    public class HolidayServices
    {
        private readonly HttpClient _httpClient = new();
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<HolidayServices> _logger;

        public HolidayServices(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, ILogger<HolidayServices> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("HolidayAPI");
        }

        public async Task<List<Holiday>> GetHolidayAsync(int year, string countryCode)
        {
            try
            {
                var cacheKey = $"{year}-{countryCode}";

                return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);
                
                    // 1. Monta a URL da API
                    var response = await _httpClient.GetAsync($"PublicHolidays/{year}/{countryCode}");

                    // 2. Verifica a resposta válida
                    response.EnsureSuccessStatusCode();

                    // Evita erro caso o conteúdo seja null
                    if (response.Content == null)
                    {
                        _logger.LogWarning("Resposta da API está vazia para {Year}-{CountryCode}", year, countryCode);
                        return new();
                    }

                    // Desserealiza o JSON para objetos c#
                    return await response.Content.ReadFromJsonAsync<List<Holiday>>() ?? new();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter feriados: {year}-{countryCode}", year, countryCode);
                return new();
            }
        }
    }

    public class Holiday
    {
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}