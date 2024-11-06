using System;
using Newtonsoft.Json;

namespace APIProject
{
    public class Results
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; } // The time of sunrise

        [JsonProperty("sunset")]
        public string Sunset { get; set; } // The time of sunset

        [JsonProperty("day_length")]
        public string DayLength { get; set; } // The total length of the day
    }
}
