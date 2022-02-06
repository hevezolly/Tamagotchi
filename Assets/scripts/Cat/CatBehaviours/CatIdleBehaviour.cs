using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "idle", menuName = "Cat Behaviours / idle")]
public class CatIdleBehaviour : CatBehaviour
{
    private bool isReset = false;
    public override void Update()
    {
        if (!isReset)
            Reset();
    }
    public override void Activate()
    {
    }

    private void Reset()
    {
        eyeAnimator.PlayIdle();
        mouthAnimator.PlayIdle();
        eyeDirection.FollowPointer();
        FaceMove.ResetPos();
        isReset = true;
    }

    public override void Disactivate()
    {
        isReset = false;
    }
}
