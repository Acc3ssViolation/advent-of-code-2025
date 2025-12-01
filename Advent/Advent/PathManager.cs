using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Advent
{
    internal static class PathManager
    {
#if DEBUG || true
        private static string BaseDir
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "D:/Projects/advent-of-code-2025/Advent/Advent/";
                else
                    return AppContext.BaseDirectory;
            }
        }
#else
        private static readonly string BaseDir = AppContext.BaseDirectory;
#endif

        public static readonly string DataDirectory = Path.Combine(BaseDir, "Data");
    }
}
