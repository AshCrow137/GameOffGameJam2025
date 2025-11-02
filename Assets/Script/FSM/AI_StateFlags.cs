using System;
    [Flags] 
    public enum AI_StateFlags
    {
        None    = 0,      // 0
        Basic   = 1 << 0, // 1
        Dash    = 1 << 1, // 2
        Defence = 1 << 2, // 4
        Long    = 1 << 3, // 8
        Ranged  = 1 << 4, // 16
        Heal    = 1 << 5, // 32
    }
