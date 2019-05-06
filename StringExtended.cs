using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymlinkMaker
{
    static class StringExtended
    {
        public static bool isDirectory(this string path)
        {
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                return Directory.Exists(path);
            }
            return false;
        }
    }
}
