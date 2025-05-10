using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepDrivingModCLI
{
    public class ReplacementOjbect
    {
        public enum DataFile
        {
            AG1, AG3, AG4, WIN
        }

        public struct ReplacementData
        {
            DataFile dataFile;
            string newSongPath;
        }
        public struct FileMapping : IEquatable<FileMapping>
        {
            public DataFile file;
            public int audioID;

            public FileMapping(int audioID)
            {
                this.audioID = audioID;
                this.file = DataFile.AG3;
            }
            
            public FileMapping(DataFile file, int audioID)
            {
                this.file = file;
                this.audioID = audioID;
            }

            public bool Equals(FileMapping other)
            {
                return this.file == other.file && this.audioID == other.audioID;
            }
        }
        public static Dictionary<DataFile, string> DataFileLUT = new Dictionary<DataFile, string>()
        {
            {DataFile.AG1, "audiogroup1.dat" },
            {DataFile.AG3, "audiogroup3.dat" }, //audiogroup_music (UndertaleAudioGroup)
            {DataFile.AG4, "audiogroup4.dat" },
            {DataFile.WIN, "data.win" } //audiogroup_default (UndertaleAudioGroup)
        };
        public static Dictionary<DataFile, string> AudioGroupLUT = new Dictionary<DataFile, string>()
        {
            {DataFile.AG1, "ag_unknown" },
            {DataFile.AG3, "ag_music" }, //audiogroup_music (UndertaleAudioGroup)
            {DataFile.AG4, "ag_unknown" },
            {DataFile.WIN, "audiogroup_default" } //audiogroup_default (UndertaleAudioGroup)
        };

        public static Dictionary<string, FileMapping> DevnameFileMapping = new Dictionary<string, FileMapping>()
        {
            {"westkust01", new FileMapping(20) },
            {"westkust02", new FileMapping(21) },
            {"westkust03", new FileMapping(22) },
            {"westkust04", new FileMapping(23) },
            {"westkust05", new FileMapping(24) },
            {"westkust06", new FileMapping(25) },
            {"westkust07", new FileMapping(26) },
            {"westkust08", new FileMapping(27) },
            {"westkust09", new FileMapping(28) },
            {"holynow1", new FileMapping(DataFile.WIN, 45) },
            {"zimmer01", new FileMapping(0) },
            {"zimmer02", new FileMapping(2) },
            {"zimmer03", new FileMapping(4) },
            {"zimmer04", new FileMapping(6) },
            {"zimmer05", new FileMapping(8) },
            {"zimmer06", new FileMapping(9) },
            {"mundane01", new FileMapping(1) },
            {"mundane02", new FileMapping(3) },
            {"mundane03", new FileMapping(5) },
            {"mundane04", new FileMapping(7) },
            {"mundane05", new FileMapping(DataFile.WIN, 9) },
            {"crystal_boys01", new FileMapping(10) },
            {"crystal_boys02", new FileMapping(11) },
            {"crystal_boys03", new FileMapping(12) },
            {"crystal_boys04", new FileMapping(13) },
            {"crystal_boys05", new FileMapping(14) },
            {"crystal_boys06", new FileMapping(15) },
            {"crystal_boys07", new FileMapping(16) },
            {"crystal_boys08", new FileMapping(17) },
            {"crystal_boys09", new FileMapping(18) },
            {"crystal_boys10", new FileMapping(19) },
            {"werewolf01", new FileMapping(DataFile.WIN, 15) },
            {"werewolf02", new FileMapping(DataFile.WIN, 16) },
            {"werewolf03", new FileMapping(DataFile.WIN, 17) },
            {"werewolf_single_1", new FileMapping(DataFile.WIN, 11) },
            {"werewolf_single_2", new FileMapping(DataFile.WIN, 12) },
            {"werewolf_single_3", new FileMapping(DataFile.WIN, 13) },
            {"werewolf_single_4", new FileMapping(DataFile.WIN, 14) },
            {"makthaverskan01", new FileMapping(DataFile.WIN, 21) },
            {"makthaverskan02", new FileMapping(DataFile.WIN, 22) },
            {"makthaverskan03", new FileMapping(DataFile.WIN, 23) },
            {"makthaverskan04", new FileMapping(DataFile.WIN, 24) },
            {"makthaverskan05", new FileMapping(DataFile.WIN, 25) },
            {"makthaverskan06", new FileMapping(DataFile.WIN, 26) },
            {"honeydrips01", new FileMapping(DataFile.WIN, 40) },
            {"honeydrips02", new FileMapping(DataFile.WIN, 41) },
            {"honeydrips03", new FileMapping(DataFile.WIN, 42) },
            {"honeydrips04", new FileMapping(DataFile.WIN, 43) },
            {"honeydrips05", new FileMapping(DataFile.WIN, 44) },
            {"dorena01", new FileMapping(DataFile.WIN, 8) },
            {"kid_feral01", new FileMapping(DataFile.WIN, 5) },
            {"mdy_01", new FileMapping(DataFile.WIN, 27) },
            {"mdy_02", new FileMapping(DataFile.WIN, 28) },
            {"mdy_03", new FileMapping(DataFile.WIN, 29) },
            {"mdy_04", new FileMapping(DataFile.WIN, 30) },
            {"mdy_05", new FileMapping(DataFile.WIN, 31) },
            {"mdy_m_01", new FileMapping(DataFile.WIN, 32) },
            {"mdy_m_02", new FileMapping(DataFile.WIN, 33) },
            {"mdy_m_03", new FileMapping(DataFile.WIN, 34) },
            {"mdy_m_04", new FileMapping(DataFile.WIN, 35) },
            {"mdy_m_05", new FileMapping(DataFile.WIN, 36) },
            {"aasma_0", new FileMapping(DataFile.WIN, 6) },
            {"holy_now_d_1", new FileMapping(DataFile.WIN, 18) },
            {"holy_now_d_2", new FileMapping(DataFile.WIN, 19) },
            {"holy_now_d_3", new FileMapping(DataFile.WIN, 20) },
            {"sw_01", new FileMapping(DataFile.WIN, 37) },
            {"sw_02", new FileMapping(DataFile.WIN, 38) },
            {"sw_03", new FileMapping(DataFile.WIN, 39) },
            {"el_huervo_five", new FileMapping(DataFile.WIN, 9) },
            {"el_huervo_recluse", new FileMapping(DataFile.WIN, 10) }
        };

        public static string ReplaceSpacesWithUnderscore(string target)
        {
            StringBuilder sb = new StringBuilder();
            char lastChar = '\0';
            foreach(char c in target)
            {
                if (lastChar == ' ' && c == ' ') continue;
                else if (c == ' ') sb.Append('_');
                else sb.Append(c);
                lastChar = c;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Populate a set of commands for each file that hosts soundtrack data
        /// </summary>
        /// <param name="inputDir"></param>
        /// <param name="outputDir"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        public static List<string> GenerateReplacementCommands(string inputDir, string outputDir, List<ReplacementOjbect> replacements)
        {
            List<string> commands = new List<string>();
            Dictionary<DataFile, List<ReplacementOjbect>> commandMapping = new Dictionary<DataFile, List<ReplacementOjbect>>();
            foreach (var obj in replacements)
            {
                DataFile targetFile = DevnameFileMapping[obj.kdr_devname].file;
                if (!commandMapping.ContainsKey(targetFile))
                {
                    commandMapping.Add(targetFile, new List<ReplacementOjbect>() { obj });
                }
                else
                {
                    commandMapping[targetFile].Add(obj);
                }

                //Ensure all commands are added to the data.win file, so that Sound metadata also gets replaced
                //Replacements of audio not native to an asset file will only target UndertaleSounds (metadata) and not UndertaleEmbeddedAudio
                if (!commandMapping.ContainsKey(DataFile.WIN))
                {
                    commandMapping.Add(DataFile.WIN, new List<ReplacementOjbect> { obj });
                }
                else if (!commandMapping[DataFile.WIN].Contains(obj))
                {
                    commandMapping[DataFile.WIN].Add(obj);
                }
            }

            foreach (var cmdPair in commandMapping)
            {
                StringBuilder sb = new StringBuilder();
                string inputFile = Path.Combine(inputDir, DataFileLUT[cmdPair.Key]);
                string outputFile = Path.Combine(outputDir, DataFileLUT[cmdPair.Key]);
                sb.Append("replace \"");
                sb.Append(inputFile);
                sb.Append('"');
                sb.Append(" --audio");
                foreach (var obj in cmdPair.Value)
                {
                    if (string.IsNullOrEmpty(obj.EmbeddedTrack)) continue;
                    sb.Append(' ');
                    sb.Append(DevnameFileMapping[obj.kdr_devname].audioID);
                    sb.Append("=\"");
                    //sb.Append("=\".\\");
                    sb.Append(obj.replacement_path);
                    sb.Append("\"");
                    if (cmdPair.Key == DataFile.WIN)
                    {
                        sb.Append("=");
                        sb.Append(AudioGroupLUT[cmdPair.Key]);
                        sb.Append("=");
                        sb.Append(obj.EmbeddedTrack);
                    }
                }
                sb.Append(" --output \"");
                sb.Append(outputFile);
                sb.Append('"');
                commands.Add(sb.ToString());
            }

            return commands;
        }
        public string kdr_devname, kdr_title, kdr_artist, kdr_track;
        public bool kdr_start, kdr_streamer;
        public int kdr_artistIndex;
        public string replacement_trackName = string.Empty, replacement_artist = string.Empty,
                      replacement_album = string.Empty, replacement_trackNo = string.Empty,
                      replacement_path = string.Empty;
        public bool replacement_start, replacement_streamer;
        public string EmbeddedTrack { get; private set; } = string.Empty;

        public void UpdateEmbeddedTitle()
        {
            EmbeddedTrack = $"music_{ReplaceSpacesWithUnderscore(replacement_artist)}__" +
                $"{ReplaceSpacesWithUnderscore(replacement_album)}__" +
                $"{replacement_trackNo}_{ReplaceSpacesWithUnderscore(replacement_trackName)}";
            
        }

        public void Print()
        {
            Console.WriteLine("=== ReplacementObject ===");
            Console.WriteLine(" KDR Data:");
            Console.WriteLine($"\tDevname: {kdr_devname} | Artist Index: {kdr_artistIndex}");
            Console.WriteLine($"\tTitle: {kdr_title} | Artist: {kdr_artist} | Track: {kdr_track}");
            Console.WriteLine($"\tStart: {kdr_start} | Streamer: {kdr_streamer}");
            Console.WriteLine(" Replacement Data:");
            Console.WriteLine($"\tTitle: {replacement_trackName} | Artist: {replacement_artist} | Track: {EmbeddedTrack}\n");

        }
    }
}
