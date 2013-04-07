using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dasmdoc.Markup;

namespace dasmdoc
{
    class Data
    {
        private DasmDocBlock        m_docBlock;
        private MarkupFileReference m_fileRef;

        public Data(MarkupFileReference fileRef, DasmDocBlock docBlock)
        {
            m_fileRef = fileRef;
            m_docBlock = docBlock;
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_fileRef;
            }
        }

        public DasmDocBlock Documentation
        {
            get
            {
                return m_docBlock;
            }
        }

        public String Name
        {
            get
            {
                return m_docBlock.DefinitionName;
            }
        }

        public String Description
        {
            get
            {
                return m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_DESCRIPTION);
            }
        }

        public String Type
        {
            get
            {
                return m_docBlock.getAttribute(DasmDocBlock.ATTRIBUTE_TYPE);
            }
        }
    }
}
