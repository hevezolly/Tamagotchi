using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ValueReaction<T> : MonoBehaviour
{
    public abstract void OnTrackingStart(Value<T> value);
    public abstract void OnTrackingEnded();

    public abstract void OnValueChanged();
}
