using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new sleep behaviour", menuName = "Cat Behaviours/sleep")]
public class SleepBehaviour : ParametrisedBehaviour<SleepBus>
{
    private SleepBus sleepBus;
    [SerializeField]
    private float sleepPerSecond;
    [SerializeField]
    private EyesAnimationReference eyesAnimation;
    [SerializeField]
    private MouthAnimationReference mouthReference;
    [SerializeField]
    private float minVelocityToWakeUp;
    [SerializeField]
    [Range(0,180)]
    private float deltaAngleToWakeUp;
    private Vector3 initialUp;

    public override void SetParam(SleepBus param)
    {
        sleepBus = param;
    }

    public override void Activate()
    {
        eyeAnimator.ForcePlay(eyesAnimation);
        mouthAnimator.ForcePlay(mouthReference);
        initialUp = rotatableTransform.up;
    }

    private void WakeUp(WakeUpType type)
    {
        if (sleepBus.WakeUp(type))
            ForceStopBehave();
        else
            Activate();
    }

    public override void Update()
    {
        if (rb.velocity.magnitude > minVelocityToWakeUp
            || Vector3.Angle(rotatableTransform.up, initialUp) > deltaAngleToWakeUp)
        {
            WakeUp(WakeUpType.Forced);
            return;
        }
        var deltaSleep = sleepPerSecond * Time.deltaTime;
        sleepBus.value.Update(deltaSleep);
        if (sleepBus.value.MainValue == sleepBus.value.MaxValue)
        {
            WakeUp(WakeUpType.Natural);
        }
    }

    public override void Disactivate()
    {
        WakeUp(WakeUpType.Forced);
    }
}
