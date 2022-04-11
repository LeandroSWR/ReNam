using System;
using System.Collections.Generic;
using System.Text;

namespace ReNam
{
    struct FileName
    {
        public string Name { get; set; }
        public string Path { get; }

        private string extention;
        public string Extention 
        { 
            get => extention.Substring(1); 
        }
        
        public string FullPath 
        { 
            get => String.Format(Path + Name + extention);
        }

        public FileName(string name, string path, string extention)
        {
            Name = name;
            Path = path;
            this.extention = extention;
        }
    }
}
