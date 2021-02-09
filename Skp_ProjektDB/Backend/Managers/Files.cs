using SkpDbLib.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkpDbLib.Managers
{
    public class Files : IDisposable
    {
        private FileReader _fileReader = new FileReader();
        private FileWriter _fileWriter = new FileWriter();

        public void WriteStringToFile(string filePath, string data)
        {
            _fileWriter.WriteStringToFile(filePath, data);
        }

        public void WritePdf(string filePath, List<string> data)
        {
            _fileWriter.WritePdf(filePath, data);
        }

        public string ReadFileToString(string filePath)
        {
            return _fileReader.ReadFileToString(filePath);
        }

        public void Dispose()
        {
            _fileReader = null;
            _fileWriter = null;
        }
    }
}
