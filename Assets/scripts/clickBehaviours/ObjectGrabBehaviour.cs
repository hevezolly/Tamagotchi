using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "object grab", menuName = "Click Behaviours/object")]
public class ObjectGrabBehaviour : ClickBehaviour
{
    [SerializeField]
    private LayerMask layer;

    private GameObject potencialObject;
    private Rigidbody2D potencialRb;

    private GameObject ancor;
    private TargetJoint2D joint;
    private IPickuppuble pickuppuble;
    public override bool CheckClick(Vector3 pos)
    {
        var hits = Physics2D.RaycastAll(pos, Vector2.zero, 1, layer, -Mathf.Infinity);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                var pickUp = hit.collider.gameObject.GetComponent<IPickuppuble>();
                if (pickUp == null)
                    continue;

                pickuppuble = pickUp;
                potencialObject = hit.collider.gameObject;
                potencialRb = hit.rigidbody;
                return true;
            }
        }
        return false;
    }

    private TargetJoint2D createAncor(Vector3 position)
    {
        var a = new GameObject();
        a.transform.position = position;
        a.AddComponent<Rigidbody2D>();
        var j = a.AddComponent<FixedJoint2D>();
        j.connectedBody = potencialRb;
        var target = a.AddComponent<TargetJoint2D>();
        target.target = position;
        return target;
    }

    public override void OnClick(Vector3 position)
    {
        //joint = createAncor(position);
        joint = potencialObject.AddComponent<TargetJoint2D>();
        joint.anchor = potencialObject.transform.InverseTransformPoint(position);
        joint.target = position;
        pickuppuble.OnPickUp();
        //ancor = joint.gameObject;
    }

    public override void OnDrag(Vector3 position)
    {
        joint.target = position;
    }

    public override void OnRelease()
    {
        pickuppuble.OnDrop();
        //Destroy(ancor);
        Destroy(joint);
    }

}
