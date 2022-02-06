using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : ScriptableObject
{

    [SerializeField]
    private int priority;
    [SerializeField]
    private bool drawGizmos;
    public int Priority => priority;

    protected GameObject gameObject;
    protected Transform mainObjectTransform;
    protected Transform rotatableTransform;
    protected AnimationSelector<EyesAnimationReference> eyeAnimator;
    protected AnimationSelector<MouthAnimationReference> mouthAnimator;
    protected PuffBallMovement movement;
    protected EyeDirection eyeDirection;
    protected CatFaceMove FaceMove;
    protected Rigidbody2D rb;

    private System.Action forceStopBehave;

    public virtual void SetUp(GameObject mainObject,
        Transform rotatableTransform,
        AnimationSelector<EyesAnimationReference> eyesAnimator,
        AnimationSelector<MouthAnimationReference> mouthAnimator,
        PuffBallMovement movement,
        EyeDirection eyeDir,
        CatFaceMove faceMove,
        System.Action forceStop)
    {
        this.rotatableTransform = rotatableTransform;
        gameObject = mainObject;
        rb = mainObject.GetComponent<Rigidbody2D>();
        mainObjectTransform = gameObject.transform;
        eyeAnimator = eyesAnimator;
        this.mouthAnimator = mouthAnimator;
        this.movement = movement;
        this.FaceMove = faceMove;
        eyeDirection = eyeDir;
        forceStopBehave = forceStop;
    }

    protected void ForceStopBehave()
    {
        forceStopBehave?.Invoke();
    }
    public virtual void Activate() { }
    public virtual void Disactivate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }

    protected virtual void DrawGizmos() { }

    public void OnDrawGizmos()
    {
        if (drawGizmos)
            DrawGizmos();
    }
}
