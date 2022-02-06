using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grab Cat Behaviour", menuName = "Click Behaviours/Grab Cat")]
public class GrabCatBehaviour : ClickBehaviour
{
    [SerializeField]
    private LayerMask catLayer;
    [SerializeField]
    private float breakForce;

    private TargetJoint2D joint;
    private Rigidbody2D target;
    private bool destroyed = true;

    private PuffBallCreator potancialCat;
    private Rigidbody2D potencialBody;
    private IPickuppuble pickUp;
    public override bool CheckClick(Vector3 pos)
    {
        var hits = Physics2D.RaycastAll(pos, Vector2.zero, 1, catLayer, -Mathf.Infinity);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                var cat = hit.collider.gameObject.GetComponent<PuffBallCreator>();
                if (cat == null)
                    continue;
                pickUp = hit.collider.GetComponent<IPickuppuble>();
                potancialCat = cat;
                potencialBody = hit.rigidbody;
                return true;
            }
        }
        return false;
    }

    public override void OnClick(Vector3 pos)
    {
        var minDist = Vector2.Distance(potencialBody.position, pos);
        foreach (var part in potancialCat.segments)
        {
            var d = Vector2.Distance(part.gameObject.transform.position, pos);
            if (d < minDist)
            {
                potencialBody = part.rb;
                minDist = d;
            }
        }
        pickUp.OnPickUp();
        target = potencialBody;
        joint = target.gameObject.AddComponent<TargetJoint2D>();
        destroyed = false;
        joint.breakForce = breakForce;
    }

    public override void OnRelease()
    {
        if (joint != null)
        {
            pickUp.OnDrop();
            Destroy(joint);
            target = null;
            joint = null;
            destroyed = true;
        }
    }

    public override void OnDrag(Vector3 position)
    {
        if (!destroyed && joint == null)
        {
            target = null;
            destroyed = true;
            pickUp.OnDrop();
            ForcedFinish();
        }
        if (!destroyed)
        {
            joint.target = position;
        }
    }
}
