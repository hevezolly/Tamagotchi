using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "grab behaviour", menuName = "Cat Behaviours/grab")]
public class CatPickUpBehaviour : CatBehaviour
{
    [SerializeField]
    private EyesAnimationReference eyesAnimation;
    [SerializeField]
    private MouthAnimationReference mouthAnimation;
    [SerializeField]
    private bool overrideDirection;

    public override void Activate()
    {
        eyeAnimator.Play(eyesAnimation);
        mouthAnimator.Play(mouthAnimation);
        FaceMove.ResetPos();
        if (overrideDirection)
            eyeDirection.SetLookTargetDirection(Vector2.zero);
    }

    public override void Disactivate()
    {
        if (overrideDirection)
            eyeDirection.FollowPointer();
    }
}
