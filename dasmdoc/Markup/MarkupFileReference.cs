using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace dasmdoc.Markup
{
    public class MarkupFileReference
    {
        public static MarkupFileReference Artificial = new MarkupFileReference("Artificial", 0);

        private String m_sRelativePath;
        private int    m_iLineNumber;

        public MarkupFileReference(String sRelativePath, int iLineNumber)
        {
            m_sRelativePath = sRelativePath;
            m_iLineNumber = iLineNumber;
        }

        public String RelativePath
        {
            get
            {
                return m_sRelativePath;
            }
        }

        public String FileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.RelativePath);
            }
        }

        public int LineNumber
        {
            get
            {
                return m_iLineNumber;
            }
        }
    }
}
