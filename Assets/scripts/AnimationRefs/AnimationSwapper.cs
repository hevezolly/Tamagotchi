using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSwapper<T> : ScriptableObject
    where T: AnimationClipReference
{
    [SerializeField]
    private int priority;
    public int Priority => priority;
    
    [SerializeField]
    private List<T> AnimationsToSwap;

    private HashSet<T> hashedAnimations;

    [SerializeField]
    private T resultAnimation;
    public T ResultAnimation => resultAnimation;

    public void HashAnimations()
    {
        hashedAnimations = new HashSet<T>(AnimationsToSwap);
    }

    public bool Contains(T anim)
    {
        return hashedAnimations.Contains(anim);
    }
}
