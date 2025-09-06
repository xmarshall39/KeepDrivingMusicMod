# KeepDrivingMusicModTools

A collection of modding tools for Keep Driving that allow you to replace the existing soundtrack with custom music from your local files. This toolkit runs a lightly modified version of the UndertaleModToolCLI

## How to Use
Download the latest precompiled build from the Releases tab. The main and currently supported method for song replacements is a process called “bulk replacement”.

This will map .wav files in specified directories to individual songs playable on the in-game radio, allowing you to replace part of or the entire soundtrack with a single command.

The steps to do so are as follows:
  
#### 1. Obtain copies of songs you wish to include as .wav files
- As there are 71 songs in the base game, you can replace as many tracks
- Any songs you don’t replace will play as normal
<Details>
<Summary><h3>Recommended: Quick song aquisition and formatting using SpotDL</h3></Summary>
  
In step 2, you'll find that the .wav files we use must be named a specific way for bulk replacement compatbility. If you plan on aquiring songs using [SpotDL](https://github.com/spotDL/spotify-downloader), I've included a script to automate this process. Spotify playlists are also a great way to manage your OST's and share them with others!

- Install [Python](https://www.python.org/downloads/) (version 3.10 or higher) and add it to your PATH
- Open the Command Prompt and add SpotDL to python by running `pip install spotdl`
- Open/navigate Command Prompt to the location of `kdmt-song-formatter.py` and run it in accordance with the following example
```
python kdmt-song-formatter [SONG_DOWNLOAD_LOCATION] [SPOTIFY SONG/PLAYLIST LINK]

--- e.x ---
python kdmt-song-formatter C:\Users\xmarshall39\Music\KDMT\FormatterTesting https://open.spotify.com/playlist/6efZ86E292MgcVIUoziP86?si=98251fa7e0b445c4

--- output ---
  1__Fugees__The Score (Expanded Edition)__Fu-Gee-La.wav
  2____3 __Living With Determination --.wav
  3__MAXIMUM THE HORMONE__Yoshu Fukushu__F.wav
  4__Bad Bunny__YHLQMDLG__3.wav
  5__Jessie Reyez__PAID IN MEMORIES__CUDN’T B ME.wav
```
- You may use `.` as the `[SONG_DOWNLOAD_LOCATION]` to download files to the current directory
- Optionally, add `-u` or `--unicode` to the end of your command to allow unicode characters in the song name output. For example:
```
python kdmt-song-formatter . https://open.spotify.com/playlist/6efZ86E292MgcVIUoziP86?si=98251fa7e0b445c4 --unicode

--- output ---
  1__Fugees__The Score (Expanded Edition)__Fu-Gee-La.wav
  2__アトラスサウンドチーム__ペルソナ3 オリジナル・サウンドトラック__Living With Determination -巌戸台分寮アレンジ-.wav
  3__MAXIMUM THE HORMONE__Yoshu Fukushu__F.wav
  4__Bad Bunny__YHLQMDLG__3.wav
  5__Jessie Reyez__PAID IN MEMORIES__CUDN’T B ME.wav
```
- NOTE: Ascii characters disallowed in Windows filenames will be excluded, which is why the Bad Bunny song `<3` gets saved as `3`
</Details>

#### 2. Plan out your replacements
- Place `dirmaker.bat` in the root folder of wherever you’d like to keep your music and run it. This will create folders in which you’ll sort your songs. The songlist.txt in each folder will indicate what the original tracks were and how many there are
  - Song folders are named with a code for representing a band in the base game followed by an underscore
  - For organizational convenience, you may add to the folder name after the underscore. This can be helpful if song replacements are grouped using a certain theme or pattern
- Add song files to each folder. You can add up to total number of songs that artist had in the base game
- Rename each song according to the following format:
    
```
[Order Number]__[Artist Name]__[Album Name]__[Song Name].wav

--- e.x ---
in the "Westkust__" folder, the following

    1__BROCKHAMPTON_GINGER__NO HALO.wav
    2__Kevin Abstract__ARIZONA BABY__Peach.wav
    3__Kendrick Lamar__GNX__tv off (feat. lefty gunplay).wav

will be used to replace Swirl, Dishwasher, and Drown in the soundtrack
```
- **Order Number** - Maps the song you're naming to a song from that folder's artist. For example, In the Crystal Boys folder, an Order Nuber of 1 will replace their first track (Nightlife) in-game.You can use the `songlist.txt` of each artists' folder to guide your order assignments.
- If you followed the SpotDL instructions in step 1, the (lengthy) file renaming process will not be necessary except for songs where a valid Artist or Song Name could not be found.

#### 3. Run the mod tools
* Open the command prompt in the location of your downloaded build
* Run `KeepDrivingModCLI.exe`
```batch
KeepDrivingModCLI.exe "C:\Program Files (x86)\Steam\steamapps\common\Keep Driving" "C:\Users\xmarshall39\Music\KDMT\DevTest" --output "C:\Program Files (x86)\Steam\steamapps\common\Keep Driving\export"
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

##Acknowledgements
Thank you to the following creators, whose work has empowered and inspired my own
- The Underminers team, and all contributers to the Undertale Mod Tool
- SpotDL
- @LordBalvin on Twitch/Youtube whose [playthrough](https://www.youtube.com/watch?v=B3oWYbKgb7o) made me buy the game a few hours after watching it.
