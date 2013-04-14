using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using dasmdoc.Markup;

namespace dasmdoc
{
    class DasmDocBlock
    {
        public const String COMMENTBLOCK_REGEX = @"(;;[\n\r]((;[^\n\r]*[\n\r])*);;)[\n\r]([^\n\r]*([\n\r][\t ][^\n\r]*|[\n\r])*)?";

        public const String ATTRIBUTE_DESCRIPTION   = "details";
        public const String ATTRIBUTE_TYPE          = "type";

        protected const String CODE_REGEX = @"";
        protected const String MACRO_REGEX = @"";

        protected const String ATTRIBUTE_REGEX              = @"\\([^\n\r \(]+(\([^\n\r]*\))?)([^\n\r]*)";
        
        protected const int ATTRIBUTE_REGEX_NAMEGROUP       = 1;
        protected const int ATTRIBUTE_REGEX_VALUEGROUP      = 3;

        public const int COMMENTBLOCK_COMMENTGROUP    = 2;
        public const int COMMENTBLOCK_DEFINITIONGROUP = 4;

        private MarkupFileReference m_fileReference;
        private Match               m_blockMatch;

        protected DasmDocBlock(Match blockMatch, MarkupFileReference fileRefernece)
        {
            m_blockMatch = blockMatch;
            m_fileReference = fileRefernece;
        }

        public static DasmDocBlock parse(String sContent, MarkupFileReference fileReference)
        {
            Match m = Regex.Match(sContent, COMMENTBLOCK_REGEX);

            if (!m.Success)
                throw new ParsingException("Improperly formed comment block.");

            return new DasmDocBlock(m, fileReference);
        }

        public String RawText
        {
            get
            {
                return m_blockMatch.Groups[0].Value;
            }
        }

        public String Content
        {
            get
            {
                return m_blockMatch.Groups[COMMENTBLOCK_DEFINITIONGROUP].Value;
            }
        }

        public String Comment
        {
            get
            {
                return Regex.Replace(m_blockMatch.Groups[COMMENTBLOCK_COMMENTGROUP].Value, "^;", String.Empty, RegexOptions.Multiline);
            }
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_fileReference;
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
