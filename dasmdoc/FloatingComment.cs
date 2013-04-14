using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dasmdoc.Markup;

namespace dasmdoc
{
    class FloatingComment : IDocumentationFeature
    {
        private DasmDocBlock m_docBlock;

        protected FloatingComment(DasmDocBlock docBlock)
        {
            m_docBlock = docBlock;
        }

        public static FloatingComment parse(DasmDocBlock doc)
        {
            if (doc.Content.Trim() != String.Empty)
                return null;

            return new FloatingComment(doc);
        }

        public MarkupFileReference FileReference
        {
            get
            {
                return m_docBlock.FileReference;
            }
        }

        public String Comment
        {
            get
            {
                return m_docBlock.Comment;
            }
        }
    }
}
