using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatTransformsParent : MonoBehaviour
{
    [SerializeField]
    private CatRotation rotation;
    [SerializeField]
    private CatScale scale;
    [SerializeField]
    private PuffBallCreator creator;
    [SerializeField]
    private Vector2 size;

    // Update is called once per frame
    void LateUpdate()
    {
        var offset = Vector2.zero;
        foreach (var p in creator.segments)
        {
            offset += (Vector2)p.gameObject.transform.position - 
                (Vector2)transform.parent.position;
        }
        offset /= creator.segments.Length;
        var pos = transform.parent.position +
            new Vector3(offset.x, offset.y, transform.localPosition.z);
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rotation.Up);
        transform.localScale = new Vector3(scale.Scale.x, scale.Scale.y, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
