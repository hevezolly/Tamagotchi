using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRotation : MonoBehaviour
{
    [SerializeField]
    private PuffBallCreator creator;

    private Dictionary<GameObject, Quaternion> rotators;

    private bool initiated = false;

    public Vector2 Up { get
        {
            if (!Application.isPlaying || !initiated)
                return Vector3.up;
            return up;
        } }
    private Vector2 up;
    public Vector2 Right => Vector3.Cross(Up, Vector3.forward);

    private void Initiate()
    {
        rotators = new Dictionary<GameObject, Quaternion>();
        foreach (var part in creator.segments)
        {
            var vec = (part.gameObject.transform.position -
                transform.position).normalized;
            var angle = Vector2.SignedAngle(vec, Vector2.up);
            var rotator = Quaternion.AngleAxis(angle, Vector3.forward);
            rotators[part.gameObject] = rotator;
        }
        initiated = true;
    }

    private void Start()
    {
        Initiate();
    }

    private void Update()
    {
        var total = Vector2.zero;
        foreach (var part in creator.segments)
        {
            var localUp = (part.gameObject.transform.position -
                transform.position).normalized;
            total += (Vector2)(rotators[part.gameObject] * localUp);
        }
        up = total / creator.segments.Length;
    }
}
