# cryptocompare-api-client

![.NET Core](https://github.com/trakx/cryptocompare-api-client/workflows/.NET%20Core/badge.svg) 
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/cc015709dab343b89b53eb42314ac159)](https://www.codacy.com/gh/trakx/cryptocompare-api-client/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=trakx/cryptocompare-api-client&amp;utm_campaign=Badge_Grade) 
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/cc015709dab343b89b53eb42314ac159)](https://www.codacy.com/gh/trakx/cryptocompare-api-client/dashboard?utm_source=github.com&utm_medium=referral&utm_content=trakx/cryptocompare-api-client&utm_campaign=Badge_Coverage)

![logo](https://raw.githubusercontent.com/joancaron/cryptocompare-api/master/cryptocompare_logo.png)

An async-based CryptoCompare API client library for .NET and .NET Core

## Supported platforms

* .NET Core 1.0
* .NET Framework 4.5
* Mono 4.6
* Xamarin.iOS 10.0
* Xamarin.Android 7.0
* Universal Windows Platform 10
* Windows 8.0
* Windows Phone 8.1

## Installation
This CryptoCompare api wrapper library is available on NuGet

Package manager
````
Install-Package CryptoCompare
````

.NET CLI
````
dotnet add package CryptoCompare
````

Paket CLI
````
paket add CryptoCompare
````

<a href="https://www.patreon.com/joancaron">
	<img src="https://c5.patreon.com/external/logo/become_a_patron_button@2x.png" width="160">
</a>

## Creating your local .env file
In order to be able to run some integration tests, you should create a `.env` file in the `src` folder with the following variables:
```secretsEnvVariables
CryptoCompareApiConfiguration__ApiKey=********
```


## Basic usage
````csharp
// Using ctor
 var client = new CryptoCompareClient();
 var eth = await client.Coins.SnapshotFullAsync(7605);
 Console.WriteLine(eth.Data.General.Name);

// Using Singleton 
var btc = await CryptoCompareClient.Instance.Coins.SnapshotFullAsync(1182);
Console.WriteLine(eth.Data.General.Name);
````

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore -->
| [<img src="https://avatars2.githubusercontent.com/u/4638821?v=4" width="100px;"/><br /><sub><b>monsieurleberre</b></sub>](https://github.com/monsieurleberre)<br />[💻](https://github.com/joancaron/cryptocompare-api/commits?author=monsieurleberre "Code") [⚠️](https://github.com/joancaron/cryptocompare-api/commits?author=monsieurleberre "Tests") | [<img src="https://avatars2.githubusercontent.com/u/10140906?v=4" width="100px;"/><br /><sub><b>Anton</b></sub>](https://github.com/stepkillah)<br />[💻](https://github.com/joancaron/cryptocompare-api/commits?author=stepkillah "Code") [⚠️](https://github.com/joancaron/cryptocompare-api/commits?author=stepkillah "Tests") | [<img src="https://avatars2.githubusercontent.com/u/634931?v=4" width="100px;"/><br /><sub><b>cohowap</b></sub>](https://github.com/cohowap)<br />[💻](https://github.com/joancaron/cryptocompare-api/commits?author=cohowap "Code") [⚠️](https://github.com/joancaron/cryptocompare-api/commits?author=cohowap "Tests") |
| :---: | :---: | :---: |
<!-- ALL-CONTRIBUTORS-LIST:END -->

## License

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)



[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fjoancaron%2Fcryptocompare-api.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fjoancaron%2Fcryptocompare-api?ref=badge_large)

## AWS Parameters
In order to be able to run some integration tests, you should ensure that you have access to the AWS parameters starting in `/CiCd`. In order for the applications in this solution to run correctly on AWS, please ensure that variables starting in `/[environment]` are defined for all 3 environments \( _Production_, _Staging_, _Development_ \) :
```awsParams
/[environment]/Trakx/CryptoCompare/ApiClient/CryptoCompareApiConfiguration/ApiKey
```