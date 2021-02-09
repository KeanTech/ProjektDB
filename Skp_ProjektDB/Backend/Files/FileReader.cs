using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkpDbLib.Files
{
    internal class FileReader
    {
        public string ReadFileToString(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
