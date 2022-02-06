using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelector<T> : MonoBehaviour
    where T: AnimationClipReference
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private T IdleAnimation;
    private bool overrideCurrentAnimation = true;
    private T currentBaseAnimation;
    private class SwappersComparer : IComparer<AnimationSwapper<T>>
    {
        public int Compare(AnimationSwapper<T> x, AnimationSwapper<T> y)
        {
            return y.Priority.CompareTo(x.Priority);
        }
    }

    private SortedSet<AnimationSwapper<T>> activeSwappers = 
        new SortedSet<AnimationSwapper<T>>(new SwappersComparer());

    public void AddSwapper(AnimationSwapper<T> swapper)
    {
        activeSwappers.Add(swapper);
        if (overrideCurrentAnimation)
            Play(currentBaseAnimation);
    }

    public void RemoveSwapper(AnimationSwapper<T> swapper)
    {
        if (activeSwappers.Contains(swapper))
            activeSwappers.Remove(swapper);
        if (overrideCurrentAnimation)
            Play(currentBaseAnimation);
    }

    public void PlayIdle()
    {
        Play(IdleAnimation);
    }

    public void ForcePlay(T animation)
    {
        currentBaseAnimation = animation;
        overrideCurrentAnimation = false;
        animator.Play(animation.ClipName);
    }

    public void Play(T animation)
    {
        currentBaseAnimation = animation;
        overrideCurrentAnimation = true;
        var animationToSet = animation;
        foreach (var swapper in activeSwappers)
        {
            if (!swapper.Contains(animation))
                continue;
            animationToSet = swapper.ResultAnimation;
            break;
        }
        animator.Play(animationToSet.ClipName);
    }
}

