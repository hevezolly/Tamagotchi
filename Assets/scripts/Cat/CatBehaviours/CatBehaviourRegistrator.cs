using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviourRegistrator : MonoBehaviour
{
    [SerializeField]
    private CatBehaviourPicker picker;
    [SerializeField]
    private AnimationSelector<EyesAnimationReference> EyesAnimator;
    [SerializeField]
    private AnimationSelector<MouthAnimationReference> MouthAnimator;
    [SerializeField]
    private Transform rotatableTransform; 
    [SerializeField]
    private PuffBallMovement movement;
    [SerializeField]
    private EyeDirection eyeDir;
    [SerializeField]
    private CatFaceMove faceMove;
    [SerializeField]
    private CatBehaviour idleObj;
    private CatBehaviour idle;

    private bool initiated = false;

    private void Awake()
    {   
        if (!initiated)
            Initiate();
    }

    private void OnEnable()
    {
        if (idleObj != null)
        {
            idle = Instantiate(idleObj);
            RegisterBehaviour(idle);
            picker.PushBehaviour(idle);
        }
    }

    private void Initiate()
    {
        initiated = true;
    }

    [ExecuteAlways]
    public void RegisterBehaviour(CatBehaviour behaviour)
    {
        if (!initiated)
            Initiate();
        behaviour.SetUp(gameObject, rotatableTransform, EyesAnimator, MouthAnimator, 
            movement, eyeDir, faceMove,
            () => picker.ReleaseBehaviour(behaviour));
    }
}
