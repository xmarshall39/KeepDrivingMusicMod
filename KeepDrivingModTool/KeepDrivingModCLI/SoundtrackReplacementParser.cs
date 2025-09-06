using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace KeepDrivingModCLI
{
    /// <summary>
    /// This will look through all of the .wav files in a given directory and create ReplacementData
    /// </summary>
    internal class SoundtrackReplacementParser
    {
        /// <summary>
        /// Single-word artists retain the whole word, while multi-word artists are abbreviated
        /// To keep directory names short and easy
        /// </summary>
        public static Dictionary<string, string> ArtistAbbreviationLUT = new Dictionary<string, string>()
        {
            { "Westkust", "Westkust" },
            { "HolyNow", "Holy Now" },
            { "Zimmer", "Zimmer Grandioso"},
            { "Mundane", "Mundane" },
            { "CrystalBoys", "Crystal Boys" },
            { "FWA", "FUCKING WEREWOLF ASSO"},
            { "Makthaverskan", "Makthaverskan"},
            { "Honeydrips", "The Honeydrips" },
            { "Dorena", "Dorena" },
            { "MyDarlingYou", "My Darling YOU!" },
            { "Aasma", "Aasma"},
            { "Songwriter", "The Songwriter"},
            { "Huervo", "El Huervo" }
        };

        public enum ParseMethod
        {
            Metadata, Filename, None
        }

        string path;

        public SoundtrackReplacementParser(string soundtrackPath)
        {
            this.path = soundtrackPath;
        }

        public ReplacementOjbect FindMatchingReplacement(string artistName, int artistIndex, List<ReplacementOjbect> replacements)
        {
            return replacements.Find(x => x.kdr_artist == artistName  && x.kdr_artistIndex == artistIndex);
        }

        /// <summary>
        /// Assign replacement songs to existing KDR data
        /// </summary>
        /// <param name="replacements"></param>
        /// <param name="parseMethod"></param>
        /// <returns></returns>
        public bool Parse(List<ReplacementOjbect> replacements, ParseMethod parseMethod = ParseMethod.Metadata)
        {
            bool anyReplacements = false;
            string[] musicFolders = Directory.GetDirectories(path);
            if (musicFolders.Length == 0)
            {
                Console.Error.WriteLine("Error: Empty music replacement directory!");
                return false;
            }

            foreach (string fullDir in musicFolders)
            {
                string dirName = new DirectoryInfo(fullDir).Name;
                string[] splitName = dirName.Split('_');
                string artistCode = splitName[0];
                string replacementCode = string.Empty;
                if (splitName.Length > 1) replacementCode = splitName[1];

                if (ArtistAbbreviationLUT.TryGetValue(artistCode, out string originalArtistName))
                {
                    if (replacementCode != string.Empty)
                    {
                        Console.WriteLine($"Mapping replacements for {originalArtistName} songs as {replacementCode} songs");
                    }
                    string[] files = Directory.GetFiles(fullDir).Where(x => Path.GetExtension(x) == ".wav").ToArray();
                    for(int i = 0; i < files.Length; ++i)
                    {
                        int replacementNo;
                        string filename = Path.GetFileName(files[i]);
                        if(!int.TryParse(filename[0].ToString(), out replacementNo))
                        {
                            Console.Error.WriteLine($"Song filename formatting error: {filename} please ensure the filename begins with a number" +
                                "denoting the target replacement!");
                        }
                        ReplacementOjbect target =
                            FindMatchingReplacement(originalArtistName, replacementNo - 1, replacements);

                        if (target != null)
                        {
                            anyReplacements = true;

                            switch (parseMethod)
                            {
                                // TODO: Fully implement metadata parsing using taglib
                                // Currently, this should never run
                                case ParseMethod.Metadata:
                                    string song = Path.GetFileName(files[i]);
                                    var tagfile = TagLib.File.Create(files[i]);
                                    break;

                                case ParseMethod.Filename:
                                    string[] splitFilename = Path.GetFileNameWithoutExtension(files[i]).Split(new string[] { Constants.SONG_METADATA_DELIM }, StringSplitOptions.None);
                                    if (splitFilename.Length >= 4)
                                    {
                                        string trackNo = splitFilename[0];
                                        string artist = splitFilename[1];
                                        string album = splitFilename[2];
                                        string trackName = splitFilename[3];
                                        target.replacement_artist = artist;
                                        target.replacement_album = album;
                                        target.replacement_trackNo = trackNo;
                                        target.replacement_trackName = trackName;
                                        target.replacement_path = files[i];
                                        target.UpdateEmbeddedTitle();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine($"Error: Failed to assign a devname value for {originalArtistName} song #{i}." +
                                $"\nYou may need to validate Keep Driving game files!");
                        }
                    }
                }
            }

            foreach (var replacement in replacements)
            {
                if(!string.IsNullOrEmpty(replacement.replacement_artist)) replacement.Print();
            }

            return anyReplacements;
        }
    }
}
