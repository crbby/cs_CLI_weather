using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Weather
{
    public class Geolocation
    {
        public class Place
        {
            public string name { get; set; }
            public string country { get; set; }
            public string state { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
        }

        public static List<Place> Geolocate(string location, int limit)
        {
            string url = new String($"http://api.openweathermap.org/geo/1.0/direct?q={location}&limit={limit}&appid={Keys.API_key}");

            var places = new List<Place>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string jsonString = response.Content.ReadAsStringAsync().Result;

                // using dynamic beacuse i really cant be bothered to figure out OpenWheaterMap API's keys
                // TODO: create class model
                dynamic jsonPlaces = JsonConvert.DeserializeObject(jsonString);


                // "convering" from dynamic jsonPlaces to List<Place>
                // this step is necessary beacuse we cant use Lambdas and LINQ on dynamic datatypes
                // also catching exceptions here, thats why I'm not checking jsonPlaces for null value
                // i mean if it crashes it crashes
                try
                {
                    foreach (var jsonPlace in jsonPlaces)
                    {
                        places.Add(new Place()
                        {
                            name = jsonPlace.name,
                            country = jsonPlace.country,
                            state = jsonPlace.state,
                            lat = jsonPlace.lat,
                            lon = jsonPlace.lon
                        });

                    }
                }
                catch (Exception e)
                {
                    Console.Write($"An error occured -> {e.ToString()}");
                    throw;
                }
            }

            // filtering (removing) place entries with the same name, state and country
            // OWM API be like: yeah dawg i heard you like :place: so we have got you a :place: in your :place: 
            // also this way of doing this is flawed (might miss one duplicate) 
            // but i dont really care and really want to finish this app asap so yeah 
            // TODO: fix this
            // that being said i dont really care anymore
            
            string lastName = "";
            string lastState = "";
            string lastCountry = "";
            var toRemove = new List<int>();

            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].name == lastName
                    && places[i].state == lastState)
                {
                    toRemove.Add(i-1);
                }

                lastName = places[i].name;
                lastState =  places[i].state; 
                lastCountry = places[i].country;

            }

            foreach (int index in toRemove)
            {
                places.RemoveAt(index);
            }


            return places;

        }

    }
}