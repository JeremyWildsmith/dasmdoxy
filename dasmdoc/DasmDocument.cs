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

        private List<Data> m_data = new List<Data>();
        private List<Function> m_functions = new List<Function>();
        private List<Constant> m_constants = new List<Constant>();
        private List<FloatingComment>   m_floating = new List<FloatingComment>();

        protected DasmDocument(String sSrcPath)
            : base(new MarkupFileReference(sSrcPath, 0))
        {
        }

        public static DasmDocument parseDocument(FileInfo document)
        {
            DasmDocument doc = new DasmDocument(document.Name);
            String sContents = new StreamReader(document.OpenRead()).ReadToEnd();

            //.Net Regex doesn't parse \r as would be expected.
            sContents = Regex.Replace(sContents, @"\r", String.Empty);

            MatchCollection matches = Regex.Matches(sContents, DasmDocBlock.COMMENTBLOCK_REGEX, RegexOptions.Singleline);
            
            foreach(Match m in matches)
            {
                MarkupFileReference fileRef = new MarkupFileReference(doc.FileReference.RelativePath, m.Index);
                DasmDocBlock docBlock = DasmDocBlock.parse(m.Value, fileRef);

                IDocumentationFeature docFeature = null;

                if ((docFeature = Data.parse(docBlock)) != null)
                    doc.m_data.Add(docFeature as Data);
                else if ((docFeature = Function.parse(docBlock)) != null)
                    doc.m_functions.Add(docFeature as Function);
                else if ((docFeature = FloatingComment.parse(docBlock)) != null)
                    doc.m_floating.Add(docFeature as FloatingComment);
                else if ((docFeature = Constant.parse(docBlock)) != null)
                    doc.m_constants.Add(docFeature as Constant);
                else
                    throw new ParsingException("Unrecognized comment structure", fileRef);
            }

            return doc;
        }

        public MarkupPage MarkupPage
        {
            get
            {
                MarkupPage doc = new MarkupPage(this.FileReference, this.FileReference.FileName);

                foreach (FloatingComment floating in m_floating)
                {
                    doc.addSection(new MarkupRawSection(String.Empty, MarkupSectionType.SubSection, floating.Comment));
                }

                foreach (Constant constant in m_constants)
                {
                    doc.addConstant(new MarkupData(constant.FileReference, constant.Type,
                                                constant.Name, constant.Description,
                                                constant.Definition));
                }

                foreach (Data data in m_data)
                {
                    doc.addData(new MarkupData(data.FileReference, data.Type,
                                                data.Name, data.Description,
                                                data.Definition));
                }

                foreach (Function func in m_functions)
                {
                    List<MarkupData> parameters = new List<MarkupData>();

                    foreach (Parameter param in func.Parameters)
                        parameters.Add(new MarkupData(func.FileReference, param.Type, param.Name, param.Description));

                    doc.addFunction(new MarkupFunction(func.FileReference,
                                    func.Name, func.Description, func.CallingConvention,
                                    func.ReturnType, func.ReturnDescription,
                                    func.Definition, parameters.ToArray()));
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
