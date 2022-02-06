using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationClipReference : ScriptableObject
{
    [SerializeField]
    private string clipName;
    public string ClipName => clipName;

    public static bool operator ==(AnimationClipReference lhs, AnimationClipReference rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(AnimationClipReference lhs, AnimationClipReference rhs) => !(lhs == rhs);

    public override string ToString()
    {
        return clipName;
    }

    public override int GetHashCode()
    {
        return clipName.GetHashCode();
    }
}
