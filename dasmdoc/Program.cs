using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using dasmdoc.Markup;
using System.Diagnostics;

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

                MarkupPageGroup root = new MarkupPageGroup("Code");

                foreach (FileInfo fi in files)
                {
                    if (!(fi.Name.EndsWith(".10c") || fi.Name.EndsWith(".dasm")))
                        continue;

                    root.addPage(DasmDocument.parseDocument(fi).MarkupPage);

                }
                StreamWriter ss = new StreamWriter(new FileStream(diDest.FullName + @"\Code.md", FileMode.Create));
                ss.WriteLine(root.MarkupEncoding);
                ss.Flush();
                ss.Close();
                ProcessStartInfo pi = new ProcessStartInfo("doxygen.exe", "config");
                pi.CreateNoWindow = false;
                Process.Start(pi);

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
