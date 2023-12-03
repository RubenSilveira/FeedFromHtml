# FeedFromHtml

Scrapes an online web page and converts it to RSS.

This is a very early release; only the basic works.

## Installation

No installation package is provided. Build and deploy at will.

## Requirements

- .NET 8.0:
	- .NET Runtime;
	- ASP.NET Core Runtime.

## Development / Build

Developed and tested on Intel macOS 12 (Monterey), using Visual Studio
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

## To Do
- Config from database;
- Scheduled refresh to database;
- Serve from database;
- In-memory and HTTP caching;
- Follow links and enrich items (date/time, content/media, etc.);
- Frontend.

## License

Published under [The
MIT License](https://github.com/RubenSilveira/WinAwake/blob/main/LICENSE).

## Changelog

### v1 (2023-12-02)
	Features:
	- Initial version.