using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using dasmdoc.Markup;

namespace dasmdoc
{
    class Function : IDocumentationFeature
    {
        protected const String FUNCTION_REGEX   = @"(:([a-zA-Z_][0-9a-z-A-Z_]+)|([a-zA-Z_][0-9a-z-A-Z_]+):)((([\n\r]*[\t ][^\n\r]*)+))";
        protected const String PARAMETER_REGEX  = @"param\(([a-zA-Z0-9*]+),[ \t]*([a-zA-Z0-9]+)\)[ \t]*([^\n\r]+)";
        protected const String RETURN_REGEX     = @"return\(([a-zA-Z0-9*]+)\)[ \t]*(([^\n\r])*)";
        
        protected const String CALL_ATTRIBUTE   = @"call";

        protected const int FUNCTION_NAME_GROUP       = 3;
        protected const int FUNCTION_DEFINITION_GROUP = 4;

        protected const int PARAMETER_TYPE_GROUP = 1;
        protected const int PARAMETER_NAME_GROUP = 2;
        protected const int PARAMETER_DESCRIPTION_GROUP = 3;

        protected const int RETURN_TYPE_GROUP             = 1;
        protected const int RETURN_DESCRIPTION_GROUP      = 2;

        protected const int DATA_REGEX_NAMEGROUP = 3;
        protected const int DATA_REGEX_DEFINITIONGROUP = 4;

        private DasmDocBlock m_docBlock;
        private Match        m_functionMatch;

        public Function(DasmDocBlock docBlock, Match funcMatch)
        {
            m_docBlock = docBlock;
            m_functionMatch = funcMatch;
        }

        public static Function parse(DasmDocBlock doc)
        {
            Match m = Regex.Match(doc.Content, FUNCTION_REGEX);

            if (!m.Success)
                return null;

            return new Function(doc, m);
        }

        private Match getReturnMatch()
        {

            foreach (KeyValuePair<String, String> attrib in m_docBlock.Attributes)
            {
                Match m = Regex.Match(attrib.Key + " " + attrib.Value, RETURN_REGEX);
                if (m.Success)
                    return m;
            }
            
            throw new ParsingException(String.Format("Error locating return type to function {0}", this.Name), this.FileReference);

        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_docBlock.FileReference;
            }
        }

        public String Name
        {
            get
            {
                return m_functionMatch.Groups[FUNCTION_NAME_GROUP].Value;
            }
        }

        public String Definition
        {
            get
            {
                return m_functionMatch.Groups[FUNCTION_DEFINITION_GROUP].Value;
            }
        }

        public String Description
        {
            get
            {
                return m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_DESCRIPTION);
            }
        }

        public String CallingConvention
        {
            get
            {
                return m_docBlock.getAttribute(CALL_ATTRIBUTE);
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
