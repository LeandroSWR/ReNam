using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReNam
{
    public class FileName
    {
        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        private string path;
        public string Path 
        { 
            get => path; 
        }

        private string format;
        public string Format 
        { 
            get => format.Substring(1);
            set => format = value;
        }
        
        public string FullPath 
        { 
            get => String.Format(Path + Name + format);
        }

        private string visibility;
        public string Visibility
        {
            get => visibility;
            set => visibility = value;
        }

        public FileName(string name, string path, string format, string visibility = "Visible")
        {
            this.name = name;
            this.path = path;
            this.format = format;
            this.visibility = visibility;
        }
    }
}
