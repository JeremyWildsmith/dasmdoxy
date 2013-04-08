using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public class MarkupRawSection : MarkupPrimitive
    {
        private static String SECTION_NAME_PREFIX = "DASMDOC_S";
        private static int m_iSections = 0;

        private String m_sName;
        private String m_sContent;

        private MarkupSectionType m_sectionType;

        public MarkupRawSection(String sName, MarkupSectionType sectionType, String sContent)
        {
            m_sName = sName;
            m_sectionType = sectionType;
            m_sContent = sContent;
            m_iSections++;
        }

        public MarkupRawSection(String sName, MarkupSectionType sectionType)
        {
            m_sName = sName;
            m_sectionType = sectionType;

            m_sContent = String.Empty;
            m_iSections++;
        }

        public String Name
        {
            get
            {
                return m_sName;
            }
        }

        public String Id
        {
            get
            {
                return SECTION_NAME_PREFIX + Convert.ToString(m_iSections);
            }
        }

        public String Content
        {
            get
            {
                return m_sContent;
            }
        }


        public override String MarkupEncoding
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (this.m_sName.Length > 0)
                {
                    switch (this.m_sectionType)
                    {
                        case MarkupSectionType.Section:
                            sb.Append(@"\section");
                            break;
                        case MarkupSectionType.SubSection:
                            sb.Append(@"\subsection");
                            break;
                        case MarkupSectionType.SubSubSection:
                            sb.Append(@"\subsubsecion");
                            break;
                        default:
                            throw new ParsingException("Invalid section type");
                    }
                    sb.AppendFormat(" {0} {1}\n{2}", this.Id, this.Name, this.m_sContent);
                }
                else
                    sb.AppendLine(m_sContent);

                return sb.ToString();
            }
        }
    }
}
