using Microsoft.EntityFrameworkCore;

namespace airlines.Models
{
   
        public class FlightSearchModel
        {

        public string SourceCityName { get; set; }
        public string DestinationCityName { get; set; }
        public List<string> Results { get; set; } = new List<string>();

    }
}
