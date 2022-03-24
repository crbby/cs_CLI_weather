using System;
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
            Console.WriteLine($"Location: {options.Location}");
            Console.WriteLine($"Type:     {options.Type}");
        }
    }

}