using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public class MarkupGroup : MarkupPrimitive
    {
        private static String GROUP_NAME_PREFIX = "DASMDOC_G";
        private static int m_iGroups = 0;

        private String m_sName;
        private List<MarkupPage> m_pages = new List<MarkupPage>();

        public MarkupGroup(String sName)
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

                sb.AppendFormat("\\defgroup {0} {1}\n", this.Id, this.Name);
                sb.AppendFormat("\\ingroup {0}\n@{{", this.Id);

                foreach (MarkupPage page in m_pages)
                {
                    sb.AppendLine(page.MarkupEncoding);
                }

                sb.AppendLine("\n@}");
                return sb.ToString();
            }
        }
    }
}
