using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrateWebAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Tworzenie klienta HTTP
            using HttpClient client = new HttpClient();

            // URL do API Sunrise and Sunset (używamy przykładowych współrzędnych dla Malagi w Hiszpanii)
            string url = "https://api.sunrise-sunset.org/json?lat=36.7201600&lng=-4.4203400&date=today";

            try
            {
                // Wysyłanie żądania HTTP GET
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Odczytywanie odpowiedzi jako string JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Wyświetlanie surowej odpowiedzi JSON
                    Console.WriteLine("Odpowiedź API:");
                    Console.WriteLine(jsonResponse);
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
}
