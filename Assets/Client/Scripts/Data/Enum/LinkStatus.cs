using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Enum
{
    public enum LinkStatus : byte
    {
        NotSpecified = 0,
        SpecifiedError = 1,
        Specified = 2,
        Downloaded = 3
    }
}