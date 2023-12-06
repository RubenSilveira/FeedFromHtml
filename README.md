# FeedFromHtml

Server application that scrapes online web pages and serves them as RSS
feeds.

These are very early releases, so only the bare minimum works.

## Installation

No installation package is provided. Build and deploy at will.

## Requirements

- .NET 8.0:
	- .NET Runtime;
	- ASP.NET Core Runtime.

## Development / Build

Developed on Intel macOS 12 (Monterey), using Visual Studio
Code:
- Framework:
	- .NET 8.0 SDK.
- Extensions:
	- .NET Install Tool (ms-dotnettools.vscode-dotnet-runtime);
	- C# (ms-dotnettools.csharp);
	- C# Dev Kit (ms-dotnettools.csdevkit);
	- IntelliCode for c# Dev Kit (ms-dotnettools.vscodeintellicode-csharp).
- Dependencies:
	- [HtmlAgilityPack](https://html-agility-pack.net).

Tested on Linux via Azure App Service.

## To Do
- Config from database;
- Scheduled refresh to database;
- Serve from database;
- In-memory and HTTP caching;
- Follow links and enrich items (date/time, content/media, etc.);
- Full-featured frontend.

## License

Published under [The
MIT License](https://github.com/RubenSilveira/FeedFromHtml/blob/main/LICENSE).

## Changelog

### v2 (2023-12-06)
	Features:
	- RSS: Added rel=self for improved compatibility
	- Frontend: Minimal initial version with feed directory
	- Frontend: Available feeds advertised as meta tags

### v1 (2023-12-02)
	Features:
	- RSS: Initial version.