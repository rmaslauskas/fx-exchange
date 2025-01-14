
# Exchange Application

## Description
The application is a Foreign Exchange (FX) Conversion System designed to facilitate currency conversions between various ISO currency pairs. The system solves the problem of converting an amount in one currency (main currency) into an equivalent amount in another currency (money currency) based on exchange rates which are retrieved from external API https://open.er-api.com/v6/latest.

## Prerequisites

- .NET 8.0 sdk [https://dotnet.microsoft.com/en-us/download/dotnet/8.0]

## Building Solution

```bash
git clone https://github.com/rmaslauskas/fx-exchange.git
cd ./fx-exchange
dotnet restore
dotnet publish -c Release -o dist
```

## Running Solution

```bash
cd ./dist
Exchange.exe EUR/DKK 1  
```

## Configuration

For configuration, please, edit ./Echange.ConsoleApp/appsettings.json file:
```json
{
  "ExchangeRatesAPI": <REMOTE_API_URL>
}
```

## Unit Tests

To run unit tests, please, execute command:
```bash
dotnet test
```


