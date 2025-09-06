using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Diagnostics;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;


namespace KeepDrivingModCLI
{
    internal class Program
    {
        static int Main(string[] args) 
        {
            var kdBaseDirArg = new Argument<DirectoryInfo>(name: "KeepDrivingSteamDir", description: "Directory containing your Steam installation of Keep Driving. This folder will include the .exe along with files like data.win and audiogroup.dat");
            var soundtrackDirArg = new Argument<DirectoryInfo>(name: "SoundtrackDir", description: "Directory containing the replacement soundtrack you created. It's subdirectories will include folders named after the base game's wonderful musical artists.");

            var outputOption = new Option<DirectoryInfo>(new string[] { "-o", "--output"}, "Directory to write new data.win and audiogroup.dat files. Defaults to Keep Driving's Steam directory.");
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
            
            Parser commandLine = new CommandLineBuilder(rootCommand)
            .UseDefaults() // automatically configures dotnet-suggest
            .Build();
            var result = commandLine.Invoke(args);
            return result;
        }

        internal static void RunBulkReplacement(DirectoryInfo baseDir, DirectoryInfo soundtrackDir, DirectoryInfo outputDir)
        {
            if (outputDir == null) { outputDir = baseDir; }

            string baseKDRPath = "music.kdr";
            if (!File.Exists(baseKDRPath)) baseKDRPath = "../../../../music.kdr";

            MusicKDRParser kdrParser = new MusicKDRParser(Path.Combine(baseDir.FullName, baseKDRPath));
            List<ReplacementOjbect> replacements = kdrParser.Deseialize();
            SoundtrackReplacementParser soundtrackParser = new SoundtrackReplacementParser(soundtrackDir.FullName);
            if (!soundtrackParser.Parse(replacements, SoundtrackReplacementParser.ParseMethod.Filename))
            {
                return;
            }

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

            kdrParser.TrySerialize(outputDir.FullName, replacements);

            return;
        }
    }
}
