﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Enums
{
    public enum ActionType
    {
        Fold = 1,
        Check = 2,
        Bet = 3,
        Raise = 4,
        Limp = 5,
        AllIn = 6,
        AllInSteal = 7,
        Unknown = 8
    }
}