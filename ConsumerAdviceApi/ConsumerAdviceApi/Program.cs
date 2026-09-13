using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsumerAdviceApi;

// Representa o objeto JSON retornado pela API:
// { "slip": { "id": 214, "advice": "Things are just things. Don't get too attached to them." } }
public class AdviceResponse
{
    [JsonPropertyName("slip")]
    public Slip? Slip { get; set; }
}

public class Slip
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("advice")]
    public string Advice { get; set; } = string.Empty;
}

public class Program
{
    private const string EndpointUrl = "https://api.adviceslip.com/advice";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando requisição para obter dados de um conselho:");
        Console.WriteLine();
        Console.WriteLine(EndpointUrl);
        Console.WriteLine();

        using HttpClient client = new HttpClient();

        try
        {
            // Faz a requisição GET para o endpoint da API
            HttpResponseMessage response = await client.GetAsync(EndpointUrl);
            response.EnsureSuccessStatusCode();

            string jsonResult = await response.Content.ReadAsStringAsync();

            // Desserializa o JSON retornado para o objeto AdviceResponse
            AdviceResponse? adviceResponse = JsonSerializer.Deserialize<AdviceResponse>(jsonResult);

            if (adviceResponse?.Slip is not null)
            {
                Console.WriteLine("Conselho de Hoje:");
                Console.WriteLine(adviceResponse.Slip.Advice);
            }
            else
            {
                Console.WriteLine("Não foi possível obter o conselho.");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro ao acessar a API: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao interpretar a resposta da API: {ex.Message}");
        }
    }
}
