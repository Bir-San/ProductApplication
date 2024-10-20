using Newtonsoft.Json;
using Resources.Interfaces;
using Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Services
{
    public class FileService(string filePath) : IFileService
    {
        private readonly string _filePath = filePath;

        public bool SaveToFile(string content)
        {
            using var sw = new StreamWriter(_filePath, false);
            try
            {
                sw.Write(content);
                return true;
            }
            catch { return false; }
        }

        public string GetFromFile()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    using var sr = new StreamReader(_filePath);
                    return sr.ReadToEnd();
                }
                catch
                {
                }
            }
            return null!;
        }
    }
}
