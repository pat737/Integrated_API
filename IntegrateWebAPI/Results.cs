using System;
using Newtonsoft.Json;

namespace APIProject
{
    public class Results // class that holds parsed API response
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; } 

        [JsonProperty("sunset")]
        public string Sunset { get; set; } 

        [JsonProperty("day_length")]
        public string DayLength { get; set; } 
    }
}
