using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dasmdoc
{
    class DasmDocBlock
    {
        public const String COMMENTBLOCK_REGEX = @"(;;[\n\r](;[^\n\r]*[\n\r])*;;)[\n\r](:([a-zA-Z_][0-9a-z-A-Z_]+)|([a-zA-Z_][0-9a-z-A-Z_]+):)?(([\n\r][\t ][^\n\r]*|[\n\r])+)";

        public const String ATTRIBUTE_DESCRIPTION   = "details";
        public const String ATTRIBUTE_TYPE          = "type";

        protected const String DATASIGNATURE_REGEX = @"(:([a-zA-Z_][0-9a-z-A-Z_]+)|([a-zA-Z_][0-9a-z-A-Z_]+):)([\n\r][\t ]+(?i:dat)[\t ][0-9xXa-zA-Z_]+(,( +)?[0-9xXa-zA-Z_]+)?)+";

        protected const String ATTRIBUTE_REGEX            = @";\\([^\n\r \(]+(\([^\n\r]*\))?)([^\n\r]+)";
        protected const int ATTRIBUTE_REGEX_NAMEGROUP       = 1;
        protected const int ATTRIBUTE_REGEX_VALUEGROUP      = 3;


        public const int COMMENTBLOCK_REGEX_COMMENTGROUP    = 1;
        public const int COMMENTBLOCK_REGEX_NAMEGROUP       = 5;
        public const int COMMENTBLOCK_REGEX_DEFINITIONGROUP = 6;

        Match m_match;

        private String m_sContent;

        public DasmDocBlock(String sContent)
        {
            m_sContent = sContent;
            m_match = Regex.Match(m_sContent, COMMENTBLOCK_REGEX);
        }

        public String Content
        {
            get
            {
                return m_sContent;
            }
        }

        public String Comment
        {
            get
            {
                return m_match.Groups[COMMENTBLOCK_REGEX_COMMENTGROUP].Value;
            }
        }

        public String Definition
        {
            get
            {
                return m_match.Groups[COMMENTBLOCK_REGEX_DEFINITIONGROUP].Value;
            }
        }

        public String DefinitionName
        {
            get
            {
                return m_match.Groups[COMMENTBLOCK_REGEX_NAMEGROUP].Value;
            }
        }

        public String getAttribute(String sName)
        {
            foreach (KeyValuePair<String, String> attrib in this.Attributes)
            {
                if (attrib.Key == sName)
                    return attrib.Value;
            }

            return null;
        }

        public DasmCommentType Type
        {
            get
            {
                if (Regex.Match(m_sContent, DATASIGNATURE_REGEX, RegexOptions.Singleline).Success)
                    return DasmCommentType.Data;
                else if (Definition.Trim().Length == 0)
                    return DasmCommentType.Floating;
                else
                    return DasmCommentType.Function;
            }
        }

        public KeyValuePair<String, String>[] Attributes
        {
            get
            {
                List<KeyValuePair<String, String>> attributes = new List<KeyValuePair<String, String>>();

                MatchCollection matches = Regex.Matches(this.Comment, ATTRIBUTE_REGEX);

                foreach (Match m in matches)
                {
                    attributes.Add(
                        new KeyValuePair<String, String>(
                                m.Groups[ATTRIBUTE_REGEX_NAMEGROUP].Value,
                                m.Groups[ATTRIBUTE_REGEX_VALUEGROUP].Value
                                )
                              );
                }

                return attributes.ToArray();
            }
        }
    }
}
