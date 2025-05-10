using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepDrivingModCLI
{
    public class MusicKDRParser
    {
        string path;
        public MusicKDRParser(string kdrPath)
        {
            this.path = kdrPath;
        }

        public List<ReplacementOjbect> Deseialize()
        {
            List<ReplacementOjbect> ret = new List<ReplacementOjbect>();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                Dictionary<string, int> artistIndexDict = new Dictionary<string, int>();
                ReplacementOjbect current = null;
                foreach (string line in lines)
                {
                    if (line == "{")
                    {
                        current = new ReplacementOjbect();
                    }
                    else if (line == "}")
                    {
                        ret.Add(current);
                        current = null;
                    }
                    else if (current != null)
                    {
                        if (string.IsNullOrEmpty(line)) { continue; }

                        string[] splitLine = line.Split(':');
                        if (splitLine.Length != 2) { continue; }

                        string tag = splitLine[0].Trim();
                        string value = splitLine[1].Trim();
                        switch (tag)
                        {
                            case "dev_name":
                                current.kdr_devname = value;
                                break;
                            case "title":
                                current.kdr_title = value;
                                break;
                            case "artist":
                                current.kdr_artist = value;
                                if (artistIndexDict.ContainsKey(value))
                                {
                                    current.kdr_artistIndex = ++artistIndexDict[value];
                                }
                                else
                                {
                                    artistIndexDict[value] = 0;
                                    current.kdr_artistIndex = 0;
                                }
                                
                                break;
                            case "track":
                                current.kdr_track = value;
                                break;
                            case "start":
                                current.kdr_start = value == "0" ? false : true;
                                current.replacement_start = current.kdr_start;
                                break;
                            case "streamer_ban":
                                current.kdr_streamer = value == "0" ? false : true;
                                current.replacement_streamer = current.kdr_streamer;
                                break;
                        }
                    }

                }
            }
            return ret;
        }

        public void TrySerialize(string outputDir, List<ReplacementOjbect> objs)
        {
            string filename = Path.Combine(outputDir, "music.kdr");
            StringBuilder sb = new StringBuilder();

            foreach (ReplacementOjbect obj in objs)
            {
                sb.AppendLine("{");
                sb.Append("\tdev_name: ");
                sb.AppendLine(obj.kdr_devname);
                sb.Append("\ttitle: ");
                sb.AppendLine(string.IsNullOrEmpty(obj.replacement_trackName) ? obj.kdr_title : obj.replacement_trackName);
                sb.Append("\tartist: ");
                sb.AppendLine(string.IsNullOrEmpty(obj.replacement_artist) ? obj.kdr_artist : obj.replacement_artist);
                sb.Append("\ttrack: ");
                sb.AppendLine(string.IsNullOrEmpty(obj.EmbeddedTrack) ? obj.kdr_track : obj.EmbeddedTrack);
                sb.Append("\tstart: "); 
                sb.AppendLine(obj.replacement_start ? "1" : "0"); //Default value is based on kdr
                sb.Append("\tstreamer_ban: ");
                sb.AppendLine(obj.replacement_streamer ? "1" : "0"); //Default value is based on kdr
                sb.AppendLine("}");
                sb.Append("\n");
            }

            File.WriteAllText(filename, sb.ToString());

        }
    }
}
