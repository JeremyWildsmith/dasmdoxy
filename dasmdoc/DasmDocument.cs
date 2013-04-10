using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using dasmdoc.Markup;

namespace dasmdoc
{
    class DasmDocument : MarkupPrimitive
    {

        private List<Data>     m_data = new List<Data>();
        private List<Function> m_functions = new List<Function>();
        private List<FloatingComment> m_floating = new List<FloatingComment>();

        private Dictionary<String, String> m_floatingArguments = new Dictionary<String, String>();

        protected DasmDocument(String sSrcPath)
            : base(new MarkupFileReference(sSrcPath, 0))
        {
        }

        public static DasmDocument parseDocument(FileInfo document)
        {
            DasmDocument doc = new DasmDocument(document.Name);
            String sContents = new StreamReader(document.OpenRead()).ReadToEnd();

            //.Net Regex doesn't parse \r as would be expected.
            sContents = Regex.Replace(sContents, @"\r", "");

            MatchCollection matches = Regex.Matches(sContents, DasmDocBlock.COMMENTBLOCK_REGEX, RegexOptions.Singleline);
            
            foreach(Match m in matches)
            {
                DasmDocBlock docBlock = new DasmDocBlock(m.Value);
                MarkupFileReference fileRef = new MarkupFileReference(doc.FileReference.RelativePath, m.Index);
                switch(docBlock.Type)
                {
                    case DasmCommentType.Function:
                        doc.m_functions.Add(new Function(fileRef, docBlock));
                        break;
                    case DasmCommentType.Data:
                        doc.m_data.Add(new Data(fileRef, docBlock));
                        break;
                    case DasmCommentType.Floating:
                        String sFilteredContent = Regex.Replace(docBlock.Content, "^(;;?)", String.Empty, RegexOptions.Multiline);
                        doc.m_floating.Add(new FloatingComment(sFilteredContent));
                        break;
                    default:
                        throw new ParsingException("Unrecognized documentation type.");
                }
            }

            return doc;
        }

        public MarkupPage MarkupPage
        {
            get
            {
                MarkupPage doc = new MarkupPage(this.FileReference, this.FileReference.FileName);

                foreach (KeyValuePair<String, String> argument in m_floatingArguments)
                    doc.addAttribute(argument.Key, argument.Value);

                foreach (FloatingComment floating in m_floating)
                {
                    doc.addSection(new MarkupRawSection(String.Empty, MarkupSectionType.SubSection, floating.Comment));
                }

                foreach (Data data in m_data)
                {
                    doc.addData(new MarkupData(data.FileReference, data.Type,
                                                data.Name, data.Description,
                                                data.Documentation.Definition));
                }

                foreach (Function func in m_functions)
                {
                    List<MarkupData> parameters = new List<MarkupData>();

                    foreach (Parameter param in func.Parameters)
                        parameters.Add(new MarkupData(func.FileReference, param.Type, param.Name, param.Description));

                    doc.addFunction(new MarkupFunction(func.FileReference,
                                    func.Name, func.Description, func.CallingConvention,
                                    func.ReturnType, func.ReturnDescription,
                                    func.Documentation.Definition, parameters.ToArray()));
                }

                return doc;
            }
        }

        public override string MarkupEncoding
        {
            get
            {
                return MarkupPage.MarkupEncoding;
            }
        }
    }
}
