# README #

## What is this repository for? ##

* This repository contains the term project for CS300 taught by Chris Gilmore at PSU.  
This is software for a hypothetical organization called Chocoholics Anonymous (ChocAn).
It allows for managing the records of professionals providing consultations and treatments to the members of ChocAn.

## How do I get set up? ##

### Windows ###
* The software is a setup as a Visual Studio 2017 project. Clone the project, open the solution in Visual Studio and build it.
* This software requires an SQLite dll for the database functionality.
This can be obtained at https://system.data.sqlite.org under the download area.
We include one built for Windows for convenience. 

### Linux ###
* Install the latest Mono and related tools from http://www.mono-project.com/download/
* Download the source code for System.Data.SQLite from https://system.data.sqlite.org
* Extract from the archive and `cd sqlite-netFx-full-source-\<version number\>/System.Data.SQLite`
* Build System.Data.SQLite.dll with `msbuild System.Data.SQLite.2013.csproj`
* `cd ../setup
* Build libSQLite.Interop.so using `./compile-interop-assembly-debug.sh`
* To build the ChocAn software, delete CS300_TermProject.sln then run `msbuild` in the folder you cloned into
* Copy System.Data.SQLite.dll and libSQLite.Interop.so from `sqlite-netFx-full-source-\<version number\>/bin/2013/Debug/bin/` into the directory containing the ChocAn executable.

## How do I log in? ##
* Provider Terminal: 
  * Username: 100000000 
  * Password: asdf
* Manager Terminal
  * Username: 123456789
  * Password: asdf
 
## Who do I talk to? ##

* Repo owner or admin
* Other community or team contact

## How do I read the text file outputs from the terminal? ##

* Use a text editor (*NOT* Notepad...please.)