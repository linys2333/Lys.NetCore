using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MyService.Common
{
    public static class FFmpegExt
    {
        public static string Convert(string inputPath, string outputPath)
        {
            var basePath = Directory.GetCurrentDirectory();
            var cmd = $@"{basePath}\ffmpeg.exe -i {inputPath} {outputPath}";
            return RunCmd(cmd);
        }

        public static string RunCmd(string cmd)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            process.Start();

            process.StandardInput.WriteLine(cmd);
            process.StandardInput.AutoFlush = true;

            //string outStr = process.StandardOutput.ReadToEnd();
            //string errStr = process.StandardError.ReadToEnd();
            process.StandardInput.Write("exit");
            process.WaitForExit();
            process.Close();

            return "";
        }

        public static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
