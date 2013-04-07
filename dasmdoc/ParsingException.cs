using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dasmdoc
{
    class ParsingException : Exception
    {

        public ParsingException(String sReason)
            : base(sReason)
        {
        }
    }
}
