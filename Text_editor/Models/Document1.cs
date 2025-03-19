using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Text_editor.Models
{
    internal class Document1
    {
        public string Content { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public void Save()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new InvalidOperationException("File path is not set.");
            }
            File.WriteAllText(FilePath, Content);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file does not exist.", path);
            }
            FilePath = path;
            Content = File.ReadAllText(path);
        }

        public void CreateNew(string path)
        {
            FilePath = path;
            Content = string.Empty;
            // don't create the file, it will be created when Save is called
        }
    }
}
