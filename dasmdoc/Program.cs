using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace dasmdoc
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("out.md", FileMode.Create);
            StreamWriter ss = new StreamWriter(fs);

            DasmDocument doc = DasmDocument.parseDocument("peLoader.10c");

            //byte[] buffer = ASCIIEncoding.ASCII.GetBytes(doc.MarkupEncoding);

            String s = (doc.MarkupEncoding);
            ss.WriteLine(doc.MarkupEncoding);
            ss.Flush();
            ss.Close();
        }
    }
}
