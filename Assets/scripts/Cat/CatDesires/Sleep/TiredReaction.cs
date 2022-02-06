using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiredReaction : ValueReaction<float>
{
    private Value<float> value;
    [Header("References")]
    [SerializeField]
    private CatBehaviourPicker behaviourPicker;
    [SerializeField]
    private CatBehaviourRegistrator registrator;
    [SerializeField]
    private EyesAnimationSelector eyesSelector;
    [SerializeField]
    private MouthAnimationSelector mouthSelector;
    [SerializeField]
    private PuffBallEmotions emotionsIndicator;
    [SerializeField]
    private PuffBallDesireIndicator desiresIndicator;

    [Header("Params")]
    [SerializeField]
    [Min(0)]
    private float forcedWakeUpValueIncrease;
    [Min(0)]
    [SerializeField]
    private float forcedWakeUpDecrease;
    [SerializeField]
    private ParametrisedBehaviour<SleepBus> sleepBehaviourObj;
    private ParametrisedBehaviour<SleepBus> sleepBehaviour;
    [SerializeField]
    private EyesSwapper eyesSwapper;
    [SerializeField]
    private MouthSwapper mouthSwapper;
    [SerializeField]
    private Sprite TiredIconBig;
    [SerializeField]
    [Range(0, 1)]
    private float startIconAlpha;
    [SerializeField]
    private Sprite TiredIconSmall;

    private IconReference icon;
    private float startValue;

    private void Awake()
    {
        if (eyesSwapper != null)
            eyesSwapper.HashAnimations();
        if (mouthSwapper != null)
            mouthSwapper.HashAnimations();
        if (sleepBehaviourObj != null)
        {
            sleepBehaviour = Instantiate(sleepBehaviourObj);
            registrator.RegisterBehaviour(sleepBehaviour);
        }
    }
    public override void OnTrackingEnded()
    {
        RemoveAnimationSwappers();
        DisableIcons();
    }

    private void AddAnimationSwappers()
    {
        if (eyesSwapper != null)
            eyesSelector.AddSwapper(eyesSwapper);
        if (mouthSwapper != null)
            mouthSelector.AddSwapper(mouthSwapper);
    }

    private void RemoveAnimationSwappers()
    {
        if (eyesSwapper != null)
            eyesSelector.RemoveSwapper(eyesSwapper);
        if (mouthSwapper != null)
            mouthSelector.RemoveSwapper(mouthSwapper);
    }

    private void EnableIcons()
    {
        emotionsIndicator.RequestEmotion(new EmotionRequest() { sprite = TiredIconBig });
        icon = desiresIndicator.AddDesire(TiredIconSmall, startIconAlpha);
    }

    private void DisableIcons()
    {
        icon.RemoveImage();
    }

    public override void OnTrackingStart(Value<float> value)
    {
        this.value = value;
        startValue = value.MainValue;
        AddAnimationSwappers();
        EnableIcons();
    }

    private void Sleep()
    {
        if (sleepBehaviour == null)
            return;
        var bus = new SleepBus()
        {
            value = value,
            WakeUp = WakeUp
        };
        sleepBehaviour.SetParam(bus);
        behaviourPicker.PushBehaviour(sleepBehaviour);
    }

    private bool WakeUp(WakeUpType type)
    {
        if (type == WakeUpType.Forced)
        {
            if (value.MainValue < startValue)
                value.SetValue(Mathf.Min(value.MainValue + forcedWakeUpValueIncrease, startValue));
            else
                value.Update(-forcedWakeUpDecrease);
            if (value.MainValue == value.MinValue)
                return false;
        }
        return true;
    }

    public override void OnValueChanged()
    {
        var t = Mathf.InverseLerp(startValue, value.MinValue, value.MainValue);
        icon.SetAlpha(Mathf.Lerp(startIconAlpha, 1, t));
        if (value.MainValue == value.MinValue)
        {
            Sleep();
        }
    }
}
