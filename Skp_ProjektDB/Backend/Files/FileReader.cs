using System.IO;

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
