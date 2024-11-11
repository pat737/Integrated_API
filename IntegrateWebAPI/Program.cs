using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // tools for JSON
using System.Collections.Generic;
using System.Globalization; // tools for culture specific operations, we used it for date format

namespace APIProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Helper logger = new Helper(); // creating logger object

            //displaying message to the user along with the project description 
            Console.WriteLine("Hi\n\nThis is our API Project which will allow you to check the sunrise, sunset and day length in the following countries:");
            Console.WriteLine("England");
            Console.WriteLine("Spain");
            Console.WriteLine("France");
            Console.WriteLine("\nThis information will help you to plan your day, holidays, or a romantic dinner in the sunset with loved ones.\n");

            string user_input; // declaring variable to store user input

            do // structure of "do" guarantees that check happens after the code runs, ensuring the user gets the initial prompt before the condition is checked
            {
                Console.WriteLine("To continue, please press Enter."); // info that user have to press "Enter" to continue
                user_input = Console.ReadLine(); // reading user input

                if (user_input != "") // we checking here is the input is not empty
                {
                    Console.WriteLine("Invalid input. Please press Enter to continue."); // displaying message if input is not valid
                }
            } while (user_input != ""); // repeating until the user press "Enter"

            logger.CreateLog("User pressed Enter to continue."); // loggin information about pressing "Enter"

            using HttpClient client = new HttpClient(); // creating an instance of HttpClient for making API requests

            string date; // we store date here
            while (true) // starting infite loop 
            {
                Console.WriteLine("Enter the date (format YYYY-MM-DD): "); // user need to put date in specific format
                date = Console.ReadLine(); // reading the date input

                if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, DateTimeStyles.None, out _)) // checking if the user input is correct with format specified // this is the idea of chatgpt, nut sure if I fully understand that but it works :)
                {
                    break; // exiting loop is format is correct
                }

                Console.WriteLine("Invalid date format. Try again."); // error information if format is not valid
                logger.CreateLog("Invalid date format entered by the user."); // loggin error for invalid date format
            }

            int country_id = -1; // we setting value at -1 so it won't match values of countries specified in condition so loop will run at least once because needs to be "true"
            while (country_id != 0 && country_id != 1 && country_id != 2) // looping until a valid country_id is selected 
            {
                Console.WriteLine("Select country: 0 for Spain, 1 for England, 2 for France:"); // displaying message for user
                string country_choice = Console.ReadLine(); // reading user choice

                if (int.TryParse(country_choice, out country_id)) // error handling for invalid integer // int.TryParse is converting country_choice into an integer and stores the result in country_id
                {
                    if (country_id != 0 && country_id != 1 && country_id != 2) // checking valid options
                    {
                        Console.WriteLine("Invalid input. Please enter 0, 1, or 2."); // error message if option selected is not correct
                        logger.CreateLog("Invalid country selection by the user."); // loggin incorrect selection
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 0, 1, or 2.");
                    logger.CreateLog("Non-numeric input for country selection."); // logging non-numeric input
                    country_id = -1; // reset to ensure loop continues
                }
            }

            List<CountrySunTimes> countries = new List<CountrySunTimes> // creating a list of countries with their coordinates and names
            {
                new CountrySunTimes { CountryName = "Spain", GeoLatitude = "36.7201600", GeoLongitude = "-4.4203400" },
                new CountrySunTimes { CountryName = "England", GeoLatitude = "51.5074", GeoLongitude = "-0.1278" }, 
                new CountrySunTimes { CountryName = "France", GeoLatitude = "48.8566", GeoLongitude = "2.3522" } 
            };

            // getting information from selected country
            string latitude = countries[country_id].GeoLatitude;
            string longitude = countries[country_id].GeoLongitude;
            string country_name = countries[country_id].CountryName;

            string api_url = "https://api.sunrise-sunset.org/json?lat=" + latitude + "&lng=" + longitude + "&date=" + date; // this is our tutor idea ;) // we building API URL using the selected country coordinates and date

            logger.CreateLog($"Fetching data for {country_name} on {date} using URL: {api_url}"); // loggin the API request // we used $ sing to have interpolated string which allow to use expressions/variables

            try
            {
                HttpResponseMessage response = await client.GetAsync(api_url); // sending an HTTP GET request to API
                if (response.IsSuccessStatusCode) // checking if response is succcesfull
                {
                    string json_response = await response.Content.ReadAsStringAsync(); // reading response content as a string
                    ResponseData data = JsonConvert.DeserializeObject<ResponseData>(json_response); // deserialize JSON to ResponseData object "data" // used sample code from labs

                    if (data != null && data.Results != null) // ensuring that the data object returned from deserialization (line 99) and it's results are valid, if not program will crash
                    {
                        Console.WriteLine("\nSunrise and Sunset Information for " + country_name + ":"); // displaying the sunrise, sunset and day length info 
                        Console.WriteLine("Sunrise: " + data.Results.Sunrise);
                        Console.WriteLine("Sunset: " + data.Results.Sunset);
                        Console.WriteLine("Day Length: " + data.Results.DayLength);
                        logger.CreateLog("Data successfully retrieved and displayed."); // logging succes 
                    }
                    else
                    {
                        Console.WriteLine("No data found."); // displaying error if no data is found
                        logger.CreateLog("No data found in the API response."); // logging if failed
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching data: " + response.StatusCode); // displaying error if API request failed
                    logger.CreateLog($"API request failed with status code: {response.StatusCode}"); // logging failed request
                }
            }
            catch (Exception ex) // is used for any exceptions such as network issues or for example if data/results at line 101 will get "null"
            {
                Console.WriteLine("An error occurred: " + ex.Message); // displaying error for exceptions
                logger.CreateLog($"Exception occurred: {ex.Message}"); // logging exceptions
            }
        }
    }
}
