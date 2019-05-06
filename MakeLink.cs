using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymlinkMaker
{
    class MakeLink
    {
        public static bool Create(string source, string output)
        {
            ProcessStartInfo mklink = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c mklink " + (source.isDirectory() ? "/d " : "") + $"\"{output}\" \"{source}\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process exec = Process.Start(mklink);
            exec.WaitForExit();
            exec.Dispose();

            if (source.isDirectory())
                return Directory.Exists(output);
            else
                return File.Exists(output);
        }
    }
}
