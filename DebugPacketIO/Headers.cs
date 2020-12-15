using System;
using System.Collections.Generic;
using System.Text;

namespace DebugPacketIO
{
    public enum DebugType : byte
    {
        LOG = 1,
        WARNING,
        ERROR,
        UNKNOWN,
    }
}
