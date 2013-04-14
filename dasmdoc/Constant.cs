using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using dasmdoc.Markup;

namespace dasmdoc
{
    class Constant : IDocumentationFeature
    {
        protected const String CONST_REGEX = @"#define[ \t]+([^\n\r]+)[ \t]+([^\n\r]+)";
        
        protected const int CONST_NAME_GROUP = 1;
        protected const int CONST_DEFINITION_GROUP = 0;

        private DasmDocBlock m_docBlock;
        private Match        m_match;

        protected Constant(DasmDocBlock docBlock, Match match)
        {
            m_docBlock = docBlock;
            m_match = match;
        }

        public static Constant parse(DasmDocBlock doc)
        {
            Match m = Regex.Match(doc.Content, CONST_REGEX);
            
            if (!m.Success)
                return null;

            return new Constant(doc, m);
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_docBlock.FileReference;
            }
        }

        public String Definition
        {
            get
            {
                return m_docBlock.Content;
            }
        }

        public String Name
        {
            get
            {
                return m_match.Groups[CONST_NAME_GROUP].Value;
            }
        }

        public String Description
        {
            get
            {
                return m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_DESCRIPTION);
            }
        }

        public String Type
        {
            get
            {
                String sType = m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_TYPE);

                if (sType == null || sType.Trim().Length == 0)
                    throw new ParsingException(String.Format("Invalid return type for data {0}", this.Name), this.FileReference);

                return sType;
            }
        }
    }
}
