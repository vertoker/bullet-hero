using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public enum StatusLink : byte
    {
        NotSpecified = 0,
        SpecifiedError = 1,
        Specified = 2,
        Download = 3
    }
}