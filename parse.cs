using CommandLine;
using CommandLine.Text;

namespace Weather
{
    internal class CommandParser
    {
        // CommandLine (CLP) input args parser
        public class Options
        {
            private readonly string location;
            private readonly char type;
            private readonly char units;

            public Options(string location, char type, char units)
            {
                this.location = location;
                this.type = type;
                this.units = units;
            }

            [Option('l', Required = true, HelpText = "Location of the forceast")]
            public string Location { get { return location; } }

            [Option('t', Required = false, HelpText = "Type of the forecast: d - daily, w - weekly, m - monthly", Default = (char)'d')]
            public char Type { get { return type; } }

            [Option('u', Required = false, HelpText = "Units of the forecast: m - metric, i - imperial", Default = (char)'m')]
            public char Units { get { return units; } }
        }

        static void Main(string[] args)
        {
            // disable CLP default help screen
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);

            parserResult
                .WithParsed<Options>(options => Run(options))
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
              {
                  h.AdditionalNewLineAfterOption = false;                   //remove the extra newline between options
                  h.Heading = "cs_CLI_weather";                             //change header
                  h.Copyright = "\"CoPyRiGhT\" (c) Patryk Kościk 2022";     //change copyrigt text
                  return HelpText.DefaultParsingErrorsHandler(result, h);
              }, e => e);
            Console.WriteLine(helpText);
        }

        private static void Run(Options options)
        {
            var places = Geolocation.Geolocate(options.Location, 10);
            var selectedPlace = new Geolocation.Place();

            // check if there were any results (non empty list)
            // TODO: make this better
            if (places.Count == 0)
            {
                Console.WriteLine("NO PLACES WERE FOUND");
            }

            // only one location matching --location parameter
            if (places.Count == 1)
            {
                selectedPlace = places[0];
            }

            // if there is more than one place
            // display selection screen
            if (places.Count > 1)
            {
                Console.WriteLine("Select a location:");
                int index = 1;
                foreach (var item in places)
                {
                    Console.Write($"{index} City: {item.name}, State: {item.state}, Country: {item.country}\n");
                    index += 1;
                }

                // get user input and check bounds
                string input;
                int selection;
                bool loop = true;

                do
                {
                    input = Console.ReadLine();

                    if (!Int32.TryParse(input, out selection))
                    {
                        Console.WriteLine("not a number!!!");
                    }
                    else if (selection > places.Count || selection <= 0)
                    {
                        Console.WriteLine("Selection out of bounds");
                    }
                    else
                    {
                        selectedPlace = places[selection - 1];

                        loop = false;
                    }

                } while (loop);

                // i'd love for forecast to be static
                // but it it really did not make much sense 
                // yeah im not following func.prog. princibles
                // : (
                var forecast = new Weather(selectedPlace.lat, selectedPlace.lon);

                switch (options.Type)
                {
                    case 'd':
                        {
                            Cli.printDaily(selectedPlace, forecast);

                            break;
                        }

                    case 'w':
                        {
                            Cli.printWeekly(selectedPlace, forecast);
                            break;
                        }

                    case 'm':
                        {
                            Cli.printMonthly(selectedPlace, forecast);
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("not supposed to be there lmao");
                            break;
                        }
                }
            }
        }
    }

}