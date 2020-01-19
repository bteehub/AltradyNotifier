# Altrady Notifier

A pushover notifier for Altrady Quick Scan functionality.

## Requirements

**Pushover**
 * Create your own account [here](https://www.pushover.net)
 * Install the app called Pushover on your mobile (5$ one time payment per platform)
 * For AltradyNotifier to send you notifications you will need to place your UserToken and an ApplicationToken into the *AltradyNotifier.json* file
 * Log into your account on the Pushover website
   * UserToken: On the top right corner you will see your UserToken below the text *Your User Key* 
   * ApplicationToken: If you scroll down the website you can create a new applicatiopn by clicking *Create an Application/API Token*
     * Name: f.e. Altrady
     * Icon: f.e. the altrady.png from the GitHub repository here
     * The ApplicationToken can now be found on the top of the website below the text *API Token/Key*
 * Please be aware that Pushover applications are limited to 7500 messages per month each

**Microsoft .NET Core**

In order to use the releases from Github you will need download and install [Microsoft .NET Core Runtime 3.1.x](https://dotnet.microsoft.com/download/dotnet-core/3.1) for your operating system. 

Github limits releases to 10MB and a self contained release is around 25-30MB.

## Compile on your own
By using [Microsoft .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) SDK

## Configuration
The program is configured by the *AltradyNotifier.json* file in the main binary directory. Edit it with a text editor:

 * `CultureInfo`: You can use [this list](https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c) and choose a language tag from the table, f.e. `en-UK`, `de-DE`, etc.
 * `MaxPrecision`: Maximum number of digits behind the comma for values in the pushover notification. `8` could be a good value
 * `Altrady`
   * `ApiKey`: You will find the API key in your Altrady account
   * `MaxApiCallsPerHour`: You will find this value in your Altrady API key settings. Dont' hammer their API, values are update every minute anyway. A sufficient value is 60 x *NumberOfTimeFramesYouFilter*
 * `Filter` A list of filters, one filter per timeframe
   * `Timeframe`: Check the [API docs](https://cryptobasescanner.docs.apiary.io/#reference/markets/v1marketsquickscan/get) for valid values, currently: `5`, `10`, `15`, `30`
   * `ExcludedMarkets` Exclude markets, comma seperated, fe. `/BTC,BTC/USD`, this would not work yet: `BINA: LTC/BTC`!
   * `ExchangeMarket` A list of exchange markets to scan for
     * `Exchange` Check the [API docs](https://cryptobasescanner.docs.apiary.io/#reference/markets/v1marketsquickscan/get) for valid values, currently: `BTRX `, `PLNX`, `KUCN`, `BINA`, `HITB`
     * `Market` List of markets to include, comma seperated, f.e. `/BTC,/ETH`
     * `Volume`
       * `Currency`: `BTC` or `USD`
       * `Value` f.e. `1000`
     * `Rise` f.e. `2`
     * `Drop` f.e. `-2`

## Launch
Just start the AltradyNotifier executeable on your operating system. Don't close the window.