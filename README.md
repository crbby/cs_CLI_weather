# cs_CLI_weather
A small CLI application to display current weather in a specified location.

## Features
- various forecast types
- designed in mind with OpenWeatherMap OneCall API
- direct geolocation with same API key
- simple CLI syntax

## Example output

```
>> weather -l "poznan"

Select a location:
1 City: Poznan, State: Greater Poland Voivodeship, Country: PL
2 City: Poznan, State: Rivne Oblast, Country: UA
3 City: Poznan, State: Lublin Voivodeship, Country: PL 

>> (1)

Current weather in: Poznan, PL
Clear, 13.7 degrees
```

## Command line syntax

```
weather -l "LOCATION" -t 'TYPE' -u 'UNITS'

-l (req) -> place to show forecast for, ex: "London"

-t       -> type of forecast:
            'c' : current (def)
            'd' : daily
            'w' : weekly
            'm' : monthly

-u        -> units:
            'm' : metric (def)
            'i' : imperial

Use weather --help for more info
```

## TODOS:
- implement hourly/daily/monthly forecast UI
- compile linux binaries

## Used libraries:

|Name               |NuGet                                                  |
|---                |---                                                    |
|Newtonsoft.Json    | https://www.nuget.org/packages/Newtonsoft.Json/       |
|CommandLineParser  | https://www.nuget.org/packages/CommandLineParser/     |