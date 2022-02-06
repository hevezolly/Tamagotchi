using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyFoodReaction : BaseEatingReaction
{
    [SerializeField]
    private AnimationSelector<EyesAnimationReference> eyesSelector;
    [SerializeField]
    private AnimationSelector<MouthAnimationReference> mouthSelector;
    [SerializeField]
    [Header("overeating")]
    private ParametrisedBehaviour<ParticleSystem> nauseaBehaviourObj;
    private ParametrisedBehaviour<ParticleSystem> nauseaBehaviour;
    [SerializeField]
    private ParticleSystem vomitPartickles;
    [SerializeField]
    [Min(0)]
    private float overeatingHungerBuff;
    [SerializeField]
    private float minValueToOverrideEmotions;
    [Header("visuals")]
    [SerializeField]
    private AnimationSwapper<EyesAnimationReference> eyesSwapper;
    [SerializeField]
    private AnimationSwapper<MouthAnimationReference> mouthSwapper;
    [SerializeField]
    private PuffBallEmotions emotions;
    [SerializeField]
    private PuffBallDesireIndicator desireIndicator;
    [SerializeField]
    private Sprite bigOvereatingIcon;
    [SerializeField]
    private Sprite smallOvereatingIcon;
    [SerializeField]
    [Range(0, 1)]
    private float minAlpha;
    [SerializeField]
    private PuffBallColor colorController;
    [SerializeField]
    private Color nauseaColor;
    private IconReference overeatingIcon;
    private float activationValue;

    private bool emotionsSwapped;

    protected override void Awake()
    {
        base.Awake();
        if (eyesSwapper != null)
            eyesSwapper.HashAnimations();
        if (mouthSwapper != null)
            mouthSwapper.HashAnimations();
    }

    public override void OnValueChanged()
    {
        if (value.MainValue == value.MaxValue)
        {
            Vomit();
        }
        if (!emotionsSwapped && value.MainValue >= minValueToOverrideEmotions)
        {
            OverrideEmotions();
        }
        else if (emotionsSwapped && value.MainValue < minValueToOverrideEmotions)
        {
            StopOverridingEmotions();
        }
        var t = Mathf.InverseLerp(activationValue, value.MaxValue, value.MainValue);
        var alpha = Mathf.Lerp(minAlpha, 1, t);
        overeatingIcon.SetAlpha(alpha);
        var color = Color.Lerp(colorController.StandartColor, nauseaColor, t);
        colorController.SetColor(color);
    }

    private void OverrideEmotions()
    {
        if (eyesSwapper != null)
            eyesSelector.AddSwapper(eyesSwapper);
        if (mouthSwapper != null)
            mouthSelector.AddSwapper(mouthSwapper);
        emotionsSwapped = true;
    }

    private void StopOverridingEmotions()
    {
        if (eyesSwapper != null)
            eyesSelector.RemoveSwapper(eyesSwapper);
        if (mouthSwapper != null)
            mouthSelector.RemoveSwapper(mouthSwapper);
        emotionsSwapped = false;
    }

    private void Vomit()
    {
        if (nauseaBehaviour != null)
        {
            nauseaBehaviour.SetParam(vomitPartickles);
            picker.PushBehaviour(nauseaBehaviour);
        }
        value.SetValue(value.MainValue - overeatingHungerBuff);
    }

    protected override void Initiate()
    {
        base.Initiate();
        if (nauseaBehaviourObj != null)
        {
            nauseaBehaviour = Instantiate(nauseaBehaviourObj);
            registrator.RegisterBehaviour(nauseaBehaviour);
        }
    }

    private void EnableEmoting()
    {
        emotions.RequestEmotion(new EmotionRequest() { sprite = bigOvereatingIcon });
        overeatingIcon = desireIndicator.AddDesire(smallOvereatingIcon, minAlpha);
    }

    private void DisableEmoting()
    {
        overeatingIcon.RemoveImage();
        overeatingIcon = null;
    }

    public override void OnTrackingStart(Value<float> value)
    {
        base.OnTrackingStart(value);
        activationValue = value.MainValue;
        EnableEmoting();
    }

    public override void OnTrackingEnded()
    {
        base.OnTrackingEnded();
        colorController.ResetColor();
        StopOverridingEmotions();
        DisableEmoting();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (nauseaBehaviour != null)
            nauseaBehaviour.OnDrawGizmos();
    }
}
