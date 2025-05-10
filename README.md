# KeepDrivingMusicModTools

A collection of modding tools for Keep Driving that allow you to replace the existing soundtrack with custom music from your local files. This toolkit runs a lightly modified version of the UndertaleModToolCLI

# How to Use

* The main and currently supported method for song replacements is a process called “bulk replacement”.
* This will map .wav files in certain directories to individual songs playable on the in-game radio, allowing you to replace part of or the entire soundtrack with a single command.

# Compilation Instructions

In order to compile the repo yourself, the `.NET Core 8 SDK` or later is required.

The following projects can be compiled:  
- `UndertaleModLib`: The core library used by all other projects.
- `UndertaleModCli`: A command line interface for interacting with GameMaker data files and applying scripts. Currently is very primitive in what it can do.
- `UndertaleModTool`: The main graphical user interface for interacting with GameMaker data files. **Windows is required in order to compile this**.

#### Compiling Via Visual Studio Command Line
- Open `Developer Command Prompt for Visual Studio`
- Set your working directory to the project root folder
- Run `vs_buildscript.bat`
- Contents will be written to the `build` folder
