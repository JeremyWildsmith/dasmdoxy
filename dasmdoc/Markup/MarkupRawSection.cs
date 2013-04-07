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

        private bool m_isSubSection;

        public MarkupRawSection(String sName, bool isSubSection, String sContent)
        {
            m_sName = sName;
            m_isSubSection = isSubSection;
            m_sContent = sContent;
            m_iSections++;
        }

        public MarkupRawSection(String sName, bool isSubSection)
        {
            m_sName = sName;
            m_isSubSection = isSubSection;

            m_sContent = String.Empty;
            m_iSections++;
        }

        public MarkupRawSection(String sName)
        {
            m_sName = sName;
            m_isSubSection = false; ;

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
                    sb.AppendFormat("{0} {1} {2}\n{3}", (m_isSubSection ? "\\subsection" : "\\section"), this.Id, this.Name, this.m_sContent);
                else
                    sb.AppendLine(m_sContent);

                return sb.ToString();
            }
        }
    }
}
