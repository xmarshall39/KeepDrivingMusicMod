import subprocess
import sys
import json
import re
import unicodedata
import os

def slugify(value, allow_unicode=False):
    """
    Convert to ASCII if 'allow_unicode' is False. Remove all characters incompatible
    with Windows file names. Based on Django Utils Slugify:
    https://docs.djangoproject.com/en/5.2/ref/utils/
    """
    value = str(value)
    if allow_unicode:
        value = unicodedata.normalize("NFKC", value)
    else:
        value = (
            unicodedata.normalize("NFKD", value)
            .encode("ascii", "ignore")
            .decode("ascii")
        )
    value = re.sub(r'[\\/*?:"<>|]', "", value)
    return value

if __name__ == "__main__":
    spotdl_argstr = ""
    allow_unicode = False
    if len(sys.argv) < 3:
        print(f"Too few arguments provided! Please execute using the following format: "/
              f"kdmt-song-formatter [SONG_DOWNLOAD_LOCATION] [SONG/PLAYLIST URL]")
        exit(0)
    if not os.path.exists(sys.argv[1]):
        print(f"Provided path {sys.argv[i]} does not exist. Please provide a valid download location!")
        exit(0)
    
    os.chdir(sys.argv[1])
    spotdl_argstr += sys.argv[2]
    for i in range(2, len(sys.argv)):
        if sys.argv[i] == "-u" or sys.argv[i] == "--unicode":
            allow_unicode = True


    #Run spotdl and produce a metadata file
    spot_save_cmd = "spotdl save " + spotdl_argstr + " --save-file out.spotdl"
    print(spot_save_cmd)
    subprocess.call(spot_save_cmd)

    with open("out.spotdl", encoding="utf-8") as f:
        jdump = json.load(f)
        for i in range(len(jdump)):
            album_name = jdump[i]["album_name"]
            album_artist = jdump[i]["album_artist"]
            song_name = jdump[i]["name"]
            song_url = jdump[i]["url"]
            wav_name = f"{i + 1}__{album_artist}__{album_name}__{song_name}"
            wav_name = slugify(wav_name, allow_unicode) + ".wav"
            print(wav_name)
            spot_dl_cmd = "spotdl " + song_url + " --format wav" + " --output kdmt_temp" #+ '"' + wav_name + '"'
            subprocess.call(spot_dl_cmd)
            dirlist = os.listdir("kdmt_temp")
            if len(dirlist) > 0:
                os.replace(f"kdmt_temp/{dirlist[0]}", wav_name)
            else:
                print("No File Found!")
        os.rmdir("kdmt_temp")

    

