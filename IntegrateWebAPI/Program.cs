using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SunriseSunsetAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using HttpClient client = new HttpClient();

            // Taking data from user in format YYYY-MM-DD
            Console.WriteLine("Set a date (in format YYYY-MM-DD): ");
            string date = Console.ReadLine();

            // URL with API for dynamic date
            string url = "https://api.sunrise-sunset.org/json?lat=36.7201600&lng=-4.4203400&date=" + date;

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Show original JSON message
                    Console.WriteLine("Original JSON:");
                    Console.WriteLine(jsonResponse);

                    // Deserializacja JSON do obiektu ResponseData
                    var apiResponse = JsonConvert.DeserializeObject<ResponseData>(jsonResponse);

                    if (apiResponse != null && apiResponse.Results != null)
                    {
                        // Wyświetlenie tylko wybranych informacji
                        Console.WriteLine("\nInfo:");
                        Console.WriteLine($"Sunrise time: {apiResponse.Results.Sunrise}");
                        Console.WriteLine($"Sunset time: {apiResponse.Results.Sunset}");
                        Console.WriteLine($"Day lenght: {apiResponse.Results.DayLength}");
                    }
                    else
                    {
                        Console.WriteLine("Nie udało się pobrać wyników z API.");
                    }
                }
                else
                {
                    Console.WriteLine($"Błąd: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
        }
    }

    // Klasa do przechowywania wyników z JSON
    public class Results
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }

        [JsonProperty("day_length")]
        public string DayLength { get; set; }
    }

    public class ResponseData
    {
        [JsonProperty("results")]
        public Results Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

