using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Eyes Animation Ref", menuName = "AnimationRefs/Eyes")]
public class EyesAnimationReference : AnimationClipReference
{
    public override bool Equals(object other)
    {
        var otherRef = other as EyesAnimationReference;
        if (otherRef == null)
            return false;
        return ClipName.Equals(otherRef.ClipName);
    }
}
