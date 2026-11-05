using System.Text.Json;

Console.WriteLine("Welke plaats wil je opzoeken?");
string plaats = Console.ReadLine();

string url = $"https://nominatim.openstreetmap.org/search?format=json&q={plaats}";

using (HttpClient client = new HttpClient())
{
    // Nominatim vereist verplicht een User-Agent header
    client.DefaultRequestHeaders.Add("User-Agent", "MijnCsharpApp/1.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");

    try
    {
        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse de JSON data (gebruikt System.Text.Json)
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                JsonElement root = doc.RootElement;
                if (root.GetArrayLength() > 0)
                {
                    JsonElement firstResult = root[0];
                    string lat = firstResult.GetProperty("lat").GetString();
                    string lon = firstResult.GetProperty("lon").GetString();
                    string displayName = firstResult.GetProperty("display_name").GetString();

                    Console.WriteLine();
                    Console.WriteLine($"Locatie gevonden: {displayName}");
                    Console.WriteLine($"Latitude: {lat}");
                    Console.WriteLine($"Longitude: {lon}");
                }
                else
                {
                    Console.WriteLine("Geen resultaten gevonden voor deze plaats.");
                }
            }
        }
        else
        {
            Console.WriteLine($"Fout bij API-aanroep: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
    }
}
