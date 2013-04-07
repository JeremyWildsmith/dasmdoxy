using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc
{
    class Parameter
    {
        private String m_sName;
        private String m_sType;
        private String m_sDescription;

        public Parameter(String sName, String sType, String sDescription)
        {
            m_sName = sName;
            m_sType = sType;
            m_sDescription = sDescription;
        }

        public String Name
        {
            get
            {
                return m_sName;
            }
        }

        public String Type
        {
            get
            {
                return m_sType;
            }
        }

        public String Description
        {
            get
            {
                return m_sDescription;
            }
        }
    }
}
