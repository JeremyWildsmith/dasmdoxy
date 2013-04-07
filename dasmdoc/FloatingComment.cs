using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc
{
    class FloatingComment
    {
        private String m_sContent;

        public FloatingComment(String sContent)
        {
            m_sContent = sContent;
        }

        public String Comment
        {
            get
            {
                return m_sContent;
            }
        }
    }
}
