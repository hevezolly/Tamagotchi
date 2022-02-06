using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "new Nausea Behaviour", menuName = "Cat Behaviours/nausea")]
public class VomitBehaviour : ParametrisedBehaviour<ParticleSystem>
{
    [SerializeField]
    private EyesAnimationReference eyesAnim;
    [SerializeField]
    private MouthAnimationReference mouthAnim;
    [SerializeField]
    private GameObject stainObject;
    [SerializeField]
    private DelayedStainApplyer stainApplyer;
    [SerializeField]
    private float nauseaTime;
    private ParticleSystem vomitParts;

    private float stainPos;
    private StainableSurface fallSurface;
    private float fallTime;

    private float activationTime;
    private bool isFinished;
    public override void SetParam(ParticleSystem param)
    {
        vomitParts = param;
    }

    public override void Activate()
    {
        isFinished = false;
        if (TryCalculateFallParams())
        {
            ApplyStain();
        }
        Emote();
        activationTime = Time.time;
        vomitParts.Play();
    }

    public override void Update()
    {
        if (!isFinished && Time.time > activationTime + nauseaTime)
        {
            isFinished = true;
            ForceStopBehave();
        }
    }

    public override void Disactivate()
    {
        if (!isFinished)
            ForceStopBehave();
    }

    private void ApplyStain()
    {
        var applyer = Instantiate(stainApplyer);
        applyer.ApplyStain(fallTime, stainObject, fallSurface, stainPos);
    }

    private bool TryCalculateFallParams()
    {
        var hits = Physics2D.RaycastAll(vomitParts.transform.position, Vector2.down);
        foreach (var hit in hits)
        {
            if (!hit.collider.gameObject.TryGetComponent<StainableSurface>(out fallSurface))
                continue;
            stainPos = hit.point.x;
            fallTime = CalculateFallTime(hit);
            return true;
        }
        return false;
    }

    private float CalculateFallTime(RaycastHit2D hit)
    {
        var distance = Vector2.Distance(vomitParts.transform.position, hit.point);
        var g = -Physics2D.gravity.y * vomitParts.main.gravityModifier.constant;
        return Mathf.Sqrt(2 * distance / g);
    }

    private void Emote()
    {
        eyeAnimator.ForcePlay(eyesAnim);
        mouthAnimator.ForcePlay(mouthAnim);
        FaceMove.SetTarget(mainObjectTransform.position + Vector3.down * 10);
        eyeDirection.SetLookTargetDirection(Vector2.zero);
    }
}
