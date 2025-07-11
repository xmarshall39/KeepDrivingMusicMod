# KeepDrivingMusicModTools

A collection of modding tools for Keep Driving that allow you to replace the existing soundtrack with custom music from your local files. This toolkit runs a lightly modified version of the UndertaleModToolCLI

## How to Use
Download the latest precompiled build from the Releases tab. The main and currently supported method for song replacements is a process called “bulk replacement”.

This will map .wav files in specified directories to individual songs playable on the in-game radio, allowing you to replace part of or the entire soundtrack with a single command.

The steps to do so are as follows:
  
#### 1. Obtain copies of songs you wish to include as .wav files
- As there are 71 songs in the base game, you can replace as many tracks
- Any songs you don’t replace will play as normal

#### 2. Plan out your replacements
- Place `dirmaker.bat` in the root folder of wherever you’d like to keep your music and run it. This will create folders in which you’ll sort your songs. The songlist.txt in each folder will indicate what the original tracks were and how many there are
  - Song folders are named with a code for representing a band in the base game followed by an underscore
  - For organizational convenience, you may add to the folder name after the underscore. This can be helpful if song replacements are grouped using a certain theme or pattern
- Add song files to each folder. You can add up to total number of songs that artist had in the base game
- Rename each song according to the following format:
    
```
[Order Number]__[Artist Name]__[Album Name]__[Song Name].wav

ex:
in the "Westkust__" folder, the following

    1__BROCKHAMPTON_GINGER__NO HALO.wav
    2__Kevin Abstract__ARIZONA BABY__Peach.wav
    3__Kendrick Lamar__GNX__tv off (feat. lefty gunplay).wav

will be used to replace Swirl, Dishwasher, and Drown in the soundtrack
```
- **Order Number** - Maps the song you're naming to a song from that folder's artist. For example, In the Crystal Boys folder, an Order Nuber of 1 will replace their first track (Nightlife) in-game.You can use the `songlist.txt` of each artists' folder to guide your order assignments.

#### 3. Run the mod tools
* Open the command prompt in the location of your downloaded build
* Run `KeepDrivingModCLI.exe`
```batch
KeepDrivingModCLI.exe "C:\Program Files (x86)\Steam\steamapps\common\Keep Driving" "C:\Users\monst\Music\Keep Driving Soundtracks\DevTest" --output "C:\Program Files (x86)\Steam\steamapps\common\Keep Driving\export"
```
#### Results
When you open the game’s radio, you’ll see the replaced songs sorted by artist name (not album) as this is the game’s original behavior. There is no known limit to the number of songs you can have attributed to a specific artist. As you progress through the game and unlock songs, those which you obtain will be determined by your replacements

## Roadmap
* Allowing the use of file metadata, instead of file names, to inform song replacements
* A GUI tool that allows for both bulk and individual song replacements

## Compiling Via Visual Studio Command Line
In order to compile the repo yourself, the `.NET Core 8 SDK` or later is required.

- Open `Developer Command Prompt for Visual Studio`
- Set your working directory to the project root folder
- Run `vs_buildscript.bat`
- Contents will be written to the `build` folder
