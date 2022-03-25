namespace Weather
{
    //TODO: Implement C/F icon (add field in Weather class)
    public class Cli
    {

        public static void printCurrent(Geolocation.Place place, Weather forecast)
        {
            string current = new string("");

            current += $"Current weather in: {place.name}, {place.country}\n";
            current += $"{forecast.weatherCurrent.main}, {forecast.weatherCurrent.temp} degrees";

            Console.WriteLine(current);
        }

        //TODO: finish this monseter
        public static void printDaily(Geolocation.Place place, Weather forecast)
        {
            string current = new string("");

            current += $"Current weather in: {place.name}, {place.country}\n";
            current += $"{forecast.weatherCurrent.main}, {forecast.weatherCurrent.temp} degrees";

            string hourlyForecast = new string("");

            foreach (var hour in forecast.weatherHourly.list)
            {
                hourlyForecast += $"{hour.datetime.ToString("HH:mm tt")}: {hour.temp} °C\n";
                
            }

            Console.WriteLine(current);
            Console.WriteLine(hourlyForecast);
        }

        // TODO: implement weekly forecast
        public static void printWeekly(Geolocation.Place place, Weather forecast)
        {
            string output = new string("TO IMPLEMENT");

            Console.WriteLine(output);
        }

        // TODO: implement monthly forecast
        public static void printMonthly(Geolocation.Place place, Weather forecast)
        {
            string output = new string("TO IMPLEMENT");

            Console.WriteLine(output);
        }
    }
}