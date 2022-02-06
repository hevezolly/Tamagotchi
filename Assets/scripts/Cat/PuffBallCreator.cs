using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuffBallCreator : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D collider;
    [SerializeField]
    private float colliderOffset;
    public float ColliderOffset => colliderOffset;
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private float radius;
    public float Radius => radius;
    [SerializeField]
    private int numberOfSegments;
    public int NumberOfSegments => numberOfSegments;
    [SerializeField]
    private float innerRadius;
    public float InnerRadius => innerRadius;
    [SerializeField]
    [Min(1)]
    private int numberOfLayers;
    public int NumberOfLayers => numberOfLayers;
    [SerializeField]
    [Range(0, 1)]
    private float innerRadiusCoefficient;


    [Header("joints set up")]
    [SerializeField]
    private float frequency;
    [SerializeField]
    [Range(0, 1)]
    private float dampening;

    public BodyPart[] segments { get; private set; }

    private float segmentMass;

    // Start is called before the first frame update
    void Awake()
    {
        segments = new BodyPart[numberOfSegments * numberOfLayers];
        segmentMass = body.mass / (numberOfSegments * numberOfLayers + 1);
        body.mass = segmentMass;
        CreateParts();
        ConfigureCollider();
    }

    private void CreateParts()
    {
        var angle = Mathf.PI * 2 / numberOfSegments;
        
        var rad = radius / numberOfLayers;
        var innerRad = innerRadius * Mathf.Pow(innerRadiusCoefficient, numberOfLayers-1);
        var firstLayer = true;
        for (var j = 0; j < numberOfLayers; j++)
        {
            var offset = new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * rad;
            var first = insertPart(offset, innerRad);
            var previous = first;
            segments[numberOfSegments * j] = first;
            for (var i = 1; i < numberOfSegments; i++)
            {
                offset = new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i)) * rad;
                var n = insertPart(offset, innerRad);
                AddDistance(n.rb, previous.rb);
                AddRotation(n.rb, body);
                if (i >= 2)
                {
                    AddSpring(n.rb, segments[numberOfSegments * j + i - 2].rb);
                }
                if (firstLayer)
                {
                    AddSpring(n.rb, body);
                }
                else
                {
                    AddSpring(n.rb, segments[numberOfSegments * (j - 1) + i].rb);
                    AddSpring(n.rb, segments[numberOfSegments * (j - 1) + (i+1)%numberOfSegments].rb);
                    AddSpring(n.rb, segments[numberOfSegments * (j - 1) + (i-1)%numberOfSegments].rb);
                }
                segments[numberOfSegments * j + i] = n;
                previous = n;
            }
            AddDistance(first.rb, previous.rb);
            AddRotation(first.rb, body);
            AddSpring(segments[numberOfSegments * j + 1].rb, previous.rb);
            if (firstLayer)
            {
                AddSpring(first.rb, body);
            }
            else
            {
                AddSpring(first.rb, segments[numberOfSegments * (j - 1)].rb);
                AddSpring(first.rb, segments[numberOfSegments * (j - 1) + 1].rb);
                AddSpring(first.rb, segments[numberOfSegments * (j - 1) + numberOfSegments - 1].rb);
            }
            firstLayer = false;
            rad += radius / numberOfLayers;
            innerRad /= innerRadiusCoefficient;
        }
    }

    private void Update()
    {
        ConfigureCollider();
    }

    private void ConfigureCollider()
    {
        var points = new Vector2[numberOfSegments];
        for (var i = 0; i < numberOfSegments; i++)
        {
            var segment = segments[numberOfSegments * (numberOfLayers - 1) + i];
            var p = segment.gameObject.transform.position;
            var offset = p - transform.position;
            offset *= (offset.magnitude + innerRadius + colliderOffset) / offset.magnitude;
            p = transform.position + offset;
            points[i] = transform.InverseTransformPoint(p);
        }
        collider.points = points;
    }

    private void AddSpring(Rigidbody2D one, Rigidbody2D other)
    {
        var neighbourJoint = one.gameObject.AddComponent<SpringJoint2D>();
        neighbourJoint.autoConfigureDistance = true;
        neighbourJoint.connectedBody = other;
        neighbourJoint.dampingRatio = dampening;
        neighbourJoint.frequency = frequency;
        neighbourJoint.enableCollision = true;
    }

    private void AddDistance(Rigidbody2D one, Rigidbody2D other)
    {
        var distance = one.gameObject.AddComponent<DistanceJoint2D>();
        distance.autoConfigureDistance = true;
        distance.connectedBody = other;
        distance.enableCollision = true;
    }

    private void AddRotation(Rigidbody2D one, Rigidbody2D other)
    {
        var rotation = one.gameObject.AddComponent<RelativeJoint2D>();
        rotation.correctionScale = 1;
        rotation.maxForce = 0;
        rotation.maxTorque = 1000;
        rotation.autoConfigureOffset = true;
        rotation.connectedBody = other;
        rotation.enableCollision = true;
    }

    private BodyPart insertPart(Vector2 offset, float radius)
    {
        var o = new GameObject();
        o.layer = gameObject.layer;
        o.transform.parent = transform;
        o.transform.position = transform.position + (Vector3)offset;
        var c = o.AddComponent<CircleCollider2D>();
        c.radius = radius;
        var rb = o.AddComponent<Rigidbody2D>();
        rb.mass = segmentMass;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        //var j = o.AddComponent<SliderJoint2D>();
        //j.autoConfigureAngle = true;
        //j.connectedBody = body;
        var part = new BodyPart()
        {
            gameObject = o,
            rb = rb,
            radius = radius
        };
        return part;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (!Application.isPlaying)
        {
            
            var angle = Mathf.PI * 2 / numberOfSegments;
            for (var i = 0; i < numberOfSegments; i++)
            {
                var offset = new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i));
                var rad = radius;
                var step = radius / numberOfLayers;
                var innerRad = innerRadius;
                for (var j = 0; j < numberOfLayers; j++) 
                {
                    Gizmos.DrawLine(transform.position, transform.position + (Vector3)offset * rad);
                    Gizmos.DrawWireSphere(transform.position + (Vector3)offset * rad, innerRad);
                    rad -= step;
                    innerRad *= innerRadiusCoefficient;
                }
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius + innerRadius + colliderOffset);
        }
        else
        {
            for (var i = 0; i < segments.Length; i++)
            {
                var offset = segments[i].gameObject.transform.position - transform.position;
                Gizmos.DrawLine(transform.position, transform.position + offset);
                Gizmos.DrawWireSphere(transform.position + offset, segments[i].radius);
            }
        }
    }
}

public class BodyPart
{
    public GameObject gameObject;
    public Rigidbody2D rb;
    public float radius;
}
