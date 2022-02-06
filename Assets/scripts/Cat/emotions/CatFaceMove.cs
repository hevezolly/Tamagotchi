using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFaceMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float moveRadius;

    private Vector2 normalPos;

    private void Awake()
    {
        normalPos = transform.localPosition;
        normalPos = transform.localPosition;
    }

    public void ResetPos()
    {
        transform.localPosition =
            new Vector3(normalPos.x, normalPos.y, transform.localPosition.z);
    }

    public void SetTarget(Vector3 pos)
    {
        var o = pos - (transform.parent.position + (Vector3)offset);
        o = o.normalized * Mathf.Min(o.magnitude, moveRadius);
        transform.position = (transform.parent.position + (Vector3)offset) + o;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.parent.position + (Vector3)offset, moveRadius);
    }
}
