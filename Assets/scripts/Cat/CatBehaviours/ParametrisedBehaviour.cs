using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParametrisedBehaviour<T> : CatBehaviour
{
    public abstract void SetParam(T param);
}

public abstract class ParametrisedBehaviour<T1, T2> : CatBehaviour
{
    public abstract void SetParam(T1 param1, T2 param2);
}

public abstract class ParametrisedBehaviour<T1, T2, T3> : CatBehaviour
{
    public abstract void SetParam(T1 param1, T2 param2, T3 param3);
}

public abstract class ParametrisedBehaviour<T1, T2, T3, T4> : CatBehaviour
{
    public abstract void SetParam(T1 param1, T2 param2, T3 param3, T4 param4);
}
