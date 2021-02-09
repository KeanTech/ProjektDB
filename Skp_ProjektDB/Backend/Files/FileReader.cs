using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Skp_ProjektDB.Backend.Files
{
    internal class FileReader
    {
        public string ReadFileToString(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
