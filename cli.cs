namespace Weather
{
    public class Cli
    {
        public static void printDaily(Geolocation.Place place, Weather forecast)
        {
            string output = new string("");

            output += $"Current weather in: {place.name}, {place.country}\n";
            output += $"Temperature: {forecast.weatherCurrent.temp}";

            Console.WriteLine(output);
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