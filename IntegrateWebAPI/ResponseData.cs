using Newtonsoft.Json;

namespace APIProject
{
   
    public class ResponseData // that class represent the structure of the deserialized JSON response 
                              // it's having main data as a results and response status for error handling which is used in lines 117,118 of Program class to display it and log it
    {
        
        [JsonProperty("results")]
        public Results Results { get; set; }

        
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
