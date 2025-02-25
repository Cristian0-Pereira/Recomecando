using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AgeCalculatorPlus.Services
{
    public class HolidayServices
    {
        private readonly HttpClient _client = new()
        {
            BaseAddress = new Uri("https://date.nager.at/api/v3/")
        };

        public async Task<List<Holiday>> GetHolidayAsync(int year, string countryCode)
        {
            try
            {
                // 1. Monta a URL da API
                var response = await _client.GetAsync($"PublicHolidays/{year}/{countryCode}");

                // 2. Verifica a resposta válida
                response.EnsureSuccessStatusCode();

                // 3. Desserealiza o JSON para objetos C#
                return await response.Content.ReadFromJsonAsync<List<Holiday>>() ?? new();
            }
            catch
            {
                // 4. Retorna uma lista vazia em caso de erro
                return new();
            }
        }

        public class Holiday
        {
            public DateTime Date { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}