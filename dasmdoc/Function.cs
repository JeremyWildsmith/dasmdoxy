using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using dasmdoc.Markup;

namespace dasmdoc
{
    class Function
    {
        private const String PARAMETER_REGEX = @"param\(([a-zA-Z0-9*]+),[ \t]*([a-zA-Z0-9]+)\)[ \t]*([^\n\r]+)";
        private const String RETURN_REGEX = @"return\(([a-zA-Z0-9*]+)\)[ \t]*(([^\n\r])+)";

        private const int PARAMETER_TYPE_GROUP = 1;
        private const int PARAMETER_NAME_GROUP = 2;
        private const int PARAMETER_DESCRIPTION_GROUP = 3;


        private const int RETURN_TYPE_GROUP             = 1;
        private const int RETURN_DESCRIPTION_GROUP      = 2;

        private DasmDocBlock        m_docBlock;
        private MarkupFileReference m_fileRef;

        public Function(MarkupFileReference fileRef, DasmDocBlock docBlock)
        {
            m_fileRef = fileRef;
            m_docBlock = docBlock;
        }

        private Match getReturnMatch()
        {

            foreach (KeyValuePair<String, String> attrib in m_docBlock.Attributes)
            {
                Match m = Regex.Match(attrib.Key + " " + attrib.Value, RETURN_REGEX);
                if (m.Success)
                    return m;
            }

            return null;
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_fileRef;
            }
        }

        public DasmDocBlock Documentation
        {
            get
            {
                return m_docBlock;
            }
        }

        public String Name
        {
            get
            {
                return m_docBlock.DefinitionName;
            }
        }

        public String Description
        {
            get
            {
                return m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_DESCRIPTION);
            }
        }

        public String ReturnType
        {
            get
            {
                Match returnMatch = getReturnMatch();

                if (!returnMatch.Success)
                    return String.Empty;
                else
                    return returnMatch.Groups[RETURN_TYPE_GROUP].Value;
            }
        }

        public String ReturnDescription
        {
            get
            {
                Match returnMatch = getReturnMatch();

                if (!returnMatch.Success)
                    return String.Empty;
                else
                    return returnMatch.Groups[RETURN_DESCRIPTION_GROUP].Value;
            }
        }

        public Parameter[] Parameters
        {
            get
            {
                List<Parameter> parameters = new List<Parameter>();

                foreach (KeyValuePair<String, String> attrib in m_docBlock.Attributes)
                {
                    Match m = Regex.Match(attrib.Key + " " + attrib.Value, PARAMETER_REGEX);
                    if (m.Success)
                    {
                        parameters.Add(new Parameter(m.Groups[PARAMETER_NAME_GROUP].Value, m.Groups[PARAMETER_TYPE_GROUP].Value, m.Groups[PARAMETER_DESCRIPTION_GROUP].Value));
                    }
                }

                return parameters.ToArray();
            }
        }
    }
}
