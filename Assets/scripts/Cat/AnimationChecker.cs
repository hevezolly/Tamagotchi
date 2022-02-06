using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    [SerializeField]
    private PuffBallEmotions emotions;
    public void InAnimationFinished()
    {
        emotions.InAnimationFinished();
    }

    public void OutAnimationFinished()
    {
        emotions.OutAnimationFinished();
    }
}
