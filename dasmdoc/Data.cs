﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using dasmdoc.Markup;

namespace dasmdoc
{
    class Data : IDocumentationFeature
    {
        protected const String DATA_REGEX = @"^(:([a-zA-Z_][0-9a-z-A-Z_]+)|([a-zA-Z_][0-9a-z-A-Z_]+):)((([\n\r][\t ]+(?i:dat)[\t ][^\n\r]+(,( +)?[^\n\r]+)*)|([\n\r]))+)$";
        
        protected const int DATA_NAME_GROUP = 1;
        protected const int DATA_DEFINITION_GROUP = 4;

        private DasmDocBlock m_docBlock;
        private Match        m_match;

        protected Data(DasmDocBlock docBlock, Match match)
        {
            m_docBlock = docBlock;
            m_match = match;
        }

        public static Data parse(DasmDocBlock doc)
        {
            Match m = Regex.Match(doc.Content, DATA_REGEX);
            
            if (!m.Success)
                return null;

            return new Data(doc, m);
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
                return m_match.Groups[DATA_DEFINITION_GROUP].Value;
            }
        }

        public String Name
        {
            get
            {
                return m_match.Groups[DATA_NAME_GROUP].Value.Trim(':');
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
