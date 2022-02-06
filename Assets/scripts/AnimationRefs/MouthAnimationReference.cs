using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Mouth Animation Ref", menuName = "AnimationRefs/Mouth")]
public class MouthAnimationReference : AnimationClipReference
{
    public override bool Equals(object other)
    {
        var otherRef = other as MouthAnimationReference;
        if (otherRef == null)
            return false;
        return ClipName.Equals(otherRef.ClipName);
    }
}
