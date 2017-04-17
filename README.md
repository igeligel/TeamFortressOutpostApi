# TeamFortressOutpostApi by <a href="https://github.com/igeligel">igeligel</a>

<div align="center"><a href="https://www.paypal.me/kevinpeters96/1"><img src="https://img.shields.io/badge/Donate-Paypal-003087.svg?style=flat" alt="badge Donate" /></a> <a href="https://steamcommunity.com/tradeoffer/new/?partner=68364320&token=CzTCv8JM"><img src="https://img.shields.io/badge/Donate-Steam-000000.svg?style=flat" alt="badge Donate" /></a> <a href="https://camo.githubusercontent.com/821427c89e1b8a9b508077c4379abac05192eae2/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6963656e73652f6967656c6967656c2f5465616d466f7274726573734f7574706f73744170692e737667"><img src="https://img.shields.io/badge/License-MIT-1da1f2.svg?style=flat" alt="badge License" /></a> </div>

<div style="text-align:center"><img src ="http://i.imgur.com/t7TEH8L.gif" /><img src ="http://i.imgur.com/X1ujmme.gif" /></div>

## Description

> A [.net core](https://www.microsoft.com/net/core) class library which reverse engineered the HTTP API of [TF2 Outpost](http://www.tf2outpost.com/).

## Dependencies

| Dependency | Version |
| -- | -- |
| AngleSharp | 0.9.9 |
| Newtonsoft.Json | 10.0.2 |


## Installation

To install just you need to have .net core installed.

You can install this package via nuget or locally. Try to reference it as package in your .csproj file or install it via:

```
pm-install ...
```

## How To Use

This project just gives you one API endpoint which is usable.

First you need to create instance of  the ``TeamFortressOutpostApiClient``.

```csharp
var teamFortressOutpostApiClient = new TeamFortressOutpostApiClient();
```

After this you can call a function called ``Login`` with the parameters:

- username
- password
- sharedSecret

After the function gets invoked the login process will begin and the function will return the uhash and Cookies which are needed for API requests.

## Examples
- [Official Console Example](https://github.com/igeligel/TeamFortressOutpostApi/tree/master/examples/TeamFortressOutpostApiConsole)


## Contributing

To contribute please lookup the following styleguides:

- Commits: [here](https://github.com/igeligel/contributing-template/blob/master/commits.md)
- C#: [here](https://github.com/igeligel/contributing-template/blob/master/code-style/csharp.md)

## Resources

### Motivation

Mainly i created this functionality for a friend who wanted to automate some trading at TF2 Outpost. Since i believe it is useful to someone else i decided to publish it.

### Architecture

The general Workflow is shown in this diagram:

![Workflow](http://svgur.com/i/1GR.svg)

This is the basic structure of the OpenId Login via Steam to TF2 Outpost.

## Contact

<p align="center">
  <a href="https://discord.gg/HS57euF"><img src="https://img.shields.io/badge/Contact-Discord-7289da.svg" alt="Discord server of Kevin Peters"></a>
  <a href="https://twitter.com/kevinpeters_"><img src="https://img.shields.io/badge/Contact-Twitter-1da1f2.svg" alt="Twitter of Kevin Peters"></a>
  <a href="http://steamcommunity.com/profiles/76561198028630048"><img src="https://img.shields.io/badge/Contact-Steam-000000.svg" alt="Steam Profile of Kevin Peters"></a>
</p>


## License

*TeamFortressOutpostApi* is realeased under the MIT License.

<h2>Contributors</h2>

<table><thead><tr><th align="center"><a href="https://github.com/igeligel"><img src="https://avatars2.githubusercontent.com/u/12736734?v=3" width="100px;" style="max-width:100%;"><br><sub>igeligel</sub></a><br><p>Contributions: 14</p></th></tbody></table>

## This readme is powered by vue-readme
Check out vue-readme [[Website](https://igeligel.github.io/vue-readme) | [GitHub](https://github.com/igeligel/vue-readme)]



