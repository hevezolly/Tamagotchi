using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TailCreator : MonoBehaviour
{
    [SerializeField]
    private PuffBallCreator puffBall;
    [SerializeField]
    private CatRotation rotation;
    [SerializeField]
    private Vector2 tailOffset;

    [SerializeField]
    private float length;
    [SerializeField]
    private float maxWidth;
    [SerializeField]
    private float maxMass;
    [SerializeField]
    [Min(2)]
    private int numOfSegments;
    [SerializeField]
    private AnimationCurve tailWidth;
    [Header("joint")]
    [SerializeField]
    [Range(0, 1)]
    private float DampenRation;

    [SerializeField]
    private float Frequency;

    private float maxSegmentMass;

    private TargetJoint2D joint;
    private LineRenderer line;

    public BodyPart[] segments { get; private set; }


    private void Start()
    {
        segments = new BodyPart[numOfSegments];
        line = GetComponent<LineRenderer>();
        CreateTail();
    }

    public void CreateTail()
    {
        line.enabled = true;
        var center = transform.position + (Vector3)rotation.Up * tailOffset.y +
            (Vector3)rotation.Right * tailOffset.x;
        var step = length / (numOfSegments - 1);
        var direction = -rotation.Up;
        var first = true;
        BodyPart prev = null;
        line.widthMultiplier = maxWidth * 2;
        line.widthCurve = tailWidth;
        line.positionCount = numOfSegments;
        for (var i = 0; i < numOfSegments; i++)
        {
            var len = step * i;
            var t = len / length;
            var mass = maxMass * tailWidth.Evaluate(t);
            var width = GetWidth(t);
            var pos = center + (Vector3)direction * len;
            var part = CreatePart(pos, width, mass);
            segments[i] = part;
            if (first)
            {
                joint = part.gameObject.AddComponent<TargetJoint2D>();
                joint.dampingRatio = DampenRation;
                joint.frequency = Frequency;
                part.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                prev = part;
            }
            else
            {
                AddJoint(part.rb, prev.rb, step);
                prev = part;
            }
            line.SetPosition(i, part.gameObject.transform.position);
            first = false;
        }
    }

    private void FixedUpdate()
    {
        var center = (Vector2)transform.position + rotation.Up * tailOffset.y +
            rotation.Right * tailOffset.x;
        joint.target = center;
    }

    private void Update()
    {
        for (var i = 0; i < numOfSegments; i++)
        {
            line.SetPosition(i, segments[i].gameObject.transform.position);
        }
    }

    private void AddJoint(Rigidbody2D one, Rigidbody2D other, float distance)
    {
        var joint = one.gameObject.AddComponent<DistanceJoint2D>();
        joint.autoConfigureDistance = false;
        joint.distance = distance;
        joint.connectedBody = other;
        joint.enableCollision = false;
    }

    private BodyPart CreatePart(Vector2 position, float width, float mass)
    {
        var o = new GameObject();
        o.layer = gameObject.layer;
        o.transform.parent = transform;
        var pos = new Vector3(position.x, position.y, transform.position.z);
        o.transform.position = pos;
        var rb = o.AddComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        var c = o.AddComponent<CircleCollider2D>();
        c.radius = width;
        var part = new BodyPart()
        {
            gameObject = o,
            radius = width,
            rb = rb
        };
        return part;
    }

    private float GetWidth(float t)
    {
        return tailWidth.Evaluate(t) * maxWidth;
    }

    private void OnDrawGizmos()
    {
        var center = (Vector2)transform.position + rotation.Up * tailOffset.y + 
            rotation.Right * tailOffset.x;
        var step = length / (numOfSegments - 1);
        var direction = -rotation.Up;
        for (var i = 0; i < numOfSegments; i++)
        {
            var len = step * i;
            var width = GetWidth(len / length);
            var pos = center + direction * len;
            Gizmos.DrawWireSphere(pos, width);
        }
    }

}
