using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc.Markup
{
    public abstract class MarkupPrimitive
    {
        private MarkupFileReference m_fileRef;

        public MarkupPrimitive(MarkupFileReference fileRef)
        {
            m_fileRef = fileRef;
        }

        public MarkupPrimitive()
        {
            m_fileRef = MarkupFileReference.Artificial;
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_fileRef;
            }
        }

        public abstract String MarkupEncoding { get; }
    }
}
