using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public class MarkupFunction : MarkupPrimitive
    {
        private String m_sName;
        private String m_sDesc;
        private String m_sCallingConvention;
        private String m_sReturnType;
        private String m_sReturnDescription;
        private String m_sCode;
        private MarkupData[] m_parameters;

        public MarkupFunction(MarkupFileReference fileRef, String sName, String sDesc, String sCallingConvention, String sReturnType, String sReturnDesc, String sCode, MarkupData[] parameters)
            : base(fileRef)
        {
            m_sName = sName;
            m_sDesc = sDesc;
            m_sCallingConvention = sCallingConvention;
            m_sReturnDescription = sReturnDesc;
            m_sReturnType = sReturnType;
            m_sCode = sCode;
            m_parameters = parameters;
        }

        private String FormattedArguments
        {
            get
            {
                StringBuilder sArgumentList = new StringBuilder();
                sArgumentList.Append("(");
                if (m_parameters.Length > 0)
                {
                    foreach (MarkupData md in m_parameters)
                        sArgumentList.AppendFormat("\\c {0} {1}{2}",
                                                    md.Type,
                                                    md.Name,
                                                    (md == m_parameters[m_parameters.Length - 1] ? String.Empty : ", "));

                }
                sArgumentList.Append(")");

                return sArgumentList.ToString();
            }
        }

        private String DoxyFormattedArguments
        {
            get
            {
                StringBuilder sArgumentList = new StringBuilder();
                if (m_parameters.Length > 0)
                {
                    foreach (MarkupData md in m_parameters)
                        sArgumentList.AppendFormat("\\param {0} \\c {1} {2}\n",
                                                    md.Name,
                                                    md.Type,
                                                    md.Description);

                }

                return sArgumentList.ToString();
            }
        }

        public String ReturnDescription
        {
            get
            {
                return m_sReturnDescription;
            }
        }

        public String ReturnType
        {
            get
            {
                return m_sReturnType;
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

        public MarkupData[] Parameters
        {
            get
            {
                return m_parameters;
            }
        }

        public override String MarkupEncoding
        {
            get
            {
                StringBuilder s = new StringBuilder();

                s.AppendLine(new MarkupRawSection(this.m_sName, MarkupSectionType.SubSection).MarkupEncoding);

                s.AppendFormat(
                "\\ref \\c {0} \\ref \\c {1} {2}{3}\n<br/><br/>" +
                "<b>Description:</b><br/>" +
                "{4}\n<br/>" +
                "{5}" +
                "\\return \\ref \\c {6} {7}\n" +
                "<br/>\n",
                m_sCallingConvention,
                m_sReturnType,
                m_sName,
                this.FormattedArguments,
                m_sDesc,
                this.DoxyFormattedArguments,
                m_sReturnType,
                m_sReturnDescription
                );

                s.AppendLine();

                s.AppendFormat("<b>Code:</b>\n\\code {0} \\endcode", m_sCode);

                return s.ToString();
            }
        }
    }
}
