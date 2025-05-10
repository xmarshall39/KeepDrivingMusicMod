using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Policy;

namespace KeepDrivingModCLI
{
    internal class Program
    {
        static int Main(string[] args) 
        {
            var kdBaseDirArg = new Argument<DirectoryInfo>(name: "Keep_Driving_Steam_Dir", description: "Location of steam keep driving dir");
            var soundtrackDirArg = new Argument<DirectoryInfo>(name: "SoundtrackDir", description: "Location of steam keep driving dir");

            var outputOption = new Option<DirectoryInfo>(new string[] { "-o", "--output"}, "Where to write files.");
            outputOption.SetDefaultValue(null);

            var rootCommand = new RootCommand("Root command")
            {
                kdBaseDirArg,
                soundtrackDirArg,
                outputOption
            };
            rootCommand.SetHandler((baseDir, soundtrackDir, outputDir) =>
            {
               RunBulkReplacement(baseDir, soundtrackDir, outputDir);
            }, kdBaseDirArg, soundtrackDirArg, outputOption);

            var result = rootCommand.Invoke(args);

            return result;
        }

        internal static void RunBulkReplacement(DirectoryInfo baseDir, DirectoryInfo soundtrackDir, DirectoryInfo outputDir)
        {
            if (outputDir == null) { outputDir = baseDir; }

            //Try to validate using steam cm
            MusicKDRParser kdrParser = new MusicKDRParser(Path.Combine(baseDir.FullName, "music.kdr"));
            List<ReplacementOjbect> replacements = kdrParser.Deseialize();
            SoundtrackReplacementParser soundtrackParser = new SoundtrackReplacementParser(soundtrackDir.FullName);
            if (!soundtrackParser.Parse(replacements, SoundtrackReplacementParser.ParseMethod.Filename))
            {
                return;
            }
            //Populate command list using replacement data
            List<string> commands = ReplacementOjbect.GenerateReplacementCommands(baseDir.FullName, outputDir.FullName, replacements);

            for (int i = 0; i < commands.Count; ++i)
            {
                string umtPath = "UMT_CLI/UndertaleModCli.exe";
                if (!File.Exists(umtPath)) umtPath = Path.Combine(Environment.CurrentDirectory, "../../../../UndertaleModCli/bin/Debug/net8.0/UndertaleModCli.exe");

                if (File.Exists(umtPath))
                {
                    ProcessStartInfo process = new ProcessStartInfo()
                    {
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        FileName = umtPath,
                        Arguments = commands[i],
                        RedirectStandardError = true
                    };

                    Process procResult = Process.Start(process);
                    procResult.WaitForExit();
                    Console.WriteLine(procResult.StartInfo.Arguments);
                    Console.WriteLine(procResult.StandardOutput.ReadToEnd());
                    Console.WriteLine(procResult.StandardError.ReadToEnd());
                }
                else
                {
                    Console.Error.WriteLine("UndertaleModCli.exe not found!!");
                }
            }

            //Execute CLI executable with generated commands
            kdrParser.TrySerialize(outputDir.FullName, replacements);

            return;
        }
    }
}
