using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Weather
{
    // TODO: THIS ONE IS IMPORTANT FOR REAL THO
    // USE CONSTRUCTORS FOR CLASSES FFS XDddd

    public class Weather
    {
        public class WeatherCurrent
        {
            // NON JSON FIELDS
            public DateTime datetime { get; set; }

            // root
            public double lat { get; set; }
            public double lon { get; set; }
            public string timezone { get; set; }
            public int timezone_offset { get; set; }

            // current
            public int current_time_unix { get; set; }
            public int sunset { get; set; }
            public int sunrise { get; set; }
            public float temp { get; set; }
            public float feels_like { get; set; }
            public float pressure { get; set; }
            public float humidity { get; set; }
            public float dew_point { get; set; }
            public float uvi { get; set; }
            public float clouds { get; set; }
            public float visibility { get; set; }
            public float wind_speed { get; set; }
            public float wind_deg { get; set; }

            // weather meta-info
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }

        }

        public class WeatherCurrentExtended : WeatherCurrent
        {
            public float temp_day { get; set; }
            public float temp_night { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }

            public float feels_like_day { get; set; }
            public float feels_like_night { get; set; }
        }

        public class WeatherHourly
        {
            public List<WeatherCurrent> list { get; set; }

            public WeatherHourly()
            {
                list = new List<WeatherCurrent>();
            }
        }

        public class WeatherDaily
        {
            public List<WeatherCurrentExtended> list { get; set; }

            public WeatherDaily()
            {
                list = new List<WeatherCurrentExtended>();
            }
        }

        public WeatherCurrent weatherCurrent { get; set; }
        public WeatherHourly weatherHourly { get; set; }
        public WeatherDaily weatherDaily { get; set; }

        public Weather(double lat, double lon)
        {
            weatherCurrent = new WeatherCurrent();
            weatherHourly = new WeatherHourly();
            weatherDaily = new WeatherDaily();

            Forecast(lat, lon);
        }

        // TODO: parse alerts (alERt rCb SilNE WiAtRY)
        public void Forecast(double lat, double lon)
        {
            string url = new String($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=minutely,alerts&appid={Keys.API_key}");

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

                // is there a better way to do this? maybe
                // do i care? not really (i do a little bit, will "fix" in a next release)
                // TODO: fix this scheiße
                // ps: there is a REALLY solid reason why im doing this that way
                //     but i cant remember it
                //
                // pps: to my future employer: please skip next 100 lines thanks
                // TODO: USE CONSTRUCOTRS FOR GOD DAMN SAKE

                // CURRENT FORECEAST
                // root
                weatherCurrent.lat = jsonPlaces.lat;
                weatherCurrent.lon = jsonPlaces.lon;
                weatherCurrent.timezone = jsonPlaces.timezone;
                weatherCurrent.timezone_offset = jsonPlaces.timezone_offset;

                // current
                weatherCurrent.current_time_unix = jsonPlaces.current.dt;
                weatherCurrent.sunset = jsonPlaces.current.sunset;
                weatherCurrent.sunrise = jsonPlaces.current.sunrise;
                weatherCurrent.temp = jsonPlaces.current.temp;
                weatherCurrent.feels_like = jsonPlaces.current.feels_like;
                weatherCurrent.pressure = jsonPlaces.current.pressure;
                weatherCurrent.humidity = jsonPlaces.current.humidity;
                weatherCurrent.dew_point = jsonPlaces.current.dew_point;
                weatherCurrent.uvi = jsonPlaces.current.uvi;
                weatherCurrent.clouds = jsonPlaces.current.clouds;
                weatherCurrent.visibility = jsonPlaces.current.visibility;
                weatherCurrent.wind_speed = jsonPlaces.current.wind_speed;
                weatherCurrent.wind_deg = jsonPlaces.current.wind_deg;

                // weather meta-info
                foreach (var meta in jsonPlaces.current.weather)
                {
                    weatherCurrent.id = meta.id;
                    weatherCurrent.main = meta.main;
                    weatherCurrent.description = meta.description;
                    weatherCurrent.icon = meta.icon;
                }


                // HOURLY FORECEAST
                foreach (var forecast in jsonPlaces.hourly)
                {

                    var newForecast = new WeatherCurrent();

                    newForecast.current_time_unix = forecast.dt;
                    newForecast.temp = forecast.temp;
                    newForecast.feels_like = forecast.feels_like;
                    newForecast.pressure = forecast.pressure;
                    newForecast.humidity = forecast.humidity;
                    newForecast.dew_point = forecast.dew_point;
                    newForecast.uvi = forecast.uvi;
                    newForecast.clouds = forecast.clouds;
                    newForecast.visibility = forecast.visibility;
                    newForecast.wind_speed = forecast.wind_speed;
                    newForecast.wind_deg = forecast.wind_deg;

                    // weather meta-info
                    foreach (var meta in forecast.weather)
                    {
                        newForecast.id = meta.id;
                        newForecast.main = meta.main;
                        newForecast.description = meta.description;
                        newForecast.icon = meta.icon;
                    }

                    weatherHourly.list.Add(newForecast);
                }

                // DAILY FORECEAST
                foreach (var forecast in jsonPlaces.daily)
                {

                    var newForecast = new WeatherCurrentExtended();

                    newForecast.current_time_unix = forecast.dt;

                    // temperature -> day/night/min/max
                    newForecast.temp_day = forecast.temp.day;
                    newForecast.temp_night = forecast.temp.night;
                    newForecast.temp_min = forecast.temp.min;
                    newForecast.temp_max = forecast.temp.max;

                    // feels like -> day/night
                    newForecast.feels_like = forecast.feels_like.day;
                    newForecast.feels_like_night = forecast.feels_like.night;

                    newForecast.pressure = forecast.pressure;
                    newForecast.humidity = forecast.humidity;
                    newForecast.dew_point = forecast.dew_point;
                    newForecast.uvi = forecast.uvi;
                    newForecast.clouds = forecast.clouds;
                    newForecast.wind_speed = forecast.wind_speed;
                    newForecast.wind_deg = forecast.wind_deg;

                    // weather meta-info
                    foreach (var meta in forecast.weather)
                    {
                        newForecast.id = meta.id;
                        newForecast.main = meta.main;
                        newForecast.description = meta.description;
                        newForecast.icon = meta.icon;
                    }

                    weatherDaily.list.Add(newForecast);
                }

                /*
                Console.WriteLine($"no of hourly samples: {weatherHourly.list.Count}");
                Console.WriteLine($"no of daily samples: {weatherDaily.list.Count}");
                */

                ConvertForecast('c');
            }
        }

        /// Convert Weather* objects 
        /// unix time -> datetime
        /// kelvin    -> degC / degF
        private void ConvertForecast(char unit)
        {
            // convert deg kelvin to human readable format 
            switch (unit)
            {
                case 'c':
                    {
                        // CURRENT
                        weatherCurrent.temp = (float)Math.Round(weatherCurrent.temp - 273.15f, 1);

                        // HOURLY
                        foreach (var forecast in weatherHourly.list)
                        {
                            forecast.temp = (float)Math.Round(forecast.temp - 273.15f, 1);
                        }

                        // DAILY
                        foreach (var forecast in weatherDaily.list)
                        {
                            forecast.temp_day = (float)Math.Round(forecast.temp_day - 273.15f, 1);
                            forecast.temp_night = (float)Math.Round(forecast.temp_night - 273.15f, 1);
                            forecast.temp_min = (float)Math.Round(forecast.temp_min - 273.15f, 1);
                            forecast.temp_max = (float)Math.Round(forecast.temp_max - 273.15f, 1);
                        }

                        break;
                    }

                case 'f':
                    {
                        // CURRENT
                        weatherCurrent.temp = (float)Math.Round(1.8f * (273.15f - weatherCurrent.temp) + 32f, 1);

                        // HOURLY
                        foreach (var forecast in weatherHourly.list)
                        {
                            forecast.temp = (float)Math.Round(1.8f * (273.15f - forecast.temp) + 32f, 1);
                        }

                        // DAILY
                        foreach (var forecast in weatherDaily.list)
                        {
                            forecast.temp_day = (float)Math.Round(1.8f * (273.15f - forecast.temp_day) + 32f, 1);
                            forecast.temp_night = (float)Math.Round(1.8f * (273.15f - forecast.temp_night) + 32f, 1);
                            forecast.temp_min = (float)Math.Round(1.8f * (273.15f - forecast.temp_min) + 32f, 1);
                            forecast.temp_max = (float)Math.Round(1.8f * (273.15f - forecast.temp_max) + 32f, 1);
                        }

                        break;
                    }
            }

            // convert unix timestamp to DateTime
            weatherCurrent.datetime = UnixTimeStampToDateTime(weatherCurrent.current_time_unix);

            // HOURLY
            foreach (var forecast in weatherHourly.list)
            {
                forecast.datetime = UnixTimeStampToDateTime(forecast.current_time_unix);
            }

            // DAILY
            foreach (var forecast in weatherDaily.list)
            {
                forecast.datetime = UnixTimeStampToDateTime(forecast.current_time_unix);
            }
        }

        // from: https://stackoverflow.com/a/250400/13264796
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}