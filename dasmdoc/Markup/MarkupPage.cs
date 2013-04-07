using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dasmdoc.Markup
{
    public class MarkupPage : MarkupPrimitive
    {
        private String m_sName;

        private Dictionary<String, String>  m_attributes = new Dictionary<string,string>();

        private List<MarkupFunction>        m_functions = new List<MarkupFunction>();
        private List<MarkupData>            m_data = new List<MarkupData>();

        private List<MarkupRawSection>      m_rawSections = new List<MarkupRawSection>();

        public MarkupPage(MarkupFileReference fileRef, String sName)
            : base(fileRef)
        {
            m_sName = sName;
        }

        public void addAttribute(String sKey, String sName)
        {
            m_attributes.Add(sKey, sName);
        }

        public void addFunction(MarkupFunction func)
        {
            m_functions.Add(func);
        }

        public void addData(MarkupData data)
        {
            m_data.Add(data);
        }

        public void addSection(MarkupRawSection section)
        {
            m_rawSections.Add(section);
        }

        public override String MarkupEncoding
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}\n----\n", m_sName);
                sb.AppendLine(@"\tableofcontents");

                foreach (KeyValuePair<String, String> attrib in m_attributes)
                {
                    sb.AppendFormat("<b>{0}</b>: {1}\n", attrib.Key, attrib.Value);
                }
                
                foreach (MarkupRawSection sec in m_rawSections)
                {
                    sb.AppendLine(sec.MarkupEncoding);
                }

                if(m_data.Count > 0)
                {
                    sb.Append(new MarkupRawSection("Data").MarkupEncoding);

                    foreach (MarkupData md in m_data)
                    {
                        sb.AppendFormat("***\n{0}\n\n", md.MarkupEncoding);
                    }
                }

                if (m_functions.Count > 0)
                {
                    sb.Append(new MarkupRawSection("Functions").MarkupEncoding);
                    foreach (MarkupFunction mf in m_functions)
                    {
                        sb.AppendFormat("***\n{0}\n\n", mf.MarkupEncoding);
                    }
                }

                
                return sb.ToString();
            }
        }
    }
}
