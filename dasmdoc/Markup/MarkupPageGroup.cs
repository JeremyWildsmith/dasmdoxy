using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public class MarkupPageGroup : MarkupPrimitive
    {
        private static String GROUP_NAME_PREFIX = "DASMDOC_G";
        private static int m_iGroups = 0;

        private String m_sName;
        private List<MarkupPage> m_pages = new List<MarkupPage>();

        public MarkupPageGroup(String sName)
        {
            m_sName = sName;
            m_iGroups++;
        }

        public void addPage(MarkupPage page)
        {
            m_pages.Add(page);
        }

        public String Id
        {
            get
            {
                return GROUP_NAME_PREFIX + Convert.ToString(m_iGroups);
            }
        }

        public String Name
        {
            get
            {
                return m_sName;
            }
        }

        public override String MarkupEncoding
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("The " + m_sName + " group contains the following pages:<br/>\n\n");
                foreach (MarkupPage page in m_pages)
                {
                    sb.AppendFormat("- \\subpage {0}\n", page.DoxyName);
                }

                foreach (MarkupPage page in m_pages)
                {
                    sb.AppendLine(page.MarkupEncoding);
                }

                return sb.ToString();
            }
        }
    }
}
