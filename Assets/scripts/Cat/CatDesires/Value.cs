using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Value<T>: MonoBehaviour
{
    protected T mainValue;
    public T MainValue => mainValue;
    public abstract T MinValue { get; }
    public abstract T MaxValue { get; }
    public abstract void SetValue(T newValue);
}

public static class ValueExtention
{
    public static void Update(this Value<float> value, float delta)
    {
        value.SetValue(value.MainValue + delta);
    }
    public static void Update(this Value<int> value, int delta)
    {
        value.SetValue(value.MainValue + delta);
    }
}

[System.Serializable]
public class ReactionParams<T>
{
    [SerializeField]
    private string name;
    public Vector2 threshold;
    public ValueReaction<T> reaction;
}

public class FloatValue : Value<float>
{

    [SerializeField]
    private float startValue;

    [SerializeField]
    private Vector2 minMaxValue;
    public override float MinValue => minMaxValue.x;
    public override float MaxValue => minMaxValue.y;
    [SerializeField]
    protected float valueChangeRate;

    private ReactionParams<float> currentReaction = null;

    [SerializeField]
    protected List<ReactionParams<float>> possibleReaction;

    protected virtual void Awake()
    {
        OnValueChange(startValue);
    }

    private void ChangeReaction(ReactionParams<float> reaction)
    {
        if (currentReaction == reaction)
            return;
        if (currentReaction != null)
            currentReaction.reaction.OnTrackingEnded();
        currentReaction = reaction;
        if (currentReaction != null)
            currentReaction.reaction.OnTrackingStart(this);
    }
    protected void OnValueChange(float newValue)
    {
        mainValue = Mathf.Clamp(newValue, minMaxValue.x, minMaxValue.y);
        for (var i = 0; i < possibleReaction.Count; i++)
        {
            if (possibleReaction[i].threshold.x <= mainValue &&
                possibleReaction[i].threshold.y >= mainValue)
            {
                ChangeReaction(possibleReaction[i]);
                currentReaction.reaction.OnValueChanged();
                return;
            }
        }
        ChangeReaction(null);
    }

    protected virtual void Update()
    {
        var delta = valueChangeRate * Time.deltaTime;
        OnValueChange(mainValue + delta);
    }

    public override void SetValue(float newValue)
    {
        OnValueChange(newValue);
    }
}
