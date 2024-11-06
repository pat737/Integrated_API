using Newtonsoft.Json;

namespace APIProject
{
    public class ResponseData
    {
        // Holds the main data for sunrise, sunset, and day length
        [JsonProperty("results")]
        public Results Results { get; set; }

        // Status of the API response (e.g., "OK")
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
