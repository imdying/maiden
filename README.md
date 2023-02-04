# Raiden

 [![NuGet Badge](https://buildstats.info/nuget/Raiden?includePreReleases=true)](https://www.nuget.org/packages/Raiden)
 
 A build tool designed to streamline the process of versioning software releases.

  ![Raiden Preview](https://raw.githubusercontent.com/imdying/raiden/main/previews/Raiden.gif)

 ## Prerequisites

  - [.NET 7+ SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
  - [PowerShell 7+](https://github.com/PowerShell/PowerShell)

 ## Getting Started

  To start using Raiden, you'll need to install the tool on your machine. You can do this by running the following command:

    dotnet tool install --global Raiden

  After installing Raiden, you can run the following command to initialize the tool in your project directory:

    raiden init

  This will create a `.raidenconfig` file in your project directory, where you can configure the tool to meet your specific needs.

 ## Usage

  To use Raiden, simply run the following command in your project directory:

    raiden build

  Raiden will increment the version number of your project and then call your build script.

 ## License

  This project is licensed under the [MIT license](/LICENSE.md).
