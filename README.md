# Dumb FTP Client #

A text-mode FTP client for the Portland State University CS410 Agile class taught by Chris Gilmore over the summer of 2018.

<img src="http://wiki.hypersweet.com/_media/public/dumbftp.jpg" width="556" height="507" title="Dumb Ftp Screenshot">

## License ##

This project uses the MIT license. Please see the file `LICENSE` for details.

## Setting up for Development ##

### Windows ###

The software is set up as a Visual Studio 2017 solution.

1. Clone the project. `git clone https://github.com/bgoldbeck/cs410-agile.git`
2. Download and install Visual Studio 2017. [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/)
3. Download and install .NET Core app SDK 2.1 from Microsoft. [.NET Core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300)
4. Open the solution `.sln` file in Visual Studio 2017.
5. Run the project with `F5`.

A "FluentFTP.dll" should be downloaded by VS for managing and providing the FTP protocol interface. [FluentFTP](https://github.com/robinrodricks/FluentFTP)

### Linux ###

1. Clone the project. `git clone https://github.com/bgoldbeck/cs410-agile.git`
2. Download and install Visual Studio Code. [Visual Studio Code](https://code.visualstudio.com/)
3. Download and install .NET Core app SDK 2.1 from Microsoft. [.NET Core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300) Instructions for your distribution may vary.
4. Launch Visual Studio Code.
5. Open the project folder from VS Code and select View > Extensions.
  1. Install "C# for visual studio code" extension.
  2. Install "C# FixFormat" extension.
  3. Install "C# Extensions" extension.
  4. Install "C# XML Documentation Comments" extension.
  5. Install NuGet package Manager extension.
6. Open the terminal in Visual Studio Code "Ctrl + tilde" and type `dotnet run` to run the application.
7. Run tests with `dotnet test`.

## Who do I talk to? ##
* Repo owner or admin
* Other community or team contact
