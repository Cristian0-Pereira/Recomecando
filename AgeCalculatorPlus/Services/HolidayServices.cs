using System.Text.Json;

public class HolidayServices
{
    private readonly HttpClient _httpClient;

    public HolidayServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Holiday>> GetHolidays(int year)
    {
        var response = await _httpClient.GetAsync($"https://brasilapi.com.br/api/feriados/v1/{year}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Holiday>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Holiday>();
        }

        return new List<Holiday>();
    }
}

public class Holiday
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}
