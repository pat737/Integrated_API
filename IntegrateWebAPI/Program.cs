using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

namespace APIProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Helper logger = new Helper(); // Create an instance of the Helper class for logging

            Console.WriteLine("Hi\n\nThis is our API Project which will allow you to check the sunrise, sunset and day length in the following countries:");
            Console.WriteLine("England");
            Console.WriteLine("Spain");
            Console.WriteLine("France");
            Console.WriteLine("\nThis information will help you to plan your day, holidays, or a romantic dinner in the sunset with loved ones.\n");

            string userInput;
            do
            {
                Console.WriteLine("To continue, please press Enter.");
                userInput = Console.ReadLine();

                if (userInput != "")
                {
                    Console.WriteLine("Invalid input. Please press Enter to continue.");
                }
            } while (userInput != "");

            logger.CreateLog("User pressed Enter to continue."); // Log that the user pressed Enter

            using HttpClient client = new HttpClient();

            string date;
            while (true)
            {
                Console.WriteLine("Enter the date (format YYYY-MM-DD): ");
                date = Console.ReadLine();

                if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, DateTimeStyles.None, out _))
                {
                    break;
                }

                Console.WriteLine("Invalid date format. Try again.");
                logger.CreateLog("Invalid date format entered by the user."); // Log invalid date format
            }

            int countryId = -1;
            while (countryId != 0 && countryId != 1 && countryId != 2)
            {
                Console.WriteLine("Select country: 0 for Spain, 1 for England, 2 for France:");
                string countryChoice = Console.ReadLine();

                if (int.TryParse(countryChoice, out countryId))
                {
                    if (countryId != 0 && countryId != 1 && countryId != 2)
                    {
                        Console.WriteLine("Invalid input. Please enter 0, 1, or 2.");
                        logger.CreateLog("Invalid country selection by the user."); // Log invalid selection
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 0, 1, or 2.");
                    logger.CreateLog("Non-numeric input for country selection."); // Log non-numeric input
                    countryId = -1; // Reset to ensure loop continues
                }
            }

            List<CountrySunTimes> countries = new List<CountrySunTimes>
            {
                new CountrySunTimes { CountryName = "Spain", GeoLatitude = "36.7201600", GeoLongitude = "-4.4203400" },
                new CountrySunTimes { CountryName = "England", GeoLatitude = "51.5074", GeoLongitude = "-0.1278" },
                new CountrySunTimes { CountryName = "France", GeoLatitude = "48.8566", GeoLongitude = "2.3522" }
            };

            string latitude = countries[countryId].GeoLatitude;
            string longitude = countries[countryId].GeoLongitude;
            string countryName = countries[countryId].CountryName;

            string apiUrl = "https://api.sunrise-sunset.org/json?lat=" + latitude + "&lng=" + longitude + "&date=" + date;

            logger.CreateLog($"Fetching data for {countryName} on {date} using URL: {apiUrl}"); // Log the API request

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ResponseData data = JsonConvert.DeserializeObject<ResponseData>(jsonResponse);

                    if (data != null && data.Results != null)
                    {
                        Console.WriteLine("\nSunrise and Sunset Information for " + countryName + ":");
                        Console.WriteLine("Sunrise: " + data.Results.Sunrise);
                        Console.WriteLine("Sunset: " + data.Results.Sunset);
                        Console.WriteLine("Day Length: " + data.Results.DayLength);
                        logger.CreateLog("Data successfully retrieved and displayed."); // Log successful data retrieval
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                        logger.CreateLog("No data found in the API response."); // Log no data found
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching data: " + response.StatusCode);
                    logger.CreateLog($"API request failed with status code: {response.StatusCode}"); // Log failed request
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                logger.CreateLog($"Exception occurred: {ex.Message}"); // Log exception
            }
        }
    }
}
