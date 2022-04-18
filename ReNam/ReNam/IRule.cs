using System;
using System.Collections.Generic;
using System.Text;

namespace ReNam
{
    public interface IRule
    {
        public int ID { get; set; }
        public string Rule { get; }
        public string Statement { get; }

        public string ApplyRule(string current);
    }
}
