using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public class MarkupData : MarkupPrimitive
    {
        private String m_sType;
        private String m_sName;
        private String m_sDesc;
        private String m_sCode;

        public MarkupData(MarkupFileReference fileRef, String sType, String sName, String sDesc, String sCode)
            : base(fileRef)
        {
            m_sType = sType;
            m_sName = sName;
            m_sDesc = sDesc;
            m_sCode = sCode;
        }

        public MarkupData(MarkupFileReference fileRef, String sType, String sName, String sDesc)
            : base(fileRef)
        {
            m_sType = sType;
            m_sName = sName;
            m_sDesc = sDesc;
            m_sCode = String.Empty;
        }

        public String Type
        {
            get
            {
                return m_sType;
            }
        }

        public String Name
        {
            get
            {
                return m_sName;
            }
        }

        public String Description
        {
            get
            {
                return m_sDesc;
            }
        }

        public String Code
        {
            get
            {
                return m_sCode;
            }
        }

        public override String MarkupEncoding
        {
            get
            {
                StringBuilder s = new StringBuilder();
                s.AppendLine(new MarkupRawSection(this.m_sName, Markup.MarkupSectionType.SubSection).MarkupEncoding);
                
                s.AppendFormat(
                "\\ref \\c {0} {1}\n<br/><br/>" +
                "<b>Description:</b><br/>" +
                "{2}\n<br/>" +
                "<br/>\n",
                m_sType,
                m_sName,
                m_sDesc
                );

                s.AppendLine();

                s.AppendFormat("<b>Code:</b>\n\\code {0} \\endcode", m_sCode);

                return s.ToString();
            }
        }
    }
}
