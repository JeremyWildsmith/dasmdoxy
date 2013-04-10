using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dasmdoc.Markup;

namespace dasmdoc
{
    class ParsingException : Exception
    {
        private MarkupFileReference m_fileRef;

        public ParsingException(String sReason)
            : base(sReason)
        {
        }

        public ParsingException(String sReason, MarkupFileReference fileRef)
            : base(sReason)
        {
            m_fileRef = fileRef;
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_fileRef;
            }
        }
    }
}
