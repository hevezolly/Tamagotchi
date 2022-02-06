using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SleepBus
{
    public Value<float> value;
    public Func<WakeUpType, bool> WakeUp;
}

public enum WakeUpType
{
    Forced,
    Natural
}
