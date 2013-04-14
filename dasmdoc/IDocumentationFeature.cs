using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dasmdoc.Markup;

namespace dasmdoc
{
    interface IDocumentationFeature
    {
        MarkupFileReference FileReference { get; }
    }
}
