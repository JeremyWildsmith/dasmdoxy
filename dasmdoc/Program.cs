using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using dasmdoc.Markup;

namespace dasmdoc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                    throw new Exception("Invalid number of arguments.");

                DirectoryInfo diSrc = new DirectoryInfo(args[0]);
                DirectoryInfo diDest = new DirectoryInfo(args[1]);

                if (!diDest.Exists)
                    diDest.Create();

                FileInfo[] files = diSrc.GetFiles();

                MarkupGroup root = new MarkupGroup("DasmDoxy");

                foreach (FileInfo fi in files)
                {
                    if (!fi.Name.EndsWith(".10c") || !fi.Name.EndsWith(".dasm"))
                        continue;

                    DasmDocument doc = DasmDocument.parseDocument(fi);
                    StreamWriter ss = new StreamWriter(new FileStream(diDest.FullName + @"\" + doc.FileReference.FileName + ".md", FileMode.Create));
                    ss.WriteLine(doc.MarkupEncoding);
                    ss.Flush();
                    ss.Close();
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
