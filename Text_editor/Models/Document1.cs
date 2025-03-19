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
        public string Content { get; set; }
        public string FilePath { get; set; }

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

        }
    }
}
